// <copyright file="MultimonitorMaximizer.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-21</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-21</date>
// </editor>
// <summary>DPI-independent P/Invoke Win32 API hotfix with multimonitor support, which prevent maximized borderless window from hiding taskbar.</summary>

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Nfm.Loader.Legacy
{
	/// <summary>
	/// DPI-independent P/Invoke Win32 API hotfix with multimonitor support,
	/// which prevent maximized borderless window from hiding taskbar.
	/// </summary>
	public class MultimonitorMaximizer
	{
		/// <summary>
		/// Nearest monitor to window.
		/// </summary>
		private const int MONITOR_DEFAULTTONEAREST = 2;

		/// <summary>
		/// To get a handle to the specified monitor.
		/// </summary>
		/// <param name="hwnd">Window handle.</param>
		/// <param name="dwFlags">Specific flags.</param>
		/// <returns>Monitor handle.</returns>
		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

		/// <summary>
		/// To get the working area of the specified monitor.
		/// </summary>
		/// <param name="hmonitor">Monitor handle.</param>
		/// <param name="monitorInfo">Monitor information structure.</param>
		/// <returns>True, if monitor info successfully retrived.</returns>
		[DllImport("user32.dll")]
		private static extern bool GetMonitorInfo(HandleRef hmonitor, [In] [Out] MONITORINFOEX monitorInfo);

		/// <summary>
		/// Can only calculate device independent working area once
		/// the window has a handle that it can use.
		/// <remarks>Call this method from Window.SourceInitialized event handler.</remarks>
		/// </summary>
		/// <param name="win">Window instance.</param>
		public static void RecalculateMaxSize(Window win)
		{
			// Make window borderless
			win.WindowStyle = WindowStyle.None;
			win.ResizeMode = ResizeMode.NoResize;

			// Get handle for nearest monitor to this window
			var windowInteropHelper = new WindowInteropHelper(win);
			IntPtr hMonitor = MonitorFromWindow(windowInteropHelper.Handle, MONITOR_DEFAULTTONEAREST);

			// Get monitor info
			var monitorInfo = new MONITORINFOEX();
			monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
			GetMonitorInfo(new HandleRef(win, hMonitor), monitorInfo);

			// Create working area dimensions, converted to DPI-independent values
			HwndSource source = HwndSource.FromHwnd(windowInteropHelper.Handle);

			if (source == null)
			{
				return; // Should never be null
			}

			if (source.CompositionTarget == null)
			{
				return; // Should never be null
			}

			Matrix matrix = source.CompositionTarget.TransformFromDevice;
			RECT workingArea = monitorInfo.rcWork;

			Point dpiIndependentSize =
				matrix.Transform(new Point(workingArea.Right - workingArea.Left, workingArea.Bottom - workingArea.Top));

			// Maximize the window to the device-independent working area ie
			// the area without the taskbar.
			// NOTE - window state must be set to Maximized as this adds certain
			// maximized behaviors eg you can't move a window while it is maximized,
			// such as by calling Window.DragMove
			win.MaxWidth = dpiIndependentSize.X;
			win.MaxHeight = dpiIndependentSize.Y;

			// Force window to be opened maximized (optional).
			//win.Top = 0;
			//win.Left = 0;
			//win.WindowState = WindowState.Maximized;
		}

		#region Nested type: MONITORINFOEX

		/// <summary>
		/// Monitor information structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public class MONITORINFOEX
		{
			/// <summary>
			/// Info size.
			/// </summary>
			public int cbSize;

			/// <summary>
			/// Total area.
			/// </summary>
			public RECT rcMonitor;

			/// <summary>
			/// Working area.
			/// </summary>
			public RECT rcWork;

			/// <summary>
			/// Specific flags.
			/// </summary>
			public int dwFlags;

			/// <summary>
			/// Device data.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
			public char[] szDevice;
		}

		#endregion

		#region Nested type: RECT

		/// <summary>
		/// Rectangle (used by MONITORINFOEX).
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			/// <summary>
			/// X1 coordinate.
			/// </summary>
			public int Left;

			/// <summary>
			/// Y1 coordinate.
			/// </summary>
			public int Top;

			/// <summary>
			/// X2 coordinate.
			/// </summary>
			public int Right;

			/// <summary>
			/// Y2 coordinate.
			/// </summary>
			public int Bottom;
		}

		#endregion
	}
}