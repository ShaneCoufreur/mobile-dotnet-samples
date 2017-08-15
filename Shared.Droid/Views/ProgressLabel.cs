﻿
using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace Shared.Droid
{
    public class ProgressLabel : BaseView
    {
        TextView label;
        BaseView progressBar;

        public bool IsVisible
        {
            get { return Alpha >= 1.0f; }
        }

        public ProgressLabel(Context context) : base(context)
        {
            label = new TextView(context);
            label.Gravity = Android.Views.GravityFlags.Center;
            label.SetTextColor(Color.White);
            AddView(label);

            progressBar = new BaseView(context);
            progressBar.SetBackgroundColor(Colors.AppleBlue);
            AddView(progressBar);
        }

        public override void LayoutSubviews()
        {
            label.SetFrame(0, 0, Frame.W, Frame.H);
        }

        public void Update(string text, float progress)
        {
            Update(text);
            UpdateProgress(progress);
        }

        public void Update(string text)
        {
            if (!IsVisible)
            {
                Show();
            }

            label.Text = text.ToUpper();
        }

        public void UpdateProgress(float progress)
        {
            var width = (int)(Frame.W * progress / 100);
            var height = (int)(3 * Density);
            var y = Frame.H - height;

            progressBar.SetFrame(0, y, width, height);
        }

        public void Show()
        {
            AnimateAlpha(1.0f);
        }

        public void Hide()
        {
            AnimateAlpha(0.0f);    
        }

		void AnimateAlpha(float to, long duration = 200)
		{
			var animator = ObjectAnimator.OfFloat(this, "alpha", to);
			animator.SetDuration(duration);
			animator.Start();
		}

	}
}
