// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.5">Created</change>
// </history>

namespace Fanex.NAP.Sample.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [Authorize]
    public class HomeController : Controller
    {
        public string Index()
        {
            using (ObjectDb objectDb = new ObjectDb("Acc_LicenseeByResellerSel"))
            {
                var lookup = new Dictionary<int, Reseller>();
                var parents = objectDb.Query<Reseller, Licensee, Reseller>(
                    (parent, child) =>
                    {
                        Reseller found = new Reseller();
                        if (!lookup.TryGetValue(parent.Id, out found))
                        {
                            lookup.Add(parent.Id, found = parent);
                        }

                        found.Licensees.Add(child);

                        return found;
                    },
                    "LicenseeID").ToDictionary(p => p.Id);
            }

            var listResellers = new List<Reseller>();
            Reseller currentReseller = new Reseller();

            using (ObjectDb objectDb = new ObjectDb("Acc_LicenseeByResellerSel"))
            {
                objectDb.Query<Reseller, Licensee, Reseller>(
                    (reseller, licensee) =>
                    {
                        if (currentReseller == null || currentReseller.Id != reseller.Id)
                        {
                            listResellers.Add(reseller);
                            currentReseller = reseller;
                        }

                        currentReseller.Licensees.Add(licensee);

                        return currentReseller;
                    },
                    "LicenseeID");
            }

            return HttpContext.Request.LogonUserIdentity.User.ToString();
        }
    }
}
