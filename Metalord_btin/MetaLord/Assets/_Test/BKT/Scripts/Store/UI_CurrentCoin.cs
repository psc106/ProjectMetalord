using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_CurrentCoin : UI_StoreCoin
{
    public float fadeDuration = 1f;
    private Coroutine fadeCoroutine;
    private CanvasGroup canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();        
    }

    //protected override void OnEnable()
    //{
    //    GameEventsManager.instance.coinEvents.onChangeCoin+= ReflectCoinToUI;
    //    ReflectCoinToUI(CoinManager.instance.currentCoin);  //자식에서 사용하니 NullReferenceException: 오류가 발생함에 따라 주석처리함
    //}

    protected override void ReflectCoinToUI(int coin)
    {
        base.ReflectCoinToUI(coin);

        if (canvasGroup != null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            canvasGroup.alpha = 1f;
            fadeCoroutine = StartCoroutine(FadeOutUI(canvasGroup));
        }
        else
        {
            Debug.LogError("CanvasGroup 컴포넌트를 찾을 수 없습니다!");
        }
    }

    IEnumerator FadeOutUI(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
