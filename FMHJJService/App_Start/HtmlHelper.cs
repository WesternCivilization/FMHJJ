using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool?>> expression, object htmlAttributes)
        {

            ModelMetadata modelMeta = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            bool? value = (modelMeta.Model as bool?);

            string name = ExpressionHelper.GetExpressionText(expression);

            return htmlHelper.CheckBox(name, value ?? true, htmlAttributes);
        }
    }
}