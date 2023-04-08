using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    Slot[] slots;

    public bool isBuild = false; // 빌딩창 열린지 확인 컴포넌트 추가 줄이기 위해 여기다 놨다

    private void Start()
    {
        slots = GetComponentsInChildren<Slot>();
    }

    public void AcquireItem(Item _item, int _count = 1) // 아이템 습득
    {
        
        if (Item.ItemType.Equipment != _item.itemType) // 무기가 아니면
        {
            for (int i = 0; i< slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName) // 해당 슬롯 찾아
                    {
                        slots[i].SetSlotCount(_count); // 아이템 개수 늘리기
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++) // 아이템 첫 습득
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

}
