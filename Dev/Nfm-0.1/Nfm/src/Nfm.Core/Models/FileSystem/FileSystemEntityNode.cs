// <copyright file="FileSystemEntityNode.cs" company="HD">
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
// <summary>Represent file in the local file system.</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Nfm.Core.Models.FileSystem
{
	/// <summary>
	/// Represent file in the local file system.
	/// </summary>
	[DebuggerDisplay(
		@"Display = {DisplayName}"
		+ @", Absolute = {Key}"
		+ @", Parent = {Parent.DisplayName}"
		)]
	public class FileSystemEntityNode : INode
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
				// Eagerly executed: checking required parametes.
				if (string.IsNullOrEmpty(Key))
				{
					throw new ArgumentException("Key should not be null or empty.");
				}

				if (!EntityType.HasValue)
				{
					throw new ArgumentException("Entity type should not be null.");
				}

				return GetChilds();
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
		/// Gets file system entity details info specific for file or directory.
		/// </summary>
		public FileSystemInfo DetailsInfo { get; private set; }

		/// <summary>
		/// Gets a value indicating whether file system element is directory.
		/// </summary>
		public FileSystemEntityType? EntityType { get; private set; }

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystemEntityNode"/> class.
		/// </summary>
		public FileSystemEntityNode()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystemEntityNode"/> class.
		/// </summary>
		/// <param name="parent">Parent node.</param>
		/// <param name="key">Information about specific file or folder.</param>
		public FileSystemEntityNode(INode parent, string key)
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
			if (Directory.Exists(Key))
			{
				EntityType = FileSystemEntityType.Directory;
				DetailsInfo = new DirectoryInfo(Key);
			}
			else if (File.Exists(Key))
			{
				EntityType = FileSystemEntityType.File;
				DetailsInfo = new FileInfo(Key);
			}
			else
			{
				DetailsInfo = null;
				throw new ArgumentException(string.Format("File or directory with name \"{0}\" is not exist.", Key));
			}

			DisplayName = DetailsInfo.Name;
			Key = DetailsInfo.FullName;	//.ToLowerInvariant();
		}

		/// <summary>
		/// Set <see cref="Parent"/> instance if available (i.e. "parent directory" or root logical drive).
		/// </summary>
		public void RefreshParent()
		{
			if (DetailsInfo != null && EntityType == FileSystemEntityType.Directory)
			{
				if (((DirectoryInfo) DetailsInfo).Parent != null)
				{
					Parent = new FileSystemEntityNode
					         {
					         	Key = ((DirectoryInfo) DetailsInfo).Parent.FullName,	//.ToLowerInvariant(),
					         	EntityType = FileSystemEntityType.Directory,
					         	DetailsInfo = ((DirectoryInfo) DetailsInfo).Parent,
					         	DisplayName = ((DirectoryInfo) DetailsInfo).Parent.Name
					         };
				}
				else
				{
					// Todo: consider to use interface instead of direct LogicalDriveNode class
					Parent = new LogicalDriveNode(RootNode.Inst.GetModule("LocalFileSystem"), DetailsInfo.Name);
				}
			}
		}

		/// <summary>
		/// Execute specific file (or launch default application for it extension).
		/// </summary>
		/// <param name="startupFolder">Application start up folder.</param>
		public void Execute(string startupFolder)
		{
			// TODO: add parameter support
			var processStartInfo = new ProcessStartInfo(Key)
			                       {
			                       	WorkingDirectory = !string.IsNullOrEmpty(startupFolder)
			                       	                   	? startupFolder
			                       	                   	: Parent == null || !(Parent is FileSystemEntityNode)
			                       	                   	  	? Environment.CurrentDirectory
			                       	                   	  	: ((FileSystemEntityNode) Parent).Key
			                       };

			Process.Start(processStartInfo);
		}

		/// <summary>
		/// Gets all child nodes.
		/// </summary>
		/// <returns>Enumerator, which supports a simple iteratetion over all child nodes.</returns>
		private IEnumerable<INode> GetChilds()
		{
			if (EntityType == FileSystemEntityType.File)
			{
				// Todo: add extension point here (i.e. "content plugins").
				yield break;
			}

			// All folders
			var list = new List<string>();
			list.AddRange(Directory.GetDirectories(Key, "*.*", SearchOption.TopDirectoryOnly));

			foreach (string path in list)
			{
				var directoryInfo = new DirectoryInfo(path);

				yield return new FileSystemEntityNode
				             {
				             	Parent = this,
				             	Key = directoryInfo.FullName,	//.ToLowerInvariant(),
				             	EntityType = FileSystemEntityType.Directory,
				             	DetailsInfo = directoryInfo,
				             	DisplayName = directoryInfo.Name
				             };
			}

			// All files
			list.Clear();
			list.AddRange(Directory.GetFiles(Key, "*.*", SearchOption.TopDirectoryOnly));

			foreach (string fileName in list)
			{
				var fileInfo = new FileInfo(fileName);

				yield return new FileSystemEntityNode
				             {
				             	Parent = this,
				             	Key = fileInfo.FullName,	//.ToLowerInvariant(),
				             	EntityType = FileSystemEntityType.File,
				             	DetailsInfo = fileInfo,
				             	DisplayName = fileInfo.Name
				             };
			}
		}
	}
}