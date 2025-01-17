﻿
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;
using Carto.Utils;
using UIKit;

namespace Shared.iOS
{
	public class MapBaseController : BaseController
	{
		protected MapView MapView { get; private set; }

		protected Projection BaseProjection { get; private set; }

		protected VectorTileLayer BaseLayer { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MapView = new MapView();
			View = MapView;

            MapView.Options.ZoomGestures = true;

			BaseProjection = MapView.Options.BaseProjection;

			Title = Name;
		}

		protected void AddOnlineBaseLayer(CartoBaseMapStyle withStyle)
		{
            // Initialize map
            if (BaseLayer != null)
            {
                MapView.Layers.Remove(BaseLayer);
            }

			BaseLayer = new CartoOnlineVectorTileLayer(withStyle);
			MapView.Layers.Add(BaseLayer);
		}
	}
}

