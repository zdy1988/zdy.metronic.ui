using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ZDY.Metronic.UI
{
    internal static class HelperContextExtension
    {
        internal static bool TryAddContext<TContext, TTagHelper>(this TagHelperContext context, out TContext tagContext)
            where TContext : IHelperContext, new()
            where TTagHelper : HelperBase
        {
            var isAdd = false;

            var key = typeof(TTagHelper);

            tagContext = default;

            if (!context.Items.ContainsKey(key))
            {
                try
                {
                    tagContext = Activator.CreateInstance<TContext>();

                    context.Items.Add(key, tagContext);

                    isAdd = true;
                }
                catch
                {
                    isAdd = false;
                }
            }

            return isAdd;
        }

        internal static bool TryGetContext<TContext, TTagHelper>(this TagHelperContext context, out TContext tagContext)
            where TContext : IHelperContext
            where TTagHelper : HelperBase
        {
            var isGet = false;

            var key = typeof(TTagHelper);

            tagContext = default;

            if (context.Items.ContainsKey(key))
            {
                isGet = context.Items.TryGetValue(key, out object value);

                if (isGet && value.IsNotNull())
                {
                    tagContext = (TContext)value;
                }
            }

            return isGet;
        }
    }
}
