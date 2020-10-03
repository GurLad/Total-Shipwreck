using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;
    public float Force;
    public float ResetForce;
    public float Accuracy;
    [Header("Items")]
    public float DistanceToCheck;
    public float SphereRadius;
    public float CheckHeight;
    [Header("Animations")]
    public AdvancedAnimation IdleAnimation;
    public AdvancedAnimation WalkAnimation;
    [HideInInspector]
    public bool Active = true;
    private Rigidbody rigidbody;
    private Pickup item;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        IdleAnimation.Activate();
    }
    protected virtual void Update()
    {
        if (Active)
        {
            // Move
            Vector3 direction = Move();
            if (direction.magnitude > 0.01f)
            {
                transform.LookAt(transform.position + direction);
            }
            // Items
            if (Control.GetButtonDown(Control.CB.Pickup))
            {
                if (item == null)
                {
                    Vector3 checkPos = transform.position + direction.normalized * DistanceToCheck;
                    checkPos.y = transform.position.y;
                    Collider[] colliders = Physics.OverlapBox(checkPos, new Vector3(SphereRadius, CheckHeight, SphereRadius));
                    List<Pickup> pickups = new List<Pickup>();
                    foreach (var item in colliders)
                    {
                        pickups.AddRange(item.GetComponents<Pickup>());
                    }
                    if (pickups.Count > 0)
                    {
                        Pickup(pickups[0]);
                    }
                }
                else
                {
                    Drop(item);
                }
            }
        }
    }
    private Vector3 Move()
    {
        float tempY = rigidbody.velocity.y;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        if (rigidbody.velocity.magnitude > Speed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * Speed;
        }
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, tempY, rigidbody.velocity.z);
        Vector3 TargetVelocity = new Vector3(Control.GetAxis(Control.Axis.X), 0, Control.GetAxis(Control.Axis.Y)) * Speed;
        if (TargetVelocity == Vector3.zero)
        {
            Vector3 force = -rigidbody.velocity * ResetForce;
            force.y = 0;
            rigidbody.AddForce(force);
            if (!IdleAnimation.Active)
            {
                IdleAnimation.Activate(true);
                WalkAnimation.Active = false;
            }
            return Vector3.zero;
        }
        if (!WalkAnimation.Active)
        {
            WalkAnimation.Activate(true);
            IdleAnimation.Active = false;
        }
        TargetVelocity.y = rigidbody.velocity.y;
        if ((rigidbody.velocity - TargetVelocity).magnitude <= 1 / Accuracy)
        {
            rigidbody.velocity = TargetVelocity;
        }
        else
        {
            Vector3 force = TargetVelocity * Force;
            force.y = 0;
            rigidbody.AddForce(force);
        }
        Vector3 temp = rigidbody.velocity;
        temp.y = 0;
        return temp;
    }
    public void Pickup(Pickup pickup)
    {
        item = pickup;
        pickup.Pick();
        pickup.transform.parent = transform;
        pickup.transform.localPosition = new Vector3(0, 0, 1);
    }
    public void Drop(Pickup pickup)
    {
        item = null;
        pickup.transform.parent = null;
        pickup.Drop();
    }
}
