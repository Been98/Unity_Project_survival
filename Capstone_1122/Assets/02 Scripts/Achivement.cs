using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Achivement : MonoBehaviour
{
    public int hunterCount = 0; //적 사냥 수
    public int fireCount = 0; // 불 피운 수
    public int buildCount = 0; //제작 수
    public int waterCount = 0; //물 획득 수
    public int fishCount = 0; //낚시 수
    public int eatCount = 0; //먹은 수
    public int dayCount = 0; //버틴 수

    public bool isClearHunt = false;
    public bool isClearFire = false;
    public bool isClearBuild = false;
    public bool isClearWater = false;
    public bool isClearFishing = false;
    public bool isClearEat = false;
    public bool isClearDay = false;

    [SerializeField] GameObject noticePanel;
    [SerializeField] Image[] img_notice;
    [SerializeField] ImageList[] medal; //0 = hunter, 1 = fire, 2 = 제작 수, 3 = 물, 4 = 낚시, 5 = 먹기, 6 = 버틴 날

    [SerializeField] ParticleSystem[] FX_notice;


    private void Start()
    {
        FX_notice[0] = GameObject.FindWithTag("Player").transform.GetChild(8).GetComponent<ParticleSystem>();
        FX_notice[1] = GameObject.FindWithTag("Player").transform.GetChild(9).GetComponent<ParticleSystem>();
    }
    public void Hunter()
    {
        switch (hunterCount)
        {
            case 1:
                isClearHunt = true;
                medal[0].btn[0].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[0].img[0]));
                break;
            case 3:
                medal[0].btn[1].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[0].img[1]));
                break;
            case 4:
                medal[0].btn[2].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[0].img[2]));
                break;
        }
    }
    public void Fire()
    {
        switch (fireCount)
        {
            case 1:
                isClearFire = true;
                medal[1].btn[0].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[1].img[0]));
                break;
            case 3:
                medal[1].btn[1].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[1].img[1]));
                break;
            case 4:
                medal[1].btn[2].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[1].img[2]));
                break;
        }
    }
    public void Build()
    {
        switch (buildCount)
        {
            case 1:
                isClearBuild = true;
                medal[2].btn[0].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[2].img[0]));
                break;
            case 3:
                medal[2].btn[1].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[2].img[1]));
                break;
            case 4:
                medal[2].btn[2].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[2].img[2]));
                break;
        }
    }
    public void Water()
    {
        switch (waterCount)
        {
            case 1:
                isClearWater = true;
                medal[3].btn[0].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[3].img[0]));
                break;
            case 3:
                medal[3].btn[1].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[3].img[1]));
                break;
            case 4:
                medal[3].btn[2].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[3].img[2]));
                break;
        }
    }
    public void Fishing()
    {
        switch (fishCount)
        {
            case 1:
                isClearFishing = true;
                medal[4].btn[0].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[4].img[0]));
                break;
            case 3:
                medal[4].btn[1].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[4].img[1]));
                break;
            case 4:
                medal[4].btn[2].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[4].img[2]));
                break;
        }
    }
    public void Eat()
    {
        switch (eatCount)
        {
            case 1:
                isClearEat = true;
                medal[5].btn[0].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[5].img[0]));
                break;
            case 3:
                medal[5].btn[1].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[5].img[1]));
                break;
            case 4:
                medal[5].btn[2].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[5].img[2]));
                break;
        }
    }
    public void Day()
    {
        switch (dayCount)
        {
            case 1:
                medal[6].btn[0].image.color = Color.white;
                isClearDay = true;
                StartCoroutine(noticeCoroutine(medal[6].img[0]));
                break;
            case 3:
                medal[6].btn[1].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[6].img[1]));
                break;
            case 4:
                medal[6].btn[2].image.color = Color.white;
                StartCoroutine(noticeCoroutine(medal[6].img[2]));
                break;
        }
    }
    IEnumerator noticeCoroutine(Sprite img)
    {
        FX_notice[0].Play();
        FX_notice[1].Play();
        noticePanel.SetActive(true);
        img_notice[0].sprite = img;
        img_notice[1].sprite = img;
        yield return new WaitForSeconds(3f);
        noticePanel.SetActive(false);
        FX_notice[0].Stop();
        FX_notice[1].Stop();
    }

    public bool checkClearAll()
    {
        if (isClearHunt && isClearFire && isClearBuild && isClearWater && isClearFishing && isClearEat && isClearDay)
            return true;
        return false;
    }
}
[System.Serializable]
public class ImageList
{
    public Sprite[] img; //0 = b, 1 = s, 2 = g
    public Button[] btn; //0 = b, 1 = s, 2 = g
}