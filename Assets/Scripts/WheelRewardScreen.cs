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

        void OnEnable()
        {

        }
    }
}
