// <copyright file="NavigateToCurrentNodeCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-01</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-01</date>
// </editor>
// <summary>Change node in panel to current node command.</summary>

using System;
using System.Windows;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	// Todo: rename this command to "DefaultDoubleClickAction" or something.

	/// <summary>
	/// Change node in panel to current node command.
	/// </summary>
	[Rescue("GeneralRescue")]
	public class NavigateToCurrentNodeCommand
	{
		/// <summary>
		/// Change node in panel to current node.
		/// </summary>
		/// <param name="oldContent">Current panel content.</param>
		/// <param name="newNode">New node for panel.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanelContent oldContent, IViewModel newNode)
		{
			if (newNode.SupportExecute())
			{
				newNode.Execute();
			}
			else
			{
				oldContent.Host.PanelContent = newNode.NavigateInto();
			}
		}

		/// <summary>
		/// Check if the current node have childs to make sense in navigation.
		/// </summary>
		/// <param name="oldContent">Current panel content.</param>
		/// <param name="newNode">New node for panel.</param>
		/// <returns>True, if the node have one or more child nodes.</returns>
		public bool CanExecute(IPanelContent oldContent, IViewModel newNode)
		{
			return oldContent != null && newNode != null && (newNode.SupportExecute() || newNode.SupportNavigateInto());
		}

		/// <summary>
		/// Defaul exception handler for <see cref="Execute"/> method.
		/// </summary>
		/// <param name="ex">Unhandled exception.</param>
		public void GeneralRescue(Exception ex)
		{
			//TODO: Change this temporary error handling to common custom error window.
			MessageBox.Show(ex.Message);
		}
	}
}