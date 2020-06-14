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
		public Effect(
			Expression economy = null,
			Expression hiEduPopu = null,
			Expression army = null,
			Expression navy = null,
			Expression food = null,
			Expression rawMaterial = null,
			Expression nuclearMaterial = null,
			Expression stability = null,
			Expression nationalism = null,
			Expression satisfaction = null,
			Expression bureaucracy = null, 
			bool isReadOnly = false
		)
		{
			_Values[0] = economy;
			_Values[1] = hiEduPopu;
			_Values[2] = army;
			_Values[3] = navy;
			_Values[4] = food;
			_Values[5] = rawMaterial;
			_Values[6] = nuclearMaterial;
			_Values[7] = stability;
			_Values[8] = nationalism;
			_Values[9] = satisfaction;
			_Values[10] = bureaucracy;

			for (int i = 0; i < Nation.NUM_VALUES; i++)
			{
				if (_Values[i] == null)
				{
					_Values[i] = (Expression)0;
				}
			}

			IsReadOnly = isReadOnly;
		}
		public readonly bool IsReadOnly;
		public event PropertyChangedEventHandler PropertyChanged;
		private readonly Expression[] _Values = new Expression[Nation.NUM_VALUES];

		public Expression this[MainIndexType index]
		{
			get => _Values[(int)index];
			set
			{
				if (!IsReadOnly)
				{
					_Values[(int)index] = value;
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
		}
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
			var newEffect = new Effect(isReadOnly:true);
			for (int i = 0; i < Nation.NUM_VALUES; i++)
			{
				newEffect[i] = effect1[i] + effect2[i];
			}
			return newEffect;
		}
	}
}
