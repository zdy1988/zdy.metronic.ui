using System;
using ZDY.Metronic.UI.Icons;

namespace ZDY.Metronic.UI
{
    internal static class IconExtension
    {
        internal static string ToIconValue(this object icon)
        {
            if (icon.IsUsed())
            {
                var value = Convert.ToInt32(icon);

                if (icon.GetType() == typeof(FlatIcon))
                {
                    return Flat.Get((FlatIcon)value);
                }
                else if (icon.GetType() == typeof(Flat2Icon))
                {
                    return Flat2.Get((Flat2Icon)value);
                }
                else if (icon.GetType() == typeof(FontawesomeIcon))
                {
                    return Fontawesome.Get((FontawesomeIcon)value);
                }
                else if (icon.GetType() == typeof(LineawesomeIcon))
                {
                    return Lineawesome.Get((LineawesomeIcon)value);
                }
                else if (icon.GetType() == typeof(SocIcon))
                {
                    return Soc.Get((SocIcon)value);
                }
                else if (icon.GetType() == typeof(SvgIcon))
                {
                    return Svg.Get((SvgIcon)value);
                }
            }

            return "";
        }

        internal static string ToIconContent(this object icon, string classNames = "")
        {
            var iconValue = icon.ToIconValue();

            if (!String.IsNullOrWhiteSpace(iconValue))
            {
                if (icon.GetType() == typeof(SvgIcon))
                {
                    return $"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1' class='kt-svg-icon {classNames}'>{iconValue}</svg>";
                }
                else
                {
                    return $"<i class='{iconValue} {classNames}'></i>";
                }
            }

            return "";
        }
    }
}
