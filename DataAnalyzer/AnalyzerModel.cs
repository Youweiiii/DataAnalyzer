using System;
using AppKit;
using Foundation;

namespace DataAnalyzer
{
	[Register("AnalyzerModel")]
	public class AnalyzerModel : NSObject
	{
		private string trainingFile = "";
		private string outputFile = "";
		private string testingFile = "";
		private bool displayTree;
		private bool displayAccuracy;
		private bool displayTestData;
		private bool displayWrongrPrediction;

		public AnalyzerModel()
		{
		}

		[Export("trainingFile")]
		public string TrainingFile
		{
			get
			{
				return trainingFile;
			}
			set
			{
				WillChangeValue("trainingFile");
				trainingFile = value;
				DidChangeValue("trainingFile");
			}
		}

		[Export("testingFile")]
		public string TestingFile
		{
			get
			{
				return testingFile;
			}
			set
			{
				WillChangeValue("testingFile");
				testingFile = value;
				DidChangeValue("testingFile");
			}
		}

		[Export("outputFile")]
		public string OutputFile
		{
			get
			{
				return outputFile;
			}
			set
			{
				WillChangeValue("outputFile");
				outputFile = value;
				DidChangeValue("outputFile");
			}
		}

		[Export("displayTree")]
		public bool DisplayTree
		{
			get
			{
				return displayTree;
			}
			set
			{
				WillChangeValue("displayTree");
				displayTree = value;
				DidChangeValue("displayTree");
			}
		}

		[Export("displayWrongrPrediction")]
		public bool DisplayWrongrPrediction
		{
			get
			{
				return displayWrongrPrediction;
			}
			set
			{
				WillChangeValue("displayWrongrPrediction");
				displayWrongrPrediction = value;
				DidChangeValue("displayWrongrPrediction");
			}
		}

		[Export("displayTestData")]
		public bool DisplayTestData
		{
			get
			{
				return displayTestData;
			}
			set
			{
				WillChangeValue("displayTestData");
				displayTestData = value;
				DidChangeValue("displayTestData");
			}
		}

		[Export("displayAccuracy")]
		public bool DisplayAccuracy
		{
			get
			{
				return DisplayAccuracy;
			}
			set
			{
				WillChangeValue("displayAccuracy");
				displayAccuracy = value;
				DidChangeValue("displayAccuracy");
			}
		}
	}
}
