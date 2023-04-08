using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1"; //게임의 버전 
    [SerializeField] TextMeshProUGUI connectionInfoText; 
    [SerializeField] Button joinButton;
    [SerializeField] GameObject selectCharacterPanel;

    void Start()
    {
        Screen.SetResolution(1600, 1000, false); //해상도 설정
        PhotonNetwork.GameVersion = gameVersion; //게임의 버전 
        PhotonNetwork.ConnectUsingSettings(); //접속시도
        joinButton.onClick.AddListener(Connect); //버튼 리스너 초기화
        joinButton.interactable = false; //버튼 하이라이트 끄기
        connectionInfoText.text = "서버에 연결 중..."; 
    }
    //서버와 연결 성공시
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "서버와 연결 성공 ! ";
    }
    //서버 접속 실패 시 자동 실행 
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "서버와 연결 실패 \n접속 재시도 중...";
        PhotonNetwork.ConnectUsingSettings();
    }
    //룸 접속 시도 
    public void Connect()
    {
        joinButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "룸에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "서버와 연결 실패 \n접속 재시도 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //(빈 방이 없어) 랜덤 룸 참가에 실패한 경우 자동 실행 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });

    }
    //룸에 참가 완료된 경우 시작 버튼 누르면 실행
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "방 참가 성공";
        selectCharacterPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //나가기 버튼 
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}