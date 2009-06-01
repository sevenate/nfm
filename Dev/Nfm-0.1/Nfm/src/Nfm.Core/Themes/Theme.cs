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

using System.Windows;

namespace Nfm.Core.Themes
{
	/// <summary>
	/// Hold all theme's resource <see cref="ComponentResourceKey"/>.
	/// </summary>
	public class Theme
	{
		#region General

		public static ComponentResourceKey ForegroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "ForegroundBrush");
			}
		}

		public static ComponentResourceKey BackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "BackgroundBrush");
			}
		}

		public static ComponentResourceKey ListBackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "ListBackgroundBrush");
			}
		}

		public static ComponentResourceKey ListItemBackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "ListItemBackgroundBrush");
			}
		}

		public static ComponentResourceKey HoverBackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "HoverBackgroundBrush");
			}
		}

		public static ComponentResourceKey BorderBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "BorderBrush");
			}
		}

		public static ComponentResourceKey SelectedTabBackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "SelectedTabBackgroundBrush");
			}
		}

		public static ComponentResourceKey GeneralFontKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "GeneralFont");
			}
		}

		#endregion

		#region Selected

		public static ComponentResourceKey SelectedForegroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "SelectedForegroundBrush");
			}
		}

		public static ComponentResourceKey SelectedBackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "SelectedBackgroundBrush");
			}
		}

		public static ComponentResourceKey SelectedBorderBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "SelectedBorderBrush");
			}
		}

		#endregion

		#region Disabled

		public static ComponentResourceKey DisabledForegroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "DisabledForegroundBrush");
			}
		}

		public static ComponentResourceKey DisabledBackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "DisabledBackgroundBrush");
			}
		}

		public static ComponentResourceKey DisabledBorderBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "DisabledBorderBrush");
			}
		}

		#endregion

		#region Focuse

		public static ComponentResourceKey FocusedBorderBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "FocusedBorderBrush");
			}
		}

		#endregion

		#region Window

		public static ComponentResourceKey MainWindowStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "MainWindowStyle");
			}
		}

		public static ComponentResourceKey WindowBackgroundBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "WindowBackgroundBrush");
			}
		}

		public static ComponentResourceKey WindowBorderBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "WindowBorderBrush");
			}
		}

		#endregion

		#region ListBox

		public static ComponentResourceKey ListBoxStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "ListBoxStyle");
			}
		}

		public static ComponentResourceKey ListBoxItemStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "ListBoxItemStyle");
			}
		}

		#endregion

		#region TabControl

		public static ComponentResourceKey TabControlStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "TabControlStyle");
			}
		}

		public static ComponentResourceKey TabItemStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "TabItemStyle");
			}
		}

		public static ComponentResourceKey CloseableTabItemButtonStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "CloseableTabItemButtonStyle");
			}
		}

		public static ComponentResourceKey DuplicateTabItemButtonStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(Theme), "DuplicateTabItemButtonStyle");
			}
		}

		#endregion
	}
}