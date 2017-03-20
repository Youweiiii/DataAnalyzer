using System;
using System.Collections.Generic;

namespace decision_tree_learning
{
	public class DecisionTreeLearning
	{
		double accuracy;
		string trainingFile;
		string testingFile;
		bool isPruning = true;
		TreeNode root;
		SampleSet sampleset;
		List<string> testResult;

		public DecisionTreeLearning(string trainingFile, string testingFile)
		{
			this.trainingFile = trainingFile;
			this.testingFile = testingFile;
		}

		public void Run()
		{
			sampleset = new SampleSet();
			FileReader reader = new	FileReader();
			reader.ReadFile(trainingFile);
			sampleset.SetTrainingData(reader.GetData());
			reader.ReadFile(testingFile, true);
			sampleset.SetTestingData(reader.GetData());
			List<Attribute> attributes = reader.GetAttributes();
			sampleset.SetTarget(attributes[attributes.Count - 1]);
			attributes.RemoveAt(attributes.Count - 1);
			sampleset.SetAttributes(attributes);
			if (isPruning)
				sampleset.SplitData();
			
			Classifier classifier = new Classifier(sampleset, isPruning);
			classifier.CalculateAccuracy();
			accuracy = classifier.GetAccuracy();
			root = classifier.GetRoot();
			testResult = classifier.GetTestResult();
		}

		public List<string> GetTestResult()
		{
			return testResult;
		}

		public double GetAccuracy()
		{
			return accuracy;
		}

		public TreeNode GetRoot()
		{
			return root;
		}

		public SampleSet GetSampleSet()
		{
			return sampleset;
		}


	}
}

