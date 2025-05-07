#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace VodVas.TextFinder
{
    public class HierarchyPathProvider : IHierarchyPathProvider
    {
        private readonly Dictionary<GameObject, string> _pathCache = new Dictionary<GameObject, string>();

        public string GetHierarchyPath(GameObject gameObject)
        {
            if (_pathCache.TryGetValue(gameObject, out string path))
                return path;

            path = gameObject.name;
            Transform parent = gameObject.transform.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            _pathCache[gameObject] = path;
            return path;
        }

        public void ClearCache()
        {
            _pathCache.Clear();
        }
    }
}
#endif