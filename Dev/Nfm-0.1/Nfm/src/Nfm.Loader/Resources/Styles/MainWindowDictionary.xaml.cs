// <copyright file="MainWindowDictionary.xaml.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-20</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-20</date>
// </editor>
// <summary>Resources for main application window.</summary>

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Nfm.Loader.Resources
{
	/// <summary>
	/// Resources for main application window.
	/// </summary>
	public partial class MainWindowDictionary
	{
		/// <summary>
		/// Specify if window is in resizing mode.
		/// </summary>
		private bool isResizing;

		/// <summary>
		/// Specify current resizing direction(s).
		/// </summary>
		private ResizeDirection resizeDirection;

		#region Initialize window resizing methods

		/// <summary>
		/// Start window resizing in "Right" direction.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeRight(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection |= ResizeDirection.Right;
		}

		/// <summary>
		/// Start window resizing in "Left" direction.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeLeft(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection |= ResizeDirection.Left;
		}

		/// <summary>
		/// Start window resizing in "Bottom" direction.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeBottom(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection |= ResizeDirection.Bottom;
		}

		/// <summary>
		/// Start window resizing in "Top" direction.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeTop(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection |= ResizeDirection.Top;
		}

		/// <summary>
		/// Start window resizing in "Right and Bottom" directions.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeRightBottom(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection = ResizeDirection.Right | ResizeDirection.Bottom;
		}

		/// <summary>
		/// Start window resizing in "Left and Bottom" directions.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeLeftBottom(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection = ResizeDirection.Left | ResizeDirection.Bottom;
		}

		/// <summary>
		/// Start window resizing in "Right and Top" directions.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeRightTop(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection = ResizeDirection.Right | ResizeDirection.Top;
		}

		/// <summary>
		/// Start window resizing in "Left and Top" directions.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void InitiateResizeLeftTop(object sender, MouseButtonEventArgs e)
		{
			isResizing = true;
			resizeDirection = ResizeDirection.Left | ResizeDirection.Top;
		}

		#endregion

		/// <summary>
		/// Finish window resizing process.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void FinishResize(object sender, MouseButtonEventArgs e)
		{
			isResizing = false;
			resizeDirection = ResizeDirection.None;
			var rect = (Rectangle) sender;
			rect.ReleaseMouseCapture();
		}

		/// <summary>
		/// Resize window into current resize direction(s).
		/// </summary>
		/// <param name="sender">Event senter.</param>
		/// <param name="e">Event data.</param>
		private void Resize(object sender, MouseEventArgs e)
		{
			if (isResizing)
			{
				var rect = (Rectangle) sender;
				var win = (Window) rect.TemplatedParent;

				rect.CaptureMouse();

				if ((resizeDirection & ResizeDirection.Right) == ResizeDirection.Right)
				{
					double width = e.GetPosition(win).X + 5;

					if (width > 0)
					{
						win.Width = width;
					}
				}

				if ((resizeDirection & ResizeDirection.Left) == ResizeDirection.Left)
				{
					double left = e.GetPosition(win).X - 5;
					double offSet = win.Width - left;

					if (win.MinWidth <= offSet && offSet <= win.MaxWidth)
					{
						win.Left += left;
						win.Width -= left;
					}
				}

				if ((resizeDirection & ResizeDirection.Bottom) == ResizeDirection.Bottom)
				{
					double height = e.GetPosition(win).Y + 5;

					if (height > 0)
					{
						win.Height = height;
					}
				}

				if ((resizeDirection & ResizeDirection.Top) == ResizeDirection.Top)
				{
					double top = e.GetPosition(win).Y - 5;
					double offSet = win.Height - top;

					if (win.MinHeight <= offSet && offSet <= win.MaxHeight)
					{
						win.Top += top;
						win.Height -= top;
					}
				}
			}
		}

		/// <summary>
		/// Move window on screen when holding left mouse button
		/// or change window state to maximized/normal - on double click.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void MoveOrMaximize(object sender, MouseButtonEventArgs e)
		{
			if (!isResizing)
			{
				var win = (Window) ((FrameworkElement) sender).TemplatedParent;

				if (e.ClickCount >= 2)
				{
					if (win.WindowState == WindowState.Normal)
					{
						win.WindowState = WindowState.Maximized;
					}
					else if (win.WindowState == WindowState.Maximized)
					{
						win.WindowState = WindowState.Normal;
					}
				}
				else
				{
					win.DragMove();
				}
			}
		}

		/// <summary>
		/// Minimize window on right mouse button click.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void Minimize(object sender, MouseButtonEventArgs e)
		{
			if (!isResizing)
			{
				var win = (Window)((FrameworkElement)sender).TemplatedParent;
				win.WindowState = WindowState.Minimized;
			}
		}

		/// <summary>
		/// Close window on middle mouse button click.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event data.</param>
		private void CloseWindow(object sender, MouseButtonEventArgs e)
		{
			if (!isResizing && e.MiddleButton == MouseButtonState.Pressed)
			{
				var win = (Window) ((FrameworkElement) sender).TemplatedParent;
				win.Close();
			}
		}

		#region Nested type: ResizeDirection

		/// <summary>
		/// Window resize direction.
		/// </summary>
		[Flags]
		private enum ResizeDirection
		{
			/// <summary>
			/// Not specified.
			/// </summary>
			None = 0,

			/// <summary>
			/// Dragging right edge of the window.
			/// </summary>
			Right = 1,

			/// <summary>
			/// Dragging left edge of the window.
			/// </summary>
			Left = 2,

			/// <summary>
			/// Dragging bottom edge of the window.
			/// </summary>
			Bottom = 4,

			/// <summary>
			/// Dragging top edge of the window.
			/// </summary>
			Top = 8
		}

		#endregion
	}
}