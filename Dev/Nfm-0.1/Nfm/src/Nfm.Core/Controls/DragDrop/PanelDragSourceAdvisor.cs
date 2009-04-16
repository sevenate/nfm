using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Controls.DragDrop
{
	public class PanelDragSourceAdvisor : IDragSourceAdvisor
	{
		private static readonly DataFormat SupportedFormat = DataFormats.GetDataFormat("IPanel");

		private IPanelContainer sourceParent;

		#region IDragSourceAdvisor Members

		public DragDropEffects SupportedEffects
		{
			get { return DragDropEffects.Copy | DragDropEffects.Move; }
		}

		public UIElement SourceUI { get; set; }

		public DataObject GetDataObject(UIElement draggedElement)
		{
			var serializedObject = draggedElement as FrameworkElement;

			if (serializedObject != null)
			{
				var sourcePanel = serializedObject.DataContext as IPanel;

				if (sourcePanel != null && sourcePanel.Parent is IPanelContainer)
				{
					sourceParent = (IPanelContainer)sourcePanel.Parent;
					return new DataObject(SupportedFormat.Name, sourcePanel);
				}
			}
			
			return null;
		}

		public UIElement GetVisualFeedback(UIElement draggedElement)
		{
			var rect = new Rectangle
			           {
			           	Width = draggedElement.RenderSize.Width,
			           	Height = draggedElement.RenderSize.Height,
			           	Fill = new VisualBrush(draggedElement),
			           	Opacity = 0.5,
			           	IsHitTestVisible = false
			           };

			// Optional animation for dragged element
			var anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)))
			           {
			           	From = 0.25,
			           	AutoReverse = true,
			           	RepeatBehavior = RepeatBehavior.Forever
			           };

			rect.BeginAnimation(UIElement.OpacityProperty, anim);

			return rect;
		}

		public void FinishDrag(UIElement draggedElement, DragDropEffects finalEffects)
		{
			if ((finalEffects & DragDropEffects.Move) == DragDropEffects.Move)
			{
//				var panelContainer = ((IPanel) ((FrameworkElement) SourceUI).DataContext).Parent as IPanelContainer;

				if (sourceParent != null)
				{
					var panel = ((FrameworkElement) draggedElement).DataContext as IPanel;

					if (panel != null && sourceParent.Childs.Contains(panel) && panel.Parent != sourceParent)
					{
						sourceParent.Childs.Remove(panel);
					}
				}
			}
		}


		public bool IsDraggable(UIElement element)
		{
			return element is FrameworkElement && ((FrameworkElement) element).DataContext is IPanel;
		}

		#endregion

		public PanelDragSourceAdvisor()
		{
			;
		}
	}
}