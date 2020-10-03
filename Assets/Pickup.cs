using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void Pick()
    {
        Destroy(rigidbody);
    }
    public void Drop()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
    }
}
