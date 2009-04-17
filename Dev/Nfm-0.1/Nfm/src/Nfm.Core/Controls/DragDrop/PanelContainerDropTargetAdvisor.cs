// <copyright file="PanelContainerDropTargetAdvisor.cs" company="HD">
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
// <summary><see cref="IPanelContainer"/> target drop area advisor.</summary>

using System.Windows;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// <see cref="IPanelContainer"/> target drop area advisor.
	/// </summary>
	public class PanelContainerDropTargetAdvisor : IDropTargetAdvisor
	{
		/// <summary>
		/// Specify <see cref="IPanel"/> as drag data format.
		/// </summary>
		private static readonly DataFormat SupportedFormat = DataFormats.GetDataFormat("IPanel");

		#region IDropTargetAdvisor Members

		/// <summary>
		/// Gets or sets target drop area element.
		/// </summary>
		public UIElement TargetUI { get; set; }

		/// <summary>
		/// Checks to see whether the data is available and valid.
		/// </summary>
		/// <param name="obj">Format-independent mechanism for transferring data.</param>
		/// <returns>"True" if the data is in and valid; otherwise, "False".</returns>
		public bool IsValidDataObject(IDataObject obj)
		{
			if (!obj.GetDataPresent(SupportedFormat.Name))
			{
				return false;
			}

			var droppedPanel = (IPanel) obj.GetData(SupportedFormat.Name);
			var targetPanelContaiger = (IPanelContainer) ((FrameworkElement) TargetUI).DataContext;

			return ! CheckIsTargetPanelIsInsideOfSourcePanel(droppedPanel, targetPanelContaiger);
		}

		/// <summary>
		/// Accept transfered data object on target side.
		/// </summary>
		/// <param name="obj">Format-independent mechanism for transferring data.</param>
		/// <param name="finalEffects">Final effects of a drag-and-drop operation.</param>
		/// <param name="dropPoint">Specific coordinates where object was dropped.</param>
		public void OnDropAccepted(IDataObject obj, DragDropEffects finalEffects, Point dropPoint)
		{
			// Todo: take into account dropPoint coordinates to specify correct order in childs collection
			var droppedPanel = (IPanel) obj.GetData(SupportedFormat.Name);
			var targetPanelContaiger = (IPanelContainer) ((FrameworkElement) TargetUI).DataContext;

			if ((finalEffects & DragDropEffects.Move) == DragDropEffects.Move)
			{
				if (!targetPanelContaiger.Childs.Contains(droppedPanel))
				{
					// Bug: Not thread safe operation!
					// Simultaneously add new item while data binding UI enumerate collection.
					targetPanelContaiger.Childs.Add(droppedPanel);
				}
				else
				{
					int oldIndex = targetPanelContaiger.Childs.IndexOf(droppedPanel);
					targetPanelContaiger.Childs.Move(oldIndex, targetPanelContaiger.Childs.Count - 1);
				}
			}
			else if ((finalEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
			{
				// Bug: Not thread safe operation!
				// Simultaneously add new item while data binding UI enumerate collection.
				targetPanelContaiger.Childs.Add(droppedPanel.CloneDeep());
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
				return target.Parent == source || CheckIsTargetPanelIsInsideOfSourcePanel(target.Parent, source);
			}

			return false;
		}
	}
}