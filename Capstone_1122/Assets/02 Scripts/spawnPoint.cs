using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class spawnPoint : MonoBehaviourPun
{
    [SerializeField] private GameObject[] enemy;
    public bool[] e_count = { false };
    public bool isRespawn = false;
    public int e_index = -1;
    void Awake()
    {
        //StartCoroutine(spawn());
        Invoke("eee", 5f);
    }

 /*   public IEnumerator spawn()
    {
        Debug.Log("Sp");
        yield return new WaitForSeconds(5f); //모든 플레이어가 들어오고 그 후에 생성되어야함 enemyctrl에서 player를 읽어와야하기 때문에
        Debug.Log("a");
        //스폰포인트 기준 랜덤 좌표
        Vector3 spawnPos = new Vector3(Random.Range(this.transform.position.x, this.transform.position.x)
            , 1, Random.Range(this.transform.position.z, this.transform.position.z));
        Debug.Log("b");
        //어떤 동물이 생성될지 랜덤 선택
        e_index = Random.Range(0, enemy.Length);
        Debug.Log("c");
        //모든 네트워크에 프리팹 생성 **호스트에서만 생성됨**
        //photonView.RPC("EnemyInstantiate", RpcTarget.All,e_index,spawnPos);
        *//*    if (!PhotonNetwork.IsMasterClient) //호스트 서버가 아니라면 탈출
                yield break;*//*
        GameObject a = PhotonNetwork.Instantiate(enemy[e_index].name, spawnPos, enemy[e_index].transform.rotation);

        //생성된 오브젝트를 자식으로 설정(그래야 enemy스크립트 가능)
        a.transform.parent = this.transform;
    }*/
    public void Espawn() //바로 부르면 enemy가 삭제되면서 invoke취소됨
    {
        Invoke("eee", 5f);
    }
    private void eee() //생성 코드
    {
        if (!PhotonNetwork.IsMasterClient) //호스트 서버가 아니라면 탈출
            return;
        Vector3 spawnPos = new Vector3(Random.Range(this.transform.position.x, this.transform.position.x)
           , 1, Random.Range(this.transform.position.z, this.transform.position.z));
        e_index = Random.Range(0, enemy.Length);
        GameObject a = PhotonNetwork.Instantiate(enemy[e_index].name, spawnPos, enemy[e_index].transform.rotation);
        a.transform.parent = this.transform;
    }
}
