// <copyright file="AddInManagerTest.cs" company="HD">
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
// <summary>Unit tests for AddInManager.</summary>

using Xunit;

namespace Nfm.Core.Tests
{
	/// <summary>
	/// Unit tests for AddInManager.
	/// </summary>
	public class AddInManagerTest
	{
		[Fact( Skip = "True")]
		public void Update()
		{
			AddInManager.Update();
		}

		[Fact(Skip = "True")]
		public void Find()
		{
			AddInManager.Update();
			AddInManager.Find();
		}
	}
}