#include <xamarin/xamarin.h>
#import <AppKit/NSAlert.h>
#import <Foundation/NSDate.h>



extern "C" int xammac_setup ()
{
	xamarin_marshal_objectivec_exception_mode = MarshalObjectiveCExceptionModeDisable;

	return 0;
}

