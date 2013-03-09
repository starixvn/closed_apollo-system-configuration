// --------------------------------------------------------------------------
// <copyright file="IMenuItemService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.MenuItem;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IMenuItemService : IBaseService
    {
        IMenuItem GetById(int id);

        IMenuItem GetByNameAndSite(string name, int siteId);

        IEnumerable<IMenuItem> GetAll(string env = Constant.UAT, bool? isActive = null);

        IList<SelectListItem> GetAllSelectListItem(
            string env = Constant.UAT,
            bool? isActive = null,
            int selectedMenuItemId = 0);

        IEnumerable<IMenuItem> GetBySite(int siteId, string env = Constant.UAT, bool? isActive = null);

        IList<SelectListItem> GetSelectListItemBySite(
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null,
            int selectedMenuItemId = 0);

        IEnumerable<IMenuItem> GetBySiteInHierarchy(int siteId, string env = Constant.UAT, bool? isActive = null);

        IEnumerable<IMenuItem> GetByUserIncludeGroup(
            int userId,
            string env = Constant.UAT,
            bool? isActive = null);

        IEnumerable<IMenuItem> GetByUserAndSiteIncludeGroup(
            int userId,
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null);

        IEnumerable<IMenuItem> Search(
            string menuItemName,
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null);

        IEnumerable<IMenuItem> GetParentById(int id, string env = Constant.UAT);

        IList<SelectListItem> GetParentSelectListItemById(int id, int selectedMenuItemId = 0, string env = Constant.UAT);

        ICommandResult Insert(InsertMenuItemCriteria criteria);

        ICommandResult Update(UpdateMenuItemCriteria criteria);

        ICommandResult UpdateStatus(UpdateStatusMenuItemCriteria criteria);
    }
}