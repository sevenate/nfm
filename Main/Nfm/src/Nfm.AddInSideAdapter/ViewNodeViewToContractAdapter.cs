// <copyright file="ViewNodeViewToContractAdapter.cs" company="HD">
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
// <summary>"View Node" add-in view to contract adapter.</summary>

using System.AddIn.Pipeline;
using Nfm.AddInView;
using Nfm.Contract;

namespace Nfm.AddInSideAdapter
{
	/// <summary>
	/// "View Node" add-in view to contract adapter.
	/// </summary>
	[AddInAdapter]
	public class ViewNodeViewToContractAdapter : ContractBase, IViewNode
	{
		/// <summary>
		/// Add-in view.
		/// </summary>
		private readonly ViewNodeAddInView view;

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewNodeViewToContractAdapter" /> class. 
		/// </summary>
		/// <param name="view">Add-in view.</param>
		public ViewNodeViewToContractAdapter(ViewNodeAddInView view)
		{
			this.view = view;
		}

		#region Implementation of IViewNode

		/// <summary>
		/// Start view node content.
		/// </summary>
		public void View()
		{
			view.ViewNode();
		}

		#endregion
	}
}