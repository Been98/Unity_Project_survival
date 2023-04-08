using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class ItemImage : MonoBehaviourPun, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    Slot slot;
    public DragSlot DragSlot;
    public RectTransform baseRect;

    private InputNumber inputNumber;
    private ItemEffectDatabase itemEffectDatabase;

    ItemCollider itemCollider;
    GameObject buildingItem;

    Player player;

    void Start()
    {
        slot = GetComponentInParent<Slot>();
       // baseRect = transform.parent.parent.parent.GetComponentInParent<RectTransform>();
        inputNumber = FindObjectOfType<InputNumber>();
        itemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>() ;
            //FindObjectOfType<Player>();
    }

    public void OnPointerClick(PointerEventData eventData) // 마우스 클릭 이벤트
    {

        if (eventData.button == PointerEventData.InputButton.Right) // 마우스 우클릭
        {
            if (slot.item != null) // 아이템 있으면
            {
                itemEffectDatabase.UseItem(slot.item);

                if (slot.item.itemType == Item.ItemType.Used)
                    slot.SetSlotCount(-1);
                //slot.OnClick();
            }
        }
    }
    //드레그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(slot.item != null && slot.item.itemType != Item.ItemType.Etc)
        {
            DragSlot.instance.dragSlot = slot;
            DragSlot.instance.DragSetImage(slot.itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    //드레그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (slot.item != null)
        {
            if(slot.item.itemType == Item.ItemType.Etc) // 설치하는 item일 때 
            {
                if (buildingItem == null)
                    buildingItem = PhotonNetwork.Instantiate(slot.item.itemPrefab.name, slot.item.itemPrefab.transform.position, slot.item.itemPrefab.transform.rotation);
                itemCollider = buildingItem.GetComponentInChildren<ItemCollider>();
                itemCollider.checkBuildCollider(); // 아이템 설치 가능한지 check
                RaycastHit hit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    buildingItem.transform.position = new Vector3(hit.point.x, slot.item.itemPrefab.transform.position.y, hit.point.z); // 아이템이 마우스를 따라다님
                    buildingItem.transform.rotation = player.transform.rotation;
                }                
            }
            else
            {
                DragSlot.instance.transform.position = eventData.position; // slot을 움직임
            }
        }
    }
    //드레그 끝 
    public void OnEndDrag(PointerEventData eventData)
    {
        if (buildingItem != null)
        { //설치중인 아이템이 있고
            if (itemCollider.checkBuildCollider()) //  설치 가능하면
            {
                if (buildingItem.GetComponent<BucketWater>())
                {
                    buildingItem.GetComponent<BucketWater>().isBuildFinish = true;
                }
                
                itemCollider.gameObject.SetActive(false);
                slot.ClearSlot();
                if (buildingItem.GetComponent<ItemPickUp>().item.itemName == "SOS") // 구조 신호 만들기
                    player.isBuildSos = true;
                buildingItem = null;

            }
            else
            {
                Destroy(buildingItem);
                buildingItem = null;
            }
        }
        else
        {
            // 마우스가 인벤토리 창 밖에 있으면
            if (!RectTransformUtility.RectangleContainsScreenPoint(baseRect,
             DragSlot.instance.transform.position))
            {
                if (DragSlot.instance.dragSlot != null)
                    inputNumber.Call();

            }
            else // 마우스가 인벤토리 창 안에 있을 때 
            {
                Debug.Log("인벤토리 안");
                DragSlot.instance.setColor(0);
                DragSlot.instance.dragSlot = null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            changeSlot();
        }
    }

    public void changeSlot()
    {
        // drop된 슬롯
        Item tempItem = slot.item;
        int tempCount = slot.itemCount;

        // drop된 슬롯에 아이템 추가
        slot.AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null) // drop된 슬롯에 아이템이 있었으면
        {
            DragSlot.instance.dragSlot.AddItem(tempItem, tempCount); // 원래 슬롯에 넣어주기
        }
        else // 없었으면 원래 슬롯 초기화
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != null)
        {
            itemEffectDatabase.ShowToolTip(slot.item, slot.GetComponent<RectTransform>().anchoredPosition3D);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemEffectDatabase.HideToolTip();
    }
}
