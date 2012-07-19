// <copyright file="IViewModel.cs" company="HD">
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
// <summary>General interface to view model of any node.</summary>

using System;
using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// General interface to view model of any node.
	/// </summary>
	public interface IViewModel : IPresenter, ICloneable
	{
		/// <summary>
		/// Gets or sets absolute path.
		/// </summary>
		string AbsolutePath { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether view model is selected.
		/// </summary>
		bool IsSelected { get; set; }

		/// <summary>
		/// Fetch data from corresponding file system element view model.
		/// </summary>
		void Refresh();

		#region Execute

		/// <summary>
		/// Checks if node support <see cref="Execute"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="Execute"/> action.</returns>
		bool SupportExecute();

		/// <summary>
		/// "Execute" action.
		/// </summary>
		void Execute();

		#endregion

		#region Navigate Into

		/// <summary>
		/// Checks if node support <see cref="NavigateInto"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="NavigateInto"/> action.</returns>
		bool SupportNavigateInto();

		/// <summary>
		/// "Navigate into" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for node in "navigated" mode.</returns>
		IPanelContent NavigateInto();

		#endregion

		#region Navigate Out

		/// <summary>
		/// Checks if node support <see cref="SupportNavigateOut"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="SupportNavigateOut"/> action.</returns>
		bool SupportNavigateOut();

		/// <summary>
		/// "Navigate out" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for parent node in "navigated" mode.</returns>
		IPanelContent NavigateOut();

		#endregion
	}
}