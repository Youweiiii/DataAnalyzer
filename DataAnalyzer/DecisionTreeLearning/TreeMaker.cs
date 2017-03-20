using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace decision_tree_learning
{
	public class TreeMaker
	{
		TreeNode root = null;
		public TreeMaker (List<DataPoint> examples, List<Attribute> attributes, Attribute target)
		{
			Dictionary<Attribute, int> attributeSet = new Dictionary<Attribute, int> ();
			int max = 0;
			foreach (Attribute attribute in attributes) {
				int num = attribute.GetValues ().Count;
				if (num > max)
					max = num;
			}
			if (max == 0)
				max = target.GetValues ().Count;
			else
				max = (max + target.GetValues ().Count) / 2;
			
			foreach (Attribute attribute in attributes) {
				int num = attribute.GetValues ().Count;
				if (attribute.GetType ().Equals ("discrete"))
					attributeSet.Add (attribute, 1);
				else
					attributeSet.Add (attribute, max);
			}

			root = MakeTree (examples, attributeSet);
		}

		public TreeNode GetRoot()
		{
			return root;
		}

		private TreeNode MakeTree (List<DataPoint> examples, Dictionary<Attribute, int> attributeSet)
		{
			double initialEntropy = Entropy.EntropyOfData (examples);

			if (initialEntropy == 0 || attributeSet.Count == 0) 
			{
				string leaf;
				if (initialEntropy == 0) 
					leaf = examples[0].GetTarget();
				else
					leaf = GetMajorityTarget(examples);
				TreeNode leafNode = new TreeNode(leaf);
				return leafNode;
			}
			
			int exampleCount = examples.Count;
			double bestGainRatio = 0;
			Attribute bestSplitAttribute = null;
			Dictionary<string, List<DataPoint>> bestSets = null;

			foreach (Attribute attribute in attributeSet.Keys) {
				Dictionary<string, List<DataPoint>> sets;
				if (attribute.GetType ().Equals ("continuous"))
					sets = SplitByContinuousAttribute (examples, attribute);
				else
					sets = SplitByAttribute (examples, attribute);

				double gainRatio = Entropy.GetGainRatio (sets, exampleCount, initialEntropy);
				if (gainRatio > bestGainRatio) {
					bestGainRatio = gainRatio;
					bestSplitAttribute = attribute;
					bestSets = sets;
				}
			}

			if (bestGainRatio == 0) {
				TreeNode leafNode = new TreeNode (GetMajorityTarget (examples));
				return leafNode;
			}

			TreeNode currNode = new TreeNode (bestSplitAttribute);
			if (attributeSet [bestSplitAttribute] == 1)
				attributeSet.Remove (bestSplitAttribute);
			else
				attributeSet [bestSplitAttribute]--;
				
			foreach (string attributeValue in bestSets.Keys) {
				List<DataPoint> subExamples = bestSets [attributeValue];
				if (subExamples.Count == 0) {
					TreeNode leafNode = new TreeNode (GetMajorityTarget (examples));
					currNode.AddChild (attributeValue, leafNode);
				} else {
					TreeNode childNode = MakeTree (subExamples, attributeSet);
					currNode.AddChild (attributeValue, childNode);
				}
			}

			if (attributeSet.ContainsKey (bestSplitAttribute))
				attributeSet [bestSplitAttribute]++;
			else 
				attributeSet.Add (bestSplitAttribute, 1);
			
			return currNode;
		}

		private Dictionary<string, List<DataPoint>> SplitByAttribute(List<DataPoint> examples, Attribute attribute)
		{
			Dictionary<string, List<DataPoint>> sets = new Dictionary<string, List<DataPoint>> ();
			List<string> values = attribute.GetValues ();
			foreach (string value in values) {
				sets.Add(value, new List<DataPoint>());
			}
				
			string attributeName = attribute.GetName ();
			foreach (DataPoint example in examples) {
				string value = example.GetAttributeValue(attributeName);
				sets [value].Add (example);
			}
			return sets;
		}

		public class ReverseComparer: IComparer<DataPoint>
		{
			Attribute attribute;
			public ReverseComparer(Attribute attribute)
			{
				this.attribute = attribute;
			}

			public int Compare(DataPoint x, DataPoint y)
			{
				double xValue = Convert.ToDouble (x.GetAttributeValue (attribute.GetName ()));
				double yValue = Convert.ToDouble (y.GetAttributeValue (attribute.GetName ()));
				if (xValue == yValue)
					return 0;
				else if (xValue > yValue)
					return 1;
				else
					return -1;
			}
		}

		private Dictionary<string, List<DataPoint>> SplitByContinuousAttribute(List<DataPoint> examples, Attribute attribute)
		{
			double bestGain = 0;
			List<DataPoint> bestSetA = null;
			List<DataPoint> bestSetB = null;
			List<DataPoint> setA = new List<DataPoint> ();
			List<DataPoint> setB = examples.ToList();
			ReverseComparer comparer = new ReverseComparer (attribute);
			setB.Sort (comparer);
			int exampleCount = setB.Count;
			double initialEntropy = Entropy.EntropyOfData (setB);
			Dictionary<string, List<DataPoint>> sets = new Dictionary<string, List<DataPoint>> ();
			sets.Add ("a", null);
			sets.Add ("b", null);

			while (setB.Count > 0) {
				string temp;
				do {
					DataPoint example = setB [0];
					setB.RemoveAt (0);
					setA.Add (example);
					temp = example.GetAttributeValue(attribute.GetName());
				} while (setB.Count > 0 && setB [0].GetAttributeValue (attribute.GetName ()).Equals(temp));
				sets["a"] = setA.ToList ();
				sets["b"] = setB.ToList ();
				double overallEntropy = Entropy.EntropyOfSets (sets, exampleCount);
				double informationGain = initialEntropy - overallEntropy;
				if (bestGain < informationGain) {
					bestGain = informationGain;
					bestSetA = sets ["a"];
					bestSetB = sets ["b"];
				}
			}

			Dictionary<string, List<DataPoint>> bestSets = new Dictionary<string, List<DataPoint>> ();
			if (bestGain == 0) {
				bestSets.Add (examples [0].GetAttributeValue (attribute.GetName ()), examples);
				return bestSets;
			}
				
			double threshold = Convert.ToDouble(bestSetA[bestSetA.Count-1].GetAttributeValue(attribute.GetName()));
			threshold += Convert.ToDouble(bestSetB[0].GetAttributeValue(attribute.GetName()));
			threshold /= 2;
			bestSets.Add ("less " + threshold, bestSetA);
			bestSets.Add ("more " + threshold, bestSetB);

			return bestSets;
		}

		private string GetMajorityTarget(List<DataPoint> examples) 
		{
			string majorTarget = examples [0].GetTarget ();
			Dictionary<string, int> count = new Dictionary<string, int> ();
			foreach (DataPoint example in examples) {
				if (!count.ContainsKey (example.GetTarget ()))
					count.Add (example.GetTarget (), 1);
				else {
					count [example.GetTarget ()]++;
					if (count [example.GetTarget ()] > count [majorTarget])
						majorTarget = example.GetTarget ();
				}
			}
			return majorTarget;
		}

		public override string ToString ()
		{
			List<string> list = new List<string> ();
			printTree (root, new StringBuilder (), list);
			return string.Join ("\n", list);
		}

		private void printTree(TreeNode root, StringBuilder sb, List<string> list)
		{
			if (root.GetType ().Equals("leaf")) {
				StringBuilder curr = new StringBuilder (sb.ToString());
				curr.Append (" => ");
				curr.Append (root.GetTarget ());
				list.Add (curr.ToString ());
			} else {
				sb.Append (root.GetAttribute ().GetName ());
				foreach (var child in root.GetChildren()) {
					StringBuilder curr = new StringBuilder (sb.ToString());
					curr.Append ("(");
					curr.Append (child.Key);
					curr.Append (")");
					printTree (child.Value, curr, list);
				}
			}
		}

		public void SetRoot(TreeNode root)
		{
			this.root = root;
		}
	}
}

