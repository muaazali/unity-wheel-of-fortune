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

        [Header("Wheel Settings")]
        [SerializeField] private float spinDuration = 3f;
        [SerializeField] private int emptySpinsBeforeReachingDestination = 3;

        [Header("UI References")]
        [SerializeField] private RectTransform spinningPart;
        [SerializeField] private GameObject rewardItemPrefab;

        public System.Action OnSpinStarted;
        public System.Action<int> OnSpinCompleted;

        private List<WheelRewardItem> rewardItems;

        public void Initialize(List<WheelRewardItem> rewardItems)
        {
            this.rewardItems = rewardItems;
            CreateRewardItems(rewardItems);
        }

        public void StartSpin()
        {
            OnSpinStarted?.Invoke();
            int winningIndex = CalculateWinningItem();
            StartCoroutine(SpinWheel(winningIndex));
        }

        private void CreateRewardItems(List<WheelRewardItem> rewardItems)
        {
            if (rewardItems.Count > MAX_REWARD_ITEMS)
            {
                Debug.LogWarning($"Expected {MAX_REWARD_ITEMS} reward items, found {rewardItems.Count}. Only the first {MAX_REWARD_ITEMS} will be used.");
            }

            int rewardItemCount = Mathf.Min(rewardItems.Count, MAX_REWARD_ITEMS);
            for (int i = 0; i < rewardItemCount; i++)
            {
                GameObject rewardItem = Instantiate(rewardItemPrefab, spinningPart.transform);
                rewardItem.transform.localPosition = Vector3.zero;
                rewardItem.transform.localScale = Vector3.one / 2;

                // * Rotate the reward item to match the wheel.
                rewardItem.transform.localRotation = Quaternion.Euler(0, 0, (360 / rewardItemCount) * i);

                // * Move the reward item to the appropriate position.
                rewardItem.transform.localPosition += rewardItem.transform.up * (spinningPart.rect.height / 4);

                WheelOfFortuneItem wheelOfFortuneItem = rewardItem.GetComponent<WheelOfFortuneItem>();
                wheelOfFortuneItem.rewardItemImage.sprite = rewardItems[i].sprite;
                wheelOfFortuneItem.rewardItemLabel.text = rewardItems[i].labelText;
                wheelOfFortuneItem.backgroundImage.color = ColorUtility.TryParseHtmlString(rewardItems[i].color, out Color color) ? color : Color.black;
            }
        }

        // * Probability based selection.
        private int CalculateWinningItem()
        {
            int winningProbabilityMeasure = Random.Range(0, 101);
            float cumulativeProbability = 0f;
            for (int i = 0; i < rewardItems.Count; i++)
            {
                cumulativeProbability += rewardItems[i].probability * 100;
                if (winningProbabilityMeasure <= cumulativeProbability)
                {
                    return i;
                }
            }

            // * Fallback to a random item if the probability calculation fails.
            return Random.Range(0, rewardItems.Count);
        }

        private IEnumerator SpinWheel(int winningIndex)
        {
            float startRotation = spinningPart.rotation.eulerAngles.z;
            float endRotation = -((360 * emptySpinsBeforeReachingDestination) + ((360 / rewardItems.Count) * winningIndex));

            float t = 0f;
            while (t < spinDuration)
            {
                t += Time.deltaTime;
                var quarticEaseOutStep = 1 - Mathf.Pow(1 - (t / spinDuration), 4);
                float rotation = Mathf.Lerp(startRotation, endRotation, quarticEaseOutStep);
                spinningPart.rotation = Quaternion.Euler(0, 0, rotation);
                yield return null;
            }

            OnSpinCompleted?.Invoke(winningIndex);
        }
    }
}
