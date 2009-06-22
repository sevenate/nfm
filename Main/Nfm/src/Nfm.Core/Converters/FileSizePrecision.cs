// <copyright file="FileSizePrecision.cs" company="HD">
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
// <summary>Specify file size value precision.</summary>

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Specify file size value precision.
	/// </summary>
	public enum FileSizePrecision
	{
		/// <summary>
		/// Size format: x
		/// <remarks>This is default value.</remarks>
		/// </summary>
		ZeroDecimalPlaces = 0,

		/// <summary>
		/// Size format: x.x
		/// </summary>
		OneDecimalPlaces,

		/// <summary>
		/// Size format: x.xx
		/// </summary>
		TwoDecimalPlaces,

		/// <summary>
		/// Size format: x.xxx
		/// </summary>
		ThreeDecimalPlaces
	}
}