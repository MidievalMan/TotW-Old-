using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{

    public static SceneManagerScript Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("Current scene index: " + SceneManager.GetActiveScene().buildIndex + "\nnext scene index: " + nextSceneIndex);
        Debug.Log(SceneManager.sceneCount);
        if(nextSceneIndex <= SceneManager.sceneCount)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

}
