#if UNITY_EDITOR
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VodVas.TextFinder
{
    public class TextMeshProFinder : ITextObjectFinder
    {
        public TextObjectType SupportedType => TextObjectType.TextMeshPro;

        public IEnumerable<TextObjectInfo> FindTextObjects(bool includeInactive)
        {
            TextMeshPro[] texts = includeInactive
                ? Resources.FindObjectsOfTypeAll<TextMeshPro>()
                : Object.FindObjectsOfType<TextMeshPro>();

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