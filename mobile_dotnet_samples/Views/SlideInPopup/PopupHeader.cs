﻿
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public class PopupHeader : UIView
    {
        public static readonly nfloat Height = 40;

        PopupBackButton backButton;
        public UILabel Label { get; private set; }

        PopupCloseButton closeButton;

        public PopupHeader()
        {
            Label = new UILabel();
            Label.TextAlignment = UITextAlignment.Center;
            Label.Font = UIFont.FromName("HelveticaNeue", 11);
            Label.TextColor = Colors.CartoNavy;
            AddSubview(Label);

            backButton = new PopupBackButton();
            backButton.Label.Font = Label.Font;
            backButton.Label.TextColor = Label.TextColor;
            backButton.BackgroundColor = UIColor.White;
            AddSubview(backButton);

            closeButton = new PopupCloseButton();
            AddSubview(closeButton);

            backButton.Hidden = true;

			var recognizer = new UITapGestureRecognizer((obj) =>
			{
                (Superview.Superview as SlideInPopup).Hide();
			});

			closeButton.AddGestureRecognizer(recognizer);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            nfloat padding = 15.0f;

            nfloat x = padding;
            nfloat y = 0;
            nfloat w = Label.Frame.Width;
            nfloat h = Frame.Height;

            Label.Frame = new CGRect(x, y, w, h);
            backButton.Frame = new CGRect(x, y, w, h);

            w = h;
            x = Frame.Width - w;

            closeButton.Frame = new CGRect(x, y, w, h);
        }

        public void SetText(string text)
        {
            Label.Text = text;
            Label.SizeToFit();
            LayoutSubviews();
        }
    }

	public class PopupCloseButton : UIView
	{
        UIImageView image;

        public PopupCloseButton()
        {
            image = new UIImageView();
            image.Image = UIImage.FromFile("icons/icon_close_dark.png");

            AddSubview(image);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var padding = Frame.Height / 3;

            image.Frame = new CGRect(padding, padding, Frame.Width - 2 * padding, Frame.Height - 2 * padding);
        }
	}

    public class PopupBackButton : UIView
    {
        public EventHandler<EventArgs> Click;

        UIImageView button;
        public UILabel Label { get; private set; }

        public PopupBackButton()
        {
            button = new UIImageView();
            button.Image = UIImage.FromFile("icons/icon_back_blue.png");
            AddSubview(button);

            Label = new UILabel();
            Label.Text = "BACK";
            AddSubview(Label);

            var recognizer = new UITapGestureRecognizer((obj) => {
                Click?.Invoke(this, EventArgs.Empty);
            });

            AddGestureRecognizer(recognizer);
        }
    }

}