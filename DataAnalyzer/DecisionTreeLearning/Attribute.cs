using System;
using System.Collections.Generic;

namespace decision_tree_learning
{
	public class Attribute
	{
		private static int count = 0;
		private int index;
		private string name;
		private string type;
		private List<string> values;

		public Attribute (string name, string values) 
		{
			index = count;
			count++;
			this.name = name;
			this.values = new List<string>();
			if (values.Equals ("real")) {
				type = "continuous";
				this.values.Add ("real");
			}
			else
			{
				type = "discrete";
				string[] array = values.Substring(1,values.Length-2).Split(',');
				foreach (string value in array) 
					this.values.Add (value.Trim());
			}
		}
		public string GetName()
		{
			return name;
		}

		public void SetName(string name)
		{
			this.name = name;
		}

		public int GetIndex() 
		{
			return index;
		}

		public List<string> GetValues() 
		{
			return values;
		}

		public string GetType() 
		{
			return type;
		}

		public override string ToString ()
		{
			return string.Format ("Attribute: {0}; Type: {1}; Values: {2}", name, type,
				String.Join (", ", values));
		}
	}
}

