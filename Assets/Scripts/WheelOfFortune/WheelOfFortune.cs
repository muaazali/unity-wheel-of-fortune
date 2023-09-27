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
        public string labelColor;
    }
    
    public class WheelOfFortune : MonoBehaviour
    {

        // ? We are limited to just 8 because of the way the wheel item is designed.
        // * Consider a different approach for the wheel item to allow for more items.
        // * e.g. render texture that just colors in the part of the wheel instead of using a sprite.
        public const int MAX_REWARD_ITEMS = 8;

        [SerializeField] private RectTransform spinningPart;
        [SerializeField] private GameObject rewardItemPrefab;

        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            CreateRewardItems();
        }

        private void CreateRewardItems()
        {
            for (int i = 0; i < MAX_REWARD_ITEMS; i++)
            {
                GameObject rewardItem = Instantiate(rewardItemPrefab, spinningPart.transform);
                rewardItem.transform.localPosition = Vector3.zero;
                rewardItem.transform.localScale = Vector3.one / 2;

                // * Rotate the reward item to match the wheel.
                rewardItem.transform.localRotation = Quaternion.Euler(0, 0, (360 / MAX_REWARD_ITEMS) * i);

                // * Move the reward item to the appropriate position.
                rewardItem.transform.localPosition += rewardItem.transform.up * (spinningPart.rect.height / 4);
            }
        }
    }
}
