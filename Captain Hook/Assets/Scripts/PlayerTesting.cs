using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTesting : MonoBehaviour {
    public bool testing;
    public bool ManipulateTimeScale;
    public float timeScale;
    public GameObject mainCam;
    public Transform startPos;
    public GrapplingHook hookScript;


    void Start() {
        TestingFeatures(testing);
        if(ManipulateTimeScale)
        {
            Time.timeScale = timeScale;
        }
    }

    public void Teleport(Vector3 pos) {
        transform.position += pos + new Vector3(0, 0, -1);
    }

    public void TestingFeatures(bool inAffect) {
        if (inAffect) {
            gameObject.transform.position = startPos.transform.position;
            //mainCam.transform.position = transform.position;
            //PlayerStats.respawnPoint = start.transform.position;
            PlayerStats.pullHookUnlocked = true;
            PlayerStats.pushHookUnlocked = true;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            transform.position = hookScript.lookDirection - new Vector3(0, 0, hookScript.lookDirection.z);
        }
    }
}
