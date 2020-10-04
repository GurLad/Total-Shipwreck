using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSack : Trigger
{
    public float DeathPlace;
    public List<AudioClip> TriggerWords;
    public List<AudioClip> DeathWords;
    private Pickup pickup;
    private void Start()
    {
        pickup = GetComponent<Pickup>();
    }
    public override void Activate()
    {
        SoundController.PlaySound(TriggerWords[Random.Range(0, TriggerWords.Count)], pickup.Holder.Pitch);
    }
    private void Update()
    {
        if (transform.position.y <= DeathPlace)
        {
            GameController.Current.DeadTreasures++;
            SoundController.PlaySound(DeathWords[Random.Range(0, TriggerWords.Count)], Random.Range(0, 4) * 0.2f + 0.7f);
            Destroy(gameObject);
        }
    }
}
