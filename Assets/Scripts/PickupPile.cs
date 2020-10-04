using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPile : Pickup
{
    public Pickup Pickup;
    public int NumPickups;
    public List<GameObject> PickupIndicators;
    public bool HeightMode;
    public Vector2 Height;
    public Transform HeightIndicator;
    private int maxPickups;
    public override void Start()
    {
        maxPickups = NumPickups;
        if (!HeightMode)
        {
            PickupIndicators.Reverse();
            PickupIndicators[PickupIndicators.Count - 1].SetActive(true);
        }
    }
    public override void Pick(PlayerController player)
    {
        Pickup pickup = Instantiate(Pickup);
        pickup.Start();
        pickup.Pick(player);
        NumPickups--;
        if (!HeightMode)
        {
            if (NumPickups <= 0)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                PickupIndicators.ForEach(a => a.SetActive(false));
                PickupIndicators[PickupIndicators.Count * (NumPickups - 1) / maxPickups].SetActive(true);
            }
        }
        else
        {
            if (NumPickups <= 0)
            {
                Destroy(HeightIndicator.gameObject);
                Destroy(this);
            }
            else
            {
                float percent = (float)NumPickups / maxPickups;
                HeightIndicator.localPosition = new Vector3(0, 0, Height.x * percent + Height.y * (1 - percent));
            }
        }
    }
}
