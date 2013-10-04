// --------------------------------------------------------------------------
// <copyright file="Formater.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.20">Created</change>
// </history>

using System;
using System.Globalization;
using System.Web;

public class Formater
{
    public const string CULT_NAME_ENG = "en-US";
    public const string CULT_NAME_VI = "vi-VN";

    public const string DECIMAL_FORMAT = "{0:#,###}";
    public const string DECIMAL_FORMAT0 = "{0:#,##0}";
    public const string DECIMAL_FORMAT1 = "{0:#,##0.#}";
    public const string DECIMAL_FORMAT10 = "{0:#,##0.0}";
    public const string DECIMAL_FORMAT2 = "{0:#,##0.##}";
    public const string DECIMAL_FORMAT20 = "{0:#,##0.00}";
    public const string DECIMAL_FORMAT2C = "{0:###0.00}"; //without comma
    public const string DECIMAL_FORMAT3 = "{0:#,##0.###}";
    public const string DECIMAL_FORMAT30 = "{0:#,##0.000}";
    public const string DECIMAL_FORMAT3C = "{0:###0.000}";
    public const string DECIMAL_FORMAT4 = "{0:#,##0.####}";
    public const string DECIMAL_FORMAT40 = "{0:#,##0.0000}";
    public const string DECIMAL_FORMAT5 = "{0:#,##0.#####}";

    public const string DECIMAL_FORMAT_VI = "{0:#.###}";
    public const string DECIMAL_FORMAT0_VI = "{0:#.##0}";
    public const string DECIMAL_FORMAT1_VI = "{0:#.##0,#}";
    public const string DECIMAL_FORMAT10_VI = "{0:#.##0,0}";
    public const string DECIMAL_FORMAT2_VI = "{0:#.##0,##}";
    public const string DECIMAL_FORMAT20_VI = "{0:#.##0,00}";
    public const string DECIMAL_FORMAT2C_VI = "{0:#.##0,00}";
    public const string DECIMAL_FORMAT3_VI = "{0:#.##0,###}";
    public const string DECIMAL_FORMAT30_VI = "{0:#.##0,000}";
    public const string DECIMAL_FORMAT3C_VI = "{0:#.##0,000}";
    public const string DECIMAL_FORMAT4_VI = "{0:#.##0,####}";
    public const string DECIMAL_FORMAT40_VI = "{0:#.##0,0000}";
    public const string DECIMAL_FORMAT5_VI = "{0:#.##0,#####}";

    public Formater()
    {
    }
    #region Decimal
    private static string FormatDecimal(object value, byte decDigits)
    {
        //"{0:0.##}"
        if (value != null)
        {
            string decDigitsString = string.Empty;
            for (int i = 0; i < decDigits; i++)
            {
                decDigitsString += "#";
            }

            string formatString = "{0:#,0." + decDigitsString + "}";
            string result = null;

            if (value != null)
            {
                result = string.Format(formatString, value);
            }

            return result;
        }
        else
        {
            throw new NullReferenceException();
        }
    }

    public static string DecFormat(decimal value, byte decDigits)
    {
        return FormatDecimal(value, decDigits);
    }

    public static string DecFormat(object value, byte decDigits)
    {
        return FormatDecimal(value, decDigits);
    }

    public static string DecFormat(object value, byte decDigits, string numberDefault)
    {
        return (Convert.IsDBNull(value) || value == null) ? numberDefault : DecFormat(Convert.ToDecimal(value), decDigits);
    }

    public static string DecFormat(decimal value, byte decDigits, bool chop)
    {
        if (!chop)
        {
            return string.Format(DECIMAL_FORMAT2, value.ToString("N" + decDigits));
        }
        else
        {
            int a = (int)Math.Pow(10, decDigits);

            //Returns the largest integer less than or equal number 
            if (value < 0)
            {
                //If the value is negative, then we use Ceiling() function instead of Floor() function
                return string.Format(DECIMAL_FORMAT2, Math.Ceiling(value * a) / a);
            }

            return string.Format(DECIMAL_FORMAT2, Math.Floor(value * a) / a);
        }
    }

