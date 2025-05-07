#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VodVas.TextFinder
{
    public class TextFinderWindow : EditorWindow
    {
        private TextFinderPresenter _presenter;
        private TextFinderView _view;
        private TextFinderService _service;
        private HierarchyPathProvider _pathProvider;

        [MenuItem("Tools/VodVas/Text Finder")]
        public static void ShowWindow()
        {
            var window = GetWindow<TextFinderWindow>("Text Finder");
            window.minSize = new Vector2(500, 600);
        }

        private void OnEnable()
        {
            _service = new TextFinderService();
            _pathProvider = new HierarchyPathProvider();
            _presenter = new TextFinderPresenter(_service, _pathProvider);
            _view = new TextFinderView(_presenter, _pathProvider);
        }

        private void OnGUI()
        {
            _view.Draw();
        }
    }
}
#endif