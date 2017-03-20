using System;
using System.Collections.Generic;

namespace decision_tree_learning
{
	public class Entropy
	{
		public static double EntropyOfData(List<DataPoint> examples)
		{
			int exampleCount = examples.Count;
			if (exampleCount == 0)
				return 0;

			Dictionary<string, int> actionTallies = new Dictionary<string, int> ();
			foreach (DataPoint example in examples) {
				string target = example.GetTarget ();
				if (!actionTallies.ContainsKey (target))
					actionTallies.Add (target, 0);
				actionTallies [target]++;

			}

			return CalculateEntropy (actionTallies, exampleCount);
		}

		private static double CalculateEntropy (Dictionary<string, int> actionTallies, int exampleCount)
		{
			if (actionTallies.Count == 1)
				return 0;

			double entropy = 0;
			foreach (string target in actionTallies.Keys) {
				int count = actionTallies[target];
				double proportion = (double)count / (double)exampleCount;
				entropy -= proportion * Math.Log (proportion, 2);
			}
			return entropy;
		}

		public static double EntropyOfSets(Dictionary<string, List<DataPoint>> sets, int exampleCount)
		{
			double entropy = 0;
			foreach (var set in sets) {
				double proportion = (double)set.Value.Count / (double)exampleCount;
				entropy += proportion * EntropyOfData (set.Value);
			}

			return entropy;
		}

		public static double GetGainRatio(Dictionary<string, List<DataPoint>> sets, int exampleCount, double initialEntropy)
		{
			double intrinsicValue = 0;
			foreach (var set in sets) {
				double proportion = (double)set.Value.Count / (double)exampleCount;
				initialEntropy -= proportion * EntropyOfData (set.Value);
				intrinsicValue -= proportion * Math.Log (proportion, 2);
			}


			return initialEntropy / intrinsicValue;
		}

	}
}