    public static string DecFormat(object value, byte decDigits, bool chop)
    {
        return DecFormat(Convert.ToDecimal(value), decDigits, chop);
    }
    #endregion

    #region Float
    public static string FloatFormat(float value, byte decDigits)
    {
        return value.ToString("N" + decDigits);
    }

    public static string FloatFormat(object value, byte decDigits)
    {
        return FloatFormat(Convert.ToSingle(value), decDigits);
    }
    #endregion

    #region Double

    public static string DoubleFormat(double value, byte decDigits)
    {
        return value.ToString("N" + decDigits);
    }

    public static string DoubleFormat(object value, byte decDigits)
    {
        return DoubleFormat(Convert.ToDouble(value), decDigits);
    }

    #endregion

    #region Number

    public const string TIME_FORMAT_MILISEC = "HH:mm:ss fff";

    private static System.Globalization.CultureInfo _engCult = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
    private static System.Globalization.CultureInfo _viCult = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");

    public static CultureInfo EngCult
    {
        get
        {
            return _engCult;
        }
    }

    public static CultureInfo ViCult
    {
        get
        {
            return _viCult;
        }
    }

    /// <summary>
    /// Format number with 1 decimal, automatically remove redundant '0'
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static string FormatNumber1(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT1_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT1, obj);
    }

    /// <summary>
    /// Format number with 2 decimal, automatically remove redundant '0'
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static string FormatNumber2(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT2_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT2, obj);
    }

    public static string FormatNumber20(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT20_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT20, obj);
    }

    /// <summary>
    /// Format number with 2 decimal, automatically remove redundant '0'
    /// And do not include comma
    /// </summary>        
    public static string FormatNumber2C(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT2C_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT2C, obj);
    }

    public static string FormatNumber30(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT30_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT30, obj);
    }

    /// <summary>
    /// Format number with 3 decimal, automatically remove redundant '0'
    /// And do not include comma
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static string FormatNumber3C(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT3C_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT3C, obj);
    }

    /// <summary>
    /// Format number with 3 decimal, automatically remove redundant '0'
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string FormatNumber3(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT3_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT3, obj);
    }

    /// <summary>
    /// Format number with 4 decimal, automatically remove redundant '0'
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string FormatNumber4(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT4_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT4, obj);
    }

    public static string FormatNumber4(object obj)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        return string.Format(DECIMAL_FORMAT4, obj);
    }

    public static string FormatNumber40(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT40_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT40, obj);
    }

    public static string FormatNumber5(object obj, string culture)
    {
        if (Convert.IsDBNull(obj) || obj == null)
        {
            return "0";
        }

        if (culture == CULT_NAME_VI)
        {
            return string.Format(DECIMAL_FORMAT5_VI, obj);
        }

        return string.Format(DECIMAL_FORMAT5, obj);
    }

    public static string FormatSignNumber(int number)
    {
        if (number > 0)
        {
            return "+" + number.ToString();
        }

        return number.ToString();
    }
    #endregion

    #region Date
    private static CultureInfo cult = new System.Globalization.CultureInfo("en-US");

    public static string LongDateFormat(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:yyyy-MM-dd HH:mm:ss}", dt);
    }

    public static string ShortDateFormat(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:yyyy-MM-dd}", dt);
    }

    public static string FullDateFormat(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:yyyy-MM-dd hh:mm:ss tt}", dt);
    }

    public static string FullDateFormat2Ln(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:yyyy-MM-dd<br/>hh:mm:ss tt}", dt);
    }

    public static string FullDateFormat2Line(object obj)
    {
        return FullDateFormat2Ln(obj);
    }

    /// <summary>
    /// format short date with no zero leading
    /// 5/23/2013
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatShortDateNoZeroLeading(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:yyyy-MM-dd}", dt);
    }

    /// <summary>
    /// format long date with no zero leading
    /// 5/23/2013 11:10:20
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatDateTimeNoZeroLeading(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:yyyy-MM-dd hh:mm:ss}", dt);
    }

    /// <summary>
    /// format long date time with no zero leading
    /// 5/23/2013 11:10:20 AM
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatLongDateTimeNoZeroLeading(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:yyyy-MM-dd hh:mm:ss tt}", dt);
    }

    /// <summary>
    /// format long time
    /// 11:10:20 AM
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatLongTime(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:hh:mm:ss tt}", dt);
    }

    /// <summary>
    /// format short time
    /// 11:10:20
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatShortTime(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        return string.Format(cult, "{0:hh:mm:ss}", dt);
    }

    /// <summary>
    /// Format date time with only month/day hour:minute
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatDateTime22(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString("MM-dd<br/>hh:mmt").ToLower();
        }
        catch
        {
            return "Error";
        }
    }

    public static string FormatDateTime22DDMM(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString("dd/MM hh:mm");
        }
        catch
        {
            return "Error";
        }
    }

    public static string FormatTimeMiliSec(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString(TIME_FORMAT_MILISEC);
        }
        catch
        {
            return "Error";
        }
    }

    public static string FormatDateEn(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
        }
        catch
        {
            return "Error";
        }
    }

    public static string FormatDateVi(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
        }
        catch
        {
            return "Error";
        }
    }

    public static string FormatDateTimeEn12(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString("yyyy-MM-dd hh:mm:ss tt");
        }
        catch
        {
            return "Error";
        }
    }

    public static string FormatDateTimeEn12NoSec(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString("yyyy-MM-dd hh:mm tt");
        }
        catch
        {
            return "Error";
        }
    }

    public static string FormatDateTimeEn24(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            return Convert.ToDateTime(dt).ToString("yyyy-MM-dd HH:mm:ss");
        }
        catch
        {
            return "Error";
        }
    }

    /// <summary>
    /// IF Time is 12:00, then do not return time
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatDateOptionalTime(object dt)
    {
        if (Convert.IsDBNull(dt) || dt == null)
        {
            return string.Empty;
        }

        try
        {
            DateTime dt2 = Convert.ToDateTime(dt);
            if ((dt2.Hour == 12 || dt2.Hour == 0) && dt2.Minute == 0)
            {
                return dt2.ToString("yyyy-MM-dd");
            }
            else
            {
                return dt2.ToString("yyyy-MM-dd hh:mm:ss tt");
            }
        }
        catch
        {
            return "Error";
        }
    }

    public static DateTime ResetStartDate(DateTime dt)
    {
        return dt.AddMilliseconds(-(((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 + dt.Millisecond));
    }

    public static DateTime ResetEndDate(DateTime dt)
    {
        return ResetStartDate(dt).Add(new TimeSpan(23, 59, 59));
    }

    #endregion

    #region String
    /// <summary>
    /// TruncateWithTip
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string TruncateWithTip(string value, int maxLength, bool hasSpanReturn)
    {
        if (value.Length > maxLength)
        {
            value = string.Format("<span title='{0}'>{1}...</span>", value.Replace("'", "&#39;"), HttpUtility.HtmlEncode(value.Substring(0, maxLength - 3)));
        }
        else
        {
            if (hasSpanReturn)
            {
                value = string.Format("<span>{0}</span>", HttpUtility.HtmlEncode(value));
            }
        }

        return value;
    }

    public static string TruncateWithTip(string value, int maxLength)
    {
        return TruncateWithTip(value, maxLength, false);
    }
    #endregion
    public static string ShowTimeFormat(string showTime)
    {
        return string.IsNullOrEmpty(showTime) ? string.Empty : showTime.Replace("</font>", "<#font>").Replace('/', '-').Replace("<#font>", "</font>").TrimEnd('M');
    }
}