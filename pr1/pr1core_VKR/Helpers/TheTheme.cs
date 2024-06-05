﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace pr1.Helpers
{
    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                //default
                case 0:
                    App.Current.UserAppTheme = AppTheme.Unspecified;
                    break;
                //light
                case 1:
                    App.Current.UserAppTheme = AppTheme.Light;
                    break;
                //dark
                case 2:
                    App.Current.UserAppTheme = AppTheme.Dark;
                    break;
            }

            var nav = App.Current.MainPage as Microsoft.Maui.Controls.NavigationPage;

            var e = DependencyService.Get<IEnvironment>();
            if (App.Current.RequestedTheme == AppTheme.Dark)
            {
                e?.SetStatusBarColor(System.Drawing.Color.Black, false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Colors.Black;
                    nav.BarTextColor = Colors.White;
                }
            }
            else
            {
                e?.SetStatusBarColor(System.Drawing.Color.White, true);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Colors.White;
                    nav.BarTextColor = Colors.Black;
                }
            }


        }
    }
}