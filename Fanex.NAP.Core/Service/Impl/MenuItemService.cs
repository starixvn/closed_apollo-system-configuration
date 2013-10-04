// --------------------------------------------------------------------------
// <copyright file="MenuItemService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.30">Implement GetParentById</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.MenuItem;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Menu item service.
    /// </summary>
    public class MenuItemService : BaseService, IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService()
            : this(null)
        {
        }

        public MenuItemService(IMenuItemRepository menuItemRepository = null)
        {
            this._menuItemRepository = menuItemRepository ?? new MenuItemRepository();
        }

        public IMenuItem GetById(int id)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(id, "Id");

            try
            {
                var criteria = new GetByIdMenuItemCriteria
                {
                    MenuItemId = id
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IMenuItem GetByNameAndSite(string name, int siteId)
        {
            // check parameters
            Check.Argument.IsNotEmpty(name, "Name");
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");

            try
            {
                var criteria = new GetByNameAndSiteMenuItemCriteria
                {
                    MenuItemName = name,
                    SiteId = siteId
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMenuItem> GetAll(string env = Constant.UAT, bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllMenuItemCriteria
                {
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<SelectListItem> GetAllSelectListItem(
            string env = Constant.UAT,
            bool? isActive = null,
            int selectedMenuItemId = 0)
        {
            // check parameters
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllMenuItemCriteria
                {
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IMenuItem> menuItemList = this._menuItemRepository.Fetch(criteria);

                    if (menuItemList != null)
                    {
                        return menuItemList.Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString(),
                                Selected = s.Id == selectedMenuItemId
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMenuItem> GetBySite(int siteId, string env = Constant.UAT, bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetBySiteMenuItemCriteria
                {
                    SiteId = siteId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<SelectListItem> GetSelectListItemBySite(
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null,
            int selectedMenuItemId = 0)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetBySiteMenuItemCriteria
                {
                    SiteId = siteId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IMenuItem> menuItemList = this._menuItemRepository.Fetch(criteria);

                    if (menuItemList != null)
                    {
                        return menuItemList.Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString(),
                                Selected = s.Id == selectedMenuItemId
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMenuItem> GetBySiteInHierarchy(int siteId, string env = Constant.UAT, bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetBySiteMenuItemCriteria
                {
                    SiteId = siteId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IMenuItem> menuItemList = this._menuItemRepository.Fetch(criteria).OrderBy(x => x.DisplayOrder);

                    return SortMenuItemsInHierarchy(menuItemList);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMenuItem> GetParentById(int id, string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(id, "Id");
            try
            {
                var criteria = new GetParentByIdMenuItemCriteria
                {
                    MenuItemId = id,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<SelectListItem> GetParentSelectListItemById(int id, int selectedMenuItemId = 0, string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(id, "Id");
            try
            {
                var criteria = new GetParentByIdMenuItemCriteria
                {
                    MenuItemId = id,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IMenuItem> menuItemList = this._menuItemRepository.Fetch(criteria);

                    if (menuItemList != null)
                    {
                        return menuItemList.Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString(),
                                Selected = s.Id == selectedMenuItemId
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMenuItem> GetByUserIncludeGroup(
            int userId,
            string env = Constant.UAT,
            bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserIncludeGroupMenuItemCriteria
                {
                    UserId = userId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMenuItem> GetByUserAndSiteIncludeGroup(
            int userId,
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserAndSiteIncludeGroupMenuItemCriteria
                {
                    UserId = userId,
                    SiteId = siteId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMenuItem> Search(
            string menuItemName,
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new SearchAllMenuItemCriteria
                {
                    MenuItemName = menuItemName,
                    SiteId = siteId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return this._menuItemRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Insert(InsertMenuItemCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._menuItemRepository.Insert(ref criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Update(UpdateMenuItemCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._menuItemRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult UpdateStatus(UpdateStatusMenuItemCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._menuItemRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        private IEnumerable<IMenuItem> SortMenuItemsInHierarchy(IEnumerable<IMenuItem> menuItemList, int parentId = 0)
        {
            var sortedMenuItemList = new List<IMenuItem>();

            foreach (var menuItem in menuItemList)
            {
                // top menu items || sub menu items
                if ((menuItem.Parent == null && parentId == 0)
                    || (menuItem.Parent != null && menuItem.Parent.Id == parentId))
                {
                    sortedMenuItemList.Add(menuItem);
                    sortedMenuItemList.AddRange(SortMenuItemsInHierarchy(menuItemList, menuItem.Id));
                }
            }

            return sortedMenuItemList;
        }
    }
}