using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [HideInInspector]
    public PlayerController Holder;
    private float Speed = 5;
    private Rigidbody rigidbody;
    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public virtual void Pick(PlayerController player)
    {
        Holder = player;
        Destroy(rigidbody);
        player.Pickup(this);
    }
    public void Drop()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.velocity = (transform.forward + -transform.up) * Speed;
    }
}
