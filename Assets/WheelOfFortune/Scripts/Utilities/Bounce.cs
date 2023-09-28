using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float upscaledSize = 1.2f;
    [SerializeField] private float bounceTime = 0.1f;
    [SerializeField] private float bounceInterval = 2f;

    private Sequence bounceTweenSequence;

    void Start()
    {
        StartBouncing();
    }

    public void StartBouncing()
    {
        bounceTweenSequence = DOTween.Sequence();
        bounceTweenSequence.Append(transform.DOScale(upscaledSize, bounceTime).SetLoops(4, LoopType.Yoyo));
        bounceTweenSequence.SetDelay(bounceInterval);
        bounceTweenSequence.SetLoops(-1);
    }

    public void StopBouncing()
    {
        bounceTweenSequence.Complete();
        bounceTweenSequence.Kill();
    }

    void OnDestroy()
    {
        StopBouncing();
    }
}
