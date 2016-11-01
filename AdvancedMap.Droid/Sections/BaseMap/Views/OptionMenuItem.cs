﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class OptionMenuItem : LinearLayout
	{
		LinearLayout headerContainer;
		LinearLayout contentContainer;

		public bool IsMultiLine { get { return section.Styles.Count > 3; } }

		TextView osmLabel, separatorLabel, tileTypeLabel;

		List<OptionLabel> optionLabels = new List<OptionLabel>();

		public Section section;
		public Section Section
		{
			get { return section; }
			set
			{
				section = value;

				osmLabel.Text = section.OSM.Name.ToUpper();
				separatorLabel.Text = "|";
				tileTypeLabel.Text = section.Type.ToString().ToUpper();

				foreach (NameValuePair option in section.Styles)
				{
					OptionLabel optionLabel = new OptionLabel(context, option);

					if (section.Styles.Count > 2)
					{
						optionLabel.SetLayout(0.3f);
					}
					else {
						optionLabel.SetLayout(0.5f);
					}

					optionLabels.Add(optionLabel);
					contentContainer.AddView(optionLabel);
				}

				Measure(0, 0);
				Console.WriteLine("Width: " + MeasuredWidth);
			}
		}

		DisplayMetrics Metrics { get { return context.Resources.DisplayMetrics; } }

		int Padding { get { return (int)(Metrics.WidthPixels * 0.05); } }

		Context context;

		public OptionMenuItem(Context context) : base(context)
		{
			Orientation = Orientation.Vertical;

			this.context = context;

			headerContainer = new LinearLayout(context);
			headerContainer.SetBackgroundColor(Colors.ActionBar);
			headerContainer.Orientation = Orientation.Horizontal;

			AddView(headerContainer);

			contentContainer = new LinearLayout(context);
			contentContainer.Orientation = Orientation.Horizontal;
			AddView(contentContainer);

			osmLabel = GetHeaderItem(TypefaceStyle.Bold);
			separatorLabel = GetHeaderItem(TypefaceStyle.Bold);
			separatorLabel.Gravity = Android.Views.GravityFlags.Center;

			tileTypeLabel = GetHeaderItem(TypefaceStyle.Normal);

			headerContainer.AddView(osmLabel);
			headerContainer.AddView(separatorLabel);
			headerContainer.AddView(tileTypeLabel);

			var parameters = new LinearLayout.LayoutParams((int)(Metrics.WidthPixels * 0.9), LayoutParams.WrapContent, 0.8f);
			parameters.LeftMargin = Padding;
			parameters.RightMargin = Padding;
			parameters.TopMargin = Padding;

			LayoutParameters = parameters;
		}

		TextView GetHeaderItem(TypefaceStyle style)
		{
			TextView view = new TextView(context);

			var parameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent, 0.33f);
			parameters.LeftMargin = Padding;
			view.LayoutParameters = parameters;

			view.Typeface = Typeface.Create("Helvetica Neue", style);
			view.Gravity = Android.Views.GravityFlags.CenterVertical;
			view.SetTextColor(Color.White);
			view.SetPadding(0, 30, 0, 30);

			return view;
		}
	}
}
