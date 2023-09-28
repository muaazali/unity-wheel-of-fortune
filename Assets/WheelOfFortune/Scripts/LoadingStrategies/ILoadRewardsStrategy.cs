using System.Collections.Generic;

namespace WheelOfFortune
{
    [System.Serializable]
    public class RewardItemData
    {
        public float multiplier;
        public float probability;
        public string color;
        public int type;
    }

    [System.Serializable]
    public class RewardsData
    {
        public int coins;
        public List<RewardItemData> rewards;
    }

    public interface ILoadRewardsStrategy
    {
        RewardsData LoadRewards();
    }
}
