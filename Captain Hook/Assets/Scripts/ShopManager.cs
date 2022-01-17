using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private const int ITEMS_PER_ROW = 6;
    private int ITEM_WIDTH = Screen.width / 10;

    public string[] names;
    public Sprite[] sprites;
    public int[] costs;

    public Canvas itemCanvas;

    //public GameObject hat;
    public GameObject itemTemplate;
    private GameObject[] items;
    public PlayerMovement playerMovement;

    private bool firstTimePressingBuy = true;

    public void Start()
    {
        items = new GameObject[sprites.Length];
    }
    public void OnBuyClick()
    {
        if(firstTimePressingBuy)
        {
            firstTimePressingBuy = !firstTimePressingBuy;

            for (int i = 0; i < sprites.Length; i++) // create and layout all items
            {
                int numRows = (i + ITEMS_PER_ROW) / ITEMS_PER_ROW;

                items[i] = Instantiate(itemTemplate, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
                items[i].transform.position = new Vector3(ITEM_WIDTH + ((i * ITEM_WIDTH) % (ITEM_WIDTH * ITEMS_PER_ROW)), ((Screen.height / 2) - (numRows * ITEM_WIDTH)) - 20, 0);
                //items[i].GetComponent<Image>().sprite = sprites[i];
                Item itemScript = items[i].GetComponent<Item>();

                itemScript.cost = costs[i];
                itemScript.name = names[i];

                TMP_Text name = items[i].transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
                name.SetText(names[i]);

                TMP_Text cost = items[i].transform.GetChild(1).GetComponent<TMP_Text>();
                cost.SetText(costs[i].ToString());

                items[i].transform.GetChild(3).GetComponent<Image>().sprite = sprites[i];

                items[i].transform.SetParent(itemCanvas.transform);
            }

            //StartCoroutine("a"); trying to get buttons to work first time around



        }
        else
        {
            foreach(GameObject g in items)
            {
                g.SetActive(true);
            }
        }
    }
    /*
    private IEnumerator a()
    {
        foreach (GameObject g in items)
        {
            g.SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject g in items)
        {
            g.SetActive(true);
        }
    }
    */

    public void CloseShop()
    {
        StartCoroutine("Close");
    }

    private IEnumerator Close()
    {
        yield return new WaitForSeconds(2f);

        FindObjectOfType<DialogueManager>().EndDialogue();

        gameObject.SetActive(false);
        SoundManager.PlaySound(SoundManager.Sound.EnterDoor);
        playerMovement.SetMasterControl(true);
    }

    public GameObject[] GetItems()
    {
        return items;
    }

}
