// <copyright file="IDefaultModuleViewModel.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-28</date>
// </editor>
// <summary>Default module view model.</summary>

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Default module view model.
	/// </summary>
	public interface IDefaultModuleViewModel : IViewModel
	{
		/// <summary>
		/// Return default view model for node in specified path location within module.
		/// </summary>
		/// <param name="path">Path to node within module.</param>
		/// <returns>Default node view model.</returns>
		IViewModel GetChildViewModel(string path);
	}
}