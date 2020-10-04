using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTime : MonoBehaviour
{
    public bool MusicOnly;
    private void Start()
    {
        if (!MusicOnly)
        {
            Time.timeScale = 0;
        }
        CrossfadeMusicPlayer.Instance.Play("Menu", false);
    }
}
