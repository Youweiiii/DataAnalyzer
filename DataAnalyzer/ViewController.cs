using System;
using decision_tree_learning;
using AppKit;
using Foundation;
using System.IO;
using System.Collections.Generic;


namespace DataAnalyzer
{
	
	public partial class ViewController : NSViewController
	{
		private AnalyzerModel analyzer;

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

		[Export("Analyzer")]
		public AnalyzerModel Analyzer
		{
			get
			{
				return analyzer;
			}
			set
			{
				WillChangeValue("Analyzer");
				analyzer = value;
				DidChangeValue("Analyzer");
			}
		}

		public override void ViewDidLoad()
		{
			base.AwakeFromNib();

			AnalyzerModel newAnalyzer = new AnalyzerModel();
			string directory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName).FullName;
			newAnalyzer.TrainingFile = directory + "/trainingData.arff";
			newAnalyzer.TestingFile = directory + "/testingData.arff";
			newAnalyzer.OutputFile = directory + "/output.txt";
			Analyzer = newAnalyzer;
		}


		partial void BrowseButtonForTraining(Foundation.NSObject sender)
		{
		}

		partial void BrowseButtonForTesting(Foundation.NSObject sender)
		{
		}

		partial void BrowseButtonForOutput(Foundation.NSObject sender)
		{
		}

		partial void StartAnalyzingButton (Foundation.NSObject sender)
		{
			ProgressLabel.StringValue = "Analyzing...";

			DecisionTreeLearning program = new DecisionTreeLearning(Analyzer.TrainingFile, Analyzer.TestingFile);
			program.Run();

			ProgressLabel.StringValue = "Saving...";

			List<string> predictedTargets = program.GetTestResult();
			SampleSet sampleset = program.GetSampleSet();
			List<DataPoint> testingData = sampleset.GetTestingData();

			using (StreamWriter file =
			       new StreamWriter(@Analyzer.OutputFile))
			{
				file.WriteLine("Test Data:\n");

				foreach (decision_tree_learning.Attribute attribute in sampleset.GetAttributes())
					file.Write("{0},", attribute.GetName());
				bool hadTarget = false;
				if (!testingData[0].GetTarget().Equals(""))
				{
					hadTarget = true;
					file.Write("Original {0},", sampleset.GetTarget().GetName());
				}
				file.WriteLine("Predicted {0}", sampleset.GetTarget().GetName());

				for (int i = 0; i < testingData.Count; i++)
				{
					file.Write(testingData[i].ToString(hadTarget));
					file.WriteLine(",{0}", predictedTargets[i]);
				}
			}

			ProgressLabel.StringValue = "Finished!";

			System.Threading.Thread.Sleep(2000);

			//NSStoryboard board = NSStoryboard.FromName("Main", null);
			//NSWindowController ctrl = (NSWindowController)board.InstantiateControllerWithIdentifier("TestWindow");
			//ctrl.ShowWindow(this);

			PerformSegue("TestSegue", this);

		}

		public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			// Take action based on Segue ID
			switch (segue.Identifier)
			{
				case "TestSegue":
		        // Prepare for the segue to happen
		        
		        break;
			}
		}

	}
}
