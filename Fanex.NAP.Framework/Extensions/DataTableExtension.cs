// -----------------------------------------------------------------------
// <copyright file="DataTableExtension.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.6.26">Created</change>
//   <change who="Anders.Le" date="2012.6.26">Check Stylecop</change>
// </history>

namespace Fanex.NAP.Framework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common.Extensions;

    /// <summary>
    /// Data table extension.
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// List of type will be check for column is numeric or not.
        /// </summary>
        public static readonly List<Type> NumericTypes = new List<Type> 
        { 
            typeof(byte), 
            typeof(decimal), 
            typeof(double), 
            typeof(short), 
            typeof(int), 
            typeof(long), 
            typeof(sbyte), 
            typeof(float), 
            typeof(uint),
            typeof(ulong)
        };

        /// <summary>
        /// Check DataColum is Numeric.
        /// </summary>
        /// <param name="col">DataColum object.</param>
        /// <returns>Is Numeric or Not.</returns>
        public static bool IsNumeric(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            return NumericTypes.Contains(col.DataType);
        }

        /// <summary>
        /// This function will add a new row which contains the sum of column number.
        /// </summary>
        /// <param name="dataTable">Table is added new row.</param>
        public static void AddSumRow(this DataTable dataTable)
        {
            DataRow newDataRow = dataTable.NewRow();

            foreach (DataColumn column in dataTable.Columns)
            {
                // If Numeric, do this
                if (column.IsNumeric())
                {
                    double sumOfColumn = 0;
                    double valueOfCell = 0;
                    bool isdouble = false;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        valueOfCell = 0;
                        isdouble = double.TryParse(row[column].ToString(), out valueOfCell);

                        if (isdouble)
                        {
                            sumOfColumn += valueOfCell;
                        }
                        else if (row[column].ToString().IsNullOrTrimEmpty())
                        { // If row has nothing, set 0 value for it
                            row[column] = 0;
                        }
                    }

                    // Add value to sum column
                    newDataRow[column] = sumOfColumn;
                    sumOfColumn = 0;
                }
            }

            dataTable.Rows.Add(newDataRow);
        }
    }
}