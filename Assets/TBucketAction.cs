using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBucketAction : Trigger
{
    public float Amount;
    public GameObject WaterObject;
    private bool full;
    public override void Activate()
    {
        if (!full && transform.position.y <= GameController.Current.WaterHeight)
        {
            full = true;
            GameController.Current.WaterValue -= Amount;
            WaterObject.SetActive(true);
        }
    }
}
