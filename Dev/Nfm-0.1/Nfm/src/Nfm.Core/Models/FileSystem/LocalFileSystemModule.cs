// <copyright file="LocalFileSystemModule.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-14</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-14</date>
// </editor>
// <summary>Logical file system module.</summary>

using System;
using System.Collections.Generic;
using System.IO;

namespace Nfm.Core.Models.FileSystem
{
	/// <summary>
	/// Logical file system module.
	/// </summary>
	public class LocalFileSystemModule : INode
	{
		#region Implementation of INode

		/// <summary>
		/// Gets node display name.
		/// </summary>
		public string DisplayName
		{
			get { return "Local File System"; }
		}

		/// <summary>
		/// Gets parent node.
		/// </summary>
		public INode Parent { get; private set; }

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over all child nodes.
		/// </summary>
		public IEnumerable<INode> Childs
		{
			get
			{
				// Eagerly executed: checking required parametes here.

				return GetLocalDrives();
			}
		}

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over node attributes.
		/// </summary>
		public IEnumerable<INodeAttribute> Attributes
		{
			get
			{
				// TODO: add support of INodeAttribute
				yield break;
			}
		}

		/// <summary>
		/// Gets or sets unique node identification key.
		/// </summary>
		public string Key
		{
			get { return "LocalFileSystem"; }
			set { throw new NotSupportedException("Local file system node key is constant."); }
		}

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalFileSystemModule"/> class.
		/// </summary>
		/// <param name="parent">Parent node (root node usually).</param>
		public LocalFileSystemModule(INode parent)
		{
			Parent = parent;
		}

		#endregion

		/// <summary>
		/// Gets all local drive nodes.
		/// </summary>
		/// <returns>Enumerator, which supports a simple iteratetion over all local drive nodes.</returns>
		private IEnumerable<INode> GetLocalDrives()
		{
			var driveInfos = new List<DriveInfo>();
			driveInfos.AddRange(DriveInfo.GetDrives());

			foreach (var driveInfo in driveInfos)
			{
				yield return new LogicalDriveNode(this, driveInfo.Name);
			}
		}
	}
}