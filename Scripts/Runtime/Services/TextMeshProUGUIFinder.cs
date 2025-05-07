#if UNITY_EDITOR
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VodVas.TextFinder
{
    public class TextMeshProUGUIFinder : ITextObjectFinder
    {
        public TextObjectType SupportedType => TextObjectType.TextMeshProUGUI;

        public IEnumerable<TextObjectInfo> FindTextObjects(bool includeInactive)
        {
            TextMeshProUGUI[] texts = includeInactive
                ? Resources.FindObjectsOfTypeAll<TextMeshProUGUI>()
                : Object.FindObjectsOfType<TextMeshProUGUI>();

            foreach (var text in texts)
            {
                if (IsSceneObject(text.gameObject))
                {
                    yield return new TextObjectInfo(text.gameObject, SupportedType, text.text);
                }
            }
        }

        private bool IsSceneObject(GameObject obj)
        {
            return obj.scene.isLoaded &&
                  (obj.hideFlags == HideFlags.None || obj.hideFlags == HideFlags.NotEditable);
        }
    }
}
#endif