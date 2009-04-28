// <copyright file="FileSystemEntityType.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-28</date>
// </editor>
// <summary>Specify file system entity type.</summary>

namespace Nfm.Core.Models.FileSystem
{
	/// <summary>
	/// Specify file system entity type.
	/// </summary>
	public enum FileSystemEntityType
	{
		/// <summary>
		/// Regulary directory.
		/// </summary>
		Directory = 0,

		/// <summary>
		/// Regulary file.
		/// </summary>
		File,

		/// <summary>
		/// Logical drive.
		/// </summary>
		Drive
	}
}