using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class sun : MonoBehaviourPun
{

    [SerializeField] private float dayTime; // 낮에 해가 움직이는 시간
    private float nightTime;  //밤 시간은 해가 낮 시간의 2배 빨리 지나감

    private bool isNight = false;   //밤인지 아닌지
    private bool pass = false;      //가장 어두울 부분을 통과했는지

    [SerializeField] private float nightFog; // 밤 상태의 fog 밀도
    [SerializeField] private float dayFog;    // 낮상태의 fog 밀도
    [SerializeField] private float currentFog;   //계산

    [SerializeField] private float angle;
    private Achivement ach;
    // Start is called before the first frame update
    void Start()
    {
        dayFog = RenderSettings.fogDensity;
        currentFog = dayFog;
        nightTime = dayTime / 2;    //밤 시간은 낮보다 2배 빠르다

        StartCoroutine(sunMove());
        StartCoroutine(Dark());
        ach = GameObject.Find("HUD").GetComponent<Achivement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        angle = transform.eulerAngles.x;
    }

    IEnumerator sunMove() // 해 이동 코루틴
    {

        while (true)
        {
            if (transform.eulerAngles.x <= 90)      //낮 시간대라면
                yield return new WaitForSeconds(dayTime); //3초뒤에 이동
            else                                    //밤 시간대라면
                yield return new WaitForSeconds(nightTime); //2배 빨리이동

            transform.Rotate(Vector3.right, 30);
        }

    }

    IEnumerator Dark() // 어두워지는 코루틴
    {
        bool isFlag = false;
        while (true)
        {

            if ((transform.eulerAngles.x > 271 && transform.eulerAngles.x <= 359) && pass == false) //어두워져야 할 구간
            {
                isNight = true;

            }
            else if ((transform.eulerAngles.x >= 270 && transform.eulerAngles.x < 271) && pass == false)
            {
                isNight = false;
                pass = true;    //가장 어두운 부분통과 이제부터 밝아지겠다
                isFlag = true;
            }
            else if ((transform.eulerAngles.x >= 0 && transform.eulerAngles.x <= 90))
            {
                pass = false;   //낮이 되었으니 가장 어두운 부분은 아직 통과하기 전(초기화)
                if (isFlag)
                {
                    isFlag = false;
                    ach.dayCount++;
                    ach.Day();
                }
            }

            if (isNight)
            {
                if (currentFog <= nightFog)
                {
                    currentFog += 0.004f;
                    RenderSettings.fogDensity = currentFog;
                }
            }
            else
            {
                if (currentFog >= dayFog)
                {
                    currentFog -= 0.002f;
                    RenderSettings.fogDensity = currentFog;
                }
            }

            yield return new WaitForSeconds(nightTime / 3); //해 이동보다 3배 빠르게 어두워짐
        }
    }

}
