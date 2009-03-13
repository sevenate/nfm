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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert file extension value to <see cref="Brush"/>.
	/// </summary>
	[ValueConversion(typeof(string), typeof(Brush))]
	public class FileExtensionToBrushConverter : IValueConverter
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets brush to draw executable file extensions.
		/// </summary>
		public Brush ExecutableBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw binary library file extensions.
		/// </summary>
		public Brush LibraryBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw video file extensions.
		/// </summary>
		public Brush VideoBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw audio file extensions.
		/// </summary>
		public Brush AudioBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw image file extensions.
		/// </summary>
		public Brush ImageBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw archive file extensions.
		/// </summary>
		public Brush ArchiveBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw temporary file extensions.
		/// </summary>
		public Brush TemporaryBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw source file extensions.
		/// </summary>
		public Brush SourceBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw web-related file extensions.
		/// </summary>
		public Brush WebBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw normal (regulary) file extensions.
		/// </summary>
		public Brush NormalBrush { get; set; }

		#endregion

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
				return null;	// new SolidColorBrush();
			}

			var ext = (string) value;

			// Executable
			if (ext.Equals("exe", StringComparison.OrdinalIgnoreCase))
			{
				return ExecutableBrush;
			}

			// Library
			if (ext.Equals("dll", StringComparison.OrdinalIgnoreCase))
			{
				return LibraryBrush;
			}

			// Video
			if (ext.Equals("avi", StringComparison.OrdinalIgnoreCase))
			{
				return VideoBrush;
			}

			// Audio
			if (ext.Equals("mp3", StringComparison.OrdinalIgnoreCase))
			{
				return AudioBrush;
			}

			// Image
			if (ext.Equals("jpg", StringComparison.OrdinalIgnoreCase))
			{
				return ImageBrush;
			}

			// Archive
			if (ext.Equals("zip", StringComparison.OrdinalIgnoreCase))
			{
				return ArchiveBrush;
			}

			// Temporary
			if (ext.Equals("tmp", StringComparison.OrdinalIgnoreCase))
			{
				return TemporaryBrush;
			}

			// Source
			if (ext.Equals("cs", StringComparison.OrdinalIgnoreCase))
			{
				return SourceBrush;
			}

			// Web
			if (ext.Equals("html", StringComparison.OrdinalIgnoreCase))
			{
				return WebBrush;
			}

			// Normal
			return NormalBrush;
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
	}
}