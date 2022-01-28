using UnityEngine;

public static class Extensions
{
    public static float DistanceSqr(this Vector3 v3, Vector3 a)
    {
        return (v3 - a).sqrMagnitude;
    }

    public static bool IsGrounded(this Rigidbody body, LayerMask groundLayer, int maxDist = 1000)
    {
        return (Physics.Raycast(body.gameObject.transform.position, Vector3.down, maxDist, groundLayer, QueryTriggerInteraction.Ignore));
    }
}