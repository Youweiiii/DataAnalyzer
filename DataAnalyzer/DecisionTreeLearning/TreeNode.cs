using System;
using System.Collections.Generic;
using System.Linq;

namespace decision_tree_learning
{
	public class TreeNode
	{
		private Dictionary<string, TreeNode> children;
		private Attribute attribute;
		private string type;
		private string target; 

		public TreeNode ()
		{
			
		}

		public TreeNode (Attribute attribute)
		{
			type = "intermediate";
			this.attribute = attribute;
			children = new Dictionary<string, TreeNode> ();
		}

		public TreeNode (string target)
		{
			type = "leaf";
			this.target = target;
		}

		public void AddChild(string name, TreeNode child)
		{
			children.Add (name, child);
		}

		public Dictionary<string, TreeNode> GetChildren()
		{
			return children;
		}

		public void SetType(string type)
		{
			this.type = type;
		}

		public string GetType()
		{
			return type;
		}

		public void SetTarget(string target)
		{
			this.target = target;
		}

		public string GetTarget()
		{
			return target;
		}

		public void SetAttribute(Attribute attribute) 
		{
			this.attribute = attribute;
		}

		public Attribute GetAttribute()
		{
			return attribute;
		}
			
		public override string ToString() 
		{
			if (type.Equals("leaf"))
				return string.Format("Leaf Node: {0}", target);
			return string.Format("Intermediate Node: {0} {1} {2}", attribute.GetName(), "Children: ", string.Join(", ", children.Keys));
		}

	}
}

