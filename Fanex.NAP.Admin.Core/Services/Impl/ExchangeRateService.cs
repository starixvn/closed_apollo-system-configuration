// --------------------------------------------------------------------------
// <copyright file="ExchangeRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Fanex.Common.Helper;
    using Fanex.NAP.Admin.Core;
    using Fanex.NAP.Admin.Core.Criterias;
    using Fanex.NAP.Admin.Core.Extensions;
    using Fanex.NAP.Admin.Core.Models;
    using Fanex.NAP.Admin.Core.Repositories.Impl;
    using Fanex.NAP.Admin.Core.Repositories.Interface;
    using Fanex.NAP.Admin.Core.Services.Interface;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Web;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Exchange Rate Service.
    /// </summary>
    public class ExchangeRateService : IExchangeRateService
    {
        private const int _RowStartBody = 3;
        private const double _MaxValueLength15 = 999999999999999;
        private const double _MaxValueLength9 = 999999999;
        private const string _FilePathExcelTemplate = "~\\ExcelTemplates\\ExchangeRateTemplate.xls";
        private const string _FilePathEmailTemplate = "~\\EmailTemplates\\ExchangeRate.cshtml";
        private readonly IExchangeRateRepository _exchangeRateRepository;

        /// <summary>
        /// Initializes a new instance of the ExchangeRateService class.
        /// </summary>
        public ExchangeRateService() : this(null) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the ExchangeRateService class.
        /// </summary>
        /// <param name="exchangeRateRepository">ExchangeRate Repository.</param>
        public ExchangeRateService(IExchangeRateRepository exchangeRateRepository)
        {
            this._exchangeRateRepository = exchangeRateRepository ?? new ExchangeRateRepository();
        }

        /// <summary>
        /// Function to get all exchange rate.
        /// </summary>
        /// <param name="criteria">GetAllExchangeRate Criteria.</param>
        /// <returns>List ExchangeRateModel.</returns>
        public IEnumerable<ExchangeRateModel> GetAll(GetAllExchangeRateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return this._exchangeRateRepository.GetAll(criteria);
        }

        /// <summary>
        /// Function to get Exchange Rate by id.
        /// </summary>
        /// <param name="criteria">GetExchangeRateById Criteria.</param>
        /// <returns>ExchangeRate Model.</returns>
        public ExchangeRateModel GetById(GetExchangeRateByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return this._exchangeRateRepository.GetById(criteria);
        }

        /// <summary>
        /// Function to Insert Exchange Rate.
        /// </summary>
        /// <param name="criteria">InsertExchangeRate Criteria.</param>
        public void InsertExchangeRate(ref InsertExchangeRateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            this._exchangeRateRepository.InsertExchangeRate(ref criteria);
        }

        /// <summary>
        /// Function to Update a Exchange Rate.
        /// </summary>
        /// <param name="criteria">UpdateExchangeRate Criteria.</param>
        public void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            this._exchangeRateRepository.UpdateExchangeRate(ref criteria);
        }

        /// <summary>
        /// Function to check valid Exchange Rate.
        /// </summary>
        /// <param name="errorMsg">ErrorMessage ExchangeRate.</param>
        /// <param name="exchangeRate">ExchangeRate Model.</param>
        /// <param name="isUpdate">Is update or no.</param>
        /// <returns>True or False.</returns>
        public bool CheckInValidExchangeRate(ref ErrorMessageExchangeRate errorMsg, ExchangeRateModel exchangeRate, bool isUpdate)
        {
            var isValid = true;

            isValid = CheckInValidCurrency(errorMsg, exchangeRate, isUpdate, isValid);

            isValid = CheckInValidActualRate(errorMsg, exchangeRate, isValid);

            isValid = CheckInValidMinBet(errorMsg, exchangeRate, isValid);

            isValid = CheckInValidMinBetLive(errorMsg, exchangeRate, isValid);

            isValid = CheckInValidMultipliedBy(errorMsg, exchangeRate, isValid);

            return isValid;
        }

        /// <summary>
        /// Function to export excel for exchange rate.
        /// </summary>
        /// <param name="exchangeRateList">List of ExchangeRateModel.</param>
        /// <returns>HSSFWorkbook file.</returns>
        public HSSFWorkbook ExportExchangeRate(List<ExchangeRateModel> exchangeRateList)
        {
            HSSFWorkbook hssfworkbook;
            var templatePath = HttpContext.Current.Server.MapPath(_FilePathExcelTemplate);
            using (var template = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(template);
            }

            var templateSheet = hssfworkbook.GetSheet("sheet1");
            var sheet = hssfworkbook.CloneSheet("sheet1", "ExchangeRate");
            var rowIndex = _RowStartBody;
            var rowTemplateNeedCopy = 0;

            foreach (var item in exchangeRateList)
            {
                if (item.ActualRate % 1 == 0)
                {
                    rowTemplateNeedCopy = _RowStartBody + 2;
                    rowTemplateNeedCopy += (rowIndex - _RowStartBody) % 2;
                }
                else
                {
                    rowTemplateNeedCopy = _RowStartBody;
                    rowTemplateNeedCopy += (rowIndex - _RowStartBody) % 2;
                }

                sheet.CopyRow(templateSheet, rowTemplateNeedCopy, rowIndex);
                var row = sheet.GetRow(rowIndex);

                var cellCurrency = row.GetCell(0);
                var cellActualRate = row.GetCell(1);
                var cellMinBet = row.GetCell(2);
                var cellMinBetLive = row.GetCell(3);
                var cellDescription = row.GetCell(4);
                var cellMultipliedBy = row.GetCell(5);

                cellCurrency.SetCellValue(item.Currency);
                cellActualRate.SetCellValue((double)item.ActualRate);
                cellMinBet.SetCellValue((double)item.MinBet);
                cellMinBetLive.SetCellValue((double)item.MinBetLive);
                cellDescription.SetCellValue(item.Description);
                cellMultipliedBy.SetCellValue((double)item.MultipliedBy);

                rowIndex++;
            }

            hssfworkbook.RemoveSheetAt(0);      // remove sheet & row template

            return hssfworkbook;
        }

        /// <summary>
        /// Function to send mail when has change base rate of currency.
        /// </summary>
        /// <param name="templatePath">Path of template email file.</param>
        /// <param name="mailingLists">List MailingListModel need send mail.</param>
        /// <param name="currency">Currency code.</param>
        /// <param name="oldActualRate">Old base rate.</param>
        /// <param name="newActualRate">New base rate.</param>
        /// <param name="userName">Username of changer.</param>
        public void SendMailExchangeRate(
            string templatePath,
            List<IMailingList> mailingLists,
            string currency,
            double oldActualRate,
            double newActualRate,
            string userName)
        {
            if (mailingLists != null && mailingLists.Any())
            {
                var bodyTemplate = Utilities.ReadTextFile(templatePath);
                var body = string.Format(bodyTemplate, currency, oldActualRate, newActualRate, userName);
                var subject = string.Format(Lang.SubjectEmailExchangeRate, currency, Utilities.ShortDateFormat(DateTime.Now), userName);

                foreach (var item in mailingLists)
                {
                    EmailService.SendEmail(item.ToList, item.CCList, item.BCCList, subject, body);
                }
            }
        }

        /// <summary>
        /// Function to get full path of exchange rate template email file.
        /// </summary>
        /// <returns>Full path.</returns>
        public string GetExchangeRateTemplateEmail()
        {
            return HttpContext.Current.Server.MapPath(_FilePathEmailTemplate);
        }

        private static bool CheckInValidMultipliedBy(
            ErrorMessageExchangeRate errorMsg, 
            ExchangeRateModel exchangeRate, 
            bool isValid)
        {
            if (!Utilities.IsNumber(exchangeRate.MultipliedBy.ToString()) 
                || exchangeRate.MultipliedBy <= 0 
                || exchangeRate.MultipliedBy > _MaxValueLength9)
            {
                isValid = false;
                errorMsg.ErrorCode = 108;
                errorMsg.MsgMultipliedBy = string.Format(ViewResources.EmailFormatValidationMsg, Lang.MultipliedBy);
            }

            return isValid;
        }

        /// <summary>
        /// Check InValid MinBetLive.
        /// </summary>
        /// <param name="errorMsg">Error Message object.</param>
        /// <param name="exchangeRate">Exchange Rate object.</param>
        /// <param name="isValid">Status before check MinBetLive.</param>
        /// <returns>Is Valid or no.</returns>
        private static bool CheckInValidMinBetLive(ErrorMessageExchangeRate errorMsg, ExchangeRateModel exchangeRate, bool isValid)
        {
            if (!exchangeRate.MinBetLive.HasValue)
            {
                isValid = false;
                errorMsg.ErrorCode = 106;
                errorMsg.MsgMinBetLive = string.Format(ViewResources.Field_Required, Lang.MinBetLive);
            }
            else
            {
                if (!Utilities.IsNumber(exchangeRate.MinBetLive.Value.ToString()) 
                    || exchangeRate.MinBetLive.Value < 0 
                    || exchangeRate.MinBetLive.Value > _MaxValueLength15)
                {
                    isValid = false;
                    errorMsg.ErrorCode = 107;
                    errorMsg.MsgMinBetLive = string.Format(ViewResources.EmailFormatValidationMsg, Lang.MinBetLive);
                }
            }

            return isValid;
        }

        /// <summary>
        /// Check InValid MinBet.
        /// </summary>
        /// <param name="errorMsg">Error Message object.</param>
        /// <param name="exchangeRate">Exchange Rate object.</param>
        /// <param name="isValid">Status before check MinBet.</param>
        /// <returns>Is Valid or no.</returns>
        private static bool CheckInValidMinBet(ErrorMessageExchangeRate errorMsg, ExchangeRateModel exchangeRate, bool isValid)
        {
            if (!exchangeRate.MinBet.HasValue)
            {
                isValid = false;
                errorMsg.ErrorCode = 104;
                errorMsg.MsgMinBet = string.Format(ViewResources.Field_Required, Lang.MinBet);
            }
            else
            {
                if (!Utilities.IsNumber(exchangeRate.MinBet.Value.ToString()) 
                    || exchangeRate.MinBet.Value < 0 
                    || exchangeRate.MinBet.Value > _MaxValueLength15)
                {
                    isValid = false;
                    errorMsg.ErrorCode = 105;
                    errorMsg.MsgMinBet = string.Format(ViewResources.EmailFormatValidationMsg, Lang.MinBet);
                }
            }

            return isValid;
        }

        /// <summary>
        /// Check InValid ActualRate.
        /// </summary>
        /// <param name="errorMsg">Error Message object.</param>
        /// <param name="exchangeRate">Exchange Rate object.</param>
        /// <param name="isValid">Status before check ActualRate.</param>
        /// <returns>Is Valid or no.</returns>
        private static bool CheckInValidActualRate(ErrorMessageExchangeRate errorMsg, ExchangeRateModel exchangeRate, bool isValid)
        {
            if (!exchangeRate.ActualRate.HasValue)
            {
                isValid = false;
                errorMsg.ErrorCode = 102;
                errorMsg.MsgActualRate = string.Format(ViewResources.Field_Required, Lang.BaseRate);
            }
            else
            {
                if (!Utilities.IsNumber(exchangeRate.ActualRate.Value.ToString()) 
                    || exchangeRate.ActualRate.Value < 0 
                    || exchangeRate.ActualRate.Value > _MaxValueLength15)
                {
                    isValid = false;
                    errorMsg.ErrorCode = 103;
                    errorMsg.MsgActualRate = string.Format(ViewResources.EmailFormatValidationMsg, Lang.BaseRate);
                }
            }

            return isValid;
        }

        private static bool CheckInValidCurrency(ErrorMessageExchangeRate errorMsg, ExchangeRateModel exchangeRate, bool isUpdate, bool isValid)
        {
            if (!isUpdate && string.IsNullOrWhiteSpace(exchangeRate.Currency))
            {
                isValid = false;
                errorMsg.ErrorCode = 101;
                errorMsg.MsgCurrency = string.Format(ViewResources.Field_Required, Lang.Currency);
            }

            return isValid;
        }
    }
}
