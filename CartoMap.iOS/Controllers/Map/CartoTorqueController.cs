﻿using System;
using System.Threading;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorTiles;
using Shared;
using Shared.iOS;

namespace CartoMap.iOS
{
	public class CartoTorqueController : VectorMapBaseController
	{
		public override string Name { get { return "Carto Torque Map"; } }

		public override string Description { get { return "How to use Carto Torque tiles with CartoCSS styling"; } }

		const long FRAMETIME = 100;

		string CartoCSS
		{
			get
			{
				return "#layer {\n" +
					   "  comp-op: lighten;\n" +
					   "  marker-type:ellipse;\n" +
					   "  marker-width: 10;\n" +
					   "  marker-fill: #FEE391;\n" +
					   "  [value > 2] { marker-fill: #FEC44F; }\n" +
					   "  [value > 3] { marker-fill: #FE9929; }\n" +
					   "  [value > 4] { marker-fill: #EC7014; }\n" +
					   "  [value > 5] { marker-fill: #CC4C02; }\n" +
					   "  [value > 6] { marker-fill: #993404; }\n" +
					   "  [value > 7] { marker-fill: #662506; }\n" +
					   "\n" +
					   "  [frame-offset = 1] {\n" +
					   "    marker-width: 20;\n" +
					   "    marker-fill-opacity: 0.1;\n" +
					   "  }\n" +
					   "  [frame-offset = 2] {\n" +
					   "    marker-width: 30;\n" +
					   "    marker-fill-opacity: 0.05;\n" +
					   "  }\n" +
					   "}\n";
			}
		}

		// Magic query to create torque tiles
		string Query
		{
			get
			{
				return "WITH par \n" +
						"AS (SELECT Cdb_xyz_resolution({zoom}) * 1   AS res,\n" +
						"256 / 1 AS tile_size,\n" +
						"Cdb_xyz_extent({x}, {y}, {zoom}) AS ext),\n" +
						"cte\n" +
						"AS (SELECT St_snaptogrid(i.the_geom_webmercator, p.res) g,\n" +
						" Count(cartodb_id) c,\n" +
						" Floor(( Date_part('epoch', date) - -1796072400 ) / 476536.5) d\n" +
						"FROM (SELECT *\n" +
						" FROM ow) i,\n" +
						"  par p\n" +
						" WHERE i.the_geom_webmercator && p.ext\n" +
						" GROUP BY g, d)\n" +
						"SELECT ( St_x(g) - St_xmin(p.ext) ) / p.res x__uint8,\n" +
						" ( St_y(g) - St_ymin(p.ext) ) / p.res y__uint8,\n" +
						" Array_agg(c) vals__uint8,\n" +
						" Array_agg(d) dates__uint16\n" +
						"FROM cte,\n" +
						"  par p\n" +
						"WHERE  ( St_y(g) - St_ymin(p.ext) ) / p.res < tile_size\n" +
						" AND ( St_x(g) - St_xmin(p.ext) ) / p.res < tile_size\n" +
						"GROUP BY x__uint8,\n" +
						" y__uint8 ";
			}
		}

		TorqueTileDecoder decoder;
		TorqueTileLayer tileLayer;

		Timer timer;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string encoded = System.Web.HttpUtility.UrlEncode(Query.Replace("\n", "")).EncodeParenthesis();

			//encoded = "WITH%20par%20AS%20(%20%20SELECT%20CDB_XYZ_Resolution({zoom})*1%20as%20res%2C%20%20256%2F1%20as" +
			//	"%20tile_size%2C%20CDB_XYZ_Extent({x}%2C%20{y}%2C%20{zoom})%20as%20ext%20)%2Ccte%20AS%20(%20%20%20SEL" +
			//	"ECT%20ST_SnapToGrid(i.the_geom_webmercator%2C%20p.res)%20g%2C%20count(cartodb_id)%20c%2C%20floor((da" +
			//	"te_part(%27epoch%27%2C%20date)%20-%20-1796072400)%2F476536.5)%20d%20%20FROM%20(select%20*%20from%20o" +
			//	"w)%20i%2C%20par%20p%20%20%20WHERE%20i.the_geom_webmercator%20%26%26%20p.ext%20%20%20GROUP%20BY%20g%2" +
			//	"C%20d)%20SELECT%20(st_x(g)-st_xmin(p.ext))%2Fp.res%20x__uint8%2C%20%20%20%20%20%20%20%20(st_y(g)-st_" +
			//	"ymin(p.ext))%2Fp.res%20y__uint8%2C%20array_agg(c)%20vals__uint8%2C%20array_agg(d)%20dates__uint16%20" +
			//	"FROM%20cte%2C%20par%20p%20where%20(st_y(g)-st_ymin(p.ext))%2Fp.res%20%3C%20tile_size%20and%20(st_x(g" +
			//	")-st_xmin(p.ext))%2Fp.res%20%3C%20tile_size%20GROUP%20BY%20x__uint8%2C%20y__uint8&last_updated=1970-" +
			//	"01-01T00%3A00%3A00.000Z";
			
			// Define datasource with the query
			string url = "http://viz2.cartodb.com/api/v2/sql?q=" + encoded + "&cache_policy=persist";
			HTTPTileDataSource source = new HTTPTileDataSource(0, 14, url);

			// Create persistent cache to make it faster
			string cacheFile = Utils.GetDocumentDirectory() + "/torque_tile_cache.db";
			TileDataSource cacheSource = new PersistentCacheTileDataSource(source, cacheFile);

			// Create CartoCSS style from Torque points
			CartoCSSStyleSet styleSheet = new CartoCSSStyleSet(CartoCSS);

			// Create tile decoder and Torque layer
			decoder = new TorqueTileDecoder(styleSheet);

			tileLayer = new TorqueTileLayer(cacheSource, decoder);

			MapView.Layers.Add(tileLayer);

			MapView.SetZoom(1, 0);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			timer = new Timer(new TimerCallback(UpdateTorque), null, FRAMETIME, FRAMETIME);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			timer.Dispose();
			timer = null;
		}

		void UpdateTorque(object state)
		{
			System.Threading.Tasks.Task.Run(delegate
				{
					int frameNumber = (tileLayer.FrameNr + 1) % decoder.FrameCount;
					tileLayer.FrameNr = frameNumber;
				});
		}
	}
}

