using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public void Death()
    {
        //SoundManager.PlaySound(SoundManager.Sound.EnemyDeath, GetComponent<Transform>().position);
        Destroy(gameObject);
    }
}
