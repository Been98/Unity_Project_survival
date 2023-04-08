using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] Button RestartBtn;
    [SerializeField] Button ConnectBtn;

    string sceneName = "GameTitle";

    Player player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        RestartBtn.onClick.AddListener(OnClickRestart); 
        ConnectBtn.onClick.AddListener(OnClickConnect);
    }

   void OnClickRestart()
    {
        SceneManager.LoadScene(sceneName); // scene 새로 불러오기
    }

    void OnClickConnect()
    {
        player.Revival();

        GameManager.Instance.SetRestartStat(); // 모든 상태 0.5f로 초기화
        this.gameObject.SetActive(false);
    }

   
}
