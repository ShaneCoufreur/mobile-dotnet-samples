﻿
using Carto.Ui;
using Foundation;
using HockeyApp.iOS;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		const string CartoLicense = "XTUM0Q0ZRQzBoRk1OeGV5MkxqYkdHd0tqNUtINE0rN3V2QUlWQUxiQnh3eXZmaUdMakk3dXoxclExMlJvUnU3cQoKYXBwVG9rZW49NjM4NWMwNGItNTgyOC00MzJhLWIzYTEtMzI0OWU1MjY3ZDRiCmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLmFkdmFuY2VkbWFwCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1pb3MtNC4qCndhdGVybWFyaz1jdXN0b20K";
		const string HockeyId = "db59e732ee3f4b9c8bc31a2fbbea4186";

		public override UIWindow Window { get; set; }

		public UINavigationController Controller { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			MapView.RegisterLicense(CartoLicense);

			UIViewController initial = new MapListController("Advanced Samples", Samples.List);
			Controller = new UINavigationController(initial);

			// Navigation bar background color
            Controller.NavigationBar.BarTintColor = Colors.CartoRed;
			// Back button color
			Controller.NavigationBar.TintColor = UIColor.White;
			// Title color
			Controller.NavigationBar.TitleTextAttributes = new UIStringAttributes { 
				ForegroundColor = UIColor.White, Font = 
					UIFont.FromName("HelveticaNeue", 15)
				};

			Controller.NavigationBarHidden = false;

			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			Window.RootViewController = Controller;

			Window.MakeKeyAndVisible();

			#if ENABLE_TEST_CLOUD
			//Xamarin.Calabash.Start();
			#endif

			BITHockeyManager manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure(HockeyId);
			manager.DisableUpdateManager = false;
			manager.StartManager();

            Carto.Utils.Log.ShowInfo = true;
            Carto.Utils.Log.ShowWarn = true;
            Carto.Utils.Log.ShowDebug = true;
            Carto.Utils.Log.ShowError = true;

            return true;
		}

	}
}


