// <copyright file="LogicalDriveNode.cs" company="HD">
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
// <summary>Represent logical drive in the local file system.</summary>

using System;
using System.Collections.Generic;
using System.IO;

namespace Nfm.Core.Models.FileSystem
{
	/// <summary>
	/// Represent logical drive in the local file system.
	/// </summary>
	public class LogicalDriveNode : INode
	{
		#region Implementation of INode

		/// <summary>
		/// Gets node display name.
		/// </summary>
		public string DisplayName { get; private set; }

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
				if (string.IsNullOrEmpty(Key))
				{
					throw new ArgumentException("Key should not be null or empty.");
				}

				return GetRootFolderContent();
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
		public string Key { get; set; }

		#endregion

		/// <summary>
		/// Gets drive details info.
		/// </summary>
		public DriveInfo DriveInfo { get; private set; }

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalDriveNode"/> class.
		/// </summary>
		public LogicalDriveNode()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalDriveNode"/> class.
		/// </summary>
		/// <param name="parent">Parent node.</param>
		/// <param name="key">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z'. A null value is not valid.</param>
		public LogicalDriveNode(INode parent, string key)
		{
			Parent = parent;
			Key = key;
			RefreshDetails();
		}

		#endregion

		/// <summary>
		/// Fetch updated details info about file system entity.
		/// </summary>
		public void RefreshDetails()
		{
			DriveInfo = new DriveInfo(Key);
			Key = DriveInfo.Name.ToLowerInvariant();
			DisplayName = string.Format("{0} ({1})", DriveInfo.VolumeLabel, DriveInfo.Name.TrimEnd('\\').ToUpperInvariant());
		}

		/// <summary>
		/// Gets all root folder nodes (sub folders and files nodes).
		/// </summary>
		/// <returns>Enumerator, which supports a simple iteratetion over all root folder nodes.</returns>
		private IEnumerable<INode> GetRootFolderContent()
		{
			var rootDriveFolder = new FileSystemEntityNode(this, DriveInfo.RootDirectory.FullName);
			rootDriveFolder.RefreshDetails();
			return rootDriveFolder.Childs;
		}
	}
}