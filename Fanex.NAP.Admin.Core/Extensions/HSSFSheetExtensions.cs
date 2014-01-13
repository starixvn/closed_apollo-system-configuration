// --------------------------------------------------------------------------
// <copyright file="HSSFSheetExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.10.4">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NPOI.SS.UserModel;

    /// <summary>
    /// HSSFSheet Extensions.
    /// </summary>
    public static class HSSFSheetExtensions
    {
        /// <summary>
        /// CloneSheet method is used to copy a sheet to new sheet.
        /// </summary>
        /// <param name="workbook">Target workbook.</param>
        /// <param name="sourceSheetName">Template sheet name.</param>
        /// <param name="newSheetName">New sheet name.</param>
        /// <returns>Return the copied sheet.</returns>
        public static ISheet CloneSheet(this IWorkbook workbook, string sourceSheetName, string newSheetName)
        {
            int sheetIndex = workbook.GetSheetIndex(sourceSheetName);
            ISheet newSheet = workbook.CloneSheet(sheetIndex);
            int newSheetIndex = workbook.GetSheetIndex(newSheet);

            workbook.SetSheetName(newSheetIndex, newSheetName);

            return newSheet;
        }

        /// <summary>
        /// Remove the sheet by sheet name.
        /// </summary>
        /// <param name="workbook">Target workbook.</param>
        /// <param name="sheetName">The sheet name.</param>
        public static void RemoveSheet(this IWorkbook workbook, string sheetName)
        {
            ISheet sheet = workbook.GetSheet(sheetName);

            if (sheet != null)
            {
                int sheetIndex = workbook.GetSheetIndex(sheet);
                workbook.RemoveSheetAt(sheetIndex);
            }
        }

        /// <summary>
        /// CopyRow method is used to copy one row from a template to another template.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="sourceSheet">Template sheet.</param>
        /// <param name="sourceRowIndex">Index of source row in template sheet, base by zero.</param>
        /// <param name="destRowIndex">Index of destination row in main sheet, base by zero.</param>
        /// <returns>Return the copied row.</returns>
        public static IRow CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowIndex, int destRowIndex)
        {
            if (sheet.LastRowNum < destRowIndex)
            {
                for (int i = sheet.LastRowNum + 1; i <= destRowIndex; i++)
                {
                    sheet.CreateRow(i);
                }
            }

            // Get the source / new row
            IRow sourceRow = sourceSheet.GetRow(sourceRowIndex);
            if (sourceRow == null)
            {
                return null;
            }

            IRow newRow = sheet.GetRow(destRowIndex) ?? sheet.CreateRow(destRowIndex);

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);
                ICell newCell = newRow.CreateCell(i);

                sheet.CopyCell(oldCell, newCell);
            }

            return newRow;
        }

        /// <summary>
        /// CopyRow method is used to copy one cell from a template to another.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="sourceCell">Source Cell.</param>
        /// <param name="destCell">Destination Cell.</param>
        /// <returns>Return the copied cell.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static ICell CopyCell(this ISheet sheet, ICell sourceCell, ICell destCell)
        {
            // If the old cell is null jump to next cell
            if (sourceCell == null)
            {
                return null;
            }

            destCell.CellStyle = sourceCell.CellStyle;

            if (destCell.CellComment != null)
            {
                destCell.CellComment = sourceCell.CellComment;
            }

            if (sourceCell.Hyperlink != null)
            {
                destCell.Hyperlink = sourceCell.Hyperlink;
            }

            //// Set the cell data type & Set the cell data value
            destCell.SetCellType(sourceCell.CellType);

            switch (sourceCell.CellType)
            {
                case CellType.BLANK:
                    destCell.SetCellValue(sourceCell.StringCellValue);
                    break;

                case CellType.BOOLEAN:
                    destCell.SetCellValue(sourceCell.BooleanCellValue);
                    break;

                case CellType.ERROR:
                    destCell.SetCellErrorValue(sourceCell.ErrorCellValue);
                    break;

                case CellType.NUMERIC:
                    destCell.SetCellValue(sourceCell.NumericCellValue);
                    break;

                case CellType.STRING:
                    destCell.SetCellValue(sourceCell.RichStringCellValue);
                    break;

                case CellType.Unknown:
                    destCell.SetCellValue(sourceCell.StringCellValue);
                    break;
            }

            return destCell;
        }

        /// <summary>
        /// Assign decimal value to cell.
        /// </summary>
        /// <param name="cell">Target cell.</param>
        /// <param name="value">The value to be set.</param>
        public static void SetCellValue(this ICell cell, decimal value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        /// <summary>
        /// Assign object value to cell.
        /// </summary>
        /// <param name="cell">Target cell.</param>
        /// <param name="value">The value to be set.</param>
        public static void SetCellValue(this ICell cell, object value)
        {
            var valueType = value.GetType();

            if (valueType == typeof(bool))
            {
                cell.SetCellValue(Convert.ToBoolean(value));
                return;
            }

            if (valueType == typeof(DateTime))
            {
                cell.SetCellValue(Convert.ToDateTime(value));
                return;
            }

            if (valueType == typeof(decimal))
            {
                cell.SetCellValue(Convert.ToDecimal(value));
                return;
            }

            if (valueType == typeof(double))
            {
                cell.SetCellValue(Convert.ToDouble(value));
                return;
            }

            if (valueType == typeof(int))
            {
                cell.SetCellValue(Convert.ToInt32(value));
                return;
            }

            if (valueType == typeof(long))
            {
                cell.SetCellValue(Convert.ToInt64(value));
                return;
            }

            if (valueType == typeof(string))
            {
                cell.SetCellValue(Convert.ToString(value));
            }
        }
    }
}
