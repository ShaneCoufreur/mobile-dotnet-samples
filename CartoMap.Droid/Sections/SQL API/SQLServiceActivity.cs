﻿using System;
using Android.App;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Styles;
using Carto.VectorTiles;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "SQL Service", Description = "Displays cities on the map via SQL query")]
	public class SQLServiceActivity : MapBaseActivity
	{
		const string query = "SELECT * FROM cities15000 WHERE population > 100000";

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Clear the default layer, add a dark one instead
			MapView.Layers.Clear();

            var baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDarkmatter);

			// Remove texts so dots would be more prominent
			(baseLayer.TileDecoder as MBVectorTileDecoder).SetStyleParameter("lang", "nolang");

			MapView.Layers.Add(baseLayer);

			Projection projection = MapView.Options.BaseProjection;

			// Create a datasource and layer for the map
			LocalVectorDataSource source = new LocalVectorDataSource(projection);
			VectorLayer layer = new VectorLayer(source);
			MapView.Layers.Add(layer);

			// Initialize CartoSQL service, set a username
			CartoSQLService service = new CartoSQLService();
			service.Username = "nutiteq";

			PointStyleBuilder builder = new PointStyleBuilder
			{
				Color = new Carto.Graphics.Color(255, 0, 0, 255),
				Size = 1
			};

			MapView.QueryFeatures(service, source, builder.BuildStyle(), query);
		}
	}
}

