using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using Firebase;
//using Firebase.Analytics;
using GoogleMobileAds.Api;
//using UnityEngine.iOS;
using UnityEngine.Advertisements;
//using HmsPlugin;
//using HuaweiMobileServices.Ads;
using System.Text.RegularExpressions;

public class manager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private BannerView bannerAd;
    private InterstitialAd interstitial;

    private AdRequest request;

    static int coin = 0;

    public static int tap = 0;

    public int randomBannerNumber = 0;
    public int godmodeNumber = 0;
    public int rewindCount = 1;

    public bool isPro;
    public bool isHardcore;
    public bool isRewindPowerup;
    public bool isFirst;
    public bool isFirstAgain;
    public bool wasRate;
    public bool tutorialTimeDone;
    public bool musicMuted;
    public bool songMuted;
    public bool vibrationMuted;

    public TMP_Text coinLabel;
    public TMP_Text menuCoinLabel;
    public TMP_Text scoreLabel;
    public TMP_Text preLevelText;
    public TMP_Text nextLevelText;
    public TMP_Text menuLevelText;
    public TMP_Text highScoreText;
    public TMP_Text lifeText;
    public TMP_Text subTitleText;
    public TMP_Text endTapText;
    public GameObject noInternetTextGameObject;
    public GameObject rewindNoInternet;
    public InputField nameInput;
    public GameObject endPanel;
    public GameObject tutorialPanel;
    public GameObject tutorialAgainPanel;
    public GameObject pausePanel;
    public GameObject rewindPanel;
    public GameObject ratePanel;
    public GameObject menuPanel;
    public GameObject shopPanel;
    public GameObject godPanel;
    public GameObject hardcorePanel;
    public GameObject leaderPanel;
    public GameObject optionsPanel;
    public GameObject againButton;
    public GameObject soundOnButton;
    public GameObject soundOffButton;
    public GameObject vibrationOnButton;
    public GameObject vibrationOffButton;
    public GameObject songOnButton;
    public GameObject songOffButton;
    public GameObject torchLitGameobject;
    public GameObject restoreGameobject;
    public GameObject charactersGameObject;
    public GameObject charactersButtonOn;
    public GameObject charactersButtonOff;
    public GameObject powerUpsGameObject;
    public GameObject powerUpsButtonOn;
    public GameObject powerUpsButtonOff;
    public GameObject coinsGameObject;
    public GameObject coinsButtonOn;
    public GameObject coinsButtonOff;
    public GameObject bannerView;
    public GameObject crownView;

    public GameObject playerScriptObject;
    private playerMovement playerScript;

    public Button rewindForCoinButton;
    public Button rewindForAdButton;

    public TMP_Text rewindForCoinButtonLabel;
    public TMP_Text rewindForAdButtonLabel;

    public TMP_Text rewindPowerUpButtonText;

    // Start is called before the first frame update
    void Start()
    {

        nameInput.onValueChanged.AddListener(inputValueChanged);

        playerScript = playerScriptObject.GetComponent<playerMovement>();

        randomBannerNumber = Random.Range(0, 3);
        godmodeNumber = Random.Range(0, 3);

        //PlayerPrefs.DeleteAll();
        Application.targetFrameRate = 60;
        coin = (PlayerPrefs.GetInt("Coin", 0));

        tap = (PlayerPrefs.GetInt("Tap", 0));

        AudioListener.volume = 1;
        coinLabel = GameObject.Find("coinLabel").GetComponent<TMP_Text>();
        menuCoinLabel = GameObject.Find("menuCoinLabel").GetComponent<TMP_Text>();
        scoreLabel = GameObject.Find("scoreLabel").GetComponent<TMP_Text>();
        preLevelText = GameObject.Find("preLevelText").GetComponent<TMP_Text>();
        nextLevelText = GameObject.Find("nextLevelText").GetComponent<TMP_Text>();
        menuLevelText = GameObject.Find("MenuLevelText").GetComponent<TMP_Text>();
        highScoreText = GameObject.Find("HighScoreText").GetComponent<TMP_Text>();
        lifeText = GameObject.Find("lifeText").GetComponent<TMP_Text>();
        subTitleText = GameObject.Find("Subtitle").GetComponent<TMP_Text>();
        againButton = GameObject.FindGameObjectWithTag("Again Button");
        torchLitGameobject = GameObject.FindGameObjectWithTag("Torch Lit");
        restoreGameobject = GameObject.FindGameObjectWithTag("Restore");
        //endPanel = GameObject.Find("endPanel");
        coinLabel.text = coin.ToString();
        menuCoinLabel.text = coin.ToString();

        //endPanel.gameObject.SetActive(false);

        torchLitGameobject.SetActive(false);
        restoreGameobject.SetActive(false);
        againButton.gameObject.SetActive(false);

        //isPro = (PlayerPrefs.GetInt("isGod") != 0);
        isRewindPowerup = (PlayerPrefs.GetInt("isRewindPowerUp") != 0);
        musicMuted = (PlayerPrefs.GetInt("musicMuted") != 0);
        songMuted = (PlayerPrefs.GetInt("songMuted") != 0);
        vibrationMuted = (PlayerPrefs.GetInt("vibrationMuted") != 0);
        isFirst = (PlayerPrefs.GetInt("isFirst") != 1);
        wasRate = (PlayerPrefs.GetInt("wasRate") != 0);
        isFirstAgain = PlayerPrefs.GetInt("againTutorial") != 1;

        if (musicMuted)
        {
            soundOnButton.SetActive(false);
            soundOffButton.SetActive(true);
            soundManager.isMuted = true;
        }

        if (vibrationMuted)
        {
            vibrationOnButton.SetActive(false);
            vibrationOffButton.SetActive(true);
        }

        if (songMuted)
        {
            songOnButton.SetActive(false);
            songOffButton.SetActive(true);
            musicBG.StopMusic();
            musicBG.isMuted = true;
        }

        if (isFirst || godmodeNumber == 1)
        {
            //showGodPanel(true);
        }

        if(isPro)
        {
            setRewindCount();
            rewindForAdButton.gameObject.SetActive(false);
            crownView.SetActive(false);

            PlayerPrefs.SetInt("Charachter05", 1);

            shopItem[] allShopItem = GameObject.FindObjectsOfType<shopItem>();

            foreach (shopItem currentItem in allShopItem)
            {
                if (currentItem.isPro)
                {
                    currentItem.isOwned = true;
                    currentItem.buyButtonText.text = ("SELECT");
                    currentItem.priceText.gameObject.SetActive(false);
                }
            }
        } else
        {
            setRewindCount();
            crownView.SetActive(true);
        }

        if(isRewindPowerup)
        {
            rewindPowerUpButtonText.text = "owned";
        }

        StartCoroutine(refreshMenuLevelLabel());

        showRewindAdButton(true);

        MobileAds.Initialize(initStatus => { });

        string rewardedAdUnitId;
        string bannerAdUnitId;
        string interAdUnitId;

#if UNITY_ANDROID
        //app ID android
        //ca-app-pub-6434100498411494~2568703845
        rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
        bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        interAdUnitId = "ca-app-pub-3940256099942544/1033173712";

#elif UNITY_IPHONE
         //app ID ios
         //ca-app-pub-6434100498411494~6316522951
         rewardedAdUnitId = "ca-app-pub-6434100498411494/2226014959";
         //bannerAdUnitId = "ca-app-pub-6434100498411494/9352633843";
         interAdUnitId = "ca-app-pub-6434100498411494/3056449111";


#else
             rewardedAdUnitId = "unexpected_platform";
         bannerAdUnitId  = "unexpected_platform";
         interAdUnitId = "unexpected_platform";
#endif

        if (Application.isPlaying)
        {
            //  rewardedAdUnitId = "ca-app-pub-3940256099942544/5135589807";
            //  bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
            //     interAdUnitId = "ca-app-pub-3940256099942544/5135589807";
        }


        //Advertisement.Initialize(unityAdID);

        this.rewardedAd = new RewardedAd(rewardedAdUnitId);

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        this.interstitial = new InterstitialAd(interAdUnitId);

        //this.bannerAd = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        if (randomBannerNumber == 1 && !isPro)
        {
            //this.bannerAd.LoadAd(request);
            //this.bannerAd.SetPosition(AdPosition.Top);
        }
        this.interstitial.LoadAd(request);

        //if (randomBannerNumber == 1 && !isPro)
        //{
        //    HMSAdsKitManager.Instance.ShowBannerAd();
        //}

        scoreLabel.text = tap.ToString();

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


    // Update is called once per frame
    void Update()
    {
        refreshScore();
    }

    public void addCoin(int amount)
    {
        coin += amount;
        //scoreText.text = score.ToString();
        PlayerPrefs.SetInt("Coin", coin);
        coinLabel.text = coin.ToString();
        menuCoinLabel.text = coin.ToString();
    }

    public void addTap()
    {
        tap ++;
        scoreLabel.text = tap.ToString();
        if(isHardcore)
        {
            PlayerPrefs.SetInt("TapHardcore", tap);
        } else
        {
            PlayerPrefs.SetInt("Tap", tap);
        }

    }

    public void minusCoin(int number)
    {
        coin -= number;
        //scoreText.text = score.ToString();
        PlayerPrefs.SetInt("Coin", coin);
        coinLabel.text = coin.ToString();
        menuCoinLabel.text = coin.ToString();
    }


    public void win()
    {
        //FirebaseAnalytics.LogEvent("win");
        endPanel.gameObject.SetActive(true);
        endTapText.text = tap + "  Taps";
    }

    public void nextMap()
    {
        SceneManager.LoadScene(1);
    }

    public void refreshLabel()
    {   
        int levelna;
        if (playerMovement.level == 0)
        {
            levelna = 1;
        } else
        {
            levelna = (int)playerMovement.level;
        }
        preLevelText.text = playerMovement.preLevelNumber.ToString();
        nextLevelText.text = playerMovement.nextLevelNumber.ToString();
        highScoreText.text = playerMovement.highScore.ToString();
    }

    IEnumerator refreshMenuLevelLabel()
    {
        yield return new WaitForSeconds(0.5f);
        int i = playerScript.checkTorchesToMenu();
        print("teszt : " + i);
        menuLevelText.text = "Level " + i;
    }


    
    public void hideAgainButton(bool hide)
    {
        if (isHardcore) return;

        tutorialAgainPanel.SetActive(false);
        if (!hide)
        {
            if(isFirstAgain)
            {
                tutorialAgainPanel.SetActive(true);
                bool istut = true;
                PlayerPrefs.SetInt("againTutorial", istut ? 1 : 0);
                isFirstAgain = false;
            }
        }

        againButton.SetActive(!hide);
        //if (playerMovement.life <= 0)
       // {
          //againButton.SetActive(false);
       // }
    }

    static string CleanInput(string strIn)
    {
        // Replace invalid characters with empty strings.
        return Regex.Replace(strIn,
              @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./]", "");
    }

    //Called when Input changes
    void inputValueChanged(string attemptedVal)
    {
        nameInput.text = CleanInput(attemptedVal);
    }

    public void refreshScore()
    {
        int score2;
        if (playerMovement.score < 0)
        {
            score2 = 0;
        }else
        {
            score2 = (int)playerMovement.score;
        }
        //scoreLabel.text = score2.ToString();
    }

    public void showTorchLot()
    {
        soundManager.PlaySound("lit");
        torchLitGameobject.SetActive(true);
        StartCoroutine(HideTorchAfterTime(2));
    }

    public void showRestorePopup()
    {
        if (playerMovement.life < rewindCount)

        {
            soundManager.PlaySound("lit");
            restoreGameobject.SetActive(true);
            StartCoroutine(HideRestoreAfterTime(2));
        }
    }

    public void playTapped()
    {
        if (randomBannerNumber == 2 && !isPro)
        {
                 this.interstitial.Show();

                //HMSAdsKitManager.Instance.ShowInterstitialAd();
        }
        playerMovement.isMenu = false;
        menuPanel.GetComponent<Animator>().SetTrigger("isHide");
        StartCoroutine(HideMenuAfterTime(1));
        playerScript.checkTorches();
        crownView.SetActive(false);
        if (isFirst)
        {
            StartCoroutine(showTutorialAfterTime(1));
            bool istut = true;
            PlayerPrefs.SetInt("isFirst", istut ? 1 : 0);
        }
    }

    public void hardcorePlayTapped()
    {
        if (randomBannerNumber == 2 && !isPro)
        {
            this.interstitial.Show();
            //HMSAdsKitManager.Instance.ShowInterstitialAd();
        }
        isHardcore = true;
        playerMovement.isMenu = false;
        menuPanel.GetComponent<Animator>().SetTrigger("isHide");
        StartCoroutine(HideMenuAfterTime(1));
        playerScript.checkTorches();
        crownView.SetActive(false);
        tap = (PlayerPrefs.GetInt("TapHardcore", 0));
        scoreLabel.text = tap.ToString();
        hardcorePanel.SetActive(false);
    }

    public void hardcoreTapped()
    {
        showHardcorePanel(true);
    }

    public void ContinueFromLastTapped()
    {
        if (isPro)
        {
            playerMovement.isMenu = false;
            menuPanel.GetComponent<Animator>().SetTrigger("isHide");
            StartCoroutine(HideMenuAfterTime(1));
            playerScript.checkTorchesContinue();
            if (isFirst)
            {
                StartCoroutine(showTutorialAfterTime(1));
                bool istut = true;
                PlayerPrefs.SetInt("isFirst", istut ? 1 : 0);
            }
        }
        else
        {
            interstitial.Show();
            playerMovement.isMenu = false;
            menuPanel.GetComponent<Animator>().SetTrigger("isHide");
            StartCoroutine(HideMenuAfterTime(1));
            playerScript.checkTorchesContinue();
            if (isFirst)
            {
                StartCoroutine(showTutorialAfterTime(1));
                bool istut = true;
                PlayerPrefs.SetInt("isFirst", istut ? 1 : 0);
            }
        }
    }

    public void hideAllShopButtons()
    {
        charactersButtonOn.SetActive(false);
        charactersButtonOff.SetActive(false);
        powerUpsButtonOn.SetActive(false);
        powerUpsButtonOff.SetActive(false);
        coinsButtonOn.SetActive(false);
        coinsButtonOff.SetActive(false);
        charactersGameObject.SetActive(false);
        powerUpsGameObject.SetActive(false);
        coinsGameObject.SetActive(false);
    }

    public void charachtersTapped()
    {
        hideAllShopButtons();
        charactersButtonOn.SetActive(true);
        powerUpsButtonOff.SetActive(true);
        coinsButtonOff.SetActive(true);
        charactersGameObject.SetActive(true);
        subTitleText.text = ("CHARACTERS");
    }
    public void powerUpsTapped()
    {
        hideAllShopButtons();
        charactersButtonOff.SetActive(true);
        powerUpsButtonOn.SetActive(true);
        coinsButtonOff.SetActive(true);
        powerUpsGameObject.SetActive(true);
        subTitleText.text = ("POWER UPS");
    }

    public void coinsTapped()
    {
        hideAllShopButtons();
        charactersButtonOff.SetActive(true);
        powerUpsButtonOff.SetActive(true);
        coinsButtonOn.SetActive(true);
        coinsGameObject.SetActive(true);
        subTitleText.text = ("COINS");
    }

    IEnumerator HideTorchAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        torchLitGameobject.GetComponent<Animator>().SetTrigger("isHide");
        yield return new WaitForSeconds(time);
        torchLitGameobject.SetActive(false);
    }

    IEnumerator HideRestoreAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        restoreGameobject.GetComponent<Animator>().SetTrigger("isHide");
        yield return new WaitForSeconds(time);
        restoreGameobject.SetActive(false);
    }

    IEnumerator HideMenuAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        menuPanel.SetActive(false);
        playerMovement.isFreezed = false;
    }

    public void restartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void refreshLife()
    {
        lifeText.text = (playerMovement.life.ToString());
    }

    public void refreshRewindCoinText()
    {
        rewindForCoinButtonLabel.text = (10 * playerMovement.rewindUsedCount).ToString() + "  Coin";
    }

    public void rewindForCoinTapped()
    {
        if (coin >= 10 * playerMovement.rewindUsedCount)
        {
            minusCoin(10 * playerMovement.rewindUsedCount);
            playerMovement.life += 1;
            refreshLife();
            showRewindPanel(false);
            hideAgainButton(false);
            playerMovement.rewindUsedCount ++;
            refreshRewindCoinText();
        } else
        {
            showGodPanel(true);
            showRewindPanel(false);
        }

          
    }

   public void rewindForAdTapped()
    {

        //HMSAdsKitManager.Instance.OnRewarded = OnRewarded;
        //HMSAdsKitManager.Instance.ShowRewardedAd();

        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        } else
        {
            interstitial.Show();
            playerMovement.life += 1;
            refreshLife();
            showRewindPanel(false);
            hideAgainButton(false);

            AdRequest request = new AdRequest.Builder().Build();
            interstitial.LoadAd(request);
            rewardedAd.LoadAd(request);
        }
    }

    public void pauseTapped()
    {
        hideAgainButton(true);
        pausePanel.SetActive(true);
        playerMovement.isFreezed = true;
    }

    public void showRewindPanel(bool show)
    {
        rewindForCoinButton.interactable = true;
        rewindPanel.SetActive(show);
        playerMovement.isFreezed = show;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            rewindNoInternet.SetActive(true);
        } else
        {
            rewindNoInternet.SetActive(false);
        }
    }

    public void showGodPanel(bool show)
    {
        if(isPro) return;

        godPanel.SetActive(show);
        playerMovement.isFreezed = show;
    }

    public void showHardcorePanel(bool show)
    {
        hardcorePanel.SetActive(show);
    }

    public void rateUsShow()
    {
           //return;
        if (!wasRate)
        {
            ratePanel.SetActive(true);
            playerMovement.isFreezed = true;
            wasRate = true;
            PlayerPrefs.SetInt("wasRate", wasRate ? 1 : 0);
        }
    }

    public void rateTapped()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.PalGabor.TowerJump");
