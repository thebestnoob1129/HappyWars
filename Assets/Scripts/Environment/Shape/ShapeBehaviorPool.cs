using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public static class ShapeBehaviorPool<T> where T : ShapeBehavior, new()
{
    static Stack<T> stack = new Stack<T>();// Lists but without unity

    public static T Get()
    {
        if (stack.Count > 0)
        {
            T behavior = stack.Pop();
            return behavior;
        }
#if UNITY_EDITOR
        return ScriptableObject.CreateInstance<T>();
#else
        return new T();
#endif
    }

    public static void Reclaim (T behavior)
    {
        stack.Push(behavior);
    }
}