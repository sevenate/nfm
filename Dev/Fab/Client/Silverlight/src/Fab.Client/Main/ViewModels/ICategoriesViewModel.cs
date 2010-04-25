// <copyright file="ICategoriesViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-13</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-13</date>
// </editor>
// <summary>General categories view model interface.</summary>

using System.Collections.Generic;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.ApiServiceReference;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// General categories view model interface.
	/// </summary>
	public interface ICategoriesViewModel
	{
		/// <summary>
		/// Gets categories for specific user.
		/// </summary>
		IObservableCollection<Category> Categories { get; }

		/// <summary>
		/// Download all categories for specific user.
		/// </summary>
		/// <returns>Operation result.</returns>
		IEnumerable<IResult> LoadAllCategories();
	}
}