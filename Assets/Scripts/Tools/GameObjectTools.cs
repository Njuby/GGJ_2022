using UnityEngine;

public static class GameObjectTools
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null) component = obj.AddComponent<T>();
        return component;
    }
}
