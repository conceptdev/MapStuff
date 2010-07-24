//
//  DrawMapAppDelegate.m
//  DrawMap
//
//  Created by rupert on 8/09/09.
//  Copyright __MyCompanyName__ 2009. All rights reserved.
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
/*	State represents state of the map
 *	0 = map
 *	1 = point
 *	2 = line
 *	3 = polygon
 */
	public class DrawGeometryMapViewController : UIViewController
	{
		UIToolbar toolbar;
		UIBarButtonItem mapBarButton, pointBarButton, lineBarButton, polygonBarButton;
		Geometry geometry;
		GeometryTouchView geometryTouchView;
		MKMapView mapView;

		LinePolygonAnnotationView currentAnnotationView;

		public DrawGeometryMapViewController ()
		{
			Title = "Map";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();
		
			float toolbarHeight = 40;
			float width = View.Bounds.Width;
			float height = View.Bounds.Height - toolbarHeight - 52;

			currentAnnotationView = null;

			mapView = new MKMapView ();
			mapView.Frame = new RectangleF (0,0,width,height);
			mapView.MapType = MKMapType.Satellite;
			mapView.Delegate = new MapViewDelegate(this);

			var points = new List<PointAnnotation>();
			geometry = new Geometry(points);

			//initialize geometryMapView which will catch touch Events
			geometryTouchView = new GeometryTouchView(new RectangleF(0,0,width,height));
			geometryTouchView.BackgroundColor = UIColor.Clear;
			geometryTouchView.MapView = mapView;
			geometryTouchView.geometry = geometry;

			View.AddSubview(mapView);
			View.AddSubview(geometryTouchView);

			mapBarButton = new UIBarButtonItem("Move Map", UIBarButtonItemStyle.Bordered, MapBarButtonPressed);
			pointBarButton = new UIBarButtonItem("Point", UIBarButtonItemStyle.Bordered, PointBarButtonPressed);
			lineBarButton = new UIBarButtonItem("Line", UIBarButtonItemStyle.Bordered, LineBarButtonPressed);
			polygonBarButton = new UIBarButtonItem("Polygon", UIBarButtonItemStyle.Bordered, PolygonBarButtonPressed);

			toolbar = new UIToolbar (new RectangleF(0,height,width,toolbarHeight));
			toolbar.Items = new UIBarButtonItem[] {mapBarButton, pointBarButton, lineBarButton, polygonBarButton};

			View.AddSubview(toolbar);

			MapBarButtonPressed(null,null);
		}
	
		void MapBarButtonPressed (object sender, EventArgs args)
		{
			Debug.WriteLine("MapBarButtonPressed");
	
			pointBarButton.Enabled = true;
			lineBarButton.Enabled = true;
			polygonBarButton.Enabled = true;
			
			mapBarButton.Style = UIBarButtonItemStyle.Bordered;
			geometryTouchView.Hidden = true;
		}
		void ShowGeometryTouchViewForGeometry ()
		{
			if (geometry.Count > 0)
			{
				Debug.WriteLine("");
				var annotations = mapView.Annotations;
				foreach (var a in annotations)
					mapView.RemoveAnnotation (a as MKAnnotation);
				geometry.Clear();
			}
			pointBarButton.Enabled = true;
			lineBarButton.Enabled = true;
			polygonBarButton.Enabled = true;
			
			mapBarButton.Style = UIBarButtonItemStyle.Done;
			geometryTouchView.Hidden = false;
		}
		void PointBarButtonPressed(object sender, EventArgs args)
		{
			Debug.WriteLine("PointBarButtonPressed");
			ShowGeometryTouchViewForGeometry();
			geometry.Type = GeometryType.Point;
			pointBarButton.Enabled = false;
		}
		void LineBarButtonPressed(object sender, EventArgs args)
		{
			Debug.WriteLine("LineBarButtonPressed");
			ShowGeometryTouchViewForGeometry();
			geometry.Type = GeometryType.Line;
			lineBarButton.Enabled = false;
		}
		void PolygonBarButtonPressed(object sender, EventArgs args)
		{
			Debug.WriteLine("PolygonBarButtonPressed");
			ShowGeometryTouchViewForGeometry();
			geometry.Type = GeometryType.Polygon;
			polygonBarButton.Enabled = false;
		}


		class MapViewDelegate : MKMapViewDelegate
		{
			DrawGeometryMapViewController _viewController;
			public MapViewDelegate (DrawGeometryMapViewController viewController)
			{
				_viewController = viewController;
			}

			public override void RegionWillChange (MKMapView mapView, bool animated)
			{
				if (_viewController.currentAnnotationView != null)
				{
					_viewController.currentAnnotationView.Hidden = true;
				}
			}
			public override void RegionChanged (MKMapView mapView, bool animated)
			{
				if (_viewController.currentAnnotationView != null)
				{
					_viewController.currentAnnotationView.Hidden = false;
					_viewController.currentAnnotationView.RegionChanged();
				}
			}
			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
			{
				MKAnnotationView annotationView = null;
				if (annotation is PointAnnotation)
				{
					Debug.WriteLine("it's a pin class");
				}
				else if (annotation is GeometryAnnotation)
				{
					Debug.WriteLine("it's a line class");
					
					GeometryAnnotation geometryAnnotation = annotation as GeometryAnnotation;

					LinePolygonAnnotationView _annotationView = new LinePolygonAnnotationView(
						new RectangleF(0,0,mapView.Frame.Size.Width, mapView.Frame.Size.Height));
					_annotationView.Annotation = geometryAnnotation;
					_annotationView.MapView = mapView;

					_viewController.currentAnnotationView = _annotationView;
					annotationView = _annotationView;
				}
				return annotationView;
			}
		}
	}
}

