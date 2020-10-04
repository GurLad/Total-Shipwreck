using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    public float Speed;
    private void Update()
    {
        transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);
    }
}
