﻿using System;
using Carto.Core;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorElements;

namespace Shared
{
	public class MyClusterElementBuilder : ClusterElementBuilder
	{
		BalloonPopupStyleBuilder balloonPopupStyleBuilder;

		public MyClusterElementBuilder()
		{
			balloonPopupStyleBuilder = new BalloonPopupStyleBuilder();
			balloonPopupStyleBuilder.CornerRadius = 3;
			balloonPopupStyleBuilder.TitleMargins = new BalloonPopupMargins(6, 6, 6, 6);
			balloonPopupStyleBuilder.LeftColor = new Color(240, 230, 140, 255);
		}

		public override VectorElement BuildClusterElement(MapPos pos, VectorElementVector elements)
		{
			BalloonPopupStyle style = balloonPopupStyleBuilder.BuildStyle();
			var popup = new BalloonPopup(pos, style, elements.Count.ToString(), "");

			return popup;
		}
	}
}

