using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeUntilDestroy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroyAfterSeconds");
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        Destroy(gameObject);
    }
}
