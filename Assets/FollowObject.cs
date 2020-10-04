using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float SmoothSpeed;
    private void FixedUpdate()
    {
        Vector3 pos = transform.position - Offset;
        Vector3 targetPos = Target.position;
        pos.x = Mathf.Lerp(pos.x, targetPos.x, SmoothSpeed * Time.deltaTime * 30) + Offset.x;
        pos.y = Mathf.Lerp(pos.y, targetPos.y, SmoothSpeed * Time.deltaTime * 30) + Offset.y;
        pos.z = Mathf.Lerp(pos.z, targetPos.z, SmoothSpeed * Time.deltaTime * 30) + Offset.z;
        transform.position = pos;
    }
}
