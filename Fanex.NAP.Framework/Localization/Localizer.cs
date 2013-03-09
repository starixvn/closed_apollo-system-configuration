using System.Linq;

namespace Fanex.NAP.Framework.Localization
{
    public delegate LocalizedString Localizer(string text, params object[] args);
}

namespace Fanex.NAP.Framework.Localization.Mvc.Html
{
    public static class LocalizerExtensions
    {
        public static LocalizedString Plural(this Localizer T, string textSingular, string textPlural, int count, params object[] args)
        {
            return T(count == 1 ? textSingular : textPlural, new object[] { count }.Concat(args).ToArray());
        }
    }
}