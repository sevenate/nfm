// <copyright file="FileExtensionBorderBrushes.cs" company="HD">
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
// <summary>Holds all file extension border brushes.</summary>

using System.Windows.Media;
using Nfm.Core.Modules.FileSystem.Configuration;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Holds all file extension border brushes.
	/// </summary>
	public class FileExtensionBorderBrushes
	{
		#region FileExtensionBorderBrushConverter

		/// <summary>
		/// Gets or sets brush to draw executable file extensions.
		/// </summary>
		public Brush ExecutableBrush { get { return new SolidColorBrush("#2200FF00".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw binary library file extensions.
		/// </summary>
		public Brush LibraryBrush { get { return new SolidColorBrush("#22007B00".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw video file extensions.
		/// </summary>
		public Brush VideoBrush { get { return new SolidColorBrush("#22FF7BFF".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw audio file extensions.
		/// </summary>
		public Brush AudioBrush { get { return new SolidColorBrush("#227B7BFF".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw image file extensions.
		/// </summary>
		public Brush ImageBrush { get { return new SolidColorBrush("#22FF007B".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw archive file extensions.
		/// </summary>
		public Brush ArchiveBrush { get { return new SolidColorBrush("#22007BC0".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw temporary file extensions.
		/// </summary>
		public Brush TemporaryBrush { get { return new SolidColorBrush("#227B7B00".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw source file extensions.
		/// </summary>
		public Brush SourceBrush { get { return new SolidColorBrush("#2200FFFF".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw web-related file extensions.
		/// </summary>
		public Brush WebBrush { get { return new SolidColorBrush("#22FF7B00".ToColor()); } }

		/// <summary>
		/// Gets or sets brush to draw normal (regulary) file extensions.
		/// </summary>
		public Brush NormalBrush { get { return new SolidColorBrush("#22999999".ToColor()); } }

		#endregion
	}
}