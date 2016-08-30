﻿
using System;
using System.IO;
using System.Linq;
using Foundation;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class OfflineVectorMapController : VectorMapBaseController
	{
		public override string Name { get { return "Offline Vector Map"; } }

		public override string Description
		{
			get
			{
				return "Uses bundled assets for the offline base map";
			}
		}

		public string SupportDirectory { get { return Utils.GetDocumentDirectory("packages"); } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string packageDirectory = SupportDirectory;
			string fullWritePath = Path.Combine(packageDirectory, "world_ntvt_0_4.mbtile");

			string resourceDirectory = NSBundle.MainBundle.PathForResource("world_ntvt_0_4", "mbtiles");

			if (!Directory.Exists(packageDirectory))
			{
				Directory.CreateDirectory(packageDirectory);
				Console.WriteLine("Directory: Does not exist... Creating");
			}
			else 
			{
				Console.WriteLine("Directory: Exists");
			}

			// Copy bundled tile data to file system so it can be imported by package manager
			using (var input = new FileStream(resourceDirectory, FileMode.Open, FileAccess.Read))
			{
				using (var output = new FileStream(fullWritePath, FileMode.Create, FileAccess.Write))
				{
					input.CopyTo(output);
				}
			}

			string downloadId = "EE"; // one of ID-s from https://developer.nutiteq.com/guides/packages

			MapSetup.InitializePackageManager(packageDirectory, resourceDirectory, MapView, downloadId);
		}
			
	}
}
