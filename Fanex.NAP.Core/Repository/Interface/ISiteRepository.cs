// --------------------------------------------------------------------------
// <copyright file="ISiteRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
//   <change who="Lewis.Le" date="2012.10.5">Added FetchSiteUrl</change>
// </history>

namespace Fanex.NAP.Core.Repository.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Criteria.Site;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ISiteRepository : IRepository<ISite>
    {
        SiteUrl GetSiteUrl(ICriteria criteria);

        IEnumerable<SiteLevel> FetchSiteLevel(ICriteria criteria);

        IEnumerable<SiteUrl> FetchSiteUrl(ICriteria criteria);

        ICommandResult Insert(ref InsertSiteCriteria criteria, ICriteria logCriteria);

        ICommandResult InsertSiteUrl(ref InsertSiteUrlCriteria criteria, ICriteria logCriteria);

        ICommandResult Update(ICriteria criteria, ICriteria logCriteria);

        ICommandResult Delete(ICriteria criteria, ICriteria logCriteria);
    }
}