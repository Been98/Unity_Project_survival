using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class InputNumber : MonoBehaviour
{
    private bool activated = false; // 입력 필드 활성화

    [SerializeField]
    private TMP_Text text_Preview; // Placeholder, 드롭될 아이템 갯수
    [SerializeField]
    private TMP_Text text_Input; // 입력 받을 텍스트
    [SerializeField]
    private TMP_InputField if_text; // 입력 필드 초기화를 위해

    [SerializeField]
    private GameObject go_Base; // Input Field UI 할당 (비)활성화 

    private Player player;

    public DragSlot dragslot;



    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (activated)
        {
            if(Input.GetKeyDown(KeyCode.Return))
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))
                Cancel();
        }
    }

    public void Call() // 입력 필드 활성화
    {
        go_Base.SetActive(true); // InputField 활성화
        activated = true;
        if_text.text = ""; // 내용 초기화
        text_Preview.text = dragslot.instance.dragSlot.itemCount.ToString(); // 드롭될 아이템에 갖고 잇는 아이템 갯수
    }

    public void Cancel()
    {
        activated = false;
        dragslot.instance.setColor(0);
        go_Base.SetActive(false);
        dragslot.instance.dragSlot = null;
    }

    public void OK() // 실제 처리
    {
        dragslot.instance.setColor(0);
     
        int num;
        if (text_Input.text.Trim((char)8203) != "") // 갯수 입력이 들어왔다(8203은 공백문자, 공백을 차지하지 않는다 zero width space)
        {
            if (CheckNumber(text_Input.text.Trim((char)8203))) // 입력 값이 숫자인지
            {
                num = int.Parse(text_Input.text.Trim((char)8203)); // 갯수를 int로 변환
                if (num > dragslot.instance.dragSlot.itemCount) // 갖고 있는 거 보다 많이 버리면
                    num = dragslot.instance.dragSlot.itemCount; // 갖고 있는 만큼 
            }
            else
                num = 1;
        }
        else
        {
            Debug.Log("입력 개수 안 들어옴");
            num = int.Parse(text_Preview.text); // 갖고 있는 만큼 몽땅
        }

        StartCoroutine(DropItemCoroutine(num)); // 드랍 아이템 코루틴
    }

    IEnumerator DropItemCoroutine(int _num)
    {
        for(int i = 0; i < _num; i++)
        {
            PhotonNetwork.Instantiate(dragslot.instance.dragSlot.item.itemPrefab.name, new Vector3(0, dragslot.instance.dragSlot.item.itemPrefab.transform.position.y, 0) + player.transform.position + player.transform.forward, Quaternion.identity);
            dragslot.instance.dragSlot.SetSlotCount(-1); // 버려진 아이템 갯수 빼기
            yield return new WaitForSeconds(0.05f);
        }

        dragslot.instance.dragSlot = null; // 드래그된 슬롯 초기화
        go_Base.SetActive(false); // 입력 필드 비활성화
        activated = false;
    }

    private bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;

        for(int i = 0; i < _tempCharArray.Length; i++)
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57) // 입력값 숫자인지 (아스키 코드 47~57 숫자)
                continue;
            isNumber = false;

        }
        return isNumber;
    }

}
