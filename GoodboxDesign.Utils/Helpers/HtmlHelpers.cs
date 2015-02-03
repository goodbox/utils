using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using GoodboxDesign.Utils.Entities;

namespace GoodboxDesign.Utils.Helpers
{
    public static class HtmlHelpers
    {
        private static string GetQueryString(object[] keys, object[] values)
        {
            if (values != null && values.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < values.Length; i++)
                {
                    sb.Append("&" + keys[i] + "=" + values[i]);
                }

                return sb.ToString();
            }
            return "";
        }

        public static MvcHtmlString PagerSummary<T>(this HtmlHelper helper, PagedDataResult<T> pagedDataResult)
        {
            string s = "Showing " + ((pagedDataResult.RecordsPerPage * pagedDataResult.CurrentPage) + 1) + " - " +
                (((pagedDataResult.RecordsPerPage * (pagedDataResult.CurrentPage + 1)) > pagedDataResult.TotalRecords ? pagedDataResult.TotalRecords : (pagedDataResult.RecordsPerPage * (pagedDataResult.CurrentPage + 1)))) + " of " +
                pagedDataResult.TotalRecords + " records.";
            
            return new MvcHtmlString(s);
        }

        public static MvcHtmlString TableHeader<T>(this HtmlHelper helper, string page, PagedDataResult<T> pagedDataResult, string[] columns, string[] columnNames)
        {
            StringBuilder sb = new StringBuilder();
            
            string template = "<th><a href=\"{0}\">{1}&nbsp;<span>{2}</span></a></th>";

            string caretDown = "&#x25BC;";

            string caretUp = "&#x25B2;";

            for (int i = 0; i < columns.Length; i++)
            {
                string url = page + "?" + Enums.QueryString.SortExpression + "=" + columns[i] + "&" + 
                    Enums.QueryString.SortDirection + "=" + (pagedDataResult.SortExpression == columns[i] && pagedDataResult.SortDirection == Enums.SortDirection.Asc ? Enums.SortDirection.Desc : Enums.SortDirection.Asc);

                string caret = string.Empty;
                if(pagedDataResult.SortExpression == columns[i])
                {
                    if (pagedDataResult.SortDirection == Enums.SortDirection.Asc)
                    {
                        caret = caretUp;
                    }
                    else
                    {
                        caret = caretDown;
                    }
                }

                sb.Append(string.Format(template, url, columnNames[i], caret));
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString Pager<T>(
            this HtmlHelper helper,
            PagedDataResult<T> pagedDataResult,
            string page,
            int pagesForward,
            object[] keys,
            object[] values)
        {
            StringBuilder sb = new StringBuilder();

            int pageIndex = 0;

            if (pagedDataResult != null)
            {
                pagedDataResult.CurrentPage = pagedDataResult.CurrentPage + 1;

                string queryStringParams = GetQueryString(keys, values);
                for (pageIndex = pagedDataResult.CurrentPage - 1; pageIndex > pagedDataResult.CurrentPage - pagesForward && pageIndex >= 1; pageIndex--)
                {
                    sb.Insert(0, string.Format("<li><a class=\"ajaxPager\" page=\"" + (pageIndex) + "\" href=\"{0}\">{1}</a></li>", page + "?" + Enums.QueryString.Page + "="
                        + (pageIndex - 1) + queryStringParams,
                        pageIndex));
                }

                // create the spacer and one before
                if (pageIndex >= 1)
                {
                    sb.Insert(0, "<li class=\"disabled\"><a href=\"#\">...</a></li>");
                    sb.Insert(0, string.Format("<li><a class=\"ajaxPager\" page=\"0\" href=\"{0}\">{1}</a></li>", page + "?" + Enums.QueryString.Page + "=0" + queryStringParams, 1));
                }

                // create the links forward
                if (pagedDataResult.TotalPages > 1)
                {
                    sb.AppendFormat("<li class=\"active\"><a href=\"#\">{0}</a></li>", pagedDataResult.CurrentPage);
                }

                // this renders three after
                for (pageIndex = pagedDataResult.CurrentPage + 1; pageIndex < pagedDataResult.CurrentPage + pagesForward && pageIndex <= pagedDataResult.TotalPages; pageIndex++)
                {
                    sb.AppendFormat("<li><a class=\"ajaxPager\" page=\"" + (pageIndex) + "\" href=\"{0}\">{1}</a></li>", page + "?" + Enums.QueryString.Page + "="
                        + (pageIndex - 1) + queryStringParams,
                        pageIndex);
                }

                if (pageIndex <= pagedDataResult.TotalPages)
                {
                    sb.AppendFormat("<li class=\"disabled\"><a href=\"#\">...</a></li>");
                    sb.AppendFormat("<li><a class=\"ajaxPager\" page=\"" + (pagedDataResult.TotalPages - 1) + "\" href=\"{0}\">{1}</a></li>", page + "?" + Enums.QueryString.Page
                        + "=" + (pagedDataResult.TotalPages - 1) + queryStringParams,
                    pagedDataResult.TotalPages);
                }


                // do the next/prev
                //	 previous
                if (pagedDataResult.CurrentPage > 1)
                {
                    sb.Insert(0,
                        string.Format("<li><a class=\"ajaxPager\" page=\"" + (pagedDataResult.CurrentPage - 2) + "\" href=\"{0}\">{1}</a></li>",
                        page + "?" + Enums.QueryString.Page + "=" + (pagedDataResult.CurrentPage - 2) + queryStringParams,
                        "&laquo;&nbsp;Prev"));
                }
                else
                {
                    sb.Insert(0, string.Format("<li class=\"disabled\"><a href=\"javascript:;\">{0}</a></li>", "&laquo;&nbsp;Prev"));
                }

                if (pagedDataResult.CurrentPage < pagedDataResult.TotalPages)
                {
                    sb.AppendFormat("<li><a class=\"ajaxPager\" page=\"" + (pagedDataResult.CurrentPage) + "\" href=\"{0}\">{1}</a></li>", page + "?" + Enums.QueryString.Page + "="
                        + (pagedDataResult.CurrentPage) + queryStringParams,
                    "Next&nbsp;&raquo;");
                }
                else
                {
                    sb.AppendFormat("<li class=\"disabled\"><a href=\"javascript:;\">{0}</a></li>", "Next&nbsp;&raquo;");
                }
            }

            sb.Insert(0, "<ul class=\"pagination\">");
            sb.Append("</ul>");

            return new MvcHtmlString(sb.ToString());
        }



        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum selectedValue)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString(),
                    Selected = (value.Equals(selectedValue))
                };

            return htmlHelper.DropDownList(
                name,
                items
                );
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                values.Select(value => new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                });

            return htmlHelper.DropDownListFor(
                expression,
                items
                );
        }
    }
}