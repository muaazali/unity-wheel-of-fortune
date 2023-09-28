using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardObtainedScreen : MonoBehaviour
{
    [SerializeField] private GameObject flare, header;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public void Initialize(Sprite itemImage, string itemText)
    {
        this.itemImage.sprite = itemImage;
        this.itemText.text = itemText;

        this.flare.transform.localScale = Vector2.zero;
        this.header.transform.localScale = Vector2.zero;
        this.itemImage.transform.localScale = Vector2.zero;
        this.itemText.transform.localScale = Vector2.zero;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.Append(this.header.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(this.itemImage.transform.DOScale(1f, 0.2f));
        sequence.Append(this.itemText.transform.DOScale(1f, 0.2f));
        sequence.Append(this.flare.transform.DOScale(1f, 0.2f));

        sequence.AppendCallback(() => Invoke(nameof(HideScreen), 5f));
    }

    void HideScreen()
    {
        this.header.transform.DOScale(0f, 0.2f);
        this.itemImage.transform.DOScale(0f, 0.2f);
        this.itemText.transform.DOScale(0f, 0.2f);
        this.flare.transform.DOScale(0f, 0.2f).OnComplete(() =>
        {
            UIController.Instance.HideRewardObtainedScreen();
        });
    }
}