#elif UNITY_IPHONE
        Device.RequestStoreReview();
#else

#endif
        ratePanel.SetActive(false);
        playerMovement.isFreezed = false;
    }

    public void continueTapped()
    {
        pausePanel.SetActive(false);
        hardcorePanel.SetActive(false);
        rewindPanel.SetActive(false);
        ratePanel.SetActive(false);
        godPanel.SetActive(false);
        leaderPanel.SetActive(false);
        optionsPanel.SetActive(false);
        playerMovement.isFreezed = false;
    }

    public void showShop()
    {
        menuPanel.GetComponent<Animator>().SetTrigger("showShop");
    }

    public void leaderTapped()
    {
         leaderPanel.SetActive(true);
    }

    public void hideShop()
    {
        menuPanel.GetComponent<Animator>().SetTrigger("hideShop");
    }

    public void homeTapped()
    {
        hideAgainButton(true);
        SceneManager.LoadScene(0);
    }

    public void optionsTapped(bool show)
    {
        optionsPanel.SetActive(show);
    }

    public void songOnTapped()
    {
        StartCoroutine(songOffAfterTime(0.25f));
        songMuted = true;
        PlayerPrefs.SetInt("songMuted", songMuted ? 1 : 0);
        musicBG.StopMusic();
        musicBG.isMuted = true;
    }

    public void songOffTapped()
    {
        StartCoroutine(songOnAfterTime(0.25f));
        songMuted = false;
        PlayerPrefs.SetInt("songMuted", songMuted ? 1 : 0);
        musicBG.PlayMusic();
        musicBG.isMuted = false;
    }

    IEnumerator songOnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        songOnButton.SetActive(true);
        songOffButton.SetActive(false);
    }

    IEnumerator songOffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        songOnButton.SetActive(false);
        songOffButton.SetActive(true);
    }

    public void vibrationOnTapped()
    {
        StartCoroutine(vibrationOffAfterTime(0.25f));
        vibrationMuted = true;
        PlayerPrefs.SetInt("vibrationMuted", vibrationMuted ? 1 : 0);
    }

    public void vibrationOffTapped()
    {
        StartCoroutine(vibrationOnAfterTime(0.25f));
        vibrationMuted = false;
        PlayerPrefs.SetInt("vibrationMuted", vibrationMuted ? 1 : 0);
    }

    IEnumerator vibrationOnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        vibrationOnButton.SetActive(true);
        vibrationOffButton.SetActive(false);
    }

    IEnumerator vibrationOffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        vibrationOnButton.SetActive(false);
        vibrationOffButton.SetActive(true);
    }

    public void soundOnTapped()
    {
        StartCoroutine(soundOffAfterTime(0.25f));
        bool musicMuted = true;
        PlayerPrefs.SetInt("musicMuted", musicMuted ? 1 : 0);
        soundManager.isMuted = true;
    }

    public void soundOffTapped()
    {
        StartCoroutine(soundOnAfterTime(0.25f));
        bool musicMuted = false;
        PlayerPrefs.SetInt("musicMuted", musicMuted ? 1 : 0);
        soundManager.isMuted = false;
    }

    IEnumerator soundOnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        soundOnButton.SetActive(true);
        soundOffButton.SetActive(false);
    }

    IEnumerator soundOffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        soundOnButton.SetActive(false);
        soundOffButton.SetActive(true);
    }

    IEnumerator showTutorialAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        tutorialPanel.SetActive(true);
        StartCoroutine(hideTutorialAfterTime(3));
    }

    IEnumerator hideTutorialAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        tutorialTimeDone = true;
    }

    public void hideTutorial()
    {
        if(tutorialTimeDone)
        {
            tutorialPanel.SetActive(false);
        }

    }

    public void againTappad()
    {
        //if(randomNumber == 4)
        //{
            //randomNumber = Random.Range(0, 5);
            //ShowInterstitial();
       // } else
       // {
            //randomNumber = Random.Range(0, 5);
       // }
    }

    public void showRewindAdButton(bool show)
    {
        if(show)
        {
            Color whiteColor = new Color(1, 1, 1, 1);
            Color blackColor = new Color(0, 0, 0, 1);
            rewindForAdButton.interactable = true;
            rewindForAdButton.GetComponent<Image>().color = whiteColor;
            rewindForAdButtonLabel.color = new Color(0, 0, 0, 1);
        } else
        {
            Color whiteColor = new Color(1, 1, 1, 0.2f);
            Color blackColor = new Color(0, 0, 0, 0.2f);
            rewindForAdButton.interactable = false;
            rewindForAdButton.GetComponent<Image>().color = whiteColor;
            rewindForAdButtonLabel.color = new Color(0, 0, 0, 0.2f);
        }
    }


    public int getCoin()
    {
        return coin;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        playerMovement.life += 1;
        refreshLife();
        showRewindPanel(false);
        hideAgainButton(false);

        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    //public void OnRewarded(HuaweiMobileServices.Ads.Reward reward)
    //{
    //     playerMovement.life += 1;
    //   refreshLife();
    //   showRewindPanel(false);
    //   hideAgainButton(false);

    //}

    public void buyGodMod()
    {
        //FirebaseAnalytics.LogEvent("buyPro");
        bool istut = true;
        PlayerPrefs.SetInt("isGod", istut ? 1 : 0);
        setRewindCount();
        playerMovement.life = 5;
        refreshLife();
        isPro = true;
        addCoin(200);
        crownView.SetActive(false);
        showGodPanel(false);
        showRewindPanel(false);
        PlayerPrefs.SetInt("Charachter05", 1);

        shopItem[] allShopItem = GameObject.FindObjectsOfType<shopItem>();

        foreach (shopItem currentItem in allShopItem)
        {
            if (currentItem.isPro)
            {
                currentItem.isOwned = true;
                currentItem.buyButtonText.text = ("SELECT");
                currentItem.priceText.gameObject.SetActive(false);
            }
        }
    }

    public void buy100()
    {
        addCoin(100);
        //FirebaseAnalytics.LogEvent("buy100");
    }

    public void buy3RewindPowerup()
    {
        if(coin >= 1000 && !isRewindPowerup)
        {
            bool istut = true;
            PlayerPrefs.SetInt("isRewindPowerUp", istut ? 1 : 0);
            isRewindPowerup = true;
            setRewindCount();
            minusCoin(1000);
            rewindPowerUpButtonText.text = "owned";
        }
    }

    public void setRewindCount()
    {
        if(isPro)
        {
            rewindCount = 5;

            if (isRewindPowerup)
            {
                rewindCount = 7;
            }
        } else
        {
            rewindCount = 1;

            if (isRewindPowerup)
            {
                rewindCount = 3;
            }
        }
    }

    public void restoreGame()
    {
        playerScript.updateT();
        if(isHardcore)
        {
            PlayerPrefs.SetInt("TapHardcore", 0);
        } else
        {
            PlayerPrefs.SetInt("Tap", 0);
        }

    }

    public void restoreGameTapped()
    {
        playerScript.updateT();
        PlayerPrefs.SetInt("Tap", 0);
        PlayerPrefs.SetInt("TapHardcore", 0);
        StartCoroutine(restoreRestart(0.5f));
    }

    IEnumerator restoreRestart(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }

    public void uploadScore()
    {
        // HighScores.UploadScore(nameInput.text, tap * (-1));
        HighScores.UploadScore(nameInput.text, tap, isHardcore);
        endPanel.SetActive(false);
        leaderPanel.SetActive(true);
        restoreGame();
    }
}


