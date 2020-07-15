using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCGuidebook;
using LCGuidebook.Core;

namespace LCGuidebook.Core.DataStructures
{
	public class Effect
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expressions">a vector of correspoonding dimension. null stands for f(x) = 0</param>
		/// <param name="isReadOnly"></param>
		public Effect(Expression[] expressions, bool isReadOnly = false)
		{
			//_Values[0] = economy;
			//_Values[1] = hiEduPopu;
			//_Values[2] = army;
			//_Values[3] = navy;
			//_Values[4] = food;
			//_Values[5] = rawMaterial;
			//_Values[6] = nuclearMaterial;
			//_Values[7] = stability;
			//_Values[8] = nationalism;
			//_Values[9] = satisfaction;
			//_Values[10] = bureaucracy;

			_Values = expressions.Select(x => x ?? (Expression)0).ToArray();
			IsReadOnly = isReadOnly;
		}

		public readonly bool IsReadOnly;
		private readonly Expression[] _Values;

		public event PropertyChangedEventHandler PropertyChanged;

		//public Expression this[MainIndexType index]
		//{
		//	get => _Values[(int)index];
		//	set
		//	{
		//		if (!IsReadOnly)
		//		{
		//			_Values[(int)index] = value;
		//		}
		//		else
		//		{
		//			throw new InvalidOperationException();
		//		}
		//	}
		//}

		public Expression this[int index]
		{
			get => _Values[index];
			set
			{
				if (!IsReadOnly)
				{
					_Values[index] = value;
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
		}

		public static Effect operator +(Effect effect1, Effect effect2)
		{
			var newEffect = new Effect(new Expression[ResourceManager.NumOfFigures], isReadOnly:true);
			for (int i = 0; i < ResourceManager.NumOfFigures; i++)
			{
				newEffect[i] = effect1[i] + effect2[i];
			}
			return newEffect;
		}

		public static Effect GenerateEmptyEffect() => new Effect(new Expression[ResourceManager.NumOfFigures]);
	}
}
