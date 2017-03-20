using System;
using System.Collections.Generic;
using System.Linq;

namespace decision_tree_learning
{
	public class DataPoint
	{
		private string target;
		public  Dictionary<string, string> values;
		public DataPoint ()
		{
			values = new Dictionary<string, string> ();
		}

		public DataPoint(List<Attribute> attributes, string data)
		{
			values = new Dictionary<string, string> ();
			string[] array = data.Split (','); 

			for (int i = 0; i < attributes.Count - 1; i++) {
				values.Add (attributes[i].GetName(), array [i].Trim());
			}

			if (attributes.Count == array.Length)
				target = array.Last();
			else
				target = "";
		}

		public Dictionary<string, string> GetValues() 
		{
			return values;
		}

		public string GetTarget() 
		{
			return target;
		}

		public void SetTarget(string target)
		{
			this.target = target;
		}

		public string GetAttributeValue(string value)
		{
			return values [value];
		}

		public override string ToString ()
		{
			return string.Format ("DataPoint: Values: {0} Target: {1}", 
				String.Join (", ", values.Select (x => x.Value)), target);
		}

		public string ToString(bool hadTarget)
		{
			//string temp = String.Join(",", values.Select(x => x.Value));
			//if (hadTarget)
			//	temp = temp + "," + target;
			//return temp;

			return string.Format("{0}{1}", String.Join(",", values.Select(x => x.Value)),
								 hadTarget ? ","  + target : "");
		}
	}
}

