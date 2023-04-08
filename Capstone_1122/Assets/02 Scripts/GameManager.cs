using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float p_hp = 1.0f;
    public float p_water = 1.0f;
    public float p_brain = 1.0f;
    public float p_hunger = 1.0f;

    public float IncreaseStat(float p_stat, float num) //p_stat = 증가하는 변수, num = 증가량
    {
        if (p_stat + num >= 1.0f)
            p_stat = 1.0f;
        else
            p_stat += num;
        return p_stat;
    }
    public void SetRestartStat()
    {
        p_hp = 0.5f;
        p_water = 0.5f;
        p_brain = 0.5f;
        p_hunger = 0.5f;
    }
}