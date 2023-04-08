using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersAnimation : MonoBehaviour
{
    [SerializeField] Animator[] anim;

    private void Start()
    {
        StartAnimation();
    }
    void StartAnimation()
    {
        for(int i = 0; i < 4; i++)
        {
            anim[i].SetFloat("Speed_f", 0.0f);
        }
        anim[0].SetBool("Dance_b 0", true);
        anim[1].SetBool("Dance_b 1", true);
        anim[2].SetBool("Dance_b 2", true);
        anim[3].SetBool("Dance_b", true);
    }
}
