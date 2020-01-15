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

		/* NAME					: name of current objectnode
		 * SHOW					: show members of current OBJECTNODE. If current is null, then show root members
		 *						  If current is not an objectnode, nothing happens
		 * SHOW #[No]			: show members of ID [No]
		 * ACSS #[No]			: change current node to the one of ID [No]
		 * ASGN #[No]			: assign the value of ID [No] to current objectnode
		 * ASGN #[No1] #[No2]	: assign the value of ID [No2] to [No1]
		 * ASGN #[No] {lit}		: assign a literal to ID [No]
		 * ASGN {lit}			: assign a literal to current objectnode
		 * PARA #[No1]
		 * 
		 * e.g. 
		 * ASGN #1 #2			ASGN #1				ASGN 0.0
		 * ASGN #1 "Hello"		ASGN #1 1			ASGN #1 false
		 */
		private static void ParseAndExecute(string input)
		{
			Queue<string> tokens = new Queue<string>(input.Split());
			var opcode = tokens.Dequeue().ToUpper();

			if (opcode == "NAME")
			{
				Console.WriteLine($"{_CurrentNode.Header}\n");
			}
			else if (opcode == "SHOW")
			{
				// if `SHOW` is followed by one parameter
				ObjectNode dstNode = _CurrentNode as ObjectNode;
				if (tokens.TryDequeue(out string token))
				{
					// modify that node instead of current objectnode
					dstNode = GetByID(ParseToID(token)) as ObjectNode;
				}

				if (dstNode != null)
				{
					if (dstNode is CollectionNode)
					{
						Tabulate((dstNode as CollectionNode).Elements);
					}

					if (!dstNode.IsValueType)
					{
						PrintElements("Objects: ", Dashboard.GetRootObjectNodes().ToList());
						PrintElements("Methods: ", Dashboard.GetRootMethodNodes().ToList());
					}
					else
					{
						Console.WriteLine(dstNode.ObjectData);
					}
				}
			}
			else if (opcode == "ACSS")
			{
				_CurrentNode = GetByID(ParseToID(tokens.Dequeue()));
			}
			else if (opcode == "ASGN" && _CurrentNode is ObjectNode currentNode)
			{
				ObjectNode firstNode = currentNode;
				// if the first parameter is an ID
				if (TryParse(tokens.Dequeue(), out object firstValue))
				{
					// convert the ID to an objectnode
					firstNode = GetByID((int)firstValue) as ObjectNode;

					// if there is a second parameter
					if (tokens.TryDequeue(out string token))
					{
						// if it is an ID
						if (TryParse(token, out object secondValue))
						{
							// At this stage, assume only objectnodes can be assigned to objectnodes
							ObjectNode secondNode = GetByID((int)secondValue) as ObjectNode;
							firstNode.ObjectData = secondNode.ObjectData;
						}
						else
						{
							firstNode.ObjectData = secondValue;
						}
					}
				}
				// it follows the `SHOW {lit}` syntax
				else
				{
					firstNode.ObjectData = firstValue;
				}
			}
			else if (_CurrentNode is MethodNode methodNode)
			{

			}
			else throw new NotImplementedException();
		}

		private static void AddToCachedNodes(Node node)
		{
			if (_CachedNodes.ContainsKey(node))
			{
				return;
			}

			_CachedNodes.Add(node, _Counter);
			_Counter++;
		}

		private static void Tabulate(List<List<ObjectNode>> table)
		{
			for (int i = 0; i < table.Count; i++)
			{
				for (int j = 0; j < table[i].Count; j++)
				{
					AddToCachedNodes(table[i][j]);
					Console.Write(string.Format("{0,20}", _CachedNodes[table[i][j]] + " " + table[i][j].Header));
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
				sb.Append(string.Format("{0,20}", _CachedNodes[members[i]] + " " + members[i].Header));
			}
			Console.WriteLine(sb.ToString());
		}

		private static bool TryParse(string token, out object ret)
		{
			string rest = token.Substring(1);
			if (token.StartsWith('#'))
			{
				ret = int.Parse(rest);
				return true;
			}
			else if (token.StartsWith('\"'))
			{
				ret = rest.Substring(0, rest.Count() - 1);
			}
			else if (char.IsLetter(token[0]))
			{
				ret = bool.Parse(token);
			}
			else if (int.TryParse(token, out int result))
			{
				ret = result;
			}
			else
			{
				ret = double.Parse(token);
			}

			return false;
		}

		private static int ParseToID(string token) => int.Parse(token.Substring(1));
	}
}
