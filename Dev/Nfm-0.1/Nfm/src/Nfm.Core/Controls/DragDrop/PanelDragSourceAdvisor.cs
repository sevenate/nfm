// <copyright file="PanelDragSourceAdvisor.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
//	<email>alevshoff@hd.com</email>
// 	<date>2009-04-16</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-16</date>
// </editor>
// <summary><see cref="IPanel"/> draggable source item advisor.</summary>

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// <see cref="IPanel"/> draggable source item advisor.
	/// </summary>
	public class PanelDragSourceAdvisor : IDragSourceAdvisor
	{
		/// <summary>
		/// Specify <see cref="IPanel"/> as drag data format.
		/// </summary>
		private static readonly DataFormat SupportedFormat = DataFormats.GetDataFormat("IPanel");

		/// <summary>
		/// Source <see cref="IPanelContainer"/>.
		/// </summary>
		private IPanelContainer sourceParent;

		#region IDragSourceAdvisor Members

		/// <summary>
		/// Gets or sets draggable source item element.
		/// </summary>
		public UIElement SourceUI { get; set; }

		/// <summary>
		/// Gets the effects of a drag-and-drop operation.
		/// </summary>
		public DragDropEffects SupportedEffects
		{
			get { return DragDropEffects.Copy | DragDropEffects.Move; }
		}

		/// <summary>
		/// Fetch data object from drag element.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <returns>Format-independent data transfer.</returns>
		public DataObject GetDataObject(UIElement dragElement)
		{
			var serializedObject = dragElement as FrameworkElement;

			if (serializedObject != null)
			{
				var sourcePanel = serializedObject.DataContext as IPanel;

				if (sourcePanel != null && sourcePanel.Parent is IPanelContainer)
				{
					sourceParent = (IPanelContainer) sourcePanel.Parent;
					return new DataObject(SupportedFormat.Name, sourcePanel);
				}
			}

			return null;
		}

		/// <summary>
		/// Get additional visual element,
		/// placed over draggable source item element to make drag-and-drop operation distinct.
		/// </summary>
		/// <param name="dragElement">Main drag element.</param>
		/// <returns>Additional visual feedback element.</returns>
		public UIElement GetVisualFeedback(UIElement dragElement)
		{
			var rect = new Rectangle
			           {
			           	Width = dragElement.RenderSize.Width,
			           	Height = dragElement.RenderSize.Height,
			           	Fill = new VisualBrush(dragElement),
			           	Opacity = 0.5,
			           	IsHitTestVisible = false
			           };

			// Optional animation for drag element
			var anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)))
			           {
			           	From = 0.25,
			           	AutoReverse = true,
			           	RepeatBehavior = RepeatBehavior.Forever
			           };

			rect.BeginAnimation(UIElement.OpacityProperty, anim);

			return rect;
		}

		/// <summary>
		/// Reject transfered data object on source side.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <param name="finalEffects">Final effects of a drag-and-drop operation.</param>
		public void OnDropStarted(UIElement dragElement, DragDropEffects finalEffects)
		{
			if ((finalEffects & DragDropEffects.Move) != DragDropEffects.Move)
			{
				return;
			}

			if (sourceParent == null)
			{
				return;
			}

			var panel = ((FrameworkElement) dragElement).DataContext as IPanel;

			if (panel != null && sourceParent.Childs.Contains(panel) && panel.Parent != sourceParent)
			{
				sourceParent.Childs.Remove(panel);
			}
		}

		/// <summary>
		/// Check, if draggable element has specific data to drag.
		/// </summary>
		/// <param name="element">Specific draggable element.</param>
		/// <returns>"True" if element could be drag; otherwise, "False".</returns>
		public bool IsDraggable(UIElement element)
		{
			return element is FrameworkElement && ((FrameworkElement) element).DataContext is IPanel;
		}

		#endregion
	}
}