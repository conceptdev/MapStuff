using System;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
/*
Discovered here
http://www.gisnotes.com/wordpress/2009/10/iphone-devnote-14-drawing-a-point-line-polygon-on-top-of-mkmapview/
but code sourced from here
http://spitzkoff.com/craig/?p=108
possible 'keep visible' extension
http://www.hanspinckaers.com/routes-on-a-mkmapview-as-an-mkannotationview-–-part-2-5
TODO: check out the new stuff in iOS4
MKOverlay - http://spinningtheweb.blogspot.com/2010/05/ios-4-and-map-kit-overlays-with.html
http://spitzkoff.com/craig/?p=136
http://developer.apple.com/iphone/library/documentation/MapKit/Reference/MKOverlay_protocol/Reference/Reference.html#//apple_ref/doc/uid/TP40009714
*/
namespace MapStuff
{
	public class Application
	{
		static void Main (string[] args)
		{
			try
			{
            	UIApplication.Main (args, null, "AppDelegate");
			}
			catch (Exception ex)
			{	// HACK: this is just here for debugging
				Debug.WriteLine(ex);
			}
		}
	}
}

