#if UNITY_EDITOR
using System.Collections.Generic;

namespace VodVas.TextFinder
{
    public class TextFinderService
    {
        private readonly List<ITextObjectFinder> _finders = new List<ITextObjectFinder>();

        public TextFinderService()
        {
            _finders.Add(new LegacyTextFinder());
            _finders.Add(new TextMeshProFinder());
            _finders.Add(new TextMeshProUGUIFinder());
        }

        public IEnumerable<TextObjectInfo> FindTextObjects(bool includeInactive, bool includeLegacyText, bool includeTMP, bool includeTMPUI)
        {
            var results = new List<TextObjectInfo>();

            foreach (var finder in _finders)
            {
                if ((finder.SupportedType == TextObjectType.LegacyText && includeLegacyText) ||
                    (finder.SupportedType == TextObjectType.TextMeshPro && includeTMP) ||
                    (finder.SupportedType == TextObjectType.TextMeshProUGUI && includeTMPUI))
                {
                    results.AddRange(finder.FindTextObjects(includeInactive));
                }
            }

            return results;
        }
    }
}
#endif