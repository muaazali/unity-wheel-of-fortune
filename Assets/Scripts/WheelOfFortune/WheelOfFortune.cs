using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune
{
    public struct WheelRewardItem
    {
        public int index;
        public Sprite sprite;
        public string labelText;
        public string color;
        public float probability;
    }

    public class WheelOfFortune : MonoBehaviour
    {

        // ? We are limited to just 8 because of the way the wheel item is designed.
        // * Consider a different approach for the wheel item to allow for more items.
        // * e.g. render texture that just colors in the part of the wheel instead of using a sprite.
        public const int MAX_REWARD_ITEMS = 8;

        [SerializeField] private RectTransform spinningPart;
        [SerializeField] private GameObject rewardItemPrefab;

        public void Initialize(List<WheelRewardItem> rewardItems)
        {
            CreateRewardItems(rewardItems);
        }

        private void CreateRewardItems(List<WheelRewardItem> rewardItems)
        {
            for (int i = 0; i < rewardItems.Count; i++)
            {
                GameObject rewardItem = Instantiate(rewardItemPrefab, spinningPart.transform);
                rewardItem.transform.localPosition = Vector3.zero;
                rewardItem.transform.localScale = Vector3.one / 2;

                // * Rotate the reward item to match the wheel.
                rewardItem.transform.localRotation = Quaternion.Euler(0, 0, (360 / rewardItems.Count) * i);

                // * Move the reward item to the appropriate position.
                rewardItem.transform.localPosition += rewardItem.transform.up * (spinningPart.rect.height / 4);

                WheelOfFortuneItem wheelOfFortuneItem = rewardItem.GetComponent<WheelOfFortuneItem>();
                wheelOfFortuneItem.rewardItemImage.sprite = rewardItems[i].sprite;
                wheelOfFortuneItem.rewardItemLabel.text = rewardItems[i].labelText;
                wheelOfFortuneItem.backgroundImage.color = ColorUtility.TryParseHtmlString(rewardItems[i].color, out Color color) ? color : Color.black;
            }
        }
    }
}
