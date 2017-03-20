using System;
using System.Collections.Generic;
using System.Linq;

namespace decision_tree_learning
{
	public class Pruning
	{
		TreeNode root;
		double z;
		public Pruning (TreeNode root, List<DataPoint> examples, double z = 1.150) //75% sure
		{
			this.root = root;
			this.z = z;
			Prune (root, examples);
		}

		private double Prune(TreeNode curr, List<DataPoint> examples)
		{
			if (curr.GetType().Equals("leaf")) 
			{
				string temp = "";
				return GetUppperEstimateError (curr, examples, ref temp);
			}
				
			Dictionary<string, List<DataPoint>> childrenExamples = new Dictionary<string, List<DataPoint>> ();
			foreach (string childValue in curr.GetChildren().Keys)
				childrenExamples.Add (childValue, new List<DataPoint> ());

			string attributeName = curr.GetAttribute ().GetName ();
			if (curr.GetAttribute ().GetType ().Equals ("discrete")) {
				foreach (DataPoint example in examples) {
					string attributeValue = example.GetValues () [attributeName];
					childrenExamples [attributeValue].Add (example);
				}
			} else {
				string key = curr.GetChildren ().Keys.ToList() [0];
				double threshold = Convert.ToDouble (key.Split (' ') [1]);
				string attributeNameMore = "more " + threshold;
				string attributeNameLess = "less " + threshold;
				foreach (DataPoint example in examples) {
					if (Convert.ToDouble (example.GetAttributeValue (attributeName)) > threshold)
						childrenExamples [attributeNameMore].Add (example);
					else
						childrenExamples [attributeNameLess].Add (example);
				}
			}	

			int childrenCount = curr.GetChildren ().Count;
			double childrenError = 0;
			foreach (var child in curr.GetChildren()) {
				if (childrenExamples[child.Key].Count == 0)
					childrenCount--;
				else
					childrenError += Prune (child.Value, childrenExamples[child.Key]);
			}
			childrenError /= (double)childrenCount;

			string targetToSet = "";
			double currError = GetUppperEstimateError (curr, examples, ref targetToSet);
//			Console.WriteLine ("{0}  {1}  {2}", curr.GetAttribute ().GetName (), currError, childrenError);
			if (currError <= childrenError) {
//				Console.WriteLine ("pruning " + curr.GetAttribute ().GetName());
				curr.SetType ("leaf");
				curr.GetChildren ().Clear ();
				curr.SetTarget (targetToSet);
				return currError;
			} else
				return childrenError;
		}

		private double GetUppperEstimateError(TreeNode curr, List<DataPoint> examples, ref string targetToSet)
		{
			int errorCount;
			int exampleCount = examples.Count;
			Dictionary<string, int> count = new Dictionary<string, int> ();
	
			foreach (DataPoint example in examples) {
				string currTarget = example.GetTarget ();
				if (count.ContainsKey (currTarget))
					count [currTarget]++;
				else
					count.Add (currTarget, 1);
			}
				
			if (curr.GetType ().Equals ("leaf")) {
				string target = curr.GetTarget ();
				if (count.ContainsKey (target))
					errorCount = exampleCount - count [target];
				else
					errorCount = exampleCount;
			} else {
				int accurate = 0;
				foreach (var countPair in count)
					if (accurate < countPair.Value) {
						accurate = countPair.Value;
						targetToSet = countPair.Key;
					}
				errorCount = exampleCount - accurate;
			}
				
			double errorRate = (double)errorCount / (double) exampleCount;
			return errorRate + z * Math.Sqrt (errorRate * (1 - errorRate) / exampleCount);
		}

		public TreeNode GetRoot()
		{
			return root;
		}
	}
}

