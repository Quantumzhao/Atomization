using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LCGuidebook.Core.DataStructures
{
	/* Stage is a general category of any action made by the government. It can be: 
	 * 1. Investigation and census (get info from sources (in-game entities))
	 * 2. Enactments and policies (make changes to values/in-game entities)
	 * 3. Orders to manufacture (create new in-game entities and deploy) 
	 * etc. */
	public abstract class Task : IExecutable, INotifyPropertyChanged, IUniqueObject
	{
		protected Task(string name, CostOfStage cost) 
		{
			GameManager.TimeElapsed += AdvanceProgress;
			Name = name;
			Cost = cost;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private int _TimeElapsed = 0;

		public List<Task> Dependence { get; } = new List<Task>();
		public List<Task> Dependent { get; } = new List<Task>();

		public Impact Impact { get; set; }

		public bool IsPending { get; protected set; } = false;

		public ConfidentialLevel ConfidentialLevel { get; set; }

		public string Name { get; set; }

		public CostOfStage Cost { get; protected set; }

		public string UID { get; } = GameManager.GenerateUID();

		/// <summary>
		///		Execute the task only if it is finally finished. 
		///		It is called in <see cref="AdvanceProgress"/>
		/// </summary>
		public abstract void Execute();

		protected void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}

		private void AdvanceProgress()
		{
			if (IsPending)
			{
				return;
			}

			_TimeElapsed++;
			ImposeLongTermEffect();
			var timeRemaining = Misc.Round(Cost.RequiredTime.Value - _TimeElapsed);
			if (timeRemaining == 0)
			{
				Execute();
			}
			EventManager.RaiseEvent(this, 
				new TaskProgressAdvancedEventArgs(timeRemaining)
				{
					Message = Name,
					Source = this, 
					Influence = this.Impact,
					ConfidentialLevel = this.ConfidentialLevel
				}
			);
		}

		private void ImposeLongTermEffect()
		{
			for (int i = 0; i < ResourceManager.NumOfFigures; i++)
			{
				ResourceManager.Me.Figures[i].Growth.AddTerm(Name, Cost.LongTermEffect[i]);
			}
		}
	}

	public class Census : Task
	{
		public Census(string name, int valueIndex) : base(name, 
			ResourceManager.GetCostOf(ResourceManager.Me.Figures[valueIndex].Name, TypesOfCostOfStage.Census))
		{
			_Index = valueIndex;
			//Init();
			ImposeShortTermEffect();
		}

		private int _Index;

		public override void Execute()
		{
			ResourceManager.Me.OutdatedFigures[_Index].Update(ResourceManager.Me.Figures[_Index]);
		}

		private void ImposeShortTermEffect()
		{
			for (int i = 0; i < ResourceManager.NumOfFigures; i++)
			{
				ResourceManager.Me.Figures[i].CurrentValue -= Cost.ShortTermEffect[i].Value;
			}
		}
	}

	public class Policy : Task
	{
		private Action _Action;

		public Policy(string name, CostOfStage cost, Action action) : 
			base(name, cost) 
		{
			_Action = action;
		}

		public override void Execute() => _Action();
	}

	public class Manufacture : Task
	{
		// Any object that is manufactured will not be automatically added to anywhere. 
		// Its destination should always be explicitly stated. 
		// e.g. send it to reserve
		public Manufacture(string name, Func<IDestroyable> produce, CostOfStage cost, Action<Manufacture> afterCompleted = null)
			: base(name, cost) 
		{
			_Produce = produce;
			_AfterCompleted = afterCompleted;
		}

		private readonly Func<IDestroyable> _Produce;
		private readonly Action<Manufacture> _AfterCompleted;

		public IDestroyable FinalProduct { get; private set; } = null;

		public override void Execute()
		{
			FinalProduct = _Produce();
			_AfterCompleted?.Invoke(this);
		}
	}

	public class Transportation : Task
	{
		public Transportation(string name, IDeployable deployable, Region nextStop, 
			CostOfStage cost, Action<Transportation> afterCompleted = null) : base(name, cost) 
		{
			Name = name;
			Cargo = deployable;
			Cost = cost;
			NextStop = nextStop;
			IsPending = true;
			_AfterCompleted = afterCompleted;
		}

		public static List<Transportation> Create(string name, IDeployable deployable, 
			Region from, Region to, CostOfStage cost)
		{
			var list = new List<Transportation>();
			var route = Misc.ShortestPath(from, to);
			var iter = route.First;
			Transportation prev = null;

			while (iter.Next != null)
			{
				Transportation transportation;

				transportation = new Transportation($"Transferring {deployable} to a new destination: {to}", deployable, iter.Next.Value,
					ResourceManager.GetCostOf(nameof(Installation), TypesOfCostOfStage.Transportation));

				if (prev != null)
				{
					prev._AfterCompleted = p => transportation.IsPending = false;
				}

				list.Add(transportation);

				prev = transportation;
				iter = iter.Next;
			}

			return list;
		}

		private Action<Transportation> _AfterCompleted;

		public IDeployable Cargo { get; private set; }
		public Region NextStop { get; set; }

		public override void Execute()
		{
			if (NextStop != null)
			{
				Cargo.DeployedRegion = NextStop;
				_AfterCompleted?.Invoke(this);
			}
		}
	}

	public class Deployment : Task
	{
		public Deployment(string name, Region destination, IDeployable deployable, CostOfStage cost) : 
			base(name, cost) 
		{
			DeployableObject = deployable;
			Destination = destination;
			Cost = cost;
		}

		public IDeployable DeployableObject { get; private set; } = null;
		public Region Destination { get; }

		public override void Execute()
		{
			DeployableObject.IsActivated = true;
			DeployableObject.DeployedRegion = Destination;
		}
	}
}
