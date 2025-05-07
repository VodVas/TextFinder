#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace VodVas.TextFinder
{
    public interface ITextFinderPresenter
    {
        bool IsSearching { get; }
        int TotalCount { get; }
        int LegacyTextCount { get; }
        int TmpTextCount { get; }
        int TmpUITextCount { get; }

        string SearchFilter { get; set; }
        bool ShowLegacyText { get; set; }
        bool ShowTMP { get; set; }
        bool ShowTMPUI { get; set; }
        bool IncludeInactive { get; set; }

        void FindAllTextObjects();
        void ClearResults();
        IEnumerable<TextObjectInfo> GetFilteredObjects();
        void SelectGameObject(GameObject gameObject);
    }
}
#endif