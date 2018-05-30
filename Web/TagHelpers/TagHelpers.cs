using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace MP.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "helper-for")]
    public sealed class ValidationSummaryTagHelper : TagHelper
    {
        private const string ValidationSummaryAttributeName = "helper-for";

        private ValidationSummary _validationSummary;

        /// <inheritdoc />
        public override int Order
        {
            get
            {
                return -1000;
            }
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext
        {
            get;
            set;
        }

        [HtmlAttributeNotBound]
        protected IHtmlGenerator Generator
        {
            get;
        }

        [HtmlAttributeName("helper-for")]
        public ValidationSummary ValidationSummary
        {
            get
            {
                return this._validationSummary;
            }
            set
            {
                if ((uint)value <= 2u)
                {
                    this._validationSummary = value;
                    return;
                }
                //  throw new ArgumentException(Resources.FormatInvalidEnumArgument("value", value, typeof(ValidationSummary).FullName), "value");
            }
        }

        /// <summary>
        /// 创建新的验证标签
        /// </summary>
        /// <param name="generator"></param>
        public ValidationSummaryTagHelper(IHtmlGenerator generator)
        {
            this.Generator = generator;
        }

        /// <summary>
        /// 生成html
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            if (this.ValidationSummary != 0)
            {
                TagBuilder tagBuilder = this.Generator.GenerateValidationSummary(this.ViewContext, this.ValidationSummary == ValidationSummary.ModelOnly, null, null, null);
                var writer = new System.IO.StringWriter();
                tagBuilder.InnerHtml.WriteTo(writer, HtmlEncoder.Default);
                var message = writer.ToString().Replace("<ul><li>", "").Replace("</li></ul>", "");
                output.Content.SetHtmlContent("<span class='text-danger field-validation-error' data-valmsg-replace='true'><span class=''>" + message + "</span></span>");
            }
        }
    }
}
