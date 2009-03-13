// <copyright file="TestViewNode.cs" company="HD">
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
// <summary>Test Add-In.</summary>

using System.AddIn;
using Nfm.AddInView;

namespace Nfm.TestViewNodeAddIn
{
	/// <summary>
	/// Test Add-In.
	/// </summary>
	[AddIn("Test Node View", Version = "1.0.0.0", Publisher = "HD", Description = "Test add-in for NFM.")]
	public class TestViewNode : ViewNodeAddInView
	{
		#region Overrides of ViewNodeAddInView

		/// <summary>
		/// Start view node content.
		/// </summary>
		public override void ViewNode()
		{
			// TODO: Add-in implementation here
			return;
		}

		#endregion
	}
}