#if UNITY_EDITOR
using UnityEngine;

namespace VodVas.TextFinder
{
    public class TextObjectInfo
    {
        public GameObject GameObject { get; }
        public TextObjectType Type { get; }
        public string Content { get; }
        public bool IsActive { get; }

        public TextObjectInfo(GameObject gameObject, TextObjectType type, string content)
        {
            GameObject = gameObject;
            Type = type;
            Content = content;
            IsActive = gameObject.activeInHierarchy;
        }
    }
}
#endif