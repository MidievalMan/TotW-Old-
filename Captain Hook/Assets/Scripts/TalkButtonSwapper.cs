using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TalkButtonSwapper : MonoBehaviour
{
    // Buttons to swap
    public Button buy;
    public Button talk;
    public Button leave;

    public Button talk1;
    public Button talk2;
    public Button talk3;
    public Button talk4;

    
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public TMP_Text text4;
    

    public Button backBuy;
    public Button backTalk;

    public GameObject shopText;

    // Displays to Swap
    public ShopManager shopManager;

    void Update()
    {
        shopText.gameObject.SetActive(true);
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);
        text3.gameObject.SetActive(true);
        text4.gameObject.SetActive(true);
    }

    public void Buy()
    {
        buy.gameObject.SetActive(false);
        talk.gameObject.SetActive(false);
        leave.gameObject.SetActive(false);

        backBuy.gameObject.SetActive(true);

        shopManager.gameObject.SetActive(true);
    }

    public void Talk()
    {
        buy.gameObject.SetActive(false);
        talk.gameObject.SetActive(false);
        leave.gameObject.SetActive(false);
        
        talk1.gameObject.SetActive(true);
        talk2.gameObject.SetActive(true);
        talk3.gameObject.SetActive(true);
        talk4.gameObject.SetActive(true);

        backTalk.gameObject.SetActive(true);
    }



    public void Back()
    {
        buy.gameObject.SetActive(true);
        talk.gameObject.SetActive(true);
        leave.gameObject.SetActive(true);

        talk1.gameObject.SetActive(false);
        talk2.gameObject.SetActive(false);
        talk3.gameObject.SetActive(false);
        talk4.gameObject.SetActive(false);
        backBuy.gameObject.SetActive(false);
        backTalk.gameObject.SetActive(false);

        GameObject[] items = shopManager.GetItems();

        foreach(GameObject g in items)
        {
            if(g != null)
            {
                g.SetActive(false);
            }
        }

        //FindObjectOfType<DialogueManager>().EndDialogue();

    }


}
