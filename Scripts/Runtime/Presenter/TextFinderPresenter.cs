#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VodVas.TextFinder
{
    public class TextFinderPresenter : ITextFinderPresenter
    {
        private readonly TextFinderService _service;
        private readonly IHierarchyPathProvider _pathProvider;
        private List<TextObjectInfo> _textObjects = new List<TextObjectInfo>();

        public bool IsSearching { get; private set; }
        public int TotalCount => _textObjects.Count;
        public int LegacyTextCount => _textObjects.Count(o => o.Type == TextObjectType.LegacyText);
        public int TmpTextCount => _textObjects.Count(o => o.Type == TextObjectType.TextMeshPro);
        public int TmpUITextCount => _textObjects.Count(o => o.Type == TextObjectType.TextMeshProUGUI);

        public string SearchFilter { get; set; } = string.Empty;
        public bool ShowLegacyText { get; set; } = true;
        public bool ShowTMP { get; set; } = true;
        public bool ShowTMPUI { get; set; } = true;
        public bool IncludeInactive { get; set; } = false;

        public TextFinderPresenter(TextFinderService service, IHierarchyPathProvider pathProvider)
        {
            _service = service;
            _pathProvider = pathProvider;
        }

        public void FindAllTextObjects()
        {
            IsSearching = true;

            try
            {
                _textObjects = _service.FindTextObjects(
                    IncludeInactive,
                    ShowLegacyText,
                    ShowTMP,
                    ShowTMPUI
                ).ToList();
            }
            finally
            {
                IsSearching = false;
            }
        }

        public void ClearResults()
        {
            _textObjects.Clear();
            (_pathProvider as HierarchyPathProvider)?.ClearCache();
        }

        public IEnumerable<TextObjectInfo> GetFilteredObjects()
        {
            return _textObjects.Where(obj =>
                ((obj.Type == TextObjectType.LegacyText && ShowLegacyText) ||
                 (obj.Type == TextObjectType.TextMeshPro && ShowTMP) ||
                 (obj.Type == TextObjectType.TextMeshProUGUI && ShowTMPUI)) &&
                (string.IsNullOrEmpty(SearchFilter) ||
                 obj.GameObject.name.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 obj.Content.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 _pathProvider.GetHierarchyPath(obj.GameObject).IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0)
            );
        }

        public void SelectGameObject(GameObject gameObject)
        {
            Selection.activeGameObject = gameObject;
            EditorGUIUtility.PingObject(gameObject);
        }
    }
}
#endif