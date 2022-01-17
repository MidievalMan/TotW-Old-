using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    private bool playerHas = false; // save this data at a later time

    private GameObject hat;
    private GameObject player;
    private PlayerStats playerStats;

    private Image image;

    public string name;
    public int cost;


    //private TMP_Text nameText;
    private TMP_Text costText;
    private GameObject coinIcon;

    public void Start()
    {
        hat = GameObject.FindWithTag("Hat");
        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        image = this.GetComponent<Image>();
        //nameText = this.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        costText = this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        coinIcon = this.transform.GetChild(2).gameObject;
    }


    public void OnClick()
    {

        if(!playerHas && PlayerStats.numCoins - cost >= 0)
        {
            Debug.Log("Total coins before purchase = " + PlayerStats.numCoins);
            Debug.Log("Player does not have this hat!\nBuying now...");

            playerStats.SpendCoins(cost);
            playerHas = true;
            SoundManager.PlaySound(SoundManager.Sound.BuyItem);
            Debug.Log("Total coins after purchase = " + PlayerStats.numCoins);
        }

        if(playerHas)
        {
            coinIcon.SetActive(false);
            costText.SetText("Owned");
            hat.GetComponent<SpriteRenderer>().sprite = gameObject.transform.GetChild(3).gameObject.GetComponent<Image>().sprite;
        }

    }

}
