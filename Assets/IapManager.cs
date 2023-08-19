using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System;

//using HmsPlugin;
//using HuaweiMobileServices.IAP;
//using HuaweiMobileServices.Utils;
using TMPro;

public class IapManager : MonoBehaviour
{
    private string godMode = "com.groovypixelsprod.towerjump.godmode2";
    private string coin100 = "com.groovypixelsprod.towerjump.100coin";
    public GameObject restorButton;

    public GameObject managerScriptObject;
    private manager managerScript;


    // private List<InAppPurchaseData> productPurchasedList;
    //public Text hmsPriceText;
    //public Text hms100PriceText;

    //public static Action<string> IAPLog;

    //private ProductInfo hueweiInfo;
    //private ProductInfo huewei100Info;
    //private string hueweiprice;
    //private string huewei100price;


    // Start is called before the first frame update
    void Start()
    {
        managerScriptObject = GameObject.Find("Manager");

        managerScript = managerScriptObject.GetComponent<manager>();
        restorButton.SetActive(true);

        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            restorButton.SetActive(false);
        }

        // Debug.Log("[HMS]: IapDemoManager Started");
        //HMSIAPManager.Instance.OnBuyProductSuccess += OnBuyProductSuccess;

        //HMSIAPManager.Instance.OnCheckIapAvailabilitySuccess += OnCheckIapAvailabilitySuccess;
        //HMSIAPManager.Instance.OnCheckIapAvailabilityFailure += OnCheckIapAvailabilityFailure;

        //HMSIAPManager.Instance.OnBuyProductFailure = OnBuyProductFailure;

        //hueweiInfo = HMSIAPManager.Instance.GetProductInfo("JumpTowerGodMode");
        //hueweiprice = hueweiInfo.Price;
        //hmsPriceText.text = hueweiprice;

        //huewei100Info = HMSIAPManager.Instance.GetProductInfo("JumpTower100");
        //huewei100price = hueweiInfo.Price;
        //hms100PriceText.text = hueweiprice;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == godMode)
        {
            managerScript.continueTapped();
            managerScript.buyGodMod();
        }
        if (product.definition.id == coin100)
        {
            managerScript.buy100();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        if (product.definition.id == coin100)
        {

        }
    }

//       private void DeliveryControl()
//   {
//       // https://developer.huawei.com/consumer/en/doc/development/HMSCore-Guides/redelivering-consumables-0000001051356573
//       // Check whether the purchase token is in the purchase token list of the delivered products.
//   }

//   // For sandbox testing
//   private void OnBuyProductFailure(int code)
//   {
//       //https://developer.huawei.com/consumer/en/doc/development/HMSCore-References/client-error-code-0000001050746111
//       // this command is the solution of the error of product has already been purchased.(ORDER_PRODUCT_OWNED)

//       if (code == OrderStatusCode.ORDER_PRODUCT_OWNED)
//       {
//           HMSIAPManager.Instance.OnObtainOwnedPurchasesSuccess = OnObtainOwnedPurchasesSuccess;
//           HMSIAPManager.Instance.ObtainOwnedPurchases(PriceType.IN_APP_NONCONSUMABLE);
//       }
//   }

//   private void OnObtainOwnedPurchasesSuccess(OwnedPurchasesResult result)
//   {
//       if (result != null)
//       {
//           foreach (var obj in result.InAppPurchaseDataList)
//           {
//               HMSIAPManager.Instance.ConsumePurchaseWithToken(obj.PurchaseToken);
//               print(obj.ProductName);
//           }
//       }
//   }



//   private void OnCheckIapAvailabilityFailure(HMSException obj)
//   {
//       IAPLog?.Invoke("IAP is not ready.");
//   }

//   private void OnCheckIapAvailabilitySuccess()
//   {
//       IAPLog?.Invoke("IAP is ready.");

//   }

//   public void SignIn()
//   {
//       Debug.Log("SignIn");

//       HMSIAPManager.Instance.CheckIapAvailability();
//   }

//   private void RestorePurchases()
//   {
//       HMSIAPManager.Instance.RestorePurchases((restoredProducts) =>
//       {
//           productPurchasedList = new List<InAppPurchaseData>(restoredProducts.InAppPurchaseDataList);
//       });
//   }

//   public void HueweiBuyTapped()
//   {
//       BuyProduct("TowerJumpGodMode");
//   }

//    public void HueweiCoinBuyTapped()
//   {
//       BuyProduct("TowerJump100");
//   }

//   public void BuyProduct(string productID)
//   {
//       Debug.Log("BuyProduct");

//       HMSIAPManager.Instance.BuyProduct(productID);
//   }

//   private void OnBuyProductSuccess(PurchaseResultInfo obj)
//   {
//       if (obj.InAppPurchaseData.ProductId == "TowerJumpGodMode")
//       {
//           managerScript.continueTapped();
//           managerScript.buyGodMod();
//       }
//       else if (obj.InAppPurchaseData.ProductId == "TowerJump100")
//       {
//           managerScript.buy100();
//       }
//       else if (obj.InAppPurchaseData.ProductId == "premium")
//       {
//           // Grant your player premium feature.
//       }
//   }
}

