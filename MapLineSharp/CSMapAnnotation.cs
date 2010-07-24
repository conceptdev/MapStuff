//
//  CSMapAnnotation.m
//  mapLines
//
//  Created by Craig on 5/15/09.
//  Copyright 2009 Craig Spitzkoff. All rights reserved.
//
using System;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace MapStuff.MapLineSharp
{
	public enum CSMapAnnotationType
	{
		Start = 0
	,	End = 1
	,	Image = 2
	}
	
	/// <seealso cref="MapStuff.DrawMap.PointAnnotation"/>
	public class CSMapAnnotation : MKAnnotation
	{
		public override CLLocationCoordinate2D Coordinate {get;set;}
		public CSMapAnnotationType AnnotationType;
		string _title;
		public override string Title {
			get {
				return _title;
			}
		}
		public override string Subtitle {
			get {
				if (AnnotationType == CSMapAnnotationType.Start 
				|| AnnotationType == CSMapAnnotationType.End)
					return Coordinate.Longitude + "," + Coordinate.Longitude;
				else 
					return "";
			}
		}
		public CSMapAnnotation (CLLocationCoordinate2D coordinate, CSMapAnnotationType annotationType, string title)
		{
			Coordinate = coordinate;
			AnnotationType = annotationType;
			_title = title;
		}
	}
}

