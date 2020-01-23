using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization.DataStructures
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
			Expression bureaucracy = null
		)
		{
			Values[0] = economy;
			Values[1] = hiEduPopu;
			Values[2] = army;
			Values[3] = navy;
			Values[4] = food;
			Values[5] = rawMaterial;
			Values[6] = nuclearMaterial;
			Values[7] = stability;
			Values[8] = nationalism;
			Values[9] = satisfaction;
			Values[10] = bureaucracy;

			for (int i = 0; i < Nation.NUM_VALUES; i++)
			{
				if (Values[i] == null)
				{
					Values[i] = (Expression)0;
				}
			}
		}

		public readonly Expression[] Values = new Expression[Nation.NUM_VALUES];

		public event PropertyChangedEventHandler PropertyChanged;

		public double Economy => Values[0].Value;
		public double HiEduPopu => Values[1].Value;
		public double Army => Values[2].Value;
		public double Navy => Values[3].Value;
		public double Food => Values[4].Value;
		public double RawMaterial => Values[5].Value;
		public double NuclearMaterial => Values[6].Value;
		public double Stability => Values[7].Value;
		public double Nationalism => Values[8].Value;
		public double Satifaction => Values[9].Value;
		public double Bureaucracy => Values[10].Value;

		public Expression this[int index]
		{
			get => Values[index];
			set => Values[index] = value;
		}
	}
}
