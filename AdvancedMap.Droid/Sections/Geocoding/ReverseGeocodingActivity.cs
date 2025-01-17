﻿
using System;
using Android.App;
using Shared.Droid;
using Shared;
using Carto.Core;
using Carto.Geocoding;

namespace AdvancedMap.Droid
{
    [Activity]
    [ActivityData(Title = "Reverse Geocoding", Description = "Click an area on the map to find out more about it")]
    public class ReverseGeocodingActivity : BaseGeocodingActivity
    {
        public ReverseGeocodingEventListener Listener { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ContentView = new ReverseGeocodingView(this);
            SetContentView(ContentView);
            GeocodingClient.Projection = ContentView.Projection;

            Listener = new ReverseGeocodingEventListener(ContentView.Projection);

            SetOnlineMode();
            ContentView.SetOnlineMode();

            // Zoom to Washington
            MapPos pos = ContentView.MapView.Options.BaseProjection.FromWgs84(new MapPos(-77.004590, 38.888702));
            ContentView.MapView.SetFocusPos(pos, 0);
            ContentView.MapView.SetZoom(16.0f, 0);
        }        

        protected override void OnResume()
        {
            base.OnResume();

            ContentView.MapView.MapEventListener = Listener;
            Listener.ResultFound += OnGeocodingResultFound;

			string text = "Click on the map to find out more about a location";
			ContentView.Banner.Show(text);
        }

        protected override void OnPause()
        {
            base.OnPause();

			ContentView.MapView.MapEventListener = null;
			Listener.ResultFound -= OnGeocodingResultFound;
        }

		void OnGeocodingResultFound(object sender, EventArgs e)
		{
			GeocodingResult result = (GeocodingResult)sender;

			if (result == null)
			{
                RunOnUiThread(delegate
                {
                    string text = "Couldn't find any addresses. Please try again";
                    ContentView.Banner.Show(text);
                });
                return;
			}

			string title = "";
			string description = result.ToString();
			bool goToPosition = false;

            var view = ContentView as BaseGeocodingView;
            view.GeocodingSource.ShowResult(ContentView.MapView, result, title, description, goToPosition);
		}

        protected override void SetOnlineMode()
        {
            
            Listener.Service = new MapBoxOnlineReverseGeocodingService(Tokens.MapBox);
        }

        protected override void SetOfflineMode()
        {
            Listener.Service = new PackageManagerReverseGeocodingService(GeocodingClient.Manager);
        }
    }
}
