using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public DragSlot instance;
    public Slot dragSlot;

    [SerializeField]
    Image imageItem;

    void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image itemImage)
    {
        imageItem.sprite = itemImage.sprite;
        setColor(1);
    }

    public void setColor(float alpha)
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }
   
}
