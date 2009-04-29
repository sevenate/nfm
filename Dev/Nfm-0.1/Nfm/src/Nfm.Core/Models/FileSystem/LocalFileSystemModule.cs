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
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Nfm.Core.Models.FileSystem
{
	/// <summary>
	/// Logical file system module.
	/// </summary>
	public class LocalFileSystemModule : IRootModule
	{
		#region Implementation of IRootModule

		/// <summary>
		/// Gets module name.
		/// </summary>
		public string Name
		{
			get { return "Local File System"; }
		}

		/// <summary>
		/// Gets unique module identification key.
		/// </summary>
		public string Key
		{
			get { return "LocalFileSystem"; }
		}

		/// <summary>
		/// Gets module description.
		/// </summary>
		public string Description
		{
			get { return "Provide access to local file system entities: logical disk drives, files and folders."; }
		}

		/// <summary>
		/// Gets module author.
		/// </summary>
		public string Author
		{
			get { return "This is built-in module."; }
		}

		/// <summary>
		/// Gets module version.
		/// </summary>
		public string Version
		{
			get
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				return asm.GetName().Version.ToString();
			}
		}

		/// <summary>
		/// Gets module copyright.
		/// </summary>
		public string Copyright
		{
			// Todo: correct company name for LFS module.
			get { return "(c) 2009 HD. All rights reserved."; }
		}

		/// <summary>
		/// Gets module homepage.
		/// </summary>
		public string Homepage
		{
			// Todo: correct homepage url for LFS module.
			get { return "http://localhost"; }
		}

		/// <summary>
		/// Gets module author's e-mail.
		/// </summary>
		public string Email
		{
			// Todo: correct e-mail for LFS module.
			get { return "nfm-lfs@localhost"; }
		}

		#endregion

		#region Drives

		/// <summary>
		/// Gets all local drives.
		/// </summary>
		public IEnumerable<DriveInfo> LocalDrives
		{
			get
			{
				// Eagerly executed: checking required parametes.

				return GetLocalDrives();
			}
		}

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over all local drives.
		/// </summary>
		/// <returns>Enumerator, which supports a simple iteratetion over all local drives.</returns>
		private static IEnumerable<DriveInfo> GetLocalDrives()
		{
			var driveInfos = new List<DriveInfo>();
			driveInfos.AddRange(DriveInfo.GetDrives());

			foreach (DriveInfo driveInfo in driveInfos)
			{
				yield return driveInfo;
			}
		}

		/// <summary>
		/// Gets specific logical drive information.
		/// </summary>
		/// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z'. A null value is not valid.</param>
		/// <returns>Logical drive info.</returns>
		public DriveInfo GetDriveInfo(string driveName)
		{
			return new DriveInfo(driveName);
		}

		#endregion

		#region Folders

		/// <summary>
		/// Gets all child sub folders in specific folder.
		/// </summary>
		/// <param name="path">Path to folder.</param>
		/// <returns>Enumerator, which supports a simple iteratetion over all child sub folders in specific folder.</returns>
		public IEnumerable<DirectoryInfo> Folders(string path)
		{
			// Eagerly executed: checking required parametes.
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			return GetFolders(path);
		}

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over all child sub folders in specific folder.
		/// </summary>
		/// <param name="path">Path to folder.</param>
		/// <returns>Enumerator, which supports a simple iteratetion over all child sub folders in specific folder.</returns>
		private static IEnumerable<DirectoryInfo> GetFolders(string path)
		{
			var list = new List<string>();
			list.AddRange(Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly));

			foreach (string folderName in list)
			{
				yield return new DirectoryInfo(folderName);
			}
		}

		/// <summary>
		/// Gets specific folder information.
		/// </summary>
		/// <param name="path">A string specifying the path on which to create the DirectoryInfo.</param>
		/// <returns>Folder information.</returns>
		public DirectoryInfo GetFolderInfo(string path)
		{
			return new DirectoryInfo(path);
		}

		#endregion

		#region Files

		/// <summary>
		/// Gets all files in specific folder.
		/// </summary>
		/// <param name="path">Path to folder.</param>
		/// <returns>Enumerator, which supports a simple iteratetion over all files in specific folder.</returns>
		public IEnumerable<FileInfo> Files(string path)
		{
			// Eagerly executed: checking required parametes.
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			return GetFiles(path);
		}

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over all files in specific folder.
		/// </summary>
		/// <param name="path">Path to folder.</param>
		/// <returns>Enumerator, which supports a simple iteratetion over all files in specific folder.</returns>
		private static IEnumerable<FileInfo> GetFiles(string path)
		{
			var list = new List<string>();
			list.AddRange(Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly));

			foreach (string fileName in list)
			{
				yield return new FileInfo(fileName);
			}
		}

		/// <summary>
		/// Gets specific file information.
		/// </summary>
		/// <param name="fileName">The fully qualified name of the new file, or the relative file name.</param>
		/// <returns>File information.</returns>
		public FileInfo GetFileInfo(string fileName)
		{
			return new FileInfo(fileName);
		}

		#endregion

		#region Common

		/// <summary>
		/// Get type of existing file system entity with provided absolute path.
		/// </summary>
		/// <param name="path">Absolute path to file system entity.</param>
		/// <returns>Corresponding file system entity type.</returns>
		public FileSystemEntityType GetEntityType(string path)
		{
			if (0 < path.Length && path.Length <= 3)
			{
				return FileSystemEntityType.Drive;
			}

			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			if (Directory.Exists(path))
			{
				return FileSystemEntityType.Directory;
			}

			if (File.Exists(path))
			{
				return FileSystemEntityType.File;
			}

			throw new ArgumentException("Incorrect argument.", "path");
		}

		/// <summary>
		/// Execute specific file (or launch default application for it extension).
		/// </summary>
		/// <param name="fileName">Executable file to start or launch default application for registered file extension.</param>
		/// <param name="startupFolder">Application start up folder.</param>
		/// <param name="arguments">Command-line arguments to use when starting the application.</param>
		public void Execute(string fileName, string startupFolder, string arguments)
		{
			// TODO: add parameter support
			var processStartInfo = new ProcessStartInfo(fileName)
			                       {
			                       	WorkingDirectory = !string.IsNullOrEmpty(startupFolder)
			                       	                   	? startupFolder
			                       	                   	: Environment.CurrentDirectory,
			                       	Arguments = arguments
			                       };

			Process.Start(processStartInfo);
		}

		#endregion
	}
}