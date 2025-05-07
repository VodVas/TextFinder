#if UNITY_EDITOR
using System.Collections.Generic;

namespace VodVas.TextFinder
{
    public interface ITextObjectFinder
    {
        TextObjectType SupportedType { get; }
        IEnumerable<TextObjectInfo> FindTextObjects(bool includeInactive);
    }
}
#endif