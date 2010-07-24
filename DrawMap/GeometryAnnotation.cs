//
//  
//	Part of this code is created by the following:
//  Created by Craig on 8/18/09.
//  Copyright Craig Spitzkoff 2009. All rights reserved.
//
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
	/// <seealso cref="MapStuff.MapLineSharp.CSRouteAnnotation"/>
	public class GeometryAnnotation  : MKAnnotation
	{
		MKCoordinateSpan _span;

		public GeometryType Type;

		public override CLLocationCoordinate2D Coordinate {get;set;}

		public MKCoordinateRegion Region
		{
			get
			{
				MKCoordinateRegion region = new MKCoordinateRegion();
				region.Center = Coordinate;
				region.Span = _span;
				return region;
			}
		}
		public UIColor LineColor {get;set;}
		public List<CLLocation> Points {get;set;}
		public string RouteId {get;set;}

		//public GeometryAnnotation (List<CLLocation> points, GeometryType @type)
		public GeometryAnnotation (List<CLLocation> points, GeometryType @type)
		{
			Type = @type;
			Points = points;

			// create a unique ID for this route so it can be added to dictionaries by this key. 
			RouteId = "";
			
			// determine a logical center point for this route based on the middle of the lat/lon extents.
			double maxLat = -91;
			double minLat =  91;
			double maxLon = -181;
			double minLon =  181;
			
			foreach (CLLocation currentLocation in Points)
			{
				CLLocationCoordinate2D coordinate = currentLocation.Coordinate;
				
				if(coordinate.Latitude > maxLat)
					maxLat = coordinate.Latitude;
				if(coordinate.Latitude < minLat)
					minLat = coordinate.Latitude;
				if(coordinate.Longitude > maxLon)
					maxLon = coordinate.Longitude;
				if(coordinate.Longitude < minLon)
					minLon = coordinate.Longitude; 
			}	
		
			_span.LatitudeDelta = (maxLat + 90) - (minLat + 90);
			_span.LongitudeDelta = (maxLon + 180) - (minLon + 180);

			CLLocationCoordinate2D _center = new CLLocationCoordinate2D();
			_center.Latitude = minLat + _span.LatitudeDelta / 2;
			_center.Longitude = minLon + _span.LongitudeDelta / 2;
			Coordinate = _center;

			LineColor = UIColor.Blue;
			System.Diagnostics.Debug.WriteLine("Found center of new Route Annotation at {0},{1}", Coordinate.Latitude, Coordinate.Longitude);			
		}
	}
}

