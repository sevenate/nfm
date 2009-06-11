// <copyright file="IPanelContainer.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-29</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-29</date>
// </editor>
// <summary>Represent a container for multiple <see cref="IPanel"/>.</summary>

using System.Collections.ObjectModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Represent a container for multiple <see cref="IPanel"/>.
	/// </summary>
	public interface IPanelContainer : IPanel
	{
		/// <summary>
		/// Gets all child panels.
		/// </summary>
		ObservableCollection<IPanel> Childs { get; }

		/// <summary>
		/// Gets or sets active child panel.
		/// </summary>
		IPanel Active { get; set; }
	}
}