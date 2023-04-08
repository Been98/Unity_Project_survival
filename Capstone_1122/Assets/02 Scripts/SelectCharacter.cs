using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class SelectCharacter : MonoBehaviourPun
{
    string sceneName = "Main";

    public void Click0(GameObject btn)
    {

        Player.selectedNum = 0;
        photonView.RPC("BtnSetFalse", RpcTarget.OthersBuffered, btn.GetComponent<PhotonView>().ViewID);
        SceneManager.LoadScene(sceneName);
        
    }
    public void Click1(GameObject btn)
    {
        Player.selectedNum = 1;
        photonView.RPC("BtnSetFalse", RpcTarget.OthersBuffered, btn.gameObject.GetComponent<PhotonView>().ViewID);
        SceneManager.LoadScene(sceneName);
    }
    public void Click2(GameObject btn)
    {
        Player.selectedNum = 2;
        photonView.RPC("BtnSetFalse", RpcTarget.OthersBuffered, btn.gameObject.GetComponent<PhotonView>().ViewID);
        SceneManager.LoadScene(sceneName);
    }
    public void Click3(GameObject btn)
    {
        Player.selectedNum = 3;
        photonView.RPC("BtnSetFalse", RpcTarget.OthersBuffered, btn.gameObject.GetComponent<PhotonView>().ViewID);
        SceneManager.LoadScene(sceneName);
    }
    [PunRPC]
    private void BtnSetFalse(int viewID)
    {
        var obj = PhotonView.Find(viewID);
        obj.gameObject.SetActive(false); 
        PhotonNetwork.IsMessageQueueRunning = false;
    }
}
