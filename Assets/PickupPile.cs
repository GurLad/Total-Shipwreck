using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPile : Pickup
{
    public Pickup Pickup;
    public int NumPickups;
    public List<GameObject> PickupIndicators;
    private int maxPickups;
    public override void Start()
    {
        maxPickups = NumPickups;
        PickupIndicators.Reverse();
        PickupIndicators[PickupIndicators.Count - 1].SetActive(true);
    }
    public override void Pick(PlayerController player)
    {
        Pickup pickup = Instantiate(Pickup);
        pickup.Start();
        pickup.Pick(player);
        NumPickups--;
        if (NumPickups <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            PickupIndicators.ForEach(a => a.SetActive(false));
            PickupIndicators[(PickupIndicators.Count * NumPickups) / maxPickups].SetActive(true);
        }
    }
}
