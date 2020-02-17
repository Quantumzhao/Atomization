using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

namespace LCGuidebook.Core.DataStructures
{
	public delegate void StageFinishedHandler(Task previous, Task next, Task task);

	//public class VMList<V> : List<V>, INotifyCollectionChanged
	//{
	//	public event PropertyChangedEventHandler PropertyChanged;
	//	public event NotifyCollectionChangedEventHandler CollectionChanged;

	//	public new void Add(V newValue)
	//	{
	//		base.Add(newValue);
	//		CollectionChanged?.Invoke(this, 
	//			new NotifyCollectionChangedEventArgs(
	//				NotifyCollectionChangedAction.Add, newValue
	//			)
	//		);
	//	}

	//	public new bool Remove(V oldValue)
	//	{
	//		if (base.Remove(oldValue))
	//		{
	//			CollectionChanged?.Invoke(this,
	//				new NotifyCollectionChangedEventArgs(
	//					NotifyCollectionChangedAction.Remove, oldValue
	//				)
	//			);
	//			return true;
	//		}
	//		else
	//		{
	//			return false;
	//		}
	//	}

	//	//public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	//	//{
	//	//	CollectionChanged?.Invoke(this, e);
	//	//}
	//}

	public class CostOfStage
	{
		public CostOfStage(Effect longTermEffect, Effect shortTermEffect, Expression requiredTime)
		{
			LongTermEffect = longTermEffect;
			ShortTermEffect = shortTermEffect;
			RequiredTime = requiredTime;
		}

		public Effect LongTermEffect { get; }
		public Effect ShortTermEffect { get; }
		public Expression RequiredTime { get; }
	}

	public static class Misc
	{
		public const string BAD_ACTIVATION_SEQUENCE = "Must be activated after all dependencies";

		public static int Round(double value) => (int)(value + 0.5);
	}

	public interface IExecutable
	{
		void Execute();
	}

	public interface IDeployable : IDestroyable
	{
		bool IsActivated { get; set; }
		Region DeployedRegion { get; set; }
	}

	public interface IDestroyable 
	{
		event Action SelfDestroyed;
		void DestroyThis();
	}

	public interface IBuildable : IDestroyable
	{
		Effect BuildCost_LongTerm { get; }
	}

	public interface IUniqueObject
	{
		string UID { get; }
	}
}
