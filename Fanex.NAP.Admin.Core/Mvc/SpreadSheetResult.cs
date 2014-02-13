// --------------------------------------------------------------------------
// <copyright file="SpreadSheetResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2011.12.09">Created</change>
//   <change who="Anders.Le" date="2012.6.20">Change namespace and classname</change>
// </history>

namespace Fanex.NAP.Admin.Core.Mvc
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Spreadsheet result implemented by NPOI.
    /// </summary>
    public abstract class SpreadSheetResult : ActionResult
    {
        private readonly HSSFWorkbook _hssfWorkBook;
        private readonly string _fileName;

        protected SpreadSheetResult(HSSFWorkbook hssfWorkBook, string fileName)
        {
            this._hssfWorkBook = hssfWorkBook;
            this._fileName = fileName;
        }

        protected abstract string ContentType { get; }

        protected abstract string Extension { get; }

        protected abstract Action<HSSFWorkbook, Stream> SaveAction { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = this.ContentType;
            context.HttpContext.Response.AddHeader("Content-disposition", "filename=" + this._fileName + this.Extension);

            this.SaveAction(this._hssfWorkBook, context.HttpContext.Response.OutputStream);
        }
    }
}
