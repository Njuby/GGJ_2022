using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float viewRadius;
    [SerializeField] private Transform headTransform;
    [SerializeField, ProgressBar(0, 360)] private float viewAngle;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private Transform nearestVisibleTarget;

    public float ViewRadius
    {
        get => viewRadius;
        set => viewRadius = value;
    }

    public float ViewAngle => viewAngle;

    public Transform NearestTarget => nearestVisibleTarget;

    public Transform HeadTransform { get => headTransform; }

    [SerializeField] private Collider[] targetsInViewRadius;
    [SerializeField] private float lowestDistance;
    [SerializeField] private float reactionTime;
    private WaitForSeconds delay;

    private void Start()
    {
        delay = new WaitForSeconds(reactionTime);
        StartCoroutine("HandleDetection");
    }

    private IEnumerator HandleDetection()
    {
        while (true)
        {
            FindVisibleTargets();
            yield return delay;
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += headTransform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool FindVisibleTargets()
    {
        targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetLayerMask);
        lowestDistance = float.MaxValue;
        nearestVisibleTarget = null;
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (!(Vector3.Angle(headTransform.forward, directionToTarget) < viewAngle / 2)) continue;

            //float distanceToTarget = Vector3.Distance(transform.position, target.position);
            float distanceToTarget = transform.position.DistanceSqr(target.position);
            if (distanceToTarget > lowestDistance * lowestDistance) continue;
            lowestDistance = distanceToTarget;
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayerMask))
            {
                nearestVisibleTarget = target;
            }
        }

        return targetsInViewRadius.Length > 0 && nearestVisibleTarget;
    }
}