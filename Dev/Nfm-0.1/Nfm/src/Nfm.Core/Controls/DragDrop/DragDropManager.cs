// <copyright file="DragDropManager.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Pavan Podila">
//	<url>http://pavanpodila.spaces.live.com/blog/cns!9C9E888164859398!199.entry</url>
// 	<date>2006-11-20</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-16</date>
// </editor>
// <summary>Drag-and-drop manager with attached properties.</summary>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// Drag-and-drop manager with attached properties.
	/// </summary>
	public static class DragDropManager
	{
		#region Fields

		/// <summary>
		/// Drop item advisor.
		/// </summary>
		private static IDragSourceAdvisor currentSourceAdvisor;

		/// <summary>
		/// Drop area advisor.
		/// </summary>
		private static IDropTargetAdvisor currentTargetAdvisor;

		/// <summary>
		/// Drag UI element.
		/// </summary>
		private static UIElement dragElement;

		/// <summary>
		/// Mouse left button pressed flag.
		/// </summary>
		private static bool isMouseDown;

		/// <summary>
		/// Drag UI element overlay.
		/// </summary>
		private static DropPreviewAdorner overlayElement;

		/// <summary>
		/// Drag start point.
		/// </summary>
		private static Point startPoint;

		#endregion

		#region Dependency Properties

		#region DragSourceAdvisor

		/// <summary>
		/// DragSourceAdvisor attached property.
		/// </summary>
		public static readonly DependencyProperty DragSourceAdvisorProperty =
			DependencyProperty.RegisterAttached(
				"DragSourceAdvisor",
				typeof (IDragSourceAdvisor),
				typeof (DragDropManager),
				new FrameworkPropertyMetadata(
					new PropertyChangedCallback(OnDragSourceAdvisorChanged)));

		/// <summary>
		/// Attach <see cref="DragSourceAdvisorProperty"/> property to target dependency object.
		/// </summary>
		/// <param name="depObj">Attach target.</param>
		/// <param name="isSet">The new local value.</param>
		public static void SetDragSourceAdvisor(DependencyObject depObj, bool isSet)
		{
			depObj.SetValue(DragSourceAdvisorProperty, isSet);
		}

		/// <summary>
		/// Detattach <see cref="DragSourceAdvisorProperty"/> property from target dependency object.
		/// </summary>
		/// <param name="depObj">Detattach target.</param>
		/// <returns>Returns the current effective value.</returns>
		public static IDragSourceAdvisor GetDragSourceAdvisor(DependencyObject depObj)
		{
			return depObj.GetValue(DragSourceAdvisorProperty) as IDragSourceAdvisor;
		}

		/// <summary>
		/// <see cref="DragSourceAdvisorProperty"/> attached property changed callback.
		/// </summary>
		/// <param name="depObj">Attached property target.</param>
		/// <param name="args">Provides data for attached property changed event.</param>
		private static void OnDragSourceAdvisorChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			var sourceUI = (UIElement) depObj;

			if (args.NewValue != null && args.OldValue == null)
			{
				sourceUI.PreviewMouseLeftButtonDown += DragSourcePreviewMouseLeftButtonDown;
				sourceUI.PreviewMouseMove += DragSourcePreviewMouseMove;
				sourceUI.PreviewMouseLeftButtonUp += DragSourcePreviewMouseLeftButtonUp;
				sourceUI.PreviewGiveFeedback += DragSourcePreviewGiveFeedback;
			}
			else if (args.NewValue == null && args.OldValue != null)
			{
				sourceUI.PreviewMouseLeftButtonDown -= DragSourcePreviewMouseLeftButtonDown;
				sourceUI.PreviewMouseMove -= DragSourcePreviewMouseMove;
				sourceUI.PreviewMouseLeftButtonUp -= DragSourcePreviewMouseLeftButtonUp;
				sourceUI.PreviewGiveFeedback -= DragSourcePreviewGiveFeedback;
			}
		}

		#endregion

		#region DropTargetAdvisor

		/// <summary>
		/// DropTargetAdvisor attached property.
		/// </summary>
		public static readonly DependencyProperty DropTargetAdvisorProperty =
			DependencyProperty.RegisterAttached(
				"DropTargetAdvisor",
				typeof (IDropTargetAdvisor),
				typeof (DragDropManager),
				new FrameworkPropertyMetadata(
					new PropertyChangedCallback(OnDropTargetAdvisorChanged)));

		/// <summary>
		/// Attach <see cref="DropTargetAdvisorProperty"/> property to target dependency object.
		/// </summary>
		/// <param name="depObj">Attach target.</param>
		/// <param name="isSet">The new local value.</param>
		public static void SetDropTargetAdvisor(DependencyObject depObj, bool isSet)
		{
			depObj.SetValue(DropTargetAdvisorProperty, isSet);
		}

		/// <summary>
		/// Detattach <see cref="DropTargetAdvisorProperty"/> property from target dependency object.
		/// </summary>
		/// <param name="depObj">Detattach target.</param>
		/// <returns>Returns the current effective value.</returns>
		public static IDropTargetAdvisor GetDropTargetAdvisor(DependencyObject depObj)
		{
			return depObj.GetValue(DropTargetAdvisorProperty) as IDropTargetAdvisor;
		}

		/// <summary>
		/// <see cref="DropTargetAdvisorProperty"/> attached property changed callback.
		/// </summary>
		/// <param name="depObj">Attached property target.</param>
		/// <param name="args">Provides data for attached property changed event.</param>
		private static void OnDropTargetAdvisorChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			var targetUI = (UIElement) depObj;

			if (args.NewValue != null && args.OldValue == null)
			{
				targetUI.PreviewDragEnter += DropTargetPreviewDragEnter;
				targetUI.PreviewDragOver += DropTargetPreviewDragOver;
				targetUI.PreviewDragLeave += DropTargetPreviewDragLeave;
				targetUI.PreviewDrop += DropTargetPreviewDrop;
				targetUI.AllowDrop = true;
			}
			else if (args.NewValue == null && args.OldValue != null)
			{
				targetUI.PreviewDragEnter -= DropTargetPreviewDragEnter;
				targetUI.PreviewDragOver -= DropTargetPreviewDragOver;
				targetUI.PreviewDragLeave -= DropTargetPreviewDragLeave;
				targetUI.PreviewDrop -= DropTargetPreviewDrop;
				targetUI.AllowDrop = false;
			}
		}

		#endregion

		#endregion

		#region Drag Source Events

		/// <summary>
		/// Called when the left mouse button is pressed while the mouse pointer is over drag source element.
		/// </summary>
		/// <param name="sender">Drag source element.</param>
		/// <param name="e">Provides data for mouse button.</param>
		private static void DragSourcePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Make this the new drag source
			currentSourceAdvisor = GetDragSourceAdvisor(sender as DependencyObject);

			// Todo: consider to pass FrameworkElement.DataContext directly instead if UIElement
			if (currentSourceAdvisor.IsDraggable(e.Source as UIElement) == false)
			{
				return;
			}

			dragElement = e.Source as UIElement;

			// Important: for global (i.e. "window root grid") adorner layer use local (to element) mouse position as start point.
			startPoint = e.GetPosition((UIElement) e.Source);

			// For local (to element) adorner layer use global mouse position as start point instead.
			//startPoint = e.GetPosition(GetTopContainer());

			isMouseDown = true;
		}

		/// <summary>
		/// Called when the left mouse button is released while the mouse pointer is over drag source element.
		/// </summary>
		/// <param name="sender">Drag source element.</param>
		/// <param name="e">Provides data for mouse button.</param>
		private static void DragSourcePreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			isMouseDown = false;
		}

		/// <summary>
		/// Called when the mouse pointer moves while the mouse pointer is over drag source element.
		/// </summary>
		/// <param name="sender">Drag source element.</param>
		/// <param name="e">Provides data for mouse button.</param>
		private static void DragSourcePreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (isMouseDown && IsDragGesture(e.GetPosition((UIElement) e.Source)))
			{
				DragStarted();
			}
		}

		/// <summary>
		/// Called when a drag-and-drop operation is started.
		/// </summary>
		/// <param name="sender">Drag source element.</param>
		/// <param name="e">Contains data about cursor and effects of the drag-and-drop operation.</param>
		private static void DragSourcePreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			// Can be used to set custom cursors
			// Note: uncomment to hide default drag'n'drop cursors.

			//e.UseDefaultCursors = false;
			//e.Handled = true;
		}

		/// <summary>
		/// Check the minimum mouse pointer distance from start point (where left mouse button was pressed)
		/// sufficient to detect drag-and-drop operation desire.
		/// </summary>
		/// <param name="point">Start point.</param>
		/// <returns>"True", if distance is sufficient.</returns>
		private static bool IsDragGesture(Point point)
		{
			bool horizontalGesture = Math.Abs(point.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance;
			bool verticalGesture = Math.Abs(point.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance;

			return horizontalGesture | verticalGesture;
		}

		/// <summary>
		/// Initiates a drag-and-drop operation and start drag item preview with mouse.
		/// </summary>
		private static void DragStarted()
		{
			isMouseDown = false;
			Mouse.Capture(dragElement);

			IDataObject data = currentSourceAdvisor.GetDataObject(dragElement);
			DragDropEffects supportedEffects = currentSourceAdvisor.SupportedEffects;

			// Perform DragDrop
			DragDropEffects effects = System.Windows.DragDrop.DoDragDrop(dragElement, data, supportedEffects);
			currentSourceAdvisor.OnDropConfirmed(dragElement, effects);

			// Clean up
			EndDragDrop();
		}

		/// <summary>
		/// Finish drag-and-drop visual tracking: release mouse and remove preview adorner.
		/// </summary>
		private static void EndDragDrop()
		{
			Mouse.Capture(null);
			RemovePreviewAdorner();
		}

		#endregion

		#region Drop Target Events

		/// <summary>
		/// Called when the input system reports an underlying drag event with drop area element as the drag target.
		/// </summary>
		/// <param name="sender">Drop area element.</param>
		/// <param name="e">Contains arguments relevant to all drag-and-drop events.</param>
		private static void DropTargetPreviewDragEnter(object sender, DragEventArgs e)
		{
			currentTargetAdvisor = GetDropTargetAdvisor(sender as DependencyObject);

			if (UpdateEffects((UIElement) sender, e) == false)
			{
				return;
			}

			Point point = CalculateAdornerOffset(e);
			CreatePreviewAdorner(point.X, point.Y);

			e.Handled = true;
		}

		/// <summary>
		/// Called when the input system reports an underlying drag event with drop area element as the potential drop target.
		/// </summary>
		/// <param name="sender">Drop area element.</param>
		/// <param name="e">Contains arguments relevant to all drag-and-drop events.</param>
		private static void DropTargetPreviewDragOver(object sender, DragEventArgs e)
		{
			if (UpdateEffects((UIElement) sender, e) == false)
			{
				return;
			}

			Point point = CalculateAdornerOffset(e);
			overlayElement.UpdatePosition(point.X, point.Y);

			e.Handled = true;
		}

		/// <summary>
		/// Called when the input system reports an underlying drag event with drop area element as the drag origin.
		/// </summary>
		/// <param name="sender">Drop area element.</param>
		/// <param name="e">Contains arguments relevant to all drag-and-drop events.</param>
		private static void DropTargetPreviewDragLeave(object sender, DragEventArgs e)
		{
			if (UpdateEffects((UIElement) sender, e) == false)
			{
				return;
			}

			RemovePreviewAdorner();

			e.Handled = true;
		}

		/// <summary>
		/// Called when the input system reports an underlying drop event with drop area element as the drop target.
		/// </summary>
		/// <param name="sender">Drop area element.</param>
		/// <param name="e">Contains arguments relevant to all drag-and-drop events.</param>
		private static void DropTargetPreviewDrop(object sender, DragEventArgs e)
		{
			if (UpdateEffects((UIElement) sender, e) == false)
			{
				return;
			}

			Point dropPoint = e.GetPosition((UIElement) sender);
			currentTargetAdvisor.OnDropAccepted((UIElement) sender, e.Data, e.Effects, dropPoint);
		}

		/// <summary>
		/// Check if drag data is valid and update drag item preview with copy or move effects.
		/// </summary>
		/// <param name="sender">Drop area element.</param>
		/// <param name="e">Contains arguments relevant to all drag-and-drop events.</param>
		/// <returns>"True", if data is valid and copy or move effects are allowed.</returns>
		private static bool UpdateEffects(UIElement sender, DragEventArgs e)
		{
			if (currentTargetAdvisor.IsValidDataObject(sender, e.Data) == false)
			{
				return false;
			}

			if ((e.AllowedEffects & DragDropEffects.Move) == 0 &&
			    (e.AllowedEffects & DragDropEffects.Copy) == 0)
			{
				e.Effects = DragDropEffects.None;
				return false;
			}

			if ((e.AllowedEffects & DragDropEffects.Move) != 0 &&
			    (e.AllowedEffects & DragDropEffects.Copy) != 0)
			{
				// Note: default drag behavior are MOVE and Ctrl+Drag is COPY.
				// Todo: make it configurable to be able to swap them
				e.Effects = ((e.KeyStates & DragDropKeyStates.ControlKey) != 0)
				            	?
				            		DragDropEffects.Copy
				            	: DragDropEffects.Move;
			}

			return true;
		}

		#endregion

		#region Utility

		/// <summary>
		/// Get the top (root) <see cref="UIElement"/> with adorner layer (for example - <see cref="Grid"/>).
		/// </summary>
		/// <returns>Element with adorner layer for drag item preview.</returns>
		private static UIElement GetTopContainer()
		{
			// Todo: determine multiple windows for drag'n'drop adorner layer.
			// Also recalculate start drag position for new window.
			return Application.Current.MainWindow.Content as UIElement;
		}

		/// <summary>
		/// Calculate adorner offset in adorner layer according 
		/// to current mouse drag position versus drag start mouse position.
		/// </summary>
		/// <param name="e">Contains arguments relevant to all drag-and-drop events.</param>
		/// <returns>Adorner offset position in adorner layer.</returns>
		private static Point CalculateAdornerOffset(DragEventArgs e)
		{
			Point position = e.GetPosition(GetTopContainer());
			return new Point(position.X - startPoint.X, position.Y - startPoint.Y);
		}

		/// <summary>
		/// Create drag-and-drop item preview adorner.
		/// </summary>
		/// <param name="left">Left offset of adorner in the adorner layer.</param>
		/// <param name="top">Top offset of adorner in the adorner layer.</param>
		private static void CreatePreviewAdorner(double left, double top)
		{
			// Clear if there is an existing adorner
			RemovePreviewAdorner();

			AdornerLayer layer = AdornerLayer.GetAdornerLayer(GetTopContainer());
			UIElement feedbackUI = currentSourceAdvisor.GetVisualFeedback(dragElement);

			// It is important to specify correct start position of adorner in the adorner layer
			// BEFORE adding it to the layer to prevent blinking when mouse leave/over different drop area elements.
			overlayElement = new DropPreviewAdorner(GetTopContainer(), feedbackUI, layer, left, top);
			layer.Add(overlayElement);
		}

		/// <summary>
		/// Remove drag-and-drop item preview adorner.
		/// </summary>
		private static void RemovePreviewAdorner()
		{
			if (overlayElement != null)
			{
				AdornerLayer.GetAdornerLayer(GetTopContainer()).Remove(overlayElement);
				overlayElement = null;
			}
		}

		#endregion
	}
}