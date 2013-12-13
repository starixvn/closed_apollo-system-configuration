// --------------------------------------------------------------------------
// <copyright file="IFunctionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
//   <change who="Lewis.Le" date="2012.11.01">Added ICommandResult AssignFunction(AssignFunctionToUsersCriteria criteria)</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Framework.Data;

    public interface IFunctionService : IBaseService
    {
        #region Gets Function
        
        IFunction GetById(int id);

        IFunction GetByNameAndSite(string name, int siteId);

        IFunction GetByKeyAndSite(string key, int siteId);

        #endregion

        #region Gets Functions
        
        IEnumerable<IFunction> GetAll(string env = Constant.UAT, bool? isActive = null);

        Dictionary<ISite, IEnumerable<IFunction>> GetAllAndGroupBySite(string env = Constant.UAT, bool? isActive = null);

        IEnumerable<IFunction> GetBySite(int siteId, string env = Constant.UAT, bool? isActive = null);

        IList<SelectListItem> GetSelectListItemBySite(
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null,
            int selectedFunctionId = 0);

        IEnumerable<IFunction> GetBySiteInHierarchy(int siteId, string env = Constant.UAT, bool? isActive = null);

        IEnumerable<IFunction> GetParentById(int id);

        IEnumerable<IFunction> Search(string functionName, int siteId, string env = Constant.UAT, bool? isActive = null);

        IEnumerable<IFunction> GetByParentKeyAndSite(GetByParentKeyAndSiteCriteria criteria);

        #endregion

        #region Gets Functions Policy
        
        IEnumerable<FunctionPolicy> GetByUser(int userId, string env = Constant.UAT);

        Dictionary<ISite, IEnumerable<FunctionPolicy>> GetByUserAndGroupBySite(int userId, string env = Constant.UAT);

        IEnumerable<FunctionPolicy> GetByGroup(int groupId, string env = Constant.UAT);

        Dictionary<ISite, IEnumerable<FunctionPolicy>> GetByGroupAndGroupBySite(
            int groupId,
            string env = Constant.UAT);

        IEnumerable<FunctionPolicy> GetByGroupAndSite(int groupId, int siteId, string env = Constant.UAT);

        IEnumerable<FunctionPolicy> GetByUserAndSite(int userId, int siteId, string env = Constant.UAT);

        IEnumerable<FunctionPolicy> GetByUserAndSiteIncludeGroup(int userId, int siteId, string env = Constant.UAT);

        IEnumerable<FunctionPolicy> GetByUserAndFunctionKeyIncludeGroup(
            int userId, 
            string functionKey,
            string env = Constant.UAT);

        #endregion

        bool CheckUserFunctionPolicy(int userId, int functionId, int policy);

        /// <summary>
        /// Duplicates function: inserts a new function and assign it to privileged groups/users of the function-to-duplicate.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result.</returns>
        ICommandResult Duplicate(DuplicateFunctionCriteria criteria);

        /// <summary>
        /// Inserts function and assigns it to all site admin of the current site and all portal admin.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result.</returns>
        ICommandResult Insert(InsertFunctionCriteria criteria);

        ICommandResult Update(UpdateFunctionCriteria criteria);

        ICommandResult UpdateStatus(UpdateStatusFunctionCriteria criteria);

        ICommandResult AssignFunctionToUsers(AssignFunctionToUsersCriteria criteria);

        ICommandResult AssignFunctionToGroups(AssignFunctionToGroupsCriteria criteria);
    }
}