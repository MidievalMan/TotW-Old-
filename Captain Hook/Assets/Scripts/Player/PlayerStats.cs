using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // References
    public GrapplingHook hookScript;

    // Misc vars
    private bool addCoinNextFixed = false;

    // Movement Stats
    public static int numCoins = 1;
    public static bool pullHookUnlocked = false;
    public static bool pushHookUnlocked = false;
    public static int hookSpeed = 16;
    public static int hookMaxReach = 16;
    public static Vector2 respawnPoint = new Vector2(0, 0);

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Hook1")) {
            hookScript.aimLine.gameObject.SetActive(true);
            pullHookUnlocked = true;
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("DropStuff")) {
            hookScript.aimLine.gameObject.SetActive(false);
            pullHookUnlocked = false;
            pushHookUnlocked = false;
        }

        if (collision.CompareTag("PushHook")) {
            pushHookUnlocked = true;
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Coin")) {
            collision.gameObject.SetActive(false);
            addCoinNextFixed = true;
        }
    }

    private void FixedUpdate() {
        if (addCoinNextFixed)
        {
            SoundManager.PlaySound(SoundManager.Sound.Coin);
            numCoins++;
            addCoinNextFixed = false;
        }
    }

    public void SpendCoins(int amount) {
        numCoins -= amount;
    }
}
