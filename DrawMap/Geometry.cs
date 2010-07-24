//
//  PinFactory.m
//  DBYD
//
//  Created by rupert on 22/07/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using MonoTouch.CoreGraphics;

namespace MapStuff.DrawMap
{
	public class Geometry : List<PointAnnotation>
	{
		public List<CLLocation> Points
		{
			get 
			{
				var l = new List<CLLocation>();
				foreach (var pa in this)
				{
					l.Add(new CLLocation (pa.Coordinate.Latitude, pa.Coordinate.Longitude));
				}
				return l;
			}
		}
		//public int PointsCount {get {return Points.Count;} }
		
		public GeometryType Type;
		public int State;

		public Geometry (List<PointAnnotation> a)
		{
			this.AddRange(a);
			State = 0;
			Type = GeometryType.Undefined;
		}
	}
}

