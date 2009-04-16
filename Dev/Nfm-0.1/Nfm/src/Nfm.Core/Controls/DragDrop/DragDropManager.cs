using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Controls.DragDrop
{
	public static class DragDropManager
	{
		#region Fields

		private static IDragSourceAdvisor currentSourceAdvisor;
		private static IDropTargetAdvisor currentTargetAdvisor;
		private static UIElement draggedElement;
		private static DropPreviewAdorner overlayElement;
		private static bool isMouseDown;
		private static Point startPoint;

		#endregion

		#region Dependency Properties

		#region DragSourceAdvisor

		public static readonly DependencyProperty DragSourceAdvisorProperty =
			DependencyProperty.RegisterAttached("DragSourceAdvisor", typeof (IDragSourceAdvisor), typeof (DragDropManager),
			                                    new FrameworkPropertyMetadata(
			                                    	new PropertyChangedCallback(OnDragSourceAdvisorChanged)));

		public static void SetDragSourceAdvisor(DependencyObject depObj, bool isSet)
		{
			depObj.SetValue(DragSourceAdvisorProperty, isSet);
		}

		public static IDragSourceAdvisor GetDragSourceAdvisor(DependencyObject depObj)
		{
			return depObj.GetValue(DragSourceAdvisorProperty) as IDragSourceAdvisor;
		}

		private static void OnDragSourceAdvisorChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			var sourceUI = depObj as UIElement;
			
			if (args.NewValue != null && args.OldValue == null)
			{
				sourceUI.PreviewMouseLeftButtonDown += DragSource_PreviewMouseLeftButtonDown;
				sourceUI.PreviewMouseMove += DragSource_PreviewMouseMove;
				sourceUI.PreviewMouseLeftButtonUp += DragSource_PreviewMouseLeftButtonUp;
				sourceUI.PreviewGiveFeedback += DragSource_PreviewGiveFeedback;

				// Set the Drag source UI
				var advisor = args.NewValue as IDragSourceAdvisor;
				advisor.SourceUI = sourceUI;
			}
			else if (args.NewValue == null && args.OldValue != null)
			{
				sourceUI.PreviewMouseLeftButtonDown -= DragSource_PreviewMouseLeftButtonDown;
				sourceUI.PreviewMouseMove -= DragSource_PreviewMouseMove;
				sourceUI.PreviewMouseLeftButtonUp -= DragSource_PreviewMouseLeftButtonUp;
				sourceUI.PreviewGiveFeedback -= DragSource_PreviewGiveFeedback;
			}
		}

		#endregion

		#region DropTargetAdvisor

		public static readonly DependencyProperty DropTargetAdvisorProperty =
			DependencyProperty.RegisterAttached("DropTargetAdvisor", typeof (IDropTargetAdvisor), typeof (DragDropManager),
			                                    new FrameworkPropertyMetadata(
			                                    	new PropertyChangedCallback(OnDropTargetAdvisorChanged)));

		public static void SetDropTargetAdvisor(DependencyObject depObj, bool isSet)
		{
			depObj.SetValue(DropTargetAdvisorProperty, isSet);
		}

		public static IDropTargetAdvisor GetDropTargetAdvisor(DependencyObject depObj)
		{

			Debug.WriteLine("GetDropTargetAdvisor: " + ((depObj as FrameworkElement).DataContext as IPanel).Header);



			return depObj.GetValue(DropTargetAdvisorProperty) as IDropTargetAdvisor;
		}

		private static void OnDropTargetAdvisorChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			var targetUI = depObj as UIElement;
			
			if (args.NewValue != null && args.OldValue == null)
			{
				targetUI.PreviewDragEnter += DropTarget_PreviewDragEnter;
				targetUI.PreviewDragOver += DropTarget_PreviewDragOver;
				targetUI.PreviewDragLeave += DropTarget_PreviewDragLeave;
				targetUI.PreviewDrop += DropTarget_PreviewDrop;
				targetUI.AllowDrop = true;

				// Set the Drag source UI
				var advisor = args.NewValue as IDropTargetAdvisor;
				advisor.TargetUI = targetUI;
			}
			else if (args.NewValue == null && args.OldValue != null)
			{
				targetUI.PreviewDragEnter -= DropTarget_PreviewDragEnter;
				targetUI.PreviewDragOver -= DropTarget_PreviewDragOver;
				targetUI.PreviewDragLeave -= DropTarget_PreviewDragLeave;
				targetUI.PreviewDrop -= DropTarget_PreviewDrop;
				targetUI.AllowDrop = false;
			}
		}

		#endregion

		#endregion

		#region Drag Source Events

		private static void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Make this the new drag source
			currentSourceAdvisor = GetDragSourceAdvisor(sender as DependencyObject);

			if (currentSourceAdvisor.IsDraggable(e.Source as UIElement) == false)
			{
				return;
			}

			draggedElement = e.Source as UIElement;
			startPoint = e.GetPosition(GetTopContainer());
			isMouseDown = true;
		}

		private static void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			isMouseDown = false;
		}

		private static void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (isMouseDown && IsDragGesture(e.GetPosition(GetTopContainer())))
			{
				DragStarted(sender);
			}
		}

		private static void DragSource_PreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			// Can be used to set custom cursors
		}

		private static bool IsDragGesture(Point point)
		{
			bool hGesture = Math.Abs(point.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance;
			bool vGesture = Math.Abs(point.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance;

			return (hGesture | vGesture);
		}

		private static void DragStarted(object uiObject)
		{
			isMouseDown = false;
			Mouse.Capture(draggedElement);

			DataObject data = currentSourceAdvisor.GetDataObject(draggedElement);


			Debug.Assert(data != null);


			DragDropEffects supportedEffects = currentSourceAdvisor.SupportedEffects;

			// Perform DragDrop
			DragDropEffects effects = System.Windows.DragDrop.DoDragDrop(draggedElement, data, supportedEffects);
			currentSourceAdvisor.FinishDrag(draggedElement, effects);

			// Clean up
			EndDragDrop();
		}

		private static void EndDragDrop()
		{
			Mouse.Capture(null);
			RemovePreviewAdorner();
		}

		#endregion

		#region Drop Target Events

		private static void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
		{
			currentTargetAdvisor = GetDropTargetAdvisor(sender as DependencyObject);


			Debug.WriteLine("Drag Enter: " + ((currentTargetAdvisor.TargetUI as FrameworkElement).DataContext as IPanel).Header);
			Debug.WriteLine("Sender Enter: " + ((sender as FrameworkElement).DataContext as IPanel).Header);



			if (UpdateEffects(sender, e) == false)
			{
				return;
			}

			CreatePreviewAdorner();

			e.Handled = true;
		}

		private static void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
		{
			if (UpdateEffects(sender, e) == false)
			{
				return;
			}

			Point position = e.GetPosition(GetTopContainer());
			overlayElement.Left = position.X - startPoint.X;
			overlayElement.Top = position.Y - startPoint.Y;

			e.Handled = true;
		}

		private static void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
		{

			Debug.WriteLine("Drag Leave: " + ((currentTargetAdvisor.TargetUI as FrameworkElement).DataContext as IPanel).Header);
			Debug.WriteLine("Sender Leave: " + ((sender as FrameworkElement).DataContext as IPanel).Header);


			if (UpdateEffects(sender, e) == false)
			{
				return;
			}

			RemovePreviewAdorner();

			e.Handled = true;
		}

		private static void DropTarget_PreviewDrop(object sender, DragEventArgs e)
		{

			Debug.WriteLine("Drag Drop: " + ((currentTargetAdvisor.TargetUI as FrameworkElement).DataContext as IPanel).Header);
			Debug.WriteLine("Sender Drop: " + ((sender as FrameworkElement).DataContext as IPanel).Header);
			
			IDropTargetAdvisor dropTargetAdvisor = GetDropTargetAdvisor(sender as DependencyObject);

			Debug.WriteLine("dropTargetAdvisor: " + ((dropTargetAdvisor.TargetUI as FrameworkElement).DataContext as IPanel).Header);


			if (UpdateEffects(sender, e) == false)
			{
				return;
			}

//			IDropTargetAdvisor dropTargetAdvisor = GetDropTargetAdvisor(sender as DependencyObject);
			Point dropPoint = e.GetPosition(sender as UIElement);

			// Calculate displacement for (Left, Top)
			Point offset = e.GetPosition(overlayElement);
			dropPoint.X = dropPoint.X - offset.X;
			dropPoint.Y = dropPoint.Y - offset.Y;

			currentTargetAdvisor.OnDropCompleted(e.Data, dropPoint);
		}

		private static bool UpdateEffects(object uiObject, DragEventArgs e)
		{
//			IDropTargetAdvisor dropTargetAdvisor = GetDropTargetAdvisor(uiObject as DependencyObject);

			if (currentTargetAdvisor.IsValidDataObject(e.Data) == false)
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
				e.Effects = ((e.KeyStates & DragDropKeyStates.ControlKey) != 0)
				            	?
				            		DragDropEffects.Copy
				            	: DragDropEffects.Move;
			}

			return true;
		}

		#endregion

		#region Utility

		private static UIElement GetTopContainer()
		{
			return Application.Current.MainWindow.Content as UIElement;
		}

		private static void CreatePreviewAdorner()
		{
			// Clear if there is an existing adorner
			RemovePreviewAdorner();

			AdornerLayer layer = AdornerLayer.GetAdornerLayer(GetTopContainer());
			UIElement feedbackUI = currentSourceAdvisor.GetVisualFeedback(draggedElement);
			overlayElement = new DropPreviewAdorner(feedbackUI, draggedElement, layer);
			layer.Add(overlayElement);
		}

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