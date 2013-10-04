// --------------------------------------------------------------------------
// <copyright file="IExchangeRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Admin.Core.Criterias;
    using Fanex.NAP.Admin.Core.Models;
    using Fanex.NAP.Core.Domain;
    using NPOI.HSSF.UserModel;

    public interface IExchangeRateService
    {
        IEnumerable<ExchangeRateModel> GetAll(GetAllExchangeRateCriteria criteria);

        ExchangeRateModel GetById(GetExchangeRateByIdCriteria criteria);

        void InsertExchangeRate(ref InsertExchangeRateCriteria criteria);

        void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria);

        bool CheckInValidExchangeRate(ref ErrorMessageExchangeRate errorMsg, ExchangeRateModel exchangeRate, bool isUpdate);

        HSSFWorkbook ExportExchangeRate(List<ExchangeRateModel> exchangeRateList);

        void SendMailExchangeRate(string templatePath, List<IMailingList> mailingLists, string currency, double oldBaseRate, double newBaseRate, string userName);

        string GetExchangeRateTemplateEmail();
    }
}
