namespace ZDY.Metronic.UI.Untils
{

    internal class CssClass
    {
        internal CssClass(string className, bool isAppend)
        {
            this.ClassName = className;
            this.IsAppend = isAppend;
        }

        internal string ClassName { get; set; }
        internal bool IsAppend { get; set; }
    }
}
