using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollider : MonoBehaviour
{
    GameObject nearObject;
    [SerializeField] Material material;

    private Color color;

  
    private void OnTriggerStay(Collider other)
    {
        if(other.name != "Floor")
            nearObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        nearObject = null;
    }
    public bool checkBuildCollider()
    {
       if(nearObject != null) // 주변에 뭐가 있을 때 설치 불가능
        {
            ColorUtility.TryParseHtmlString("#DB6B6B43", out color); // 빨간색
            material.color = color;
            return false;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#7B6BDB43", out color); // 파란색
            material.color = color;
            return true;
        }

    }
}
