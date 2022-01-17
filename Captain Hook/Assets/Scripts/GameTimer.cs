using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timer;
    private float time = 0f;
    private int seconds = 0;
    private int minutes = 0;
    private int hours = 0;
    private bool endOfGameReached = false;

    void Update()
    {
        if (!endOfGameReached) {
            time += Time.deltaTime;

            hours = (int)time / 3600;
            minutes = ((int)time / 60) % 60;
            seconds = ((int)time / 1) % 60;

            timer.SetText(hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString());
        }

    }
    public void EndOfGameReached()
    {
        endOfGameReached = true;
    }
}
