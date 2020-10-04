using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Rigidbody rigidbody;
    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public virtual void Pick(PlayerController player)
    {
        Destroy(rigidbody);
        player.Pickup(this);
    }
    public void Drop()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
    }
}
