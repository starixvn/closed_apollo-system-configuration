// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web.Session
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// Current session.
    /// </summary>
    public class CurrentSession : ICurrentSession
    {
        #region const

        public const string INTERNALUSERNAME = "InternalUsername";

        public const string INTERNALUSERID = "InternalUserId";

        public const string INTERNALONUSERID = "InternalOnUserId";

        public const string INTERNALNETSESSIONID = "InternalNetSessionId";

        public const string INTERNALLEVEL = "InternalLevel";

        public const string INTERNALMENUITEMDIC = "InternalMenuItemList";

        public const string MENU_DIC = "MenuDic";

        #endregion Constants

        protected readonly int _SiteId = NapSettings.Settings.SiteId;

        public string Username
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[INTERNALUSERNAME])
                : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERNAME] = value;
            }
        }

        public int UserId
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[INTERNALUSERID])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERID] = value;
            }
        }

        public int OnUserId
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[INTERNALONUSERID])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[INTERNALONUSERID] = value;
            }
        }

        public string SessionId
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[INTERNALNETSESSIONID])
                : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALNETSESSIONID] = value;
            }
        }

        public IList<MenuItem> Menu
        {
            get
            {
                var menuItemList = HttpContext.Current.Session[INTERNALMENUITEMDIC] as Dictionary<int, List<MenuItem>>;

                if (menuItemList == null)
                {
                    // ensure menu should be empty instead of null
                    menuItemList = new Dictionary<int, List<MenuItem>>();
                    HttpContext.Current.Session[INTERNALMENUITEMDIC] = menuItemList;
                }

                return menuItemList.ContainsKey(this._SiteId) ? menuItemList[this._SiteId] : null;
            }

            set
            {
                var menuItemList = HttpContext.Current.Session[INTERNALMENUITEMDIC] as Dictionary<int, List<MenuItem>>;

                if (menuItemList == null)
                {
                    // ensure menu should be empty instead of null
                    menuItemList = new Dictionary<int, List<MenuItem>>();
                    HttpContext.Current.Session[INTERNALMENUITEMDIC] = menuItemList;
                }

                if (menuItemList.ContainsKey(this._SiteId))
                {
                    menuItemList[this._SiteId] = (List<MenuItem>)value;
                }
                else
                {
                    menuItemList.Add(this._SiteId, (List<MenuItem>)value);
                }

                HttpContext.Current.Session[INTERNALMENUITEMDIC] = menuItemList;
            }
        }

        public int Level
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[INTERNALLEVEL])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[INTERNALLEVEL] = value;
            }
        }

        public void ClearAll()
        {
            if (!ReferenceEquals(HttpContext.Current, null))
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}