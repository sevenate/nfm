// <copyright file="FileSystemElementNodeTest.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </editor>
// <summary>Tests for FileSystemEntityNode.</summary>

using System.Collections.Generic;
using Nfm.Core.Models;
using Nfm.Core.Models.FileSystem;
using Xunit;

namespace Nfm.Core.Tests
{
	/// <summary>
	/// Tests for FileSystemEntityNode.
	/// </summary>
	public class FileSystemElementNodeTest
	{
		[Fact]
		public void GetAllDirectoryContent()
		{
			var dir = new FileSystemEntityNode
			{
				AbsoluteName = @"D:\1"
			};

			var nodes = new List<INode>(dir.Childs);
		}
	}
}