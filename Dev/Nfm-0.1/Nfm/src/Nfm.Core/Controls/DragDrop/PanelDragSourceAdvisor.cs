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
		/// Source parent <see cref="IPanelContainer"/>.
		/// </summary>
		private IPanelContainer parentContainer;

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
		public IDataObject GetDataObject(UIElement dragElement)
		{
			// Note: consider to replace this method with strong type vertion for FrameworkElement
			// (i.e. "drag'n'drop for elements with DataContext only")
			var sourcePanel = (IPanel) ((FrameworkElement) dragElement).DataContext;

			// In case when source panel has parent panel container,
			// it may be required to remove it (later, after drop) from parent panel container childs collection
			// if "drop move" operation will be used.
			// For "drop copy" operation cloning will be used.
			parentContainer = sourcePanel.Parent is IPanelContainer
			                        	? (IPanelContainer) sourcePanel.Parent
			                        	: null;

			return new DataObject(SupportedFormat.Name, sourcePanel);
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
			           	Opacity = 0.75,
			           	IsHitTestVisible = false
			           };

			// Optional animation for drag element
			var anim = new DoubleAnimation(1.00, new Duration(TimeSpan.FromMilliseconds(500)))
			           {
			           	From = 0.75,
			           	AutoReverse = true,
			           	RepeatBehavior = RepeatBehavior.Forever
			           };

			rect.BeginAnimation(UIElement.OpacityProperty, anim);

			return rect;
		}

		/// <summary>
		/// Check, if draggable element has specific data to drag.
		/// </summary>
		/// <param name="element">Specific draggable element.</param>
		/// <returns>"True" if element could be drag; otherwise, "False".</returns>
		public bool IsDraggable(UIElement element)
		{
			// Todo: consider to pass FrameworkElement.DataContext directly instead if UIElement
			return element is FrameworkElement && ((FrameworkElement) element).DataContext is IPanel;
		}

		/// <summary>
		/// Confirm acceptance of successfully drop transfered data object to target area on source side.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <param name="finalEffects">Final effects of a drag-and-drop operation.</param>
		public void OnDropConfirmed(UIElement dragElement, DragDropEffects finalEffects)
		{
			if ((finalEffects & DragDropEffects.Move) != DragDropEffects.Move)
			{
				return;
			}

			// For IPanel (instead of IPanelContainer) parent there is no need anything to do.
			if (parentContainer == null)
			{
				return;
			}

			var panel = (IPanel) ((FrameworkElement) dragElement).DataContext;

			if (parentContainer.Childs.Contains(panel) && panel.Parent != parentContainer)
			{
				// This will also remove handlers from old parent panel container "Closing/Closed" events.
				parentContainer.Childs.Remove(panel);

				// Todo: consider to use empty panel containers and make it configurable.
				if (parentContainer.Childs.Count == 0)
				{
					parentContainer.RequestClose();
				}
			}
		}

		#endregion
	}
}