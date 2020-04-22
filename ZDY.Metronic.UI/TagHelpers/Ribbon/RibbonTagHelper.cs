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
    [HtmlTargetElement(Attributes = "ribbon")]
    [HtmlTargetElement(Attributes = "ribbon-text")]
    [HtmlTargetElement(Attributes = "ribbon-icon")]
    [HtmlTargetElement(Attributes = "ribbon-state")]
    [HtmlTargetElement(Attributes = "ribbon-position")]
    [HtmlTargetElement(Attributes = "ribbon-border")]
    [HtmlTargetElement(Attributes = "ribbon-shadow")]
    [HtmlTargetElement(Attributes = "ribbon-top")]
    [HtmlTargetElement(Attributes = "ribbon-bottom")]
    [HtmlTargetElement(Attributes = "ribbon-left")]
    [HtmlTargetElement(Attributes = "ribbon-right")]
    public class RibbonTagHelper : TagHelper
    {
        public virtual bool Ribbon { get; set; } = false;

        public virtual string RibbonText { get; set; }

        public virtual Flat2Icon RibbonIcon { get; set; }

        public virtual State RibbonState { get; set; } = State.None;

        public virtual RibbonMode RibbonMode { get; set; } = RibbonMode.None;

        public virtual RibbonPosition RibbonPosition { get; set; } = RibbonPosition.RightAndTop;

        public virtual RibbonBorder RibbonBorder { get; set; } = RibbonBorder.None;

        public virtual bool RibbonShadow { get; set; } = false;

        public virtual int RibbonTop { get; set; } = int.MaxValue;

        public virtual int RibbonBottom { get; set; } = int.MaxValue;

        public virtual int RibbonLeft { get; set; } = int.MaxValue;

        public virtual int RibbonRight { get; set; } = int.MaxValue;

        protected virtual string PlacementValue
        {
            get
            {
                if (RibbonRight <= 0)
                {
                    return RibbonMode == RibbonMode.Clip ? "right" : "left";
                }

                if (RibbonLeft <= 0)
                {
                    return RibbonMode == RibbonMode.Clip ? "left" : "right";
                }

                if (RibbonTop <= 0)
                {
                    return RibbonMode == RibbonMode.Clip ? "" : "bottom";
                }

                if (RibbonBottom <= 0)
                {
                    return RibbonMode == RibbonMode.Clip ? "" : "top";
                }

                return "";
            }
        }

        protected virtual string RibbonStateValue
        {
            get
            {
                return RibbonState.IsUsed() ? RibbonState.ToValue() : "brand";
            }
        }

        protected virtual string Styles
        {
            get
            {
                return StyleBuilder.Build(
                    ("top", $"{RibbonTop}px", RibbonTop != int.MaxValue),
                    ("bottom", $"{RibbonBottom}px", RibbonBottom != int.MaxValue),
                    ("left", $"{RibbonLeft}px", RibbonLeft != int.MaxValue),
                    ("right", $"{RibbonRight}px", RibbonRight != int.MaxValue)
                );
            }
        }

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new CssClass("kt-ribbon", true),
                    new CssClass($"kt-ribbon--{PlacementValue}", true),
                    new CssClass($"kt-ribbon--{RibbonStateValue}", true),
                    new CssClass($"kt-ribbon--{RibbonMode.ToValue()}", RibbonMode.IsUsed()),
                    new CssClass($"kt-ribbon--{RibbonBorder.ToValue()}", RibbonBorder.IsUsed()),
                    new CssClass("kt-ribbon--shadow", RibbonShadow),
                    new CssClass("kt-ribbon--ver", RibbonMode == RibbonMode.Flag)
                );
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Ribbon)
            {
                output.Attributes.RemoveAll("ribbon");

                SetDefaultPosition();

                if (output.Attributes.TryGetAttribute("class", out TagHelperAttribute attribute))
                {
                    output.Attributes.SetAttribute("class", $"{attribute.Value} {Classes}");
                }
                else
                {
                    output.Attributes.Add("class", Classes);
                }

                var ribbon = new TagBuilder("div");

                ribbon.Attributes.Add("class", "kt-ribbon__target");

                ribbon.Attributes.Add("style", Styles);

                if (RibbonMode == RibbonMode.Clip || RibbonMode == RibbonMode.Flag)
                {
                    ribbon.InnerHtml.AppendHtml("<span class='kt-ribbon__inner'></span>");
                }

                if (RibbonIcon.IsUsed())
                {
                    ribbon.InnerHtml.AppendHtml(RibbonIcon.ToIconContent());
                }
                else if(RibbonMode == RibbonMode.Flag)
                {
                    ribbon.InnerHtml.AppendHtml(Flat2Icon.Star.ToIconContent());
                }

                if (!String.IsNullOrWhiteSpace(RibbonText) && RibbonMode != RibbonMode.Flag)
                {
                    if (RibbonIcon.IsUsed())
                    {
                        ribbon.InnerHtml.AppendHtml("<span class='mr-2'></span>");
                    }

                    ribbon.InnerHtml.Append(RibbonText);
                }

                output.Content.AppendHtml(ribbon);
            }
        }

        internal Dictionary<RibbonMode, ValueTuple<int, int>> Positions
        {
            get
            {
                return new Dictionary<RibbonMode, (int, int)> {
                    {RibbonMode.None,(-5, 12)},
                    {RibbonMode.Round,(-5, 12)},
                    {RibbonMode.Clip,(-10, 15)},
                    {RibbonMode.Flag,(20, 0)}
                };
            }
        }

        internal void SetDefaultPosition()
        {
            if (RibbonTop == int.MaxValue && RibbonBottom == int.MaxValue && RibbonLeft == int.MaxValue && RibbonRight == int.MaxValue)
            {
                switch (RibbonPosition)
                {
                    case RibbonPosition.RightAndTop:
                        RibbonRight = Positions[RibbonMode].Item1;
                        RibbonTop = Positions[RibbonMode].Item2;
                        break;
                    case RibbonPosition.RightAndBottom:
                        RibbonRight = Positions[RibbonMode].Item1;
                        RibbonBottom = Positions[RibbonMode].Item2;
                        break;
                    case RibbonPosition.LeftAndTop:
                        RibbonLeft = Positions[RibbonMode].Item1;
                        RibbonTop = Positions[RibbonMode].Item2;
                        break;
                    case RibbonPosition.LeftAndBottom:
                        RibbonLeft = Positions[RibbonMode].Item1;
                        RibbonBottom = Positions[RibbonMode].Item2;
                        break;
                    case RibbonPosition.TopAndRight:
                        RibbonTop = Positions[RibbonMode].Item1;
                        RibbonRight = Positions[RibbonMode].Item2;
                        break;
                    case RibbonPosition.TopAndLeft:
                        RibbonTop = Positions[RibbonMode].Item1;
                        RibbonLeft = Positions[RibbonMode].Item2;
                        break;
                    case RibbonPosition.BottomAndRight:
                        RibbonBottom = Positions[RibbonMode].Item1;
                        RibbonRight = Positions[RibbonMode].Item2;
                        break;
                    case RibbonPosition.ButtomAndLeft:
                        RibbonBottom = Positions[RibbonMode].Item1;
                        RibbonLeft = Positions[RibbonMode].Item2;
                        break;
                }
            }
        }
    }
}
