﻿
using System;
using Carto.Geocoding;
using Shared;

namespace AdvancedMap.iOS
{
    public class ReverseGeocodingController : BaseGeocodingController
    {
        public PackageManagerReverseGeocodingService Service { get; set; }

        public ReverseGeocodingEventListener Listener { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            ContentView = new ReverseGeocodingView();
			View = ContentView;

			Listener = new ReverseGeocodingEventListener(ContentView.Projection);
			Listener.Service = new PackageManagerReverseGeocodingService(Geocoding.Manager);

            Geocoding.Projection = ContentView.Projection;

            Title = "REVERSE GEOCODING";

            SetOnlineMode();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ContentView.MapView.MapEventListener = Listener;
            Listener.ResultFound += OnFoundResult;

            string text = "Click on a location to find out more about it";
            ContentView.Banner.Show(text);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

			ContentView.MapView.MapEventListener = null;
            Listener.ResultFound -= OnFoundResult;
        }

        void OnFoundResult(object sender, EventArgs e)
        {
            GeocodingResult result = (GeocodingResult)sender;

            if (result == null)
            {
                Alert("Couldn't find any addresses. Are you sure you have downloaded the region you're trying to reverse geocode?");
                return;
            }

            string title = "";
            string description = result.ToString();
            bool goToPosition = false;

            var source = (ContentView as ReverseGeocodingView).ObjectSource;
            source.ShowResult(ContentView.MapView, result, title, description, goToPosition);
        }

		public override void SetOnlineMode()
		{
            Listener.Service = new MapBoxOnlineReverseGeocodingService(Tokens.MapBox);
		}

		public override void SetOfflineMode()
		{
			string text = "Click the globa icon to download geocoding packages";
			ContentView.Banner.Show(text);
            Listener.Service = new PackageManagerReverseGeocodingService(Geocoding.Manager);
		}
    }
}
