#if UNITY_EDITOR
using UnityEngine;

namespace VodVas.TextFinder
{
    public interface IHierarchyPathProvider
    {
        string GetHierarchyPath(GameObject gameObject);
    }
}
#endif