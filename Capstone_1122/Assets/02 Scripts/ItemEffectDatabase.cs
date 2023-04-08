using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP, HUNGRY, WATER, MENTAL 뭐여")] // 체력, 배고픔, 수분량, 정신력
    public string[] part; // 아이템들
    public float[] num; // 수치들
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;
    [SerializeField]
    private SlotToolTip slotToolTip;

    private const string HP = "HP", HUNGRY = "HUNGRY", WATER = "WATER", MENTAL = "MENTAL";

    [SerializeField]
    private Player player;

    [SerializeField] Image[] p_fill; //0 =hp, 1 = hunger, 2 = water, 3 = mental
    private Achivement ach;
    private void Start()
    {

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        ach = GameObject.Find("HUD").GetComponent<Achivement>();
    }
    public void UseItem(Item item)
    {
        if (player.isEquip && item.itemName == player.equippedItem) // 같은 아이템을 두 번 눌렀을 때
        {
            player.unEquip();
            return;
        }
        
        if (item.itemType == Item.ItemType.Equipment) // 장착
        {
            if(player.isEquip)
                player.unEquip();
            
           
            player.Equip(item.itemPrefab);
        }
        if (item.itemType == Item.ItemType.Used) // 소비
        {
            player.PlayAudio("Eat");
            ach.eatCount++;
            ach.Eat();
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part[j])
                        {
                            case HP:
                                //player.IncreseHP(itemEffects[i].num[j]);
                                GameManager.Instance.p_hp = GameManager.Instance.IncreaseStat(GameManager.Instance.p_hp, itemEffects[i].num[j]);
                                p_fill[0].fillAmount = GameManager.Instance.p_hp;
                                break;
                            case HUNGRY:
                                //player.IncreaseHungry(itemEffects[i].num[j]);
                                GameManager.Instance.p_hunger = GameManager.Instance.IncreaseStat(GameManager.Instance.p_hunger, itemEffects[i].num[j]);
                                p_fill[1].fillAmount = GameManager.Instance.p_hunger;
                                break;
                            case WATER:
                                //player.IncreaseWater(itemEffects[i].num[j]);
                                GameManager.Instance.p_water = GameManager.Instance.IncreaseStat(GameManager.Instance.p_water, itemEffects[i].num[j]);
                                p_fill[2].fillAmount = GameManager.Instance.p_water;
                                break;
                            case MENTAL:
                                //player.IncreaseMental(itemEffects[i].num[j]);
                                GameManager.Instance.p_brain = GameManager.Instance.IncreaseStat(GameManager.Instance.p_brain, itemEffects[i].num[j]);
                                p_fill[3].fillAmount = GameManager.Instance.p_brain;
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위");
                                break;
                        }
                        Debug.Log(item.itemName + "을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("itemEffectDatabase에 일치하는 itemName이 없습니다.");
        }

        if(item.itemName == "Bedding") // 이불일 때
        {
            player.bedEffect = 1.5f;
           player.bedding.SetActive(true);
            
        }
    }

    public void ShowToolTip(Item item, Vector3 pos)
    {
        slotToolTip.showToolTip(item, pos);
    }
    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }
}
