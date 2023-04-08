using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * 아이템들이 가지는 기본적인 데이터 관리
 * */
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]

public class Item : ScriptableObject
{    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
     //   Consumables,
        Etc
    }

    [TextArea]
    public string itemDesc; // 아이템 설명
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage; // 아이템 이미지
    public GameObject itemPrefab; // 아이템 프리팹

}
