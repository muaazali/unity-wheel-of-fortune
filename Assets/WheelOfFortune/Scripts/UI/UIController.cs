using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune;

public class UIController : MonoBehaviour
{
    /* 
        ! DISCLAIMER: This class is only for the demo scene and is not production-ready.
    */
    public static UIController Instance { get; private set; }

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Button rewardsButton;

    [Space]
    [SerializeField] private Sprite coinSprite;

    [Space]
    [SerializeField] private GameObject coinRewardsScreenPrefab;
    private GameObject coinRewardsScreen;

    [Space]
    [SerializeField] private GameObject rewardObtainedScreenPrefab;
    private GameObject rewardObtainedScreen;

    private RewardsData rewardsData;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        rewardsButton.onClick.AddListener(ShowCoinRewardsScreen);
    }

    void Start()
    {
        rewardsData = new LoadRewardsFromJSONStrategy(Resources.Load<TextAsset>("Data/data").text).LoadRewards();
    }

    public void ShowCoinRewardsScreen()
    {
        if (coinRewardsScreen != null) return;
        coinRewardsScreen = Instantiate(coinRewardsScreenPrefab, mainCanvas.transform);
        coinRewardsScreen.GetComponent<CoinRewardsScreen>().Initialize(rewardsData);
        coinRewardsScreen.GetComponent<CoinRewardsScreen>().GrantReward += (reward) =>
        {
            ShowRewardObtainedScreen(coinSprite, $"{rewardsData.coins * rewardsData.rewards[reward].multiplier}");
        };
    }

    public void ShowRewardObtainedScreen(Sprite itemImage, string itemText)
    {
        if (rewardObtainedScreen != null) return;
        rewardObtainedScreen = Instantiate(rewardObtainedScreenPrefab, mainCanvas.transform);
        rewardObtainedScreen.GetComponent<RewardObtainedScreen>().Initialize(itemImage, itemText);
    }

    public void HideRewardObtainedScreen()
    {
        if (rewardObtainedScreen == null) return;
        Destroy(rewardObtainedScreen);
        rewardObtainedScreen = null;
    }
}
