using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Fishing : MonoBehaviourPun
{
    [SerializeField] Inventory inventory;
    Player player;
    [SerializeField] GameObject[] fish;

    [SerializeField] Item fishItem;


    [SerializeField] RectTransform fishIcon; // 조종할 이미지
    [SerializeField] RectTransform fishIconBack; // 맞춰야 하는 이미지
    float iconBackRandom_Y; // random 생성될 y 값
    float fishIcon_Y = 0.0f; // 이동 y
    int minus = 1;

    int randomProbability;
    float randomTime;
    int fishIndex;
    public bool isCatch = false;

    [SerializeField] GameObject FishingPanel;
    private Achivement ach;

    private void Start()
    {
        if (!photonView.IsMine)
            return;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        inventory = GameObject.Find("InventoryPanel").GetComponent<Inventory>();
        FishingPanel = GameObject.Find("PanelParent").transform.GetChild(4).gameObject;
        fishIcon = FishingPanel.transform.GetChild(1).GetComponent<RectTransform>();
        fishIconBack = FishingPanel.transform.GetChild(2).GetComponent<RectTransform>();  
        ach = GameObject.Find("HUD").GetComponent<Achivement>();
    }

    private void Update()
    {
        moveIcon();
    }

    public void RandomFishing() // 낚시 중일 때
    {
        randomProbability = Random.Range(0, 3); // 1/3 확률
        randomTime = Random.Range(1.0f, 3.0f); // 잡히는 시간 random
        randomProbability = 0;
        if (randomProbability == 0) // 물고기 잡힘
        {
            fishIndex = Random.Range(0, 3);
            Invoke("CatchFish", randomTime * 3); // 물고기 생성 함수 부르기
            Debug.Log("물고기 잡히노");
        }
    }

    private void CatchFish()
    {
        if (!photonView.IsMine)
            return;
        isCatch = true;
        fish[fishIndex].SetActive(true); // 물고기 3마리 중 한 마리 활성화
        FishingGame(); // game 함수 호출
        FishingPanel.SetActive(true);
        Debug.Log(player.transform.position);
        player.FishingText.text = "Press\n'G' Key to Stop";
    }

    public void finishFish() // 낚시 끝
    {
        if (isCatch) // 물고기가 잡혔었으면
        {
            FishingPanel.SetActive(false);
            fish[fishIndex].SetActive(false); // 물고기 비활성화
            isCatch = false;

            if (checkSuccess())
            { // game을 성공하면
                ach.fishCount++;
                ach.Fishing();
                inventory.AcquireItem(fishItem);    // add inventory
            }

        }
    }

    private void FishingGame()
    {
        iconBackRandom_Y = Random.Range(-110.0f, 110.0f); // 목표물 아이콘 랜덤 생성
        fishIconBack.anchoredPosition = new Vector3(0.0f, iconBackRandom_Y);

    }

    private void moveIcon()
    {
        if (player.isFishing && isCatch) // 낚시 중이고 물고기가 걸렸을 때
        {
            fishIcon_Y += minus * 0.5f;
            if (fishIcon_Y + minus * 1.0f > 110.0f // 110보다 커지거나 -110보다 작아지면 방향 바꾸기
                || fishIcon_Y + minus * 1.0f < -110.0f)
                minus *= -1;

            fishIcon.anchoredPosition = new Vector3(0.0f, fishIcon_Y);
        }


    }

    public bool checkSuccess()
    {
        if (fishIcon_Y < iconBackRandom_Y + 5.0f && // 오차 범위 안에 들어오면
                    fishIcon_Y > iconBackRandom_Y - 5.0f)
            return true;
        else
            return false;
    }
}