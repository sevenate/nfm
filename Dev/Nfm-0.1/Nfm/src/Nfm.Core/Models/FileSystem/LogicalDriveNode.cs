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

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalDriveNode"/> class.
		/// </summary>
		/// <param name="parent">Parent node.</param>
		/// <param name="driveInfo">Information about specific drive.</param>
		public LogicalDriveNode(INode parent, DriveInfo driveInfo)
		{
			Parent = parent;
			DriveInfo = driveInfo;
			DisplayName = DriveInfo.Name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalDriveNode"/> class.
		/// </summary>
		/// <param name="parent">Parent node.</param>
		/// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z'. A null value is not valid.</param>
		public LogicalDriveNode(INode parent, string driveName)
		{
			Parent = parent;
			DisplayName = driveName;
			RefreshDetails();
		}

		#endregion

		/// <summary>
		/// Gets drive details info.
		/// </summary>
		public DriveInfo DriveInfo { get; private set; }

		/// <summary>
		/// Gets all root folder nodes (sub folders and files nodes).
		/// </summary>
		/// <returns>Enumerator, which supports a simple iteratetion over all root folder nodes.</returns>
		private IEnumerable<INode> GetRootFolderContent()
		{
			var rootDriveFolder = new FileSystemEntityNode(this, DriveInfo.RootDirectory);
			rootDriveFolder.RefreshDetails();
			return rootDriveFolder.Childs;
		}

		/// <summary>
		/// Fetch updated details info about file system entity.
		/// </summary>
		public void RefreshDetails()
		{
			DriveInfo = new DriveInfo(DisplayName);
		}
	}
}