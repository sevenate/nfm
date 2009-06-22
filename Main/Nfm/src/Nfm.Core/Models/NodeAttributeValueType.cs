// <copyright file="NodeAttributeValueType.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </editor>
// <summary>Represent all node attribute types.</summary>

namespace Nfm.Core
{
	/// <summary>
	/// Represent all available node attribute types.
	/// </summary>
	public enum NodeAttributeValueType
	{
		/// <summary>
		/// Attribute value is string.
		/// </summary>
		String,

		/// <summary>
		/// Attribute value is integer.
		/// </summary>
		Int,
		
		/// <summary>
		/// Attribute value is integer.
		/// </summary>
		Long,

		/// <summary>
		/// Attribute value is float.
		/// </summary>
		Float,
		
		/// <summary>
		/// Attribute value is double.
		/// </summary>
		Double,

		/// <summary>
		/// Attribute value is boolean.
		/// </summary>
		Bool,

		/// <summary>
		/// Attribute value is date and time.
		/// </summary>
		DateTime,

		/// <summary>
		/// Attribute value is complex object.
		/// </summary>
		Object
	}
}