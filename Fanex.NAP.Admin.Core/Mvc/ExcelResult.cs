// --------------------------------------------------------------------------
// <copyright file="ExcelResult.cs" company="Nexcel Solutions Vietnam">
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
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Excel action result implemented by NPOI.
    /// </summary>
    public class ExcelResult : SpreadSheetResult
    {
        public ExcelResult(HSSFWorkbook hssfWorkBook, string fileName)
            : base(hssfWorkBook, fileName)
        {
        }

        protected override string ContentType
        {
            get { return "application/vnd.ms-excel"; }
        }

        protected override string Extension
        {
            get { return ".xls"; }
        }

        protected override Action<HSSFWorkbook, Stream> SaveAction
        {
            get { return (wb, s) => wb.Write(s); }
        }
    }
}