using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune
{
    public class CoinRewardsScreen : MonoBehaviour
    {
        #region UI References
        [Header("UI References")]
        [SerializeField] private GameObject uiContentsContainer;

        [Space]
        [SerializeField] private WheelOfFortune wheelOfFortune;
        [SerializeField] private Button spinButton;
        [SerializeField] private Button closeButton;

        [Space]
        [SerializeField] private GameObject multiplierContainer;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private GameObject rewardContainer;
        [SerializeField] private TextMeshProUGUI rewardText;

        [Space]
        [SerializeField] private Sprite coinSprite;
        #endregion

        public System.Action<int> GrantReward;

        private RewardsData rewardsData;
        private List<WheelRewardItem> rewardItems;
        private List<WheelRewardItem> shuffledRewardItems;

        void Start()
        {
            uiContentsContainer.SetActive(false);
            Initialize(new LoadRewardsFromJSONStrategy($"{Application.dataPath}/Data/data.json"));
            uiContentsContainer.SetActive(true);
            uiContentsContainer.transform.localScale = Vector2.zero;
            uiContentsContainer.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        }

        public void Initialize(ILoadRewardsStrategy loadRewardsStrategy)
        {
            multiplierContainer.SetActive(false);
            multiplierText.text = string.Empty;
            rewardContainer.SetActive(false);
            rewardText.text = string.Empty;
            spinButton.onClick.AddListener(wheelOfFortune.StartSpin);
            closeButton.onClick.AddListener(CloseScreen);

            rewardsData = loadRewardsStrategy.LoadRewards();
            rewardItems = ConvertRewardDataToWheelRewardItems(rewardsData);
            shuffledRewardItems = Utility.ShuffleItems(rewardItems);

            wheelOfFortune.Initialize(shuffledRewardItems);

            wheelOfFortune.OnSpinStarted += OnSpinStarted;
            wheelOfFortune.OnSpinCompleted += OnSpinCompleted;
        }

        // * Serves as an adapter between the two data structures.
        List<WheelRewardItem> ConvertRewardDataToWheelRewardItems(RewardsData rewards)
        {
            List<WheelRewardItem> rewardItems = new List<WheelRewardItem>();

            for (int i = 0; i < rewards.rewards.Count; i++)
            {
                WheelRewardItem rewardItem = new WheelRewardItem
                {
                    index = i,
                    labelText = $"x{rewards.rewards[i].multiplier}",
                    color = rewards.rewards[i].color,
                    probability = rewards.rewards[i].probability
                };

                if (rewards.rewards[i].type == 0) // ? Assuming that type 0 is coins.
                {
                    rewardItem.sprite = coinSprite;
                }
                rewardItems.Add(rewardItem);
            }

            return rewardItems;
        }

        void OnSpinStarted()
        {
            DisableInteraction();
            multiplierContainer.SetActive(false);
            multiplierText.text = string.Empty;
            rewardContainer.SetActive(false);
            rewardText.text = string.Empty;
        }

        void OnSpinCompleted(int winningIndex)
        {
            int originalItemIndex = shuffledRewardItems[winningIndex].index;
            ShowRewardDetails(originalItemIndex);
        }

        void DisableInteraction()
        {
            spinButton.interactable = false;
            if (spinButton.TryGetComponent<Bounce>(out Bounce bounce)) bounce.StopBouncing();
            closeButton.interactable = false;
        }

        void EnableInteraction()
        {
            spinButton.interactable = true;
            if (spinButton.TryGetComponent<Bounce>(out Bounce bounce)) bounce.StartBouncing();
            closeButton.interactable = true;
        }

        void ShowRewardDetails(int winningIndex)
        {
            var sequence = DOTween.Sequence();

            multiplierContainer.SetActive(true);
            multiplierText.color = ColorUtility.TryParseHtmlString(rewardsData.rewards[winningIndex].color, out Color color) ? color : Color.white;

            float counter = 0;
            sequence.Append(DOTween.To(() => counter, x => counter = x, rewardsData.rewards[winningIndex].multiplier, 1).OnUpdate(() =>
            {
                multiplierText.text = $"x{Mathf.RoundToInt(counter)}";
            }));
            sequence.AppendCallback(() => counter = 0);
            sequence.AppendInterval(1);

            sequence.AppendCallback(() => rewardContainer.SetActive(true));
            sequence.Append(DOTween.To(() => counter, x => counter = x, rewardsData.coins * rewardsData.rewards[winningIndex].multiplier, 1).OnUpdate(() =>
            {
                rewardText.text = $"{Mathf.RoundToInt(counter)}";
            }));

            sequence.AppendInterval(1);
            sequence.AppendCallback(() =>
            {
                EnableInteraction();
                GrantReward?.Invoke(winningIndex);
            });
        }

        void CloseScreen()
        {
            Destroy(this.gameObject);
        }
    }
}
