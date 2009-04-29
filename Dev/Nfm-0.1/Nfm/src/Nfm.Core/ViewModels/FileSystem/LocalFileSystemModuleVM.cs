// <copyright file="LocalFileSystemModuleVM.cs" company="HD">
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
// <summary>Logical file system module view model.</summary>

using System;
using System.Diagnostics;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Logical file system module view model.
	/// </summary>
	[DebuggerDisplay("{Name}")]
	public class LocalFileSystemModuleVM : NotificationBase, IDefaultModuleViewModel
	{
		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public virtual object Clone()
		{
			return new LocalFileSystemModuleVM(this);
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>
		/// Gets or sets absolute path.
		/// </summary>
		public string AbsolutePath
		{
			get { return Model.Key; }
			set { throw new NotSupportedException("Absolute path is read only for module."); }
		}

		/// <summary>
		/// Fetch data from corresponding file system element view model.
		/// </summary>
		public virtual void Refresh()
		{
			// Nothing to do here.
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
			// Todo: consider using "execute" for LFS module as additional usefull action (like launching Computer Managment MMC snap-in or some thing).
			throw new NotImplementedException("Execute action for local file system module is not implemented yet.");
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
			var vm = new LocalFileSystemModuleFullVM(Model);
			vm.Refresh();
			return vm;
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
			throw new NotSupportedException(
				"NavigateOut action is not supported for local file system module in base view model.");
		}

		#endregion

		#endregion

		#region Implementation of IDefaultModuleViewModel

		/// <summary>
		/// Return default view model for node in specified path location.
		/// </summary>
		/// <param name="path">Path to node.</param>
		/// <returns>Default node view model.</returns>
		public IViewModel GetChildViewModel(string path)
		{
			switch (Model.GetEntityType(path))
			{
				case FileSystemEntityType.Directory:
					var folder = new FolderFullVM(Model)
					             {
					             	AbsolutePath = path
					             };
					return folder;

					// Todo: add support of FileFullVM.
				case FileSystemEntityType.File:
					var file = new FolderFullVM(Model)
					           {
					           	AbsolutePath = path
					           };
					return file;

				default: // FileSystemEntityType.Drive
					var drive = new DriveFullVM(Model)
					            {
					            	AbsolutePath = path
					            };
					return drive;
			}
		}

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalFileSystemModuleVM" /> class.
		/// </summary>
		/// <param name="model">Local file system data model.</param>
		public LocalFileSystemModuleVM(LocalFileSystemModule model)
		{
			Model = model;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalFileSystemModuleVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="LocalFileSystemModuleVM"/> instance to copy data from.</param>
		protected LocalFileSystemModuleVM(LocalFileSystemModuleVM another)
		{
			Model = another.Model;
		}

		#endregion

		#region Model Data

		/// <summary>
		/// Gets corresponding node model.
		/// </summary>
		protected LocalFileSystemModule Model { get; private set; }

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets module name.
		/// </summary>
		public string Name
		{
			get { return Model.Name; }
		}

		/// <summary>
		/// Gets unique module identification key.
		/// </summary>
		public string Key
		{
			get { return Model.Key; }
		}

		/// <summary>
		/// Gets module description.
		/// </summary>
		public string Description
		{
			get { return Model.Description; }
		}

		/// <summary>
		/// Gets module author.
		/// </summary>
		public string Author
		{
			get { return Model.Author; }
		}

		/// <summary>
		/// Gets module version.
		/// </summary>
		public string Version
		{
			get { return Model.Version; }
		}

		/// <summary>
		/// Gets module copyright.
		/// </summary>
		public string Copyright
		{
			get { return Model.Copyright; }
		}

		/// <summary>
		/// Gets module homepage.
		/// </summary>
		public string Homepage
		{
			get { return Model.Homepage; }
		}

		/// <summary>
		/// Gets module author's e-mail.
		/// </summary>
		public string Email
		{
			get { return Model.Email; }
		}

		#endregion
	}
}