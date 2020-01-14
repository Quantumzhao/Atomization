using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIEngine;

namespace CLITestSample
{
	class Program
	{
		private static readonly Dictionary<Node, int> _CachedNodes = new Dictionary<Node, int>();
		private static int _Counter = 0;
		
		private static Node _CurrentNode;

		static void Main(string[] args)
		{
			ParseAndExecute("show");
			while (true)
			{
				ParseAndExecute(Console.ReadLine());
			}
		}

		private static void ParseAndExecute(string input)
		{
			Queue<string> tokens = new Queue<string>(input.Split());
			var opcode = tokens.Dequeue().ToUpper();

			if (opcode == "NAME")
			{
				Console.WriteLine($"{_CurrentNode.Header}\n");
			}
			else if (_CurrentNode == null)
			{
				switch (opcode)
				{
					case "SHOW":
						PrintElements("Objects: ", Dashboard.GetRootObjectNodes().ToList());
						PrintElements("Methods: ", Dashboard.GetRootMethodNodes().ToList());
						break;

					default:
						return;
				}
			}
			else if (_CurrentNode is ObjectNode objectNode)
			{
				int propertyIndex;
				switch (opcode)
				{
					case "ACSS":
						propertyIndex = int.Parse(tokens.Dequeue());
						_CurrentNode = GetByID(propertyIndex);
						break;

					case "ASGN":
						propertyIndex = int.Parse(tokens.Dequeue());
						var dstObjectNode = GetByID(propertyIndex);
						objectNode.ObjectData = (dstObjectNode as ObjectNode).ObjectData;
						break;

					case "SHOW":
						if (_CurrentNode is CollectionNode collectionNode)
							Tabulate(collectionNode.Elements);
						PrintElements("Properties: ", objectNode.Properties);
						PrintElements("Methods: ", objectNode.Methods);
						break;

					default:
						return;
				}
			}
			else if (_CurrentNode is MethodNode methodNode)
			{

			}
			else throw new NotImplementedException();
		}

		private static void AddToCachedNodes(Node node)
		{
			if (_CachedNodes.ContainsKey(node)) return;

			_CachedNodes.Add(node, _Counter);
			_Counter++;
		}

		private static void Tabulate(List<List<ObjectNode>> table)
		{
			for (int i = 0; i < table.Count; i++)
			{
				for (int j = 0; j < table[i].Count; j++)
				{
					Console.Write(string.Format("{0,20}", table[i][j].Header));
				}
				Console.WriteLine();
			}
		}

		private static Node GetByID(int id) => _CachedNodes.Single(p => p.Value == id).Key;

		private static void PrintElements<T>(string caption, List<T> members) where T : Node
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("{0,12}", caption));
			for (int i = 0; i < members.Count; i++)
			{
				AddToCachedNodes(members[i]);
				sb.Append(string.Format("{0,20}", _CachedNodes[members[i]] + members[i].Header));
			}
			Console.WriteLine(sb.ToString());
			sb.Clear();
		}
	}
}
