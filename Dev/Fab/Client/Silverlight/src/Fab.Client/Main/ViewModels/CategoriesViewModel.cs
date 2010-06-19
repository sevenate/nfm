// <copyright file="CategoriesViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-12</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-12</date>
// </editor>
// <summary>Categories view model.</summary>

using System;
using System.Collections.Generic;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.RoutedMessaging;
using Caliburn.PresentationFramework.ViewModels;
using Caliburn.ShellFramework.Results;
using Fab.Client.ApiServiceReference;
using Fab.Client.Models;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// Categories view model.
	/// </summary>
	[Singleton(typeof(ICategoriesViewModel))]
	public class CategoriesViewModel : BaseViewModel, ICategoriesViewModel
	{
		#region Fields

		/// <summary>
		/// Accounts owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="CategoriesViewModel"/> class.
		/// </summary>
		/// <param name="validator">Validator for view model data.</param>
		public CategoriesViewModel(IValidator validator)
			: base(validator)
		{
			Categories = new BindableCollection<Category>();
		}

		#endregion

		#region Implementation of ICategoriesViewModel

		/// <summary>
		/// Gets categories for specific user.
		/// </summary>
		public IObservableCollection<Category> Categories { get; private set; }

		/// <summary>
		/// Download all categories for specific user.
		/// </summary>
		/// <returns>Operation result.</returns>
		public IEnumerable<IResult> LoadAllCategories()
		{
			yield return Show.Busy(new BusyScreen { Message = "Loading..." });

			var request = new CategoriesResult(userId);
			yield return request;

			Categories.Clear();
			Categories.AddRange(request.Categories);

			if (Reloaded != null)
			{
				Reloaded(this, EventArgs.Empty);
			}

			yield return Show.NotBusy();
		}

		/// <summary>
		/// Raised right after categories were reloaded from server.
		/// </summary>
		public event EventHandler<EventArgs> Reloaded;

		#endregion
	}
}