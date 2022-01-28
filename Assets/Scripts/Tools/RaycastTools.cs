using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Raycasting tools for quickly executing a raycast
/// </summary>
public static class RaycastTools
{
    public static bool RayCastFromMouse(LayerMask mask, out RaycastHit hit, bool ignoreUICheck = false)
    {
        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        return RayCastFromPos(ray, mask, out hit, ignoreUICheck);
    }

    public static bool RayCastFromPos(Vector3 position, Vector3 direction, LayerMask mask, out RaycastHit hit, bool ignoreUICheck = false)
    {
        Ray ray = new Ray(position, direction);
        return RayCastFromPos(ray, mask, out hit, ignoreUICheck);
    }

    public static bool RayCastFromPos(Ray ray, LayerMask mask, out RaycastHit hit, bool ignoreUICheck = false)
    {
        bool hitSomething = Physics.Raycast(ray, out hit, 1000, mask);
        if (!ignoreUICheck && EventSystem.current.IsPointerOverGameObject()) return false;
        return hitSomething;
    }

    public static bool RaycastToPos(Vector3 start, Vector3 end, LayerMask mask, out RaycastHit hit, bool ignoreUICheck = false)
    {
        Ray ray = new Ray(start, end - start);
        return RayCastFromPos(ray, mask, out hit, ignoreUICheck);
    }

    public static float GetDistanceFromPointer(Vector3 position, LayerMask mask, bool ignoreUICheck = false)
    {
        if (RayCastFromMouse(mask, out RaycastHit hit, ignoreUICheck))
            return Vector3.Distance(hit.point, position);
        return float.MaxValue;
    }
}
