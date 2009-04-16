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
			return obj.GetDataPresent(SupportedFormat.Name);
		}

		/// <summary>
		/// Accept transfered data object on target side.
		/// </summary>
		/// <param name="obj">Format-independent mechanism for transferring data.</param>
		/// <param name="dropPoint">Specific coordinates where object was dropped.</param>
		public void OnDropCompleted(IDataObject obj, Point dropPoint)
		{
			var droppedPanel = obj.GetData(SupportedFormat.Name) as IPanel;
			var targetPanelContaiger = (IPanelContainer) ((FrameworkElement) TargetUI).DataContext;

			if (!targetPanelContaiger.Childs.Contains(droppedPanel))
			{
				// Bug: NOT Thread safe operation!
				// Simultaneously add item to it and data binding UI enumerate it.
				targetPanelContaiger.Childs.Add(droppedPanel);
			}
			else
			{
				var oldIndex = targetPanelContaiger.Childs.IndexOf(droppedPanel);
				targetPanelContaiger.Childs.Move(oldIndex, targetPanelContaiger.Childs.Count - 1);
			}
		}

		#endregion
	}
}