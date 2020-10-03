using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float Force;
    public float ResetForce;
    public float Accuracy;
    public bool Active;
    private Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        if (Active)
        {
            if (rigidbody.velocity.magnitude > Speed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * Speed;
            }
            Vector3 TargetVelocity = new Vector3(Control.GetAxis(Control.Axis.X), rigidbody.velocity.y, Control.GetAxis(Control.Axis.Y)) * Speed;
            if (TargetVelocity == Vector3.zero)
            {
                rigidbody.AddForce(-rigidbody.velocity * ResetForce);
                return;
            }
            if ((rigidbody.velocity - TargetVelocity).magnitude <= 1 / Accuracy)
            {
                rigidbody.velocity = TargetVelocity;
            }
            else
            {
                rigidbody.AddForce(TargetVelocity * Force);
            }
        }
    }
}
