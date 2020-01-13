using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIEngine;

namespace CLI
{
	class Program
	{
		private static Dictionary<Node, int> _CachedNodes = new Dictionary<Node, int>();
		private static int _Counter = 0;
		
		private static Node _CurrentNode;

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

			var opcode = tokens.Dequeue().ToUpper();

			//if (opcode == "NEWX")
			//{
			//	Expression.Clear();

			//	// print candidates
			//	StringBuilder sb = new StringBuilder();
			//	var properties = Dashboard.GetRootObjectNodes().ToArray();
			//	for (int i = 0; i < properties.Length; i++)
			//	{
			//		sb.Append(string.Format("{0,20}", properties[i].Header));
			//	}
			//	Console.WriteLine(sb.ToString());
			//	sb.Clear();
			//	var methods = Dashboard.GetRootObjectNodes().ToArray();
			//	for (int i = 0; i < methods.Length; i++)
			//	{
			//		sb.Append(string.Format("{0,20}", methods[i].Header));
			//	}
			//	Console.WriteLine(sb.ToString());
			//}
			//else if (opcode == "CXPN")
			//{
			//	var iter = Expression.First;
			//	while (iter != null)
			//	{
			//		Console.Write(iter.Value.Header);
			//		if (iter.Next != null)
			//		{
			//			Console.Write(" > ");
			//		}
			//	}
			//	Console.WriteLine("\n");
			//}

			if (_CurrentNode is CollectionNode collectionNode)
			{

			}
			else if (_CurrentNode is ObjectNode objectNode)
			{
				int propertyIndex;
				switch (opcode)
				{
					case "ACSS":
						propertyIndex = int.Parse(tokens.Dequeue());
						_CurrentNode = _CachedNodes.Single(p => p.Value == propertyIndex).Key;
						//Expression.AddLast(property);
						break;

					case "ASGN":
						propertyIndex = int.Parse(tokens.Dequeue());
						var dstObjectNode = _CachedNodes.Single(p => p.Value == propertyIndex).Key;
						objectNode.ObjectData = (dstObjectNode as ObjectNode).ObjectData;
						break;

					case "SHOW":
						StringBuilder sb = new StringBuilder();

						sb.Append("Properties: ");
						var properties = objectNode.Properties;
						for (int i = 0; i < properties.Count; i++)
						{
							AddToCachedNodes(properties[i]);
							sb.Append(string.Format("{0,20}", _CachedNodes[properties[i]] + properties[i].Header));
						}
						Console.WriteLine(sb.ToString());
						sb.Clear();

						sb.Append("Methods:    ");
						var methods = objectNode.Methods;
						for (int i = 0; i < methods.Count; i++)
						{
							AddToCachedNodes(methods[i]);
							sb.Append(string.Format("{0,20}", _CachedNodes[methods[i]] + methods[i].Header));
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

		private static void AddToCachedNodes(Node node)
		{
			if (!_CachedNodes.ContainsKey(node))
			{
				_CachedNodes.Add(node, _Counter);
				_Counter++;
			}
		}
	}
}
