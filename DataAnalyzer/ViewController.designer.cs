// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DataAnalyzer
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField ProgressLabel { get; set; }

		[Action ("BrowseButtonForOutput:")]
		partial void BrowseButtonForOutput (Foundation.NSObject sender);

		[Action ("BrowseButtonForTesting:")]
		partial void BrowseButtonForTesting (Foundation.NSObject sender);

		[Action ("BrowseButtonForTraining:")]
		partial void BrowseButtonForTraining (Foundation.NSObject sender);

		[Action ("StartAnalyzingButton:")]
		partial void StartAnalyzingButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ProgressLabel != null) {
				ProgressLabel.Dispose ();
				ProgressLabel = null;
			}
		}
	}
}
