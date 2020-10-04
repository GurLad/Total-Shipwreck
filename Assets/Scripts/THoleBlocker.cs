using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THoleBlocker : Trigger
{
    public override void Activate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        List<Hole> holes = new List<Hole>();
        foreach (var item in colliders)
        {
            holes.AddRange(item.GetComponents<Hole>());
        }
        if (holes.Count > 0)
        {
            Hole hole = holes[0];
            GameController.Current.RemoveHole(hole);
            GetComponent<Pickup>().Holder.Drop(GetComponent<Pickup>());
            Destroy(gameObject);
        }
    }
}
