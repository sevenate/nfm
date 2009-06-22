// <copyright file="ModuleConfig.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-09</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-09</date>
// </editor>
// <summary>FileSystem module configuration section.</summary>

using System.Windows.Media;
using Nfm.Core.Converters;

namespace Nfm.Core.Modules.FileSystem.Configuration
{
	/// <summary>
	/// FileSystem module configuration section.
	/// </summary>
	public static class ModuleConfig
	{
		/// <summary>
		/// Single instance of color converter.
		/// </summary>
		private static readonly ColorConverter ColorConverter = new ColorConverter();

		#region File Size Brushes

		/// <summary>
		/// Gets or sets brush to draw byte size.
		/// </summary>
		public static Brush ByteBrush
		{
			get { return new SolidColorBrush("White".ToColor()); }
		}

		/// <summary>
		/// Gets or sets brush to draw kilo-byte size.
		/// </summary>
		public static Brush KByteBrush
		{
			get { return new SolidColorBrush("LightGreen".ToColor()); }
		}

		/// <summary>
		/// Gets or sets brush to draw mega-byte size.
		/// </summary>
		public static Brush MByteBrush
		{
			get { return new SolidColorBrush("Orange".ToColor()); }
		}

		/// <summary>
		/// Gets or sets brush to draw giga-byte size.
		/// </summary>
		public static Brush GByteBrush
		{
			get { return new SolidColorBrush("#FFFF4F4F".ToColor()); }
		}

		/// <summary>
		/// Gets or sets brush to draw tera-byte size.
		/// </summary>
		public static Brush TByteBrush
		{
			get { return new SolidColorBrush("LightSeaGreen".ToColor()); }
		}

		#endregion

		#region File Size String Formats

		/// <summary>
		/// Gets or sets file size full string format.
		/// </summary>
		public static FileSizeFullStringFormat FileSizeFull { get { return new FileSizeFullStringFormat(); } }

		/// <summary>
		/// Gets or sets file size brief string format.
		/// </summary>
		public static FileSizeBriefStringFormat FileSizeBrief { get { return new FileSizeBriefStringFormat(); } }

		#endregion

		#region FileExtensionToBrushConverter

		/// <summary>
		/// Gets or sets brush to draw file extensions.
		/// </summary>
		public static FileExtensionBrushes FileExtension { get { return new FileExtensionBrushes(); } }

		/// <summary>
		/// Gets or sets brush to draw file borders.
		/// </summary>
		public static FileExtensionBorderBrushes FileExtensionBorder { get { return new FileExtensionBorderBrushes(); } }

		#endregion

		/// <summary>
		/// Convert color string representatino (known name or hex value) to <see cref="Color"/> instance.
		/// </summary>
		/// <param name="color">Color known name or hex value.</param>
		/// <returns>Corresponding <see cref="Color"/> instance.</returns>
		public static Color ToColor(this string color)
		{
			return (Color)ColorConverter.ConvertFrom(color);
		}
	}
}