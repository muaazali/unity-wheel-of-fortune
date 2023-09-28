using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune
{
    public class WheelRewardScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private WheelOfFortune wheelOfFortune;
        [SerializeField] private Button spinButton;

        [Space]
        [SerializeField] private GameObject multiplierContainer;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private GameObject rewardContainer;
        [SerializeField] private TextMeshProUGUI rewardText;

        [Space]
        [SerializeField] private Sprite coinSprite;

        private RewardsData rewardsData;
        private List<WheelRewardItem> rewardItems;
        private List<WheelRewardItem> shuffledRewardItems;

        void Start()
        {
            Initialize(new LoadRewardsFromJSONStrategy($"{Application.dataPath}/Data/data.json"));
        }

        public void Initialize(ILoadRewardsStrategy loadRewardsStrategy)
        {

            multiplierContainer.SetActive(false);
            rewardContainer.SetActive(false);
            spinButton.onClick.AddListener(wheelOfFortune.StartSpin);

            rewardsData = loadRewardsStrategy.LoadRewards();
            rewardItems = ConvertRewardDataToWheelRewardItems(rewardsData);
            shuffledRewardItems = ShuffleRewardItems(rewardItems);

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
                WheelRewardItem rewardItem = new WheelRewardItem();
                rewardItem.index = i;
                rewardItem.labelText = $"x{rewards.rewards[i].multiplier}";
                rewardItem.color = rewards.rewards[i].color;
                rewardItem.probability = rewards.rewards[i].probability;

                if (rewards.rewards[i].type == 0) // ? Assuming that type 0 is coins.
                {
                    rewardItem.sprite = coinSprite;
                }
                rewardItems.Add(rewardItem);
            }

            return rewardItems;
        }

        List<WheelRewardItem> ShuffleRewardItems(List<WheelRewardItem> items)
        {
            List<WheelRewardItem> shuffledItems = new List<WheelRewardItem>(items);

            for (int i = 0; i < shuffledItems.Count; i++)
            {
                WheelRewardItem temp = shuffledItems[i];
                int randomIndex = Random.Range(i, shuffledItems.Count);
                shuffledItems[i] = shuffledItems[randomIndex];
                shuffledItems[randomIndex] = temp;
            }

            return shuffledItems;
        }

        void OnSpinStarted()
        {
            DisableSpinButton();
            multiplierContainer.SetActive(false);
            rewardContainer.SetActive(false);
        }

        void OnSpinCompleted(int winningIndex)
        {
            EnableSpinbutton();
            int originalItemIndex = shuffledRewardItems[winningIndex].index;
            GrantReward(originalItemIndex);
        }

        void DisableSpinButton()
        {
            spinButton.interactable = false;
            spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Spinning";
        }

        void EnableSpinbutton()
        {
            spinButton.interactable = true;
            spinButton.GetComponentInChildren<TextMeshProUGUI>().text = "Spin";
        }

        void GrantReward(int winningIndex)
        {
            multiplierContainer.SetActive(true);
            multiplierText.text = $"x{rewardsData.rewards[winningIndex].multiplier}";
            multiplierText.color = ColorUtility.TryParseHtmlString(rewardsData.rewards[winningIndex].color, out Color color) ? color : Color.white;

            rewardContainer.SetActive(true);
            rewardText.text = $"{rewardsData.coins * rewardsData.rewards[winningIndex].multiplier}";
        }
    }
}
