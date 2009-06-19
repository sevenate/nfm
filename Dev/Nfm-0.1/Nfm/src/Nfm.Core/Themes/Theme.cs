// <copyright file="Theme.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-30</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-30</date>
// </editor>
// <summary>Hold all theme's resource <see cref="ComponentResourceKey"/>.</summary>

using System;
using System.Linq;
using System.Windows;
using Nfm.Core.Resources;

namespace Nfm.Core.Themes
{
	/// <summary>
	/// Hold all theme's resource <see cref="ComponentResourceKey"/>.
	/// </summary>
	public static class Theme
	{
		#region General

		public static ComponentResourceKey ForegroundBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "ForegroundBrush"); }
		}

		public static ComponentResourceKey BackgroundBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "BackgroundBrush"); }
		}

		public static ComponentResourceKey HoverBackgroundBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "HoverBackgroundBrush"); }
		}

		public static ComponentResourceKey BorderBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "BorderBrush"); }
		}

		public static ComponentResourceKey SelectedTabBackgroundBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "SelectedTabBackgroundBrush"); }
		}

		public static ComponentResourceKey GeneralFontKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "GeneralFont"); }
		}

		#endregion

		#region Selected

		public static ComponentResourceKey SelectedForegroundBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "SelectedForegroundBrush"); }
		}

		public static ComponentResourceKey SelectedBackgroundBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "SelectedBackgroundBrush"); }
		}

		public static ComponentResourceKey SelectedBorderBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "SelectedBorderBrush"); }
		}

		#endregion

		#region Disabled

		public static ComponentResourceKey DisabledForegroundBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "DisabledForegroundBrush"); }
		}

		public static ComponentResourceKey DisabledBorderBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "DisabledBorderBrush"); }
		}

		#endregion

		#region Focuse

		public static ComponentResourceKey FocusedBorderBrushKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "FocusedBorderBrush"); }
		}

		#endregion

		#region Window

		public static ComponentResourceKey MainWindowStyleKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "MainWindowStyle"); }
		}

		#endregion

		#region ScrollViewer

		public static ComponentResourceKey ScrollViewerStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ScrollViewerStyle"); }
		}

		public static ComponentResourceKey ScrollBarStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ScrollBarStyle"); }
		}

		public static ComponentResourceKey ScrollBarLineButtonStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ScrollBarLineButtonStyle"); }
		}

		public static ComponentResourceKey ScrollBarPageButtonStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ScrollBarPageButtonStyle"); }
		}

		public static ComponentResourceKey ScrollBarThumbStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ScrollBarThumbStyle"); }
		}

		#endregion

		#region ListBox

		public static ComponentResourceKey ListBoxStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ListBoxStyle"); }
		}

		public static ComponentResourceKey ListBoxItemStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ListBoxItemStyle"); }
		}

		#endregion

		#region TabControl

		public static ComponentResourceKey TabControlStyleKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "TabControlStyle"); }
		}

		public static ComponentResourceKey TabItemStyleKey
		{
			get { return new ComponentResourceKey(typeof (Theme), "TabItemStyle"); }
		}

		#endregion

		#region Toolbar

		public static ComponentResourceKey ToolbarButtonStyleKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "ToolbarButtonStyle"); }
		}

		public static ComponentResourceKey IconBrushKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "IconBrush"); }
		}

		public static ComponentResourceKey IconHoverBrushKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "IconHoverBrush"); }
		}

		public static ComponentResourceKey IconPressedBrushKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "IconPressedBrush"); }
		}

		public static ComponentResourceKey IconDisabledBrushKey
		{
			get { return new ComponentResourceKey(typeof(Theme), "IconDisabledBrush"); }
		}

		#endregion

		#region Theme Managment

		/// <summary>
		/// Reset application theme to default.
		/// </summary>
		public static void ClearApplicationThemeToDefault()
		{
			foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
			{
				// Note: Consider to use another way to find current applied theme.
				if (dictionary.Source.ToString().EndsWith("Theme.xaml"))
				{
					Application.Current.Resources.MergedDictionaries.Remove(dictionary);
					break;
				}
			}
		}

		/// <summary>
		/// Apply new theme to application.
		/// </summary>
		/// <param name="path">Path to theme <see cref="ResourceDictionary"/>.</param>
		public static void ApplyTheme(string path)
		{
			Uri uri = ResourceCache.GetPackUri(path);
			ResourceDictionary theme = ResourceCache.GetResourceDictionary(uri);

			theme.Source = uri;
			ClearApplicationThemeToDefault();

			try
			{
				// Bug: Can't change windows "AllowsTransparency" property after window is created.
				Application.Current.Resources.MergedDictionaries.Add(theme);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// Get current applied theme name.
		/// </summary>
		/// <returns>Current application theme name or <see cref="string.Empty"/> for default theme.</returns>
		public static string GetCurrentThemeName()
		{
			foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
			{
				// Note: Consider to use another way to find current applied theme.
				if (dictionary.Source.ToString().EndsWith("Theme.xaml"))
				{
					// Todo: change code to produce "Dark" and "Light"
					// instead of "DarkTheme.xaml" abd "LightTheme.xaml".
					return dictionary.Source.Segments.Where(s => s.EndsWith("Theme.xaml")).First();
				}
			}

			return string.Empty;
		}

		#endregion
	}
}