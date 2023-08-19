using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class shopItem : MonoBehaviour
{
    public TMP_Text buyButtonText;
    public TMP_Text priceText;
    public Button buyButton;

    public bool isBasic;
    public bool isOwned;
    public bool isPro;
    public bool isSelectedCharachter;
    public string selectedCharachter;
    public int price;
    public string textInsteadOfPrice;
    public int index;

    public GameObject managerScriptObject;
    private manager managerScript;

    public GameObject playerScriptObject;
    private playerMovement playerScript;

    //public Button.ButtonClickedEvent buttonFunction;

    // Start is called before the first frame update
    void Start()
    {

        managerScriptObject = GameObject.Find("Manager");

        managerScript = managerScriptObject.GetComponent<manager>();

        playerScriptObject = GameObject.Find("Player2");

        playerScript = playerScriptObject.GetComponent<playerMovement>();

        //buyButtonText = GameObject.Find("BuyButtonLabel").GetComponentInChildren<TMP_Text>();
        buyButton = GameObject.Find("Button").GetComponent<Button>();
        if(isBasic)
        {
            isOwned = true;
        } else
        {
            isOwned = (PlayerPrefs.GetInt(gameObject.name) != 0);
        }


        selectedCharachter = PlayerPrefs.GetString("SelectedCharacter", "Charachter01");

        if(textInsteadOfPrice.Length != 0)
        {
            priceText.text = ("");
        } else
        {
            priceText.text = price.ToString();
        }

        if (selectedCharachter == gameObject.name)
        {
            isSelectedCharachter = true;
            playerScript.changeCharacter(index);
        }

        if (isOwned)
        {
            priceText.gameObject.SetActive(false);

            if (isSelectedCharachter)
            {
                buyButtonText.text = ("SELECTED");
            } else
            {
                buyButtonText.text = ("SELECT");
            }
        } else
        {
            if (textInsteadOfPrice.Length != 0)
            {
                buyButtonText.text = ("Follow");
            }
            else
            {
                buyButtonText.text = ("BUY");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyTapped()
    {
        if (!isOwned)
        {
            int coin = managerScript.getCoin();
            if (coin >= price)
            {
                if(textInsteadOfPrice.Length != 0)
                {
                    Application.OpenURL("https://www.instagram.com/weare1tapgames/");
                }
                UnselectAllItem();
                isOwned = true;
                PlayerPrefs.SetInt(gameObject.name, isOwned ? 1 : 0);
                PlayerPrefs.SetString("SelectedCharacter", gameObject.name);
                buyButtonText.text = ("SELECTED");
                managerScript.minusCoin(price);
                priceText.gameObject.SetActive(false);
                playerScript.changeCharacter(index);
            }

        } else {
            UnselectAllItem();
            PlayerPrefs.SetString("SelectedCharacter", gameObject.name);
            buyButtonText.text = ("SELECTED");
            playerScript.changeCharacter(index);
        }
    }

    public void UnselectAllItem()
    {
         shopItem[] allShopItem = GameObject.FindObjectsOfType<shopItem>();

        foreach (shopItem currentItem in allShopItem)
        {
            if(currentItem.isOwned)
            {
                currentItem.buyButtonText.text = ("SELECT");
            }
        }
    }
}
