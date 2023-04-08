using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DeathPointerEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject Tip;

    [SerializeField] TMP_Text text;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.tag == "Restart") // 새로 시작
        {
            Tip.SetActive(true); // tip 활성화

            Vector3 pos = new Vector3(260, 20, 0);
            Tip.GetComponent<RectTransform>().anchoredPosition = pos;

            text.text = "처음부터 다시 시작하기\n\n플레이어 모든 상태 초기화                                                                                                                                                                                                                                                                         ";
        }
        else if (eventData.pointerEnter.tag == "Connect") // 이어하기
        {
            Tip.SetActive(true); // tip 활성화

            Vector3 pos = new Vector3(260, -110, 0);
            Tip.GetComponent<RectTransform>().anchoredPosition = pos;

            text.text = "이어서 하기 \n\n 캐릭터 모든 상태 50으로 부활";

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tip.SetActive(false);
    }
}
