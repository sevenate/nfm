// <copyright file="FolderVM.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-26</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-26</date>
// </editor>
// <summary>Folder view model.</summary>

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Nfm.Core.Models.FileSystem;
using Nfm.Core.Modules.FileSystem.ViewModels.Icons;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Folder view model.
	/// </summary>
	[DebuggerDisplay("{Name}")]
	public class FolderVM : NotificationBase, IViewModel
	{
		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public virtual object Clone()
		{
			return new FolderVM(this);
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>
		/// Flag value indicating whether view model is selected.
		/// </summary>
		private bool isSelected;

		/// <summary>
		/// Gets or sets absolute path.
		/// </summary>
		public string AbsolutePath { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether view model is selected.
		/// </summary>
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				OnPropertyChanging("IsSelected");
				isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}

		/// <summary>
		/// Fetch data from corresponding folder node model.
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

			Folder = Model.GetFolderInfo(AbsolutePath);

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
			return false;
		}

		/// <summary>
		/// "Execute" action.
		/// </summary>
		public virtual void Execute()
		{
			// TODO: add support of starting explorer with current folder.
			throw new NotImplementedException("Open explorer with current folder is not implemented yet.");
		}

		#endregion

		#region Navigate Into

		/// <summary>
		/// Checks if node support <see cref="IViewModel.NavigateInto"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.NavigateInto"/> action.</returns>
		public virtual bool SupportNavigateInto()
		{
			return true;
		}

		/// <summary>
		/// "Navigate into" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for node in "navigated" mode.</returns>
		public virtual IPanelContent NavigateInto()
		{
			var fullVM = new FolderFullVM(Model)
			             {
			             	AbsolutePath = Folder.FullName
			             };
			
			fullVM.Refresh();

			return fullVM;
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
			throw new NotSupportedException("NavigateOut action is not supported for folder in base view model.");
		}

		#endregion

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="FolderVM" /> class.
		/// </summary>
		/// <param name="model">Local file system data model.</param>
		public FolderVM(LocalFileSystemModule model)
		{
			Model = model;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FolderVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="FolderVM"/> instance to copy data from.</param>
		protected FolderVM(FolderVM another)
		{
			AbsolutePath = another.AbsolutePath;
			isSelected = another.IsSelected;
			Model = another.Model;
			Folder = another.Folder;
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
		protected DirectoryInfo Folder { get; set; }

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets folder name.
		/// </summary>
		public string Name
		{
			get { return Folder.Name; }
		}

		/// <summary>
		/// Gets folder creation date.
		/// </summary>
		public DateTime DateCreated
		{
			get { return Folder.CreationTime; }
		}

		/// <summary>
		/// Gets folder last modified date.
		/// </summary>
		public DateTime DateModified
		{
			get { return Folder.LastWriteTime; }
		}

		/// <summary>
		/// Gets a value indicating whether "Archive" attribute is set.
		/// </summary>
		public bool IsArchive
		{
			get { return (Folder.Attributes & FileAttributes.Archive) == FileAttributes.Archive; }
		}

		/// <summary>
		/// Gets a value indicating whether "Hidden" attribute is set.
		/// </summary>
		public bool IsHidden
		{
			get { return (Folder.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; }
		}

		/// <summary>
		/// Gets a value indicating whether "ReadOnly" attribute is set.
		/// </summary>
		public bool IsReadOnly
		{
			get { return (Folder.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly; }
		}

		/// <summary>
		/// Gets a value indicating whether "System" attribute is set.
		/// </summary>
		public bool IsSystem
		{
			get { return (Folder.Attributes & FileAttributes.System) == FileAttributes.System; }
		}

		#endregion

		#region Actions

		/// <summary>
		/// Show shell context menu for file.
		/// Todo: should be replaced with WPF-styled context menu.
		/// Todo: remove System.Windows.Forms.dll dependency.
		/// </summary>
		public void ShowContextMenu()
		{
			var scm = new ShellContextMenu();
			var files = new FileInfo[1];
			files[0] = new FileInfo(AbsolutePath);
			scm.ShowContextMenu(files, Cursor.Position);
		}

		#endregion
	}
}