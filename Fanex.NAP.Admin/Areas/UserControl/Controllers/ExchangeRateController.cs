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
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.NAP.Admin.Core;
    using Fanex.NAP.Admin.Core.Criterias;
    using Fanex.NAP.Admin.Core.Extensions;
    using Fanex.NAP.Admin.Core.Models;
    using Fanex.NAP.Admin.Core.Services.Impl;
    using Fanex.NAP.Admin.Core.Services.Interface;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Mvc;

    /// <summary>
    /// ExchangeRate Controller.
    /// </summary>
    [NapSuperAuth]
    public class ExchangeRateController : BaseController
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IUserService _userService;
        private readonly IFunctionService _functionService;
        private readonly IMailingListService _mailingListService;

        /// <summary>
        /// Initializes a new instance of the ExchangeRateController class.
        /// </summary>
        public ExchangeRateController()
            : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ExchangeRateController class.
        /// </summary>
        /// <param name="exchangeRateService">ExchangeRate Service.</param>
        /// <param name="userService">User Service.</param>
        /// <param name="functionService">Function Service.</param>
        /// <param name="mailingListService">MailingList Service.</param>
        public ExchangeRateController(
            IExchangeRateService exchangeRateService,
            IUserService userService,
            IFunctionService functionService,
            IMailingListService mailingListService)
        {
            this._exchangeRateService = exchangeRateService ?? new ExchangeRateService();
            this._userService = userService ?? new UserService { ServiceContext = ServiceContext };
            this._functionService = functionService ?? new FunctionService();
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
            var exchangeRateList = this._exchangeRateService.GetAll(criteria);

            int policy = this._userService.GetUserSiteFunctionKeyPolicy(
                        this.CurrentSession.UserId,
                        this.SiteId,
                        FunctionKeys.ExchangeRate.ToString());

            ViewBag.HasAllowEdit = policy >= (int)Constant.Policy.Host;

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

            var exchangeRate = this._exchangeRateService.GetById(criteria);

            return this.View(exchangeRate);
        }

        /// <summary>
        /// Post update action.
        /// </summary>
        /// <param name="model">ExchangeRate Model.</param>
        /// <returns>Message result.</returns>
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

            var getExchangeRateByIdCriteria =
                new GetExchangeRateByIdCriteria { ExchangeId = model.ExchangeId };
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

                    this.SendMail(model, oldExchangeRate, criteria.ErrCode);
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
            var exchangeRateList = this._exchangeRateService.GetAll(criteria);

            var excelResult = this._exchangeRateService.ExportExchangeRate(exchangeRateList.ToList());
            var fileName = string.Format("ExchangeRate_{0}", Utilities.ShortDateFormat(DateTime.Now));

            return new ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Function to send mail when change base rate of currency.
        /// </summary>
        /// <param name="templatePath">Path of template mail file.</param>
        /// <param name="currency">Currency code.</param>
        /// <param name="oldBaseRate">Old base rate value.</param>
        /// <param name="newBaseRate">New base rate value.</param>
        /// <param name="userName">Username of changer.</param>
        [NonAction]
        private void SendMailExchangeRate(string templatePath, string currency, double oldBaseRate, double newBaseRate, string userName)
        {
            var criteria = new GetMailingListByFunctionKeyAndSiteCriteria
            {
                FunctionKey = FunctionKeys.ExchangeRate,
                SiteId = this.SiteId
            };
            
            var mailingLists = this._mailingListService.GetByFunctionKeyAndSite(criteria);

            this._exchangeRateService.SendMailExchangeRate(
                templatePath, 
                mailingLists != null ? mailingLists.ToList() : null, 
                currency, 
                oldBaseRate, 
                newBaseRate, 
                userName);
        }

        /// <summary>
        /// Function to send mail when change base rate of currency.
        /// </summary>
        /// <param name="newExchangeRate">New ExchangeRate Model.</param>
        /// <param name="oldExchangeRate">Old ExchangeRate Model.</param>
        /// <param name="errCode">Value is check update error or no.</param>
        [NonAction]
        private void SendMail(ExchangeRateModel newExchangeRate, ExchangeRateModel oldExchangeRate, int errCode)
        {
            if (errCode == 0 && oldExchangeRate.ActualRate.Value != newExchangeRate.ActualRate.Value)
            {
                var templatePath = this._exchangeRateService.GetExchangeRateTemplateEmail();
                var userName = this.CurrentSession.Username;
                //// send mail
                Task taskSendMail = new Task(() => this.SendMailExchangeRate(
                    templatePath,
                    oldExchangeRate.Currency,
                    oldExchangeRate.ActualRate.Value,
                    newExchangeRate.ActualRate.Value,
                    userName));
                taskSendMail.Start();
            }
        }
    }
}
