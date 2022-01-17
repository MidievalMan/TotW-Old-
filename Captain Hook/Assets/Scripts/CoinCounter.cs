using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{

    private TMP_Text coinCount;

    private void Start()
    {
        coinCount = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        coinCount.SetText(PlayerStats.numCoins.ToString());
    }

}
