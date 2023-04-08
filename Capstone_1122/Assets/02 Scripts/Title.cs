using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] GameObject selectCharacterPanel;

   public void ClickStart()
    {
        Debug.Log("로딩");
        selectCharacterPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
