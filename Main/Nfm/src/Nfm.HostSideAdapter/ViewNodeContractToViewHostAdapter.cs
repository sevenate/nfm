// <copyright file="ViewNodeContractToViewHostAdapter.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-07</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-07</date>
// </editor>
// <summary>IViewNode host adapter.</summary>

using System.AddIn.Pipeline;
using Nfm.Contract;
using Nfm.HostView;

namespace Nfm.HostSideAdapter
{
	/// <summary>
	/// IViewNode host adapter.
	/// </summary>
	[HostAdapter]
	public class ViewNodeContractToViewHostAdapter : ViewNodeHostView
	{
		private readonly IViewNode contract;
		private ContractHandle contractHandle;

		public ViewNodeContractToViewHostAdapter(IViewNode contract)
		{
			this.contract = contract;
			contractHandle = new ContractHandle(contract);
		}

		#region Overrides of ViewNodeHostView

		/// <summary>
		/// Start view node content.
		/// </summary>
		public override void View()
		{
			contract.View();
		}

		#endregion
	}
}