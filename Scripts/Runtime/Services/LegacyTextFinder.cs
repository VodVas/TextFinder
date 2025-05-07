#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VodVas.TextFinder
{
    public class LegacyTextFinder : ITextObjectFinder
    {
        public TextObjectType SupportedType => TextObjectType.LegacyText;

        public IEnumerable<TextObjectInfo> FindTextObjects(bool includeInactive)
        {
            Text[] texts = includeInactive
                ? Resources.FindObjectsOfTypeAll<Text>()
                : Object.FindObjectsOfType<Text>();

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