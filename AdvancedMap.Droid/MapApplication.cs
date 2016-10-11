﻿
using System;
using Android.App;
using Android.Runtime;
using Carto.Ui;
using Carto.Utils;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Application]
	public class MapApplication : Application
	{
		const string LICENSE = "XTUN3Q0ZFMmhzUWEwdlZwNlJNVW5kL1hMRExMYXNYVUxBaFFhKzlSK2drYjEzNnBhSTJzY1c5aDBkL2licFE9PQoKcHJvZHVjdHM9c2RrLXhhbWFyaW4tYW5kcm9pZC00LioKcGFja2FnZU5hbWU9Y29tLmNhcnRvLmFkdmFuY2VkbWFwLnhhbWFyaW4uZHJvaWQKd2F0ZXJtYXJrPWRldmVsb3BtZW50CnZhbGlkVW50aWw9MjAxNi0wOS0xOApvbmxpbmVMaWNlbnNlPTEK";
		//const string LICENSE = "XTUN3Q0ZEeHN5b2dXLzM4WWtCQ1lZYU8zVkVOcEU0dUdBaFJYQnJzSFNTQlhqYTNIa2FLTkVRLzJtRzBLUWc9PQoKYXBwVG9rZW49MjEwYzQ4ZTAtNWVjOC00NzQyLWIwY2EtOWU4YjcyNGZmMmYwCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by5jYXJ0b21hcC54YW1hcmluLmRyb2lkCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1hbmRyb2lkLTQuKgp3YXRlcm1hcms9Y2FydG9kYgo=";

		public MapApplication(IntPtr a, JniHandleOwnership b) : base(a, b)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();

			BaseMapActivity.ViewResource = Resource.Layout.Main;
			BaseMapActivity.MapViewResource = Resource.Id.mapView;

			Log.ShowError = true;
			Log.ShowWarn = true;

			// Register license
			MapView.RegisterLicense(LICENSE, ApplicationContext);
		}
	}
}

