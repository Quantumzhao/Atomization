using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIEngine;

namespace CLI
{
	class Program
	{
		private static LinkedList<Node> _Nodes = new LinkedList<Node>();
		
		private static Node _CurrentNode => _Nodes.Last.Value;

		static void Main(string[] args)
		{
			while (true)
			{
				ParseAndExecute(Console.ReadLine());
			}
		}

		private static void ParseAndExecute(string input)
		{
			Queue<string> tokens = new Queue<string>(input.Split());

			if (_CurrentNode is CollectionNode collectionNode)
			{

			}
			else if (_CurrentNode is ObjectNode objectNode)
			{
				switch (tokens.Dequeue().ToUpper())
				{
					case "ACSS":
						var propertyIndex = tokens.Dequeue();
						_Nodes.AddLast(objectNode.Properties[int.Parse(propertyIndex)]);
						break;

					case "ASGN":
						break;

					case "SHOW":
						// read manifest
						int tab = 20;
						var table = new List<List<string>> { new List<string>(), new List<string>() };
						table[0].Add("Properties: ");
						table[1].Add("Methods: ");
						foreach (var p in objectNode.Properties)
						{
							table[0].Add(p.Header);
						}
						foreach (var m in objectNode.Methods)
						{
							table[1].Add(m.Header);
						}

						// print
						StringBuilder sb = new StringBuilder();
						for (int i = 0; i < table[0].Count; i++)
						{
							sb.Append(string.Format($"{{0,{tab}}}", table[0][i]));
						}
						Console.WriteLine(sb.ToString());
						sb.Clear();
						for (int i = 0; i < table[1].Count; i++)
						{
							sb.Append(string.Format($"{{0, {tab}}}", table[1][i]));
						}
						Console.WriteLine(sb.ToString());
						break;

					default:
						throw new InvalidOperationException("Invalid Opcode");
				}
			}
			else if (_CurrentNode is MethodNode methodNode)
			{

			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
