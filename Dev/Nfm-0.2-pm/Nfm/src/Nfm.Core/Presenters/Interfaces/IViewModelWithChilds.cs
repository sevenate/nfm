// <copyright file="IViewModelWithChilds.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-26</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-26</date>
// </editor>
// <summary>General interface to view model with child view models.</summary>

using System.Collections.ObjectModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// General interface to view model with child view models.
	/// </summary>
	public interface IViewModelWithChilds : IViewModel
	{
		/// <summary>
		/// Gets childs view models.
		/// </summary>
		ObservableCollection<IViewModel> Childs { get; }

		/// <summary>
		/// Gets selected childs view models.
		/// </summary>
		ObservableCollection<IViewModel> SelectedItems { get; }

		/// <summary>
		/// Gets or sets current child item index.
		/// </summary>
		int CurrentItemIndex { get; set; }
	}
}