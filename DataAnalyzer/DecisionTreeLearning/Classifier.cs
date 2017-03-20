using System;
using System.Linq;
using System.Collections.Generic;

namespace decision_tree_learning
{
	public class Classifier
	{
		TreeNode root;
		SampleSet sampleset;
		TreeMaker treeMaker;
		List<string> targets;
		double accuracy;

		public Classifier (SampleSet sampleset, bool prune = false)
		{
			this.sampleset = sampleset;

			treeMaker = new TreeMaker (sampleset.GetTrainingData(), sampleset.GetAttributes(), sampleset.GetTarget());
			root = treeMaker.GetRoot ();

			if (prune)
				Prune ();
		}

		public void Prune()
		{
			Pruning pruning = new Pruning (root, sampleset.GetPruningData());
			root = pruning.GetRoot ();
			treeMaker.SetRoot (root);
		}

		public double GetAccuracy()
		{
			return accuracy;
		}

		public TreeNode GetRoot()
		{
			return root;
		}

		public void CalculateAccuracy()
		{
			List<DataPoint> examples = sampleset.GetTestingData();
			targets = new List<string>();
			int errorCount = 0;

			foreach (DataPoint example in examples) {
				string target = Predict (example);
				targets.Add(target);
				if (!target.Equals (example.GetTarget ())) {
					errorCount++;
//					Console.WriteLine (example + "  " + target);
				}

				//Console.WriteLine (example + "  " + target);
			}
				
//			Console.WriteLine (errorCount + "     " + examples.Count);

			accuracy = 1 - (double)errorCount / (double)examples.Count;
		}
			
		public string Predict(DataPoint example)
		{
			TreeNode curr = root;
			while (!curr.GetType ().Equals ("leaf")) {
				string attributeValue = "";
				if (example.GetValues () [curr.GetAttribute ().GetName ()].Equals ("")) {
					Random rand = new Random ();
					double num = rand.NextDouble ();
					Dictionary<string, double> distribution = curr.GetDistribution ();
					foreach (string value in distribution.Keys) {
						attributeValue = value;
						num = num - distribution [value];
						if (num < 0)
							break;
					}
//					Console.WriteLine ("missing value in test: " + attributeValue);
				} else {
					if (curr.GetAttribute ().GetType ().Equals ("discrete")) {
						attributeValue = example.GetValues () [curr.GetAttribute ().GetName ()];
					} else {
						string key = curr.GetChildren ().Keys.ToList() [0];
						double threshold = Convert.ToDouble (key.Split (' ') [1]);
						if (Convert.ToDouble (example.GetAttributeValue (curr.GetAttribute ().GetName ())) > threshold)
							attributeValue = "more ";
						else
							attributeValue = "less ";
						attributeValue = attributeValue + threshold;
					}
				}
				curr = curr.GetChildren () [attributeValue];
			}
			return curr.GetTarget ();
		}

		public List<string> GetTestResult()
		{
			return targets;
		}
	}
}
	