// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using decision_tree_learning;

namespace DataAnalyzer
{
	public partial class OutputViewController : NSViewController
	{
		private NSMutableArray data = new NSMutableArray();

		public OutputViewController (IntPtr handle) : base (handle)
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

		public override void ViewDidLoad()
		{
			base.AwakeFromNib();
		}

		//[Export("testingData")]
		//public NSArray Data
		//{
		//	get
		//	{
		//		return data;
		//	}
		//}

		//[Export("addDataPoint:")]
		//public void AddDataPoint(DataPoint example)
		//{
		//	WillChangeValue("testingData");
		//	data.Add(example);
		//	DidChangeValue("testingData");
		//}
	}
}