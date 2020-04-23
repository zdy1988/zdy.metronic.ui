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
    [HtmlTargetElement("note-item", ParentTag = "note-list", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class NoteItemTagHelper : HelperBase<Flat2Icon>
    {
		public virtual string Title { get; set; }

		public virtual string Descrption { get; set; }

		public virtual string Content { get; set; }

		public virtual State State { get; set; }

		public virtual string Placeholder { get; set; }

		public virtual string ImageUrl { get; set; }

		public virtual string NavigationUrl { get; set; } = "javascript:;";

		protected virtual bool IsCleaned 
		{
			get
			{
				return String.IsNullOrWhiteSpace(Placeholder) && String.IsNullOrWhiteSpace(ImageUrl) && !Icon.IsUsed();
			}
		}


		public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			output.SuppressOutput();

			if (context.TryAddContext<NoteItemContext, NoteItemTagHelper>(out NoteItemContext noteItemContext))
			{
				context.TryAddContext<BadgePlacedContext, BadgeTagHelper>(out BadgePlacedContext badgePlacedContext);

				var childContent = await output.GetChildContentAsync();

				var item = new TagBuilder("div");

				item.AddCssClass("kt-notes__item");

				if (IsCleaned)
				{
					item.AddCssClass("kt-notes__item--clean");
				}

				item.InnerHtml.AppendHtml(BuildMedia());

				Content = String.IsNullOrWhiteSpace(Content) ? childContent.GetContent() : Content;

				var content = new TagBuilder("div");

				content.AddCssClass("kt-notes__content");

				content.InnerHtml.AppendHtml(BuildInfoSection(noteItemContext, badgePlacedContext));

				content.InnerHtml.AppendHtml($"<span class='kt-notes__body'>{Content}</span>");

				item.InnerHtml.AppendHtml(content);

				if (context.TryGetContext<NoteListContext, NoteListTagHelper>(out NoteListContext noteListContext))
				{
					noteListContext.Notes.Add(item);
				}
			}
		}

		internal TagBuilder BuildMedia()
		{
			var media = new TagBuilder("div");

			media.AddCssClass("kt-notes__media");

			if (!String.IsNullOrWhiteSpace(ImageUrl))
			{
				media.InnerHtml.AppendHtml($"<img src='{ImageUrl}' alt='image'>");
			}
			else if (Icon.IsUsed())
			{
				media.InnerHtml.AppendHtml($"<span class='kt-notes__icon kt-notes__icon--{State.ToValue()} kt-font-boldest'>{Icon.ToIconContent($"kt-font-{State.ToValue()}")}</span>");
			}
			else if (!string.IsNullOrWhiteSpace(Placeholder))
			{
				media.InnerHtml.AppendHtml($"<h3 class='kt-notes__user kt-font-{State.ToValue()} kt-font-boldest'>{Placeholder.SubByteString(2)}</h3>");
			}
			else
			{
				media.InnerHtml.AppendHtml("<span class='kt-notes__circle'></span>");
			}

			return media;
		}

		internal TagBuilder BuildInfoSection(NoteItemContext noteItemContext, BadgePlacedContext badgePlacedContext)
		{
			var section = new TagBuilder("div");

			section.AddCssClass("kt-notes__section");

			var info = new TagBuilder("div");

			info.AddCssClass("kt-notes__info");

			if (!String.IsNullOrWhiteSpace(Title))
			{
				info.InnerHtml.AppendHtml($"<a href='{NavigationUrl}' class='kt-notes__title'>{Title}</a>");
			}

			if (!String.IsNullOrWhiteSpace(Descrption))
			{
				info.InnerHtml.AppendHtml($"<span class='kt-notes__desc'>{Descrption}</span>");
			}

			if (badgePlacedContext.Badges.Any())
			{
				foreach (var badge in badgePlacedContext.Badges)
				{
					info.InnerHtml.AppendHtml(badge);
				}
			}

			section.InnerHtml.AppendHtml(info);

			if (noteItemContext.Dropdown.IsNotNull())
			{
				section.InnerHtml.AppendHtml(noteItemContext.Dropdown);
			}

			return section;
		}
    }
}
