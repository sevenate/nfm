// <copyright file="PanelDropTargetAdvisor.cs" company="HD">
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
// <summary><see cref="IPanel"/> target drop area advisor.</summary>

using System.Windows;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// <see cref="IPanel"/> target drop area advisor.
	/// </summary>
	public class PanelDropTargetAdvisor : IDropTargetAdvisor
	{
		/// <summary>
		/// Specify <see cref="IPanel"/> as drag data format.
		/// </summary>
		private static readonly DataFormat SupportedFormat = DataFormats.GetDataFormat("IPanel");

		#region IDropTargetAdvisor Members

		/// <summary>
		/// Checks to see whether the data is available and valid.
		/// </summary>
		/// <param name="dropAreaElement">Drop area element.</param>
		/// <param name="obj">Format-independent mechanism for transferring data.</param>
		/// <returns>"True" if the data is in and valid; otherwise, "False".</returns>
		public bool IsValidDataObject(UIElement dropAreaElement, IDataObject obj)
		{
			if (!obj.GetDataPresent(SupportedFormat.Name))
			{
				return false;
			}

			var sourcePanel = (IPanel) obj.GetData(SupportedFormat.Name);
			var targetPanel = (IPanel) ((FrameworkElement) dropAreaElement).DataContext;

			// Make panel under mouse cursor selected to be able to drop inside it child panels.
			if (!targetPanel.IsSelected && targetPanel.Parent is IPanelContainer)
			{
//				var dispatcherTimer = new DispatcherTimer
//				                      {
//				                      	Interval = TimeSpan.FromSeconds(1.25),
////				                      	IsEnabled = true,
//				                      };
//
//				dispatcherTimer.Tick += delegate
//				                        {
				foreach (IPanel panel in ((IPanelContainer) targetPanel.Parent).Childs)
				{
					if (panel.IsSelected)
					{
						panel.IsSelected = false;
					}
				}

				targetPanel.IsSelected = true;
//											dispatcherTimer.Stop();
//				                        };
//
//				dispatcherTimer.Start();
			}

			return ! CheckIsTargetPanelIsInsideOfSourcePanel(sourcePanel, targetPanel);
		}

		/// <summary>
		/// Accept transfered data object on target side.
		/// </summary>
		/// <param name="dropAreaElement">Drop area element.</param>
		/// <param name="obj">Format-independent mechanism for transferring data.</param>
		/// <param name="finalEffects">Final effects of a drag-and-drop operation.</param>
		/// <param name="dropPoint">Specific coordinates where object was dropped.</param>
		public void OnDropAccepted(UIElement dropAreaElement, IDataObject obj, DragDropEffects finalEffects, Point dropPoint)
		{
			var targetPanel = (IPanel) ((FrameworkElement) dropAreaElement).DataContext;

			// If drop target area has
			if (targetPanel.Parent is IPanelContainer)
			{
				var sourcePanel = (IPanel) obj.GetData(SupportedFormat.Name);
				double actualDropAreaWidth = ((FrameworkElement) dropAreaElement).ActualWidth;
				var targetParentContainer = (IPanelContainer) targetPanel.Parent;
				int targetIndex = targetParentContainer.Childs.IndexOf(targetPanel);
				bool insertBeforeTarget = dropPoint.X < actualDropAreaWidth/2;

				if ((finalEffects & DragDropEffects.Move) == DragDropEffects.Move)
				{
					if (!targetParentContainer.Childs.Contains(sourcePanel))
					{
						// Bug: Not thread safe operation!
						// Simultaneously add new item while data binding UI enumerate collection.
						if (insertBeforeTarget)
						{
							targetParentContainer.Childs.Insert(targetIndex, sourcePanel);
						}
						else
						{
							targetParentContainer.Childs.Insert(targetIndex + 1, sourcePanel);
						}
					}
					else
					{
						int oldIndex = targetParentContainer.Childs.IndexOf(sourcePanel);

						if (insertBeforeTarget)
						{
							// Todo: select moved tab
							var prev = oldIndex < targetIndex
							           	? targetIndex - 1
										: targetIndex;
							targetParentContainer.Childs.Move(oldIndex, prev);
						}
						else
						{
							var next = oldIndex < targetIndex 
										? targetIndex < targetParentContainer.Childs.Count - 1
											? targetIndex + 1
											: targetIndex
										: oldIndex != targetIndex
											? targetIndex + 1
											: targetIndex;
							targetParentContainer.Childs.Move(oldIndex, next);
						}
					}
				}
				else if ((finalEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
				{
					// Bug: Not thread safe operation!
					// Simultaneously add new item while data binding UI enumerate collection.

					if (insertBeforeTarget)
					{
						targetParentContainer.Childs.Insert(targetIndex, (IPanel) sourcePanel.Clone());
					}
					else
					{
						targetParentContainer.Childs.Insert(targetIndex + 1, (IPanel) sourcePanel.Clone());
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Prevent from copy/move outer source panel to inner target panel
		/// if they linked through parental hierarchy.
		/// </summary>
		/// <param name="source">Source <see cref="IPanel"/>.</param>
		/// <param name="target">Target <see cref="IPanel"/>.</param>
		/// <returns>"True", if target panel contains in source panel.</returns>
		private static bool CheckIsTargetPanelIsInsideOfSourcePanel(IPanel source, IPanel target)
		{
			if (target.Parent != null)
			{
				return target.Parent == source || CheckIsTargetPanelIsInsideOfSourcePanel(source, target.Parent);
			}

			return false;
		}
	}
}