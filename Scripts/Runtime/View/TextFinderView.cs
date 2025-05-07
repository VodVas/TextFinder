#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VodVas.TextFinder
{
    public class TextFinderView
    {
        private readonly ITextFinderPresenter _presenter;
        private readonly IHierarchyPathProvider _pathProvider;
        private Vector2 _scrollPosition;
        private GUIStyle _headerStyle;
        private bool _stylesInitialized;

        public TextFinderView(ITextFinderPresenter presenter, IHierarchyPathProvider pathProvider)
        {
            _presenter = presenter;
            _pathProvider = pathProvider;
        }

        public void Draw()
        {
            InitializeStyles();
            DrawHeader();
            EditorGUILayout.Space(27);
            DrawControls();
            DrawTextObjectList();
            DrawFooter();
        }

        private void InitializeStyles()
        {
            if (_stylesInitialized) return;

            _headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = new Color(1f, 0.5f, 0.2f),
                    background = CreateColoredTex(new Color(0f, 0f, 0f))
                },
                padding = new RectOffset(15, 15, 12, 12),
                margin = new RectOffset(0, 0, 15, 15),
                fixedHeight = 60,
                stretchWidth = true
            };

            _stylesInitialized = true;
        }

        private Texture2D CreateColoredTex(Color col)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, col);
            tex.Apply();
            return tex;
        }

        private void DrawHeader()
        {
            EditorGUILayout.LabelField("TEXT FINDER", _headerStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space(10);
        }

        private void DrawControls()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(_presenter.IsSearching);
            if (GUILayout.Button("Start", GUILayout.Height(30)))
            {
                _presenter.FindAllTextObjects();
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Clear", GUILayout.Height(30)))
            {
                _presenter.ClearResults();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

            _presenter.SearchFilter = EditorGUILayout.TextField("Search:", _presenter.SearchFilter);

            EditorGUILayout.BeginHorizontal();
            _presenter.ShowLegacyText = EditorGUILayout.ToggleLeft("Legacy UI Text", _presenter.ShowLegacyText, GUILayout.Width(120));
            _presenter.ShowTMP = EditorGUILayout.ToggleLeft("TextMeshPro", _presenter.ShowTMP, GUILayout.Width(120));
            _presenter.ShowTMPUI = EditorGUILayout.ToggleLeft("TMP UI", _presenter.ShowTMPUI, GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();

            _presenter.IncludeInactive = EditorGUILayout.ToggleLeft("Include Inactive Objects", _presenter.IncludeInactive);

            EditorGUILayout.EndVertical();

            if (_presenter.TotalCount > 0)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Statistics", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Total Text Objects: {_presenter.TotalCount}");

                if (_presenter.LegacyTextCount > 0)
                    EditorGUILayout.LabelField($"Legacy UI Text: {_presenter.LegacyTextCount}");
                if (_presenter.TmpTextCount > 0)
                    EditorGUILayout.LabelField($"TextMeshPro: {_presenter.TmpTextCount}");
                if (_presenter.TmpUITextCount > 0)
                    EditorGUILayout.LabelField($"TextMeshProUGUI: {_presenter.TmpUITextCount}");

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();
        }

        private void DrawTextObjectList()
        {
            if (_presenter.IsSearching)
            {
                EditorGUILayout.HelpBox("Searching for text objects...", MessageType.Info);
                return;
            }

            if (_presenter.TotalCount == 0)
            {
                EditorGUILayout.HelpBox("No text objects found. Click START to find text objects in the scene.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField("Text Objects:", EditorStyles.boldLabel);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            foreach (var textObj in _presenter.GetFilteredObjects())
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();

                GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);

                switch (textObj.Type)
                {
                    case TextObjectType.LegacyText:
                        labelStyle.normal.textColor = new Color(0.9f, 0.4f, 0.3f);
                        break;
                    case TextObjectType.TextMeshPro:
                        labelStyle.normal.textColor = new Color(0.3f, 0.6f, 0.9f);
                        break;
                    case TextObjectType.TextMeshProUGUI:
                        labelStyle.normal.textColor = new Color(0.3f, 0.9f, 0.4f);
                        break;
                }

                string activeStatus = textObj.IsActive ? "" : " (inactive)";
                EditorGUILayout.LabelField($"{textObj.GameObject.name}{activeStatus} ({textObj.Type})", labelStyle);

                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    _presenter.SelectGameObject(textObj.GameObject);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("Path:", EditorStyles.miniBoldLabel);
                EditorGUILayout.LabelField(_pathProvider.GetHierarchyPath(textObj.GameObject), EditorStyles.miniLabel);

                EditorGUILayout.LabelField("Content:", EditorStyles.miniBoldLabel);
                EditorGUILayout.LabelField(textObj.Content, EditorStyles.wordWrappedLabel);

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawFooter()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tip: Click on an object to select it in the hierarchy.", EditorStyles.miniLabel);
        }
    }
}
#endif