// <copyright file="ModuleTheme.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-30</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-30</date>
// </editor>
// <summary>Hold all module theme's resource <see cref="ComponentResourceKey"/>.</summary>

using System.Windows;

namespace Nfm.Core.Modules.FileSystem.Resources
{
	/// <summary>
	/// Hold all module theme's resource <see cref="ComponentResourceKey"/>.
	/// </summary>
	public class ModuleTheme
	{
		#region Common for Files and Folders

		public static ComponentResourceKey FileExtensionKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ModuleTheme), "FileExtension");
			}
		}

		public static ComponentResourceKey FileDateModifiedKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ModuleTheme), "FileDateModified");
			}
		}

		public static ComponentResourceKey FileAttributeKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ModuleTheme), "FileAttribute");
			}
		}

		public static ComponentResourceKey FileAttributeSystemBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ModuleTheme), "FileAttributeSystemBrush");
			}
		}

		public static ComponentResourceKey FileAttributeHiddenBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ModuleTheme), "FileAttributeHiddenBrush");
			}
		}

		public static ComponentResourceKey FileAttributeArchiveBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ModuleTheme), "FileAttributeArchiveBrush");
			}
		}

		public static ComponentResourceKey FileAttributeReadOnlyBrushKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ModuleTheme), "FileAttributeReadOnlyBrush");
			}
		}

		#endregion
	}
}