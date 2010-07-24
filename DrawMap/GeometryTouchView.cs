//
//  GeometryMapView.m
//  DrawMap
//
//  Created by rupert on 30/09/09.
//  Copyright 2009 __MyCompanyName__. All rights reserved.
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
	public class GeometryTouchView : UIView
	{
		public MKMapView MapView;
		public Geometry geometry;
		List<GeometryAnnotation> annotationViewsArray;

		public GeometryTouchView (RectangleF frame) : base (frame)
		{
			MultipleTouchEnabled = true;
			annotationViewsArray = new List<GeometryAnnotation>();
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			UITouch aTouch = (UITouch)touches.AnyObject;
			PointF location = aTouch.LocationInView(this);
			Debug.WriteLine("TouchesBegan {0},{1}", location.X, location.Y);

			var coordinate = MapView.ConvertPoint (location, this);

			switch (geometry.Type)
			{
				case GeometryType.Point:
					if (geometry.Count == 1)
					{
						var point = geometry[0];
						MapView.RemoveAnnotation(point);
						geometry.Remove(geometry[0]);
						
						var newPoint = new PointAnnotation(coordinate);
						geometry.Add(newPoint);
						MapView.AddAnnotation(newPoint);
					}
					else 
					{
						var point = new PointAnnotation(coordinate);
						geometry.Add(point);
						MapView.AddAnnotation(point);		
					}
					break;
				case GeometryType.Line:
					var _point = new PointAnnotation (coordinate);
					geometry.Add(_point);

					foreach (var a in annotationViewsArray)
						MapView.RemoveAnnotation(a as MKAnnotation);
					annotationViewsArray.Clear();
					
					if (geometry.Count >= 2)
					{
						var _lineAnnotation = new GeometryAnnotation(geometry.Points, geometry.Type);
						MapView.AddAnnotation(_lineAnnotation);
						annotationViewsArray.Add(_lineAnnotation);
					}
					MapView.AddAnnotation(_point);
					break;
				case GeometryType.Polygon:
					var __point = new PointAnnotation (coordinate);
					geometry.Add(__point);

					foreach (var a in annotationViewsArray)
						MapView.RemoveAnnotation(a as MKAnnotation);
					annotationViewsArray.Clear();
					
					if (geometry.Count == 2)
					{
						var _lineAnnotation = new GeometryAnnotation(geometry.Points, geometry.Type);
						MapView.AddAnnotation(_lineAnnotation);
						annotationViewsArray.Add(_lineAnnotation);
					}
					else
					{
						var _polygonAnnotation = new GeometryAnnotation(geometry.Points, geometry.Type);
						MapView.AddAnnotation(_polygonAnnotation);
						annotationViewsArray.Add(_polygonAnnotation);
					}
					MapView.AddAnnotation(__point);
					break;
				default:
					break;
			}
		}
	}
}

