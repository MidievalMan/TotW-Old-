using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutscene : MonoBehaviour
{
    PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        director.Play();
    }
}
