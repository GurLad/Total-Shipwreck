using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWater : MonoBehaviour
{
    public Vector3 Speed;
    public float RiseSpeed;
    public float RiseStrength;
    public float HeightMod;
    public float BumpScale;
    public float BumpScaleSpeed;
    public Material Material;
    [HideInInspector]
    public float WaterHeight = 0;
    private Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Speed;
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * RiseSpeed) * RiseStrength + WaterHeight + HeightMod, transform.position.z);
        float bump = Mathf.Sin(Time.time * BumpScaleSpeed);
        Material.SetFloat("_BumpScale", Mathf.Sign(bump) * Mathf.Sqrt(Mathf.Abs(bump)) * BumpScale);
        if (transform.position.x >= 100)
        {
            transform.position -= new Vector3(100, 0, 100);
        }
    }
}
