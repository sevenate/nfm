// <copyright file="DriveVM.cs" company="HD">
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
// <summary>Logical drive view model.</summary>

using System;
using System.Diagnostics;
using System.IO;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Logical drive view model.
	/// </summary>
	[DebuggerDisplay("{Name}")]
	public class DriveVM : NotificationBase, IViewModel
	{
		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public virtual object Clone()
		{
			return new DriveVM(this);
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
		/// Fetch data from corresponding file system element node model.
		/// </summary>
		public virtual void Refresh()
		{
			OnPropertyChanging("Name");
			OnPropertyChanging("TotalSize");
			OnPropertyChanging("TotalFreeSpace");
			OnPropertyChanging("VolumeLabel");
			OnPropertyChanging("AvailableFreeSpace");
			OnPropertyChanging("IsReady");
			OnPropertyChanging("DriveFormat");
			OnPropertyChanging("DriveType");

			Drive = Model.GetDriveInfo(AbsolutePath);
			AbsolutePath = Name.TrimEnd('\\').ToUpperInvariant();

			OnPropertyChanged("Name");
			OnPropertyChanged("TotalSize");
			OnPropertyChanged("TotalFreeSpace");
			OnPropertyChanged("VolumeLabel");
			OnPropertyChanged("AvailableFreeSpace");
			OnPropertyChanged("IsReady");
			OnPropertyChanged("DriveFormat");
			OnPropertyChanged("DriveType");
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
			// TODO: add support of starting explorer with current logical drive root folder.
			throw new NotImplementedException("Open explorer with current logical drive root folder is not implemented yet.");
		}

		#endregion

		#region Navigate Into

		/// <summary>
		/// Checks if node support <see cref="IViewModel.NavigateInto"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.NavigateInto"/> action.</returns>
		public virtual bool SupportNavigateInto()
		{
			return IsReady;
		}

		/// <summary>
		/// "Navigate into" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for node in "navigated" mode.</returns>
		public virtual IPanelContent NavigateInto()
		{
			var fullVM = new DriveFullVM(Model)
			             {
			             	AbsolutePath = Drive.Name
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
			throw new NotSupportedException("NavigateOut action is not supported for logical drive in base view model.");
		}

		#endregion

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="DriveVM" /> class.
		/// </summary>
		/// <param name="model">Local file system data model.</param>
		public DriveVM(LocalFileSystemModule model)
		{
			Model = model;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DriveVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="DriveVM"/> instance to copy data from.</param>
		protected DriveVM(DriveVM another)
		{
			AbsolutePath = another.AbsolutePath;
			isSelected = another.IsSelected;
			Model = another.Model;
			Drive = another.Drive;
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
		protected DriveInfo Drive { get; set; }

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets the name of a drive.
		/// </summary>
		public string Name
		{
			get { return Drive.Name; }
		}

		/// <summary>
		/// Gets the name of a drive.
		/// </summary>
		public string DrivaLetter
		{
			get { return Name.TrimEnd('\\').ToUpperInvariant(); }
		}

		/// <summary>
		/// Gets the total size of the drive, in bytes.
		/// </summary>
		public long TotalSize
		{
			get
			{
				return IsReady
				       	? Drive.TotalSize
				       	: 0;
			}
		}

		/// <summary>
		/// Gets the total amount of free space available on a drive.
		/// </summary>
		public long TotalFreeSpace
		{
			get
			{
				return IsReady
				       	? Drive.TotalFreeSpace
				       	: 0;
			}
		}

		/// <summary>
		/// Gets the volume label of a drive.
		/// </summary>
		public string VolumeLabel
		{
			get
			{
				return IsReady
				       	? Drive.VolumeLabel
				       	: string.Empty;
			}
		}

		/// <summary>
		/// Gets the amount of available free space on a drive.
		/// </summary>
		public long AvailableFreeSpace
		{
			get
			{
				return IsReady
				       	? Drive.AvailableFreeSpace
				       	: 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether a drive is ready.
		/// </summary>
		public bool IsReady
		{
			get { return Drive.IsReady; }
		}

		/// <summary>
		/// Gets the name of the file system, such as NTFS or FAT32.
		/// </summary>
		public string DriveFormat
		{
			get
			{
				return IsReady
				       	? Drive.DriveFormat
				       	: string.Empty;
			}
		}

		/// <summary>
		/// Gets the drive type.
		/// </summary>
		public string DriveType
		{
			get { return Drive.DriveType.ToString(); }
		}

		/// <summary>
		/// Gets the amount of occupied space on a drive.
		/// </summary>
		public long OccupiedSpace
		{
			get
			{
				return IsReady
				       	? Drive.TotalSize - Drive.AvailableFreeSpace
				       	: 0;
			}
		}

		/// <summary>
		/// Gets the percentage of used drive capacity.
		/// </summary>
		public float UsedCapacity
		{
			get
			{
				return IsReady
				       	? 100*(Drive.TotalSize - Drive.AvailableFreeSpace)/(float) Drive.TotalSize
				       	: 0;
			}
		}

		#endregion
	}
}