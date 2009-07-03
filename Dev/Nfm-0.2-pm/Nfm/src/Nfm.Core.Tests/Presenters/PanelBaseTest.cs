// <copyright file="PanelBaseTest.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-15</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-15</date>
// </editor>
// <summary>Unit tests for <see cref="PanelBase"/>.</summary>

using Moq;
using Nfm.Core.ViewModels;
using Xunit;

namespace Nfm.Core.Tests.ViewModels
{
	/// <summary>
	/// Unit tests for <see cref="PanelBase"/> class.
	/// </summary>
	public class PanelBaseTest
	{
		[Fact]
		public void Clone()
		{
			var panelContentMoq = new Mock<IPanelContent>();
			panelContentMoq.Setup(pc => pc.Clone()).Returns(new Mock<IPanelContent>().Object);

			var panelBase = new PanelBase
			                {
								PanelContent = panelContentMoq.Object
			                };

			var clonePanelBase = (PanelBase)panelBase.Clone();

			Assert.NotEqual(clonePanelBase.PanelContent, panelBase.PanelContent);
		}
	}
}