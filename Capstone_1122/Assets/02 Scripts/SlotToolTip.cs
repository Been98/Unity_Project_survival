using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;
    
    [SerializeField]
    private TMP_Text txt_ItemName;
    [SerializeField]
    private TMP_Text txt_ItemDesc;
    [SerializeField]
    private TMP_Text txt_ItemHowtoUsed;

    public void showToolTip(Item item, Vector3 pos)
    {
        go_Base.SetActive(true);
        // 슬롯의 옆, 아래 절반에 위치
        pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 1.25f,
                            go_Base.GetComponent<RectTransform>().rect.height,
                            0);

        go_Base.GetComponent<RectTransform>().anchoredPosition3D = pos;
        // go_Base.transform.position = pos;

        // 이름, 설명 추가
       // txt_ItemName.text = item.itemName; 
        txt_ItemDesc.text = item.itemDesc;

        // 사용 방법 추가
        if (item.itemType == Item.ItemType.Equipment)
            txt_ItemHowtoUsed.text = "우 클릭 - 장착";
        else if (item.itemType == Item.ItemType.Used)
            txt_ItemHowtoUsed.text = "우 클릭 - 먹기";
        else
            txt_ItemHowtoUsed.text = "";

    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }

}
