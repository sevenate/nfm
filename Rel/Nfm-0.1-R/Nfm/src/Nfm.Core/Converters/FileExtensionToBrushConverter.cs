// <copyright file="FileExtensionToBrushConverter.cs" company="HD">
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
// <summary>Convert file extension value to <see cref="Brush"/>.</summary>

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Nfm.Core.Modules.FileSystem.Configuration;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert file extension value to <see cref="Brush"/>.
	/// </summary>
	[ValueConversion(typeof (string), typeof (Brush))]
	public class FileExtensionToBrushConverter : IValueConverter
	{
		#region Implementation of IValueConverter

		/// <summary>
		/// Converts a value. 
		/// </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				//TODO: Check, if this correct
				return null; // new SolidColorBrush();
			}

			var ext = (string) value;

			// Executable
			if (ext.Equals("exe", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.ExecutableBrush;
			}

			// Library
			if (ext.Equals("dll", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.LibraryBrush;
			}

			// Video
			if (ext.Equals("avi", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.VideoBrush;
			}

			// Audio
			if (ext.Equals("mp3", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.AudioBrush;
			}

			// Image
			if (ext.Equals("jpg", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.ImageBrush;
			}

			// Archive
			if (ext.Equals("zip", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.ArchiveBrush;
			}

			// Temporary
			if (ext.Equals("tmp", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.TemporaryBrush;
			}

			// Source
			if (ext.Equals("cs", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.SourceBrush;
			}

			// Web
			if (ext.Equals("html", StringComparison.OrdinalIgnoreCase))
			{
				return ModuleConfig.FileExtension.WebBrush;
			}

			// Normal
			return ModuleConfig.FileExtension.NormalBrush;
		}

		/// <summary>
		/// Converts a value. 
		/// </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Singleton

		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static FileExtensionToBrushConverter instance;

		/// <summary>
		/// Prevents a default instance of the <see cref="FileExtensionToBrushConverter"/> class from being created.
		/// </summary>
		private FileExtensionToBrushConverter()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static FileExtensionToBrushConverter Inst
		{
			[DebuggerStepThrough]
			get
			{
				if (instance == null)
				{
					instance = new FileExtensionToBrushConverter();
				}

				return instance;
			}
		}

		#endregion
	}
}