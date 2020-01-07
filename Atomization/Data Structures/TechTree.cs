using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization.DataStructures
{
	public class TechTreeNode
	{
		public string Name { get; set; }
		public string UID { get; set; }
		public string Description { get; set; }
		public bool IsEnabled { get; private set; }
		public Effect Effect { get; private set; }
		public List<TechTreeNode> ChildNodes { get; } = new List<TechTreeNode>();
		public TechTreeNode ParentNode { get; set; }

		private static TechTreeNode _Tree = null;
		public static TechTreeNode Tree
		{
			get
			{
				if (_Tree == null)
				{
					_Tree = CreateTree();
				}

				return Tree;
			}
		}
		private static TechTreeNode CreateTree()
		{
			TechTreeNode tree = new TechTreeNode();

			TechTreeNode science = new TechTreeNode();
			{

			}
			tree.ChildNodes.Add(science);

			TechTreeNode domestic = new TechTreeNode();
			{

			}
			tree.ChildNodes.Add(domestic);

			return tree;
		}
	}
}
