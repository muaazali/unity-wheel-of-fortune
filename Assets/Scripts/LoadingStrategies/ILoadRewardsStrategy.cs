using System.Collections.Generic;

namespace WheelOfFortune
{
  public interface ILoadRewardsStrategy
  {
    RewardsData LoadRewards();
  }
}
