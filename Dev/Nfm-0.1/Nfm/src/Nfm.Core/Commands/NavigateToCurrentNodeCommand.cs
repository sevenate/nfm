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
using Caliburn.Actions.Filters;
using Nfm.Core.ViewModels.FileSystem;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Change node in panel to current node command.
	/// </summary>
	[Rescue("GeneralRescue")]
	public class NavigateToCurrentNodeCommand
	{
		/// <summary>
		/// Change node in panel to current node.
		/// </summary>
		/// <param name="panel">Specific panel with node.</param>
		/// <param name="currentNode">New node for panel.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(FileSystemEntityNodeVM panel, FileSystemEntityNodeVM currentNode)
		{
			if (panel != null)
			{
				panel.ChangeNode(currentNode);
			}
		}

		/// <summary>
		/// Check if the current node have childs to make sense in navigation.
		/// </summary>
		/// <param name="panel">Specific panel with node.</param>
		/// <param name="currentNode">New node for panel.</param>
		/// <returns>True, if the node have one or more child nodes.</returns>
		public bool CanExecute(FileSystemEntityNodeVM panel, FileSystemEntityNodeVM currentNode)
		{
			if (currentNode == null)
			{
				return false;
			}

			try
			{
				currentNode.RefreshChilds();
				return (currentNode.IsDirectory.HasValue && currentNode.IsDirectory.Value) || currentNode.Childs.Count > 1;
			}
			catch
			{
				return false;
			}
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