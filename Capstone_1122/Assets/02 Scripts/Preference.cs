using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Photon.Pun;

public class Preference : MonoBehaviourPun
{
    [SerializeField] GameObject setPausePanel;
    [SerializeField] GameObject setBookPanel;
    [SerializeField] GameObject setBuildPanel;
    [SerializeField] GameObject setMixPanel;
    [SerializeField] GameObject setQuestPanel;
    private bool isActive = false;
    private bool isBook = false;
    [SerializeField] Button btnChoose;
    [SerializeField] Button btnCancel;
    [SerializeField] Button btnExit;
    public BuildSlot buildSlot; // buildSlot이 onClick될 때 자신을 여기에 넣어 줌
    [SerializeField] Slot[] slot;
    bool isCheck;
    Inventory inventory;
 
    [SerializeField] GameObject checkPanel;

    private Achivement ach;
    private void Awake() //player스폰포인트 찾아서 거기서 생성
    {
        Transform[] points = GameObject.Find("PlayerSpawnPoint").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
    void Start()
    {
        btnChoose.onClick.AddListener(btnChooseClick);
        btnCancel.onClick.AddListener(btnCancelClick);
        btnExit.onClick.AddListener(btnExitClick);
        inventory = GetComponentInChildren<Inventory>();
        ach = GameObject.Find("HUD").GetComponent<Achivement>();
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        openPause();
        openBook();
        openBuild();
        openQuest();
    }

    private void openPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isActive)
        {
            setPausePanel.SetActive(true);
            isActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            setPausePanel.SetActive(false);
            isActive = false;
        }
    }
    private void openBook()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!isBook)
            {
                setBookPanel.SetActive(true);
                isBook = true;
            }
            else
            {
                isBook = false;
                setBookPanel.SetActive(false);
            }
        }
    }
    private void openBuild()
    {
        if (Input.GetKeyDown(KeyCode.B) && !setBuildPanel.activeSelf)
        {
            setBuildPanel.SetActive(true);
            inventory.isBuild = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && setBuildPanel.activeSelf)
        {
            setBuildPanel.SetActive(false);
            inventory.isBuild = false;
        }
    }
    private void openQuest()
    {
        if (Input.GetKeyDown(KeyCode.K) && !setQuestPanel.activeSelf)
        {
            setQuestPanel.SetActive(true);
            //inventory.isBuild = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) && setQuestPanel.activeSelf)
        {
            setQuestPanel.SetActive(false);
            //inventory.isBuild = false;
        }
    }

    private bool FindItemCount()
    {
        bool checkTree = false, checkStone = false, checkVine = false, checkBigTree = false;

        int cost_tree = buildSlot.buildItem.GetCostTree();
        if (cost_tree == 0) checkTree = true;
        int cost_stone = buildSlot.buildItem.GetCostStone();
        if (cost_stone == 0) checkStone = true;
        int cost_vine = buildSlot.buildItem.GetCostVine();
        if (cost_vine == 0) checkVine = true;
        int cost_bigtree = buildSlot.buildItem.GetCostBigTree();
        if (cost_bigtree == 0) checkBigTree = true;

        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].item != null) // 아이템을 갖고 있을 때
            {
                if (!checkTree && slot[i].item.itemName == "Twig") // 나뭇가지
                {
                    if (slot[i].itemCount >= cost_tree)
                    {
                        checkTree = true;
                        slot[i].SetSlotCount(-cost_tree);
                    }
                }
                else if (!checkStone && slot[i].item.itemName == "Rock")
                {
                    if (slot[i].itemCount >= cost_stone)
                    {
                        checkStone = true;
                        slot[i].SetSlotCount(-cost_stone);
                    }
                }
                else if (!checkVine && slot[i].item.itemName == "Vine")
                {
                    if (slot[i].itemCount >= cost_vine)
                    {
                        checkVine = true;
                        slot[i].SetSlotCount(-cost_vine);
                    }
                }
                else if (!checkBigTree && slot[i].item.itemName == "BigTree")
                {
                    if (slot[i].itemCount >= cost_bigtree)
                    {
                        checkBigTree = true;
                        slot[i].SetSlotCount(-cost_bigtree);
                    }
                }
            }
        }
        return checkTree && checkStone && checkVine && checkBigTree;
    }

    public void btnChooseClick()
    {
        if (FindItemCount())
        {

            Debug.Log("재료 다 있어!");
            ach.buildCount++;
            ach.Build();
            inventory.AcquireItem(buildSlot.item); // 만든 아이템 슬롯에 넣기
            setBuildPanel.gameObject.SetActive(false);
            setMixPanel.SetActive(false);
            inventory.isBuild = false;
        }
        else
        {
            checkPanel.SetActive(true);
            Invoke("disappearPanel", 1.0f);
        }
    }

    private void disappearPanel()
    {
        checkPanel.SetActive(false);
    }

    private void btnCancelClick()
    {
        setMixPanel.SetActive(false);
    }

    public void btnExitClick()
    {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        
    }
}
