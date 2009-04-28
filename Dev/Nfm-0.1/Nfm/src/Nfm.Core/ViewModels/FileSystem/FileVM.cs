// <copyright file="FileVM.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-27</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-27</date>
// </editor>
// <summary>File view model.</summary>

using System;
using System.Diagnostics;
using System.IO;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// File view model.
	/// </summary>
	[DebuggerDisplay("{Name}")]
	public class FileVM : NotificationBase, IViewModel
	{
		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public virtual object Clone()
		{
			return new FileVM(this);
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>
		/// Gets or sets absolute path.
		/// </summary>
		public string AbsolutePath { get; set; }

		/// <summary>
		/// Fetch data from corresponding file system element node model.
		/// </summary>
		public virtual void Refresh()
		{
			OnPropertyChanging("Name");
			OnPropertyChanging("DateCreated");
			OnPropertyChanging("DateModified");
			OnPropertyChanging("IsArchive");
			OnPropertyChanging("IsHidden");
			OnPropertyChanging("IsReadOnly");
			OnPropertyChanging("IsSystem");

			File = Model.GetFileInfo(AbsolutePath);

			OnPropertyChanged("Name");
			OnPropertyChanged("DateCreated");
			OnPropertyChanged("DateModified");
			OnPropertyChanged("IsArchive");
			OnPropertyChanged("IsHidden");
			OnPropertyChanged("IsReadOnly");
			OnPropertyChanged("IsSystem");
		}

		#region Execute

		/// <summary>
		/// Checks if node support <see cref="IViewModel.Execute"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.Execute"/> action.</returns>
		public virtual bool SupportExecute()
		{
			return true;
		}

		/// <summary>
		/// "Execute" action.
		/// </summary>
		public virtual void Execute()
		{
			Model.Execute(File.FullName, File.DirectoryName, string.Empty);
		}

		#endregion

		#region Navigate Into

		/// <summary>
		/// Checks if node support <see cref="IViewModel.NavigateInto"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.NavigateInto"/> action.</returns>
		public virtual bool SupportNavigateInto()
		{
			return false;
		}

		/// <summary>
		/// "Navigate into" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for node in "navigated" mode.</returns>
		public virtual IPanelContent NavigateInto()
		{
			// TODO: Extension point here - "archive" add-ins model.
			throw new NotImplementedException("Archive add-ins model is not implemented yet.");
		}

		#endregion

		#region Navigate Out

		/// <summary>
		/// Checks if node support <see cref="IViewModel.SupportNavigateOut"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.SupportNavigateOut"/> action.</returns>
		public virtual bool SupportNavigateOut()
		{
			return false;
		}

		/// <summary>
		/// "Navigate out" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for parent node in "navigated" mode.</returns>
		public virtual IPanelContent NavigateOut()
		{
			// TODO: Extension point here - "archive" add-ins model.
			throw new NotImplementedException("Archive add-ins model is not implemented yet.");
		}

		#endregion

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="FileVM" /> class.
		/// </summary>
		/// <param name="model">Local file system data model.</param>
		public FileVM(LocalFileSystemModule model)
		{
			Model = model;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="FileVM"/> instance to copy data from.</param>
		protected FileVM(FileVM another)
		{
			Model = another.Model;
			File = another.File;
		}

		#endregion

		#region Model Data

		/// <summary>
		/// Gets or sets corresponding node model.
		/// </summary>
		protected LocalFileSystemModule Model { get; set; }

		/// <summary>
		/// Gets or sets folder details info.
		/// </summary>
		protected FileInfo File { get; set; }

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets file name.
		/// </summary>
		public string Name
		{
			get { return Path.GetFileNameWithoutExtension(File.Name); }
		}

		/// <summary>
		/// Gets file creation date.
		/// </summary>
		public DateTime DateCreated
		{
			get { return File.CreationTime; }
		}

		/// <summary>
		/// Gets file last modified date.
		/// </summary>
		public DateTime DateModified
		{
			get { return File.LastWriteTime; }
		}

		/// <summary>
		/// Gets a value indicating whether "Archive" attribute is set.
		/// </summary>
		public bool IsArchive
		{
			get { return (File.Attributes & FileAttributes.Archive) == FileAttributes.Archive; }
		}

		/// <summary>
		/// Gets a value indicating whether "Hidden" attribute is set.
		/// </summary>
		public bool IsHidden
		{
			get { return (File.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; }
		}

		/// <summary>
		/// Gets a value indicating whether "ReadOnly" attribute is set.
		/// </summary>
		public bool IsReadOnly
		{
			get { return (File.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly; }
		}

		/// <summary>
		/// Gets a value indicating whether "System" attribute is set.
		/// </summary>
		public bool IsSystem
		{
			get { return (File.Attributes & FileAttributes.System) == FileAttributes.System; }
		}

		#region Additional

		/// <summary>
		/// Gets file extension.
		/// </summary>
		public string Extension
		{
			get { return File.Extension.TrimStart('.'); }
		}

		/// <summary>
		/// Gets a value indicating whether a file has an extension.
		/// </summary>
		public bool HasExtension
		{
			get { return !string.IsNullOrEmpty(File.Extension); }
		}

		/// <summary>
		/// Gets file size.
		/// </summary>
		public long Size
		{
			get { return File.Length; }
		}

		#endregion

		#endregion
	}
}