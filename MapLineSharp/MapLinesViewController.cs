//
//  MapLinesViewController.m
//  mapLines
//
//  Created by Craig on 5/15/09.
//  Copyright 2009 Craig Spitzkoff. All rights reserved.
//
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace MapStuff.MapLineSharp
{
	public class MapLinesViewController : UIViewController
	{
		MKMapView _mapView;
		public Dictionary<string,CSRouteView> RouteViews;
		public MapLinesViewController ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		
			RouteViews = new Dictionary<string,CSRouteView>();

			// load the points from local resource (file)
			var filePath = NSBundle.MainBundle.PathForResource("route", "csv", "MapLineSharp","");
			var fileContents = System.IO.File.ReadAllText(filePath);
			var pointStrings = fileContents.Split('\n');
			
			var points = new List<CLLocation>();

			foreach (var ps in pointStrings)
			{
				// break the string down into latitude and longitude fields
				var latLonArr = ps.Split(',');
				double latitude = Convert.ToDouble(latLonArr[0]);
				double longitude = Convert.ToDouble(latLonArr[1]);
				CLLocation currentLocation = new CLLocation(latitude, longitude);
				points.Add(currentLocation);
			}
			//
			// Create our map view and add it as as subview. 
			//
			_mapView = new MKMapView();
			_mapView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			View.AddSubview(_mapView);
			_mapView.Delegate = new MapViewDelegate(this);


			// CREATE THE ANNOTATIONS AND ADD THEM TO THE MAP

			// first create the route annotation
			CSRouteAnnotation routeAnnotation = new CSRouteAnnotation(points);
			_mapView.AddAnnotation(routeAnnotation);

			CSMapAnnotation annotation = null;

			annotation = new CSMapAnnotation (points[0].Coordinate, CSMapAnnotationType.Start, "Start Point");
			_mapView.AddAnnotation (annotation);

			annotation = new CSMapAnnotation (points[points.Count - 1].Coordinate, CSMapAnnotationType.End, "End Point");
			_mapView.AddAnnotation (annotation);

			//TODO:create the image annotation

			_mapView.SetRegion (routeAnnotation.Region, false);

		}


		class MapViewDelegate : MKMapViewDelegate
		{	
			MapLinesViewController _viewController;
			public MapViewDelegate (MapLinesViewController viewController)
			{
				_viewController = viewController;
			}
			public override void RegionWillChange (MKMapView mapView, bool animated)
			{
				foreach (var rv in _viewController.RouteViews.Values)
				{
					rv.Hidden = true;
				}
			}
			public override void RegionChanged (MKMapView mapView, bool animated)
			{
				foreach (var rv in _viewController.RouteViews.Values)
				{
					rv.Hidden = false;
					rv.RegionChanged();
				}
			}
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
			{
				MKAnnotationView annotationView = null;

				if (annotation is CSMapAnnotation)
				{
					var csAnnotation = (CSMapAnnotation)annotation;
					if (csAnnotation.AnnotationType == CSMapAnnotationType.Start
					|| csAnnotation.AnnotationType == CSMapAnnotationType.End)
					{
						var identifier = "pin";
						MKPinAnnotationView pin = (MKPinAnnotationView)mapView.DequeueReusableAnnotation(identifier);
						if (pin == null)
						{
							pin = new MKPinAnnotationView(csAnnotation, identifier);
						}
						pin.PinColor = (csAnnotation.AnnotationType == CSMapAnnotationType.End) ? MKPinAnnotationColor.Red : MKPinAnnotationColor.Green;
						annotationView = pin;
					}
					else if (csAnnotation.AnnotationType == CSMapAnnotationType.Image)
					{
						// TODO:
					}
					annotationView.Enabled = true;
					annotationView.CanShowCallout = true;
				}
				else if (annotation is CSRouteAnnotation)
				{
					var routeAnnotation = (CSRouteAnnotation)annotation;

					if (_viewController.RouteViews.ContainsKey(routeAnnotation.RouteId))
						annotationView = _viewController.RouteViews[routeAnnotation.RouteId];

					if (annotationView == null)
					{
						var routeView = new CSRouteView(new RectangleF (0,0, mapView.Frame.Size.Width, mapView.Frame.Size.Height));
						routeView.Annotation = routeAnnotation;
						routeView.MapView = mapView;
						_viewController.RouteViews.Add(routeAnnotation.RouteId, routeView);
						annotationView = routeView;
					}
				}
				return annotationView;
			}
			public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
			{
				Debug.WriteLine ("CalloutAccessoryControlTapped");
			}
		}
	}
}

