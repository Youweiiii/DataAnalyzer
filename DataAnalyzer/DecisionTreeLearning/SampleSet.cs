using System;
using System.Collections.Generic;
using System.Linq;

namespace decision_tree_learning
{
	public class SampleSet
	{
		List<DataPoint> trainingData;
		List<DataPoint> testingData;
		List<DataPoint> pruningData;
		List<Attribute> attributes;
		Attribute target;

		public SampleSet ()
		{
			trainingData = new List<DataPoint> (); 
			attributes = new List<Attribute> ();
		}

		public void SetTrainingData(List<DataPoint> data)
		{
			this.trainingData = data;
		}

		public void SetTestingData(List<DataPoint> data)
		{
			this.testingData = data;
		}

		public List<Attribute> GetAttributes()
		{
			return attributes;
		}

		public void SetAttributes(List<Attribute> attributes)
		{
			this.attributes = attributes;
		}

		public void SetTarget(Attribute attribute)
		{
			this.target = attribute;
		}

		public Attribute GetTarget()
		{
			return target;
		}

		public void AddAttribute(Attribute attribute)
		{
			attributes.Add (attribute);
		}

		public void RemoveAttribute(int i)
		{
			attributes.RemoveAt (i);
		}

		public void Shuffle()
		{
			List<DataPoint> newData = new List<DataPoint> ();

			Random r = new Random();
			int randomIndex = 0;
			while (trainingData.Count > 0)
			{
				randomIndex = r.Next(0, trainingData.Count); 
				newData.Add(trainingData[randomIndex]);
				trainingData.RemoveAt(randomIndex);
			}

			trainingData = newData;
		}

		public void SplitData(int trainPercent = 70, int prunePercent = 30)
		{
			Shuffle();

			prunePercent = 100 - trainPercent;
			pruningData = trainingData.GetRange(0, trainingData.Count * prunePercent / 100);
			trainingData = trainingData.GetRange(pruningData.Count, trainingData.Count - pruningData.Count);
		}

		public List<DataPoint> GetTrainingData()
		{
			return trainingData;
		}

		public List<DataPoint> GetTestingData()
		{
			return testingData;
		}

		public List<DataPoint> GetPruningData()
		{
			return pruningData;
		}

		public override string ToString ()
		{
			string[] attributeNames = (from attribute in attributes
				select attribute.ToString ()).ToArray ();
			string[] training = (from example in trainingData 
				select example.ToString ()).ToArray ();
			string[] testing = (from example in testingData
								 select example.ToString()).ToArray();

			return string.Format ("Attribtues:\n{0} \nTarget:\n{1} \nTrainingData\n{2}\nTestingData\n{3}\n", 
				String.Join ("\n", attributeNames), target,
			    String.Join ("\n", training), String.Join("\n", testing));
		}
			
	}
}
	