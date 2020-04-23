using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;

namespace ZDY.Metronic.UI.TagHelpers
{
	[HtmlTargetElement("portlet")]
	public class PortletTagHelper : HelperBase<Flat2Icon>
	{
		public virtual string Title { get; set; } = "Portlet";

		public virtual string SubTitle { get; set; }

		public virtual Size Size { get; set; } = Size.None;

		public virtual Fit Fit { get; set; } = Fit.None;

		public virtual State TitleState { get; set; } = State.None;

		public virtual State IconState { get; set; } = State.None;

		public virtual State SolidState { get; set; } = State.None;

		public virtual Fit HanderFit { get; set; } = Fit.None;

		public virtual bool IsHeaderDestroyed { get; set; } = false;

		public virtual bool IsHeaderBordered { get; set; } = true;

		public virtual bool IsLayoutSpaced { get; set; } = false;

		public virtual bool IsHeightFluid { get; set; } = false;

		public virtual int BodyHeight { get; set; }

		protected virtual string Classes
		{
			get
			{
				return CssClassBuilder.Build(
					new CssClass("kt-portlet", true),
					new CssClass("kt-portlet--height-fluid", IsHeightFluid),
					new CssClass("kt-portlet--space", IsLayoutSpaced),
					new CssClass($"kt-portlet--head-{Size.ToValue()}", Size.IsUsed()),
					new CssClass($"kt-portlet--solid-{SolidState.ToValue()}", SolidState.IsUsed()),
					new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
				);
			}
		}

		protected virtual string IconClasses
		{
			get
			{
				return CssClassBuilder.Build(
					new CssClass("kt-portlet__head-icon", true),
					new CssClass($"kt-font-{IconState.ToValue()}", IconState.IsUsed())
				);
			}
		}

		protected virtual string HeadClasses
		{
			get
			{
				return CssClassBuilder.Build(
					new CssClass("kt-portlet__head", true),
					new CssClass($"kt-portlet__head--{HanderFit.ToValue()}", HanderFit.IsUsed()),
					new CssClass("kt-portlet__head--noborder", !IsHeaderBordered)
				);
			}
		}

		protected virtual string TitleClasses
		{
			get
			{
				return CssClassBuilder.Build(
					new CssClass("kt-portlet__head-title", true),
					new CssClass($"kt-font-{TitleState.ToValue()}", TitleState.IsUsed())
				);
			}
		}

		protected virtual string BodyClasses
		{
			get
			{
				return CssClassBuilder.Build(
				   new CssClass("kt-portlet__body", true),
				   new CssClass($"kt-portlet__body--{Fit.ToValue()}", Fit.IsUsed())
				);
			}
		}

		public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
			if (context.TryAddContext<PortletContext, PortletTagHelper>(out PortletContext portletContext))
			{
				var childContent = await output.GetChildContentAsync();

				output.TagName = "div";

				output.TagMode = TagMode.StartTagAndEndTag;

				output.Attributes.Add("id", Id);

				output.Attributes.Add("class", Classes);

				if (!IsHeaderDestroyed)
				{
					if (portletContext.HanderToolbar.IsNotNull())
					{
						output.Attributes.Add("data-portlet", "true");
					}

					var toolbar = portletContext.HanderToolbar ?? portletContext.HanderActionContainer;

					var icon = Icon.IsUsed() ? $"<span class='{IconClasses}'>{Icon.ToIconContent()}</span>" : "";

					var head = $@"<div class='{HeadClasses}'>
						  		 <div class='kt-portlet__head-label'>
						  	 		{icon}
						  	 		<h3 class='{TitleClasses}'>
						  	 			{Title}
										<small>{SubTitle}</small>
						  	 		</h3>
						  		 </div>
								 {Use(toolbar)}
							  </div>";

					output.Content.AppendHtml(head);
				}

				var body = new TagBuilder("div");

				body.Attributes.Add("class", BodyClasses);

				if (BodyHeight > 0)
				{
					body.InnerHtml.AppendHtml($@"<div class='kt-scroll' data-scroll='true' data-height='{BodyHeight}' data-scrollbar-shown='true'>
												   {childContent.GetContent()}
												 </div>");
				}
				else
				{
					body.InnerHtml.AppendHtml(childContent.GetContent());
				}

				output.Content.AppendHtml(body);

				if (portletContext.FooterActionContainer.IsNotNull())
				{
					output.Content.AppendHtml(portletContext.FooterActionContainer);
				}

				return;
			}

			output.SuppressOutput();
		}
	}
}
