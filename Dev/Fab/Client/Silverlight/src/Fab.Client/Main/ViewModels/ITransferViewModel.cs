// <copyright file="ITransferViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-19</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-19</date>
// </editor>
// <summary>General transfer view model interface..</summary>

using Fab.Client.ApiServiceReference;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// General transfer view model interface..
	/// </summary>
	public interface ITransferViewModel
	{
		/// <summary>
		/// Open specific transfer transaction to edit.
		/// </summary>
		/// <param name="transaction">Transaction to edit.</param>
		void Edit(Transaction transaction);
	}
}