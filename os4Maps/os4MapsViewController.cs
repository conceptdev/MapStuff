using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace MapStuff.os4Maps
{
	public class os4MapsViewController : UIViewController
	{
		MKMapView _mapView;
		public MKPolyline RouteLine;
		public MKPolylineView RouteLineView;
		//MKMapRect _routeRect;
		public MKCoordinateRegion RouteRegion;

		public os4MapsViewController () : base()
		{}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//
			// Create our map view and add it as as subview. 
			//
			_mapView = new MKMapView();
			_mapView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			View.AddSubview(_mapView);
			_mapView.Delegate = new MapViewDelegate(this);

			LoadRoute();

			if (RouteLine != null)
			{
				_mapView.AddOverlay(RouteLine);
			}
			ZoomInOnRoute();
		}

		void LoadRoute ()
		{
			var filePath = NSBundle.MainBundle.PathForResource("route", "csv", "os4Maps","");
			var fileContents = System.IO.File.ReadAllText(filePath);
			var pointStrings = fileContents.Split('\n');		

			MKMapPoint northEastPoint = new MKMapPoint(), southWestPoint = new MKMapPoint();
			var pointArr = new List<MKMapPoint>();
			var coordArr = new List<CLLocationCoordinate2D>();


			// start with the widest possible viewport
			var tl = new CLLocationCoordinate2D (-90, 180);
			// top left
			var br = new CLLocationCoordinate2D (90, -180);
			foreach (var s in pointStrings)
			{
				string[] latlonArr = s.Split(',');
				double latitude = Convert.ToDouble(latlonArr[0]);
				double longitude = Convert.ToDouble(latlonArr[1]);

				CLLocationCoordinate2D coordinate = new CLLocationCoordinate2D (latitude, longitude);
							
				// narrow the viewport bit-by-bit
				tl.Longitude = Math.Min (tl.Longitude, coordinate.Longitude);
				tl.Latitude = Math.Max (tl.Latitude, coordinate.Latitude);
				br.Longitude = Math.Max (br.Longitude, coordinate.Longitude);
				br.Latitude = Math.Min (br.Latitude, coordinate.Latitude);
				
				coordArr.Add(coordinate);
				
				// Can't find MonoTouch **MKMapPointForCoordinate**
//				MKMapPoint point = ???????? (coordinate);
//				if (northEastPoint.X == 0 & northEastPoint.Y == 0)
//				{
//					Debug.WriteLine("First Point: {0},{1}",point.X, point.Y);
//					northEastPoint = point;
//					southWestPoint = point;
//				}
//				else
//				{
//					if (point.X > northEastPoint.X)
//						northEastPoint.X = point.X;
//					if(point.Y > northEastPoint.Y)
//						northEastPoint.Y = point.Y;
//					if (point.X < southWestPoint.X) 
//						southWestPoint.X = point.X;
//					if (point.Y < southWestPoint.Y) 
//						southWestPoint.Y = point.Y;	
//				}
//				pointArr.Add(point);
			}
			// Can't use MKMapRect without the **MKMapPointForCoordinate**
			//_routeRect = new MKMapRect (southWestPoint.X, southWestPoint.Y, northEastPoint.X - southWestPoint.X, northEastPoint.Y - southWestPoint.Y);
			//Debug.WriteLine("RouteRect: {0},{1};{2},{3}", _routeRect.Origin.X, _routeRect.Origin.Y, _routeRect.Size.Width, _routeRect.Size.Height);

			CLLocationCoordinate2D[] clar = coordArr.ToArray();
			RouteLine = MKPolyline.FromCoordinates (clar);
			// divide the range by two to get the center
			var center = new CLLocationCoordinate2D { Latitude = tl.Latitude - (tl.Latitude - br.Latitude) * 0.5, Longitude = tl.Longitude + (br.Longitude - tl.Longitude) * 0.5 };
			// calculate the span, with 20% margin so pins aren't on the edge
			var span = new MKCoordinateSpan { LatitudeDelta = Math.Abs (tl.Latitude - br.Latitude) * 1.2, LongitudeDelta = Math.Abs (br.Longitude - tl.Longitude) * 1.2 };
			RouteRegion = new MKCoordinateRegion { Center = center, Span = span };
		}

		void ZoomInOnRoute()
		{
			//_mapView.SetCenterCoordinate (new CLLocationCoordinate2D(_routeRect.Origin.X, _routeRect.Origin.Y), false);
			//_routeRect.Size = new MKMapSize(1,1);

			//_mapView.SetVisibleMapRect (_routeRect, false);

			_mapView.SetRegion(RouteRegion, false);
		}

		class MapViewDelegate : MKMapViewDelegate
		{
			os4MapsViewController _viewController;
			public MapViewDelegate (os4MapsViewController viewController)
			{
				_viewController = viewController;
			}
			public override MKOverlayView GetViewForOverlay (MKMapView mapView, NSObject overlay)
			{
				MKOverlayView overlayView = null;
				if (overlay == _viewController.RouteLine)
				{
					if (null == _viewController.RouteLineView)
					{
						_viewController.RouteLineView = new MKPolylineView(_viewController.RouteLine);
						_viewController.RouteLineView.FillColor = UIColor.Red;
						_viewController.RouteLineView.StrokeColor = UIColor.Red;
						_viewController.RouteLineView.LineWidth = 3;
					}
					overlayView = _viewController.RouteLineView;
				}
				return overlayView;
			}
		}
	}
}