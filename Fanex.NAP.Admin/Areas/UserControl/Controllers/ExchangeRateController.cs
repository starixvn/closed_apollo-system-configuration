// --------------------------------------------------------------------------
// <copyright file="ExchangeRateController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.10.4">Created</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.NAP.Admin.Core;
    using Fanex.NAP.Admin.Core.Criterias;
    using Fanex.NAP.Admin.Core.Extensions;
    using Fanex.NAP.Admin.Core.Models;
    using Fanex.NAP.Admin.Core.Mvc;
    using Fanex.NAP.Admin.Core.Services.Impl;
    using Fanex.NAP.Admin.Core.Services.Interface;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// ExchangeRate Controller.
    /// </summary>
    [NapSuperAuth]
    public class ExchangeRateController : BaseController
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IMailingListService _mailingListService;

        /// <summary>
        /// Initializes a new instance of the ExchangeRateController class.
        /// </summary>
        public ExchangeRateController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ExchangeRateController class.
        /// </summary>
        /// <param name="exchangeRateService">ExchangeRate Service.</param>
        /// <param name="mailingListService">MailingList Service.</param>
        public ExchangeRateController(IExchangeRateService exchangeRateService, IMailingListService mailingListService)
        {
            this._exchangeRateService = exchangeRateService ?? new ExchangeRateService();
            this._mailingListService = mailingListService ?? new MailingListService();
        }

        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns>Action Result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ExchangeRate, Policy = Constant.Policy.Read)]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            var criteria = new GetAllExchangeRateCriteria();
            var exchangeRateList = _exchangeRateService.GetAll(criteria);

            ViewBag.HasAllowEdit = HasRight(FunctionKeys.ExchangeRate, Constant.Policy.Host);

            return this.View(exchangeRateList.ToList());
        }

        /// <summary>
        /// Insert action.
        /// </summary>
        /// <returns>Action Result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ExchangeRate, Policy = Constant.Policy.Host)]
        public ActionResult Insert()
        {
            var model = new ExchangeRateModel
            {
                MultipliedBy = 1,
                MinBet = null
            };

            return this.View(model);
        }

        /// <summary>
        /// Insert action.
        /// </summary>
        /// <param name="model">ExchangeRate Model.</param>
        /// <returns>Message result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ExchangeRate, Policy = Constant.Policy.Host)]
        public JsonResult Insert(ExchangeRateModel model)
        {
            var errorMsg = new ErrorMessageExchangeRate
            {
                MsgCurrency = string.Empty,
                MsgActualRate = string.Empty,
                MsgMinBet = string.Empty,
                MsgMinBetLive = string.Empty,
                MsgMultipliedBy = string.Empty
            };

            model.Currency = !string.IsNullOrWhiteSpace(model.Currency) ? model.Currency.Trim() : string.Empty;

            if (this._exchangeRateService.CheckInValidExchangeRate(ref errorMsg, model, false))
            {
                var criteria = new InsertExchangeRateCriteria
                {
                    Currency = model.Currency,
                    UserId = CurrentSession.UserId,
                    IP = CurrentContext.IP,
                    Description = !string.IsNullOrWhiteSpace(model.Description) ? model.Description.Trim() : string.Empty,
                    MultipliedBy = model.MultipliedBy,
                    ActualRate = model.ActualRate.Value,
                    MinBet = model.MinBet.Value,
                    MinBetLive = model.MinBetLive.Value
                };

                this._exchangeRateService.InsertExchangeRate(ref criteria);

                errorMsg.ErrorCode = criteria.ErrCode;
                errorMsg.MsgCurrency = criteria.ErrMsg;
            }

            return this.Json(errorMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get update action.
        /// </summary>
        /// <param name="id">Id of exchange rate.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [NapAuth(FunctionKey = FunctionKeys.ExchangeRate, Policy = Constant.Policy.Host)]
        public ActionResult Update(int id)
        {
            var criteria = new GetExchangeRateByIdCriteria
            {
                ExchangeId = id
            };

            ExchangeRateModel exchangeRate = this._exchangeRateService.GetById(criteria);

            return this.View(exchangeRate);
        }

        /// <summary>
        /// Post update action.
        /// </summary>
        /// <param name="model">ExchangeRate Model.</param>
        /// <returns>Message result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ExchangeRate, Policy = Constant.Policy.Host)]
        public JsonResult Update(ExchangeRateModel model)
        {
            var errorMsg = new ErrorMessageExchangeRate
            {
                MsgCurrency = string.Empty,
                MsgActualRate = string.Empty,
                MsgMinBet = string.Empty,
                MsgMinBetLive = string.Empty,
                MsgMultipliedBy = string.Empty
            };

            var getExchangeRateByIdCriteria = new GetExchangeRateByIdCriteria 
            { 
                ExchangeId = model.ExchangeId 
            };

            var oldExchangeRate = this._exchangeRateService.GetById(getExchangeRateByIdCriteria);

            if (oldExchangeRate == null)
            {
                errorMsg.ErrorCode = 101;
                errorMsg.MsgCurrency = string.Format(Lang.NameExistedTpl, model.Currency);
            }
            else
            {
                if (this._exchangeRateService.CheckInValidExchangeRate(ref errorMsg, model, true))
                {
                    var criteria = new UpdateExchangeRateCriteria
                    {
                        UserId = CurrentSession.UserId,
                        IP = CurrentContext.IP,
                        ExchangeId = model.ExchangeId,
                        Description = !string.IsNullOrWhiteSpace(model.Description) ? model.Description.Trim() : string.Empty,
                        MultipliedBy = model.MultipliedBy,
                        ActualRate = model.ActualRate.Value,
                        MinBet = model.MinBet.Value,
                        MinBetLive = model.MinBetLive.Value
                    };

                    this._exchangeRateService.UpdateExchangeRate(ref criteria);

                    errorMsg.ErrorCode = criteria.ErrCode;
                    errorMsg.MsgCurrency = criteria.ErrMsg;

                    if (criteria.ErrCode == 0 && oldExchangeRate.ActualRate.Value != model.ActualRate.Value)
                    {
                        this.SendMailExchangeRate(oldExchangeRate.Currency, oldExchangeRate.ActualRate.Value, model.ActualRate.Value);
                    }
                }
            }

            return this.Json(errorMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ExportExchangeRate action.
        /// </summary>
        /// <returns>Action Result.</returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [NapAuth(FunctionKey = FunctionKeys.ExchangeRate, Policy = Constant.Policy.Read)]
        public ActionResult ExportExchangeRate()
        {
            var criteria = new GetAllExchangeRateCriteria();
            IEnumerable<ExchangeRateModel> exchangeRateList = this._exchangeRateService.GetAll(criteria);
            var excelResult = this._exchangeRateService.ExportExchangeRate(exchangeRateList.ToList());
            var fileName = string.Format("ExchangeRate_{0}", Utilities.ShortDateFormat(DateTime.Now));

            return new ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Function to send mail when change actual rate of currency.
        /// </summary>
        /// <param name="currency">Currency name.</param>
        /// <param name="oldActualRate">Old actual rate.</param>
        /// <param name="newActualRate">New actual rate.</param>
        [NonAction]
        private void SendMailExchangeRate(string currency, double oldActualRate, double newActualRate)
        {
            string templatePath = this._exchangeRateService.GetExchangeRateTemplateEmail();
            string userName = this.CurrentSession.Username;

            var criteria = new GetMailingListByFunctionKeyAndSiteCriteria
            {
                FunctionKey = FunctionKeys.ExchangeRate,
                SiteId = this.SiteId
            };

            IEnumerable<IMailingList> mailingLists = this._mailingListService.GetByFunctionKeyAndSite(criteria);

            //// send mail
            Task taskSendMail = new Task(() => this._exchangeRateService.SendMailExchangeRate(
                templatePath,
                mailingLists != null ? mailingLists.ToList() : null,
                currency,
                oldActualRate,
                newActualRate,
                userName));

            taskSendMail.Start();
        }
    }
}
