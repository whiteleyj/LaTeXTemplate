namespace LaTeXTemplate
{
    public static class TemplateConsts
    {
        public const string TMPL_START = @"%<";
        public const string TMPL_END = @">%";
        public static readonly int TMPL_LEN = TMPL_START.Length;


        public static string LaTeXEscape(string data)
        {
            // I dunno where to put this :(
            return data.Replace(@"\", @"\textbackslash")
                       .Replace("~", @"\textasciitilde")
                       .Replace("^", @"\textasciicircum")
                       .Replace("&", @"\&")
                       .Replace("%", @"\%")
                       .Replace("$", @"\$")
                       .Replace("#", @"\#")
                       .Replace("_", @"\_")
                       .Replace("}", @"\}")
                       .Replace("{", @"\{");
        }
    }
}