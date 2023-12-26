using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 설명 창
/// 231226 배경택
/// </summary>
public class ExplainCanvas : MonoBehaviour
{

    private int pageIndex = 0;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;

    const int FIRST_IMAGE_INDEX = 1;
    const int LAST_IMAGE_INDEX = 4;


    private void OnEnable()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);

        pageIndex = FIRST_IMAGE_INDEX;
        buttonCheck();
    }

    //좌측버튼 클릭
    public void ClickLeftButton()
    {
        pageIndex--;
        for(int i = FIRST_IMAGE_INDEX; i <= LAST_IMAGE_INDEX; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(pageIndex).gameObject.SetActive(true);

        buttonCheck();
    }

    //우측버튼 클릭
    public void ClickRightButton()
    {
        pageIndex++;
        for (int i = FIRST_IMAGE_INDEX; i <= LAST_IMAGE_INDEX; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(pageIndex).gameObject.SetActive(true);

        buttonCheck();
    }

    // 버튼 체크
    private void buttonCheck()
    {
        CanUseSound();

        if (pageIndex == FIRST_IMAGE_INDEX) // 첫 페이지
        {
            leftButton.SetActive(false);
            rightButton.SetActive(true);
        }
        else if (pageIndex == LAST_IMAGE_INDEX) // 가장 끝 페이지
        {
            leftButton.SetActive(true);
            rightButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }
    }

    // 임시 소리
    private void CanUseSound()
    {
        // 사운드 추가       
        SoundManager.instance.PlaySound(GroupList.UI, (int)UISoundList.Can_BuySound);
    }
}