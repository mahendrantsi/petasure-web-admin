using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartPay.Core.CoreMethods
{
    public  class CoreMethods
    {
        public static string StripHTML(string input)
        {
            Regex rx = new Regex("<[^>]*>");
            var strHtml = rx.Replace(input, "");
            strHtml = strHtml.Replace("\n", " ");
            strHtml = strHtml.Replace("&nbsp;", " ");
            strHtml = strHtml.Replace("&amp;", "& ");
            strHtml = strHtml.Replace("&#39;", "'");
            strHtml = strHtml.Replace("&sbquo;", ",");
            strHtml = strHtml.Replace("&rsquo;", "'");

            // ASCII Html
            strHtml = strHtml.Replace("&szlig;", "ß");
            strHtml = strHtml.Replace("&auml;", "ä");
            strHtml = strHtml.Replace("&ouml;", "ö");
            strHtml = strHtml.Replace("&uuml;", "ü");
            strHtml = strHtml.Replace("&Auml;", "Ä");
            strHtml = strHtml.Replace("&Ouml;", "Ö");
            strHtml = strHtml.Replace("&Uuml;", "Ü");
            strHtml = strHtml.Replace("&Eacute;", "É");
            strHtml = strHtml.Replace("&eacute;", "é");
            strHtml = strHtml.Replace("&easter;", "⩮");

            // Other characters
            strHtml = strHtml.Replace("&pound;", "£");
            return strHtml;
        }
    }
}
