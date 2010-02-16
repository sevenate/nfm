// <copyright file="ICategoryService.cs" company="HD">
//  Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </editor>
// <summary>
//   Category service.
// </summary>

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Fab.Server.Core
{
	/// <summary>
	/// Category service.
	/// </summary>
	[ServiceContract]
	public interface ICategoryService
	{
		/// <summary>
		/// Create new category.
		/// </summary>
		/// <param name="userId">
		/// User unique ID for which this category should be created.
		/// </param>
		/// <param name="name">
		/// Category name.
		/// </param>
		[OperationContract]
		void CreateCategory(Guid userId, string name);

		/// <summary>
		/// Update category details to new values.
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="categoryId">
		/// Category ID.
		/// </param>
		/// <param name="name">
		/// Category new name.
		/// </param>
		[OperationContract]
		void UpdateCategory(Guid userId, int categoryId, string name);

		/// <summary>
		/// Mark category as "deleted".
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="categoryId">
		/// Category ID to mark as deleted.
		/// </param>
		[OperationContract]
		void DeleteCategory(Guid userId, int categoryId);

		/// <summary>
		/// Retrieve all categories for user.
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <returns>
		/// All categories.
		/// </returns>
		[OperationContract]
		IList<Category> GetAllCategories(Guid userId);
	}
}