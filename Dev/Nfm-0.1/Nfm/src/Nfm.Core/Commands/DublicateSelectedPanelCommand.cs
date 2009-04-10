// <copyright file="DublicateSelectedPanelCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-10</date>
// </editor>
// <summary>Dublicate selected panel command.</summary>

using System.Linq;
using Caliburn.Actions.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Dublicate selected panel command.
	/// </summary>
	public class DublicateSelectedPanelCommand
	{
		/// <summary>
		/// Dublicate selected panel in the specific container.
		/// </summary>
		/// <param name="container">Ccntainer with selected panel.</param>
		[Preview("CanExecute")]
		public void Execute(IPanelContainer container)
		{
			var selectedPanel = container.Childs.Where(child => child.IsSelected).FirstOrDefault();

			if (selectedPanel != null)
			{
				container.Childs.Add(selectedPanel.CloneDeep());
			}
		}

		/// <summary>
		/// Check if the selected panel can be dublicated.
		/// </summary>
		/// <param name="container">Ccntainer with selected panel.</param>
		/// <returns>True, if at least one selected child panel exist and it can be dublicated in the container.</returns>
		public bool CanExecute(IPanelContainer container)
		{
			return container != null && container.Childs.Where(child => child.IsSelected).FirstOrDefault() != null;
		}
	}
}