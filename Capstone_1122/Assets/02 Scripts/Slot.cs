using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public Item item; // 획득한 아이템
    public int itemCount; // 아이템 개수
    public Image itemImage;

    [SerializeField]
    public TextMeshProUGUI text_Count;
    [SerializeField]
    GameObject go_CountImage;


    public Slot dragSlot;

    // 아이템 이미지 불투명도

    void setColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
   

    // 인벤토리에 새로운 아이템 슬롯 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        //itemImage.enabled = true;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment) // 아이템이 무기가 아닐 때
        {
            go_CountImage.SetActive(true); // 아이템 개수 이미지 활성화
            text_Count.text = itemCount.ToString(); // 아이템 개수 텍스트
        }
        else // 무기이면 개수 표시 x
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        setColor(1);
    }

    // 해당 슬롯의 아이템 갯수 업데이트
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }
    
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        setColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }



 
}

