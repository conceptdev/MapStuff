//
//  Pin.m
//  DBYD
//
//  Created by rupert on 20/07/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//
//  If this implementation is to be modified, then it is better to subclass it.
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace MapStuff.DrawMap
{
	/// <seealso cref="MapStuff.MapLineSharp.CSMapAnnotation"/>
	public class PointAnnotation : MKAnnotation
	{
		string _title, _subtitle;
		public PointAnnotation (CLLocationCoordinate2D c) : base()
		{
			_title="";
			_subtitle="";
			Coordinate = c;
			Debug.WriteLine("Newly created pin at {0},{1}", c.Latitude, c.Longitude);
		}
		public override CLLocationCoordinate2D Coordinate {get;set;}
		public override string Title {
			get {
				return _title;
			}
		}
		public override string Subtitle {
			get {
				return _subtitle;
			}
		}
	}
}

