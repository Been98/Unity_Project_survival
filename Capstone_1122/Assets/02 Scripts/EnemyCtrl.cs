using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EnemyCtrl : MonoBehaviourPun, IPunObservable
{
    private float e_speed = 0.5f; //enemySpeed
    private Transform p_tf;
    Rigidbody rigid;

    [SerializeField] private GameObject meat;
    private GameObject player;

    private Animator e_ani;//enemy ?????????? 
    private bool isEscape = true; //?????? while?? ??????
    public float e_hp = 1.0f; //enemy hp

    float triggerDelay;
    bool isAttacked = false;

    private int e_index = -1; //생성되면 자기가 몇번 인덱스를 가진 적인지 알도록

    private spawnPoint sp;
    private Player p_ctrl;
    private Achivement ach;

    private float distance; //나와 플레이어의 사이 거리

    AudioSource audioSource;
    [SerializeField] private ParticleSystem FX_blood;

    private void Awake()
    {
        StartCoroutine(randomState());
        e_ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        p_tf = player.transform;
        p_ctrl = player.GetComponent<Player>();
        rigid = GetComponent<Rigidbody>();
        ach = GameObject.Find("HUD").GetComponent<Achivement>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        sp = GameObject.Find("spawnPoint").GetComponent<spawnPoint>(); //Awake에 두면 sp.spawn보다 빠름
        if (!PhotonNetwork.IsMasterClient) //호스트 서버가 아니라면 탈출
            return;
        e_index = sp.e_index; //번호 알려줌
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) //호스트 서버가 아니라면 탈출
            return;
        e_Move();

    }

    IEnumerator randomState()
    {
        while (isEscape)
        {
            int num = Random.Range(1, 4);
            float angle = this.transform.rotation.y;
            yield return new WaitForSeconds(2);

            switch (num)
            {
                case 1: //Idle
                    e_speed = 0.0f;
                    e_ani.SetFloat("e_Speed", 0.0f);
                    angle = this.transform.rotation.y;
                    break;
                case 2: //walk
                    e_speed = 0.5f;
                    e_ani.SetFloat("e_Speed", 0.5f);
                    angle = Random.Range(-180f, 180f);

                    break;
                case 3: //run
                    e_speed = 0.8f;
                    e_ani.SetFloat("e_Speed", 0.8f);
                    angle = Random.Range(-180f, 180f);
                    break;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(this.transform.rotation.x, angle, this.transform.rotation.z), 1.0f);
        }
    }
    private void e_Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * e_speed);
        distance = Vector3.Distance(p_tf.transform.position, this.transform.position);
        if (sp.e_count[e_index])
        {
            if (distance <= 5.0f)
            {
                e_ani.SetFloat("e_Speed", 1.0f);
                e_speed = 2;
                this.transform.LookAt(player.transform);
                isEscape = false;
            }
            else
            {
                if (!isEscape)
                {
                    isEscape = true;
                    StartCoroutine(randomState());
                }
            }
        }

    }
    private void e_escape()
    {
        transform.rotation = p_tf.rotation;
        e_speed = 10.0f;
        e_ani.SetFloat("e_Speed", 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemPickUp itempickup = other.GetComponent<ItemPickUp>();

        if (itempickup != null)
        {
            if (itempickup.item.itemName == "Axe" && !p_ctrl.isFireReady)
            {
                FX_blood.Play();
                audioSource.volume = 0.5f;
                audioSource.Play();
                player.GetComponent<Player>().PlayAudio("Hit"); // 맞는 소리 재생
                if (!isAttacked)
                { // 첫 피격
                    isAttacked = true;
                    e_hp -= 0.5f;
                    if (e_hp <= 0.0f)
                        e_die();
                    isEscape = false;
                    if (!sp.e_count[e_index])
                        e_escape();
                    Invoke("invokeCor", 3.0f);
                }
                else
                {
                    e_hp -= 0.5f;
                    if (e_hp <= 0.0f)
                        e_die();
                    isEscape = false;
                    if (!sp.e_count[e_index])
                        e_escape();
                    Invoke("invokeCor", 3.0f);
                }
               
            }
        }
    }

    private void invokeCor()
    {
        isEscape = true;
        StartCoroutine(randomState());
    }
    private void e_die()
    {
        sp.e_count[e_index] = true;
        PhotonNetwork.Instantiate(meat.name, this.transform.position, this.transform.rotation);
        ach.hunterCount++; //사냥성공
        ach.Hunter(); //업적 부르기
        //StartCoroutine(sp.spawn());
        //sp.Espawn();
        photonView.RPC("sendRpc", RpcTarget.MasterClient);
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject);
        else
            photonView.RPC("EDestroyRPC", RpcTarget.Others, this.gameObject.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    private void sendRpc()
    {
        sp.Espawn();
    }
    [PunRPC]
    private void EDestroyRPC(int object_id)
    {
        PhotonNetwork.Destroy(PhotonView.Find(object_id));
    }
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        FreezeRotation();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(e_hp);
            stream.SendNext(e_index);
        }
        else
        {
            e_hp = (float)stream.ReceiveNext();
            e_index = (int)stream.ReceiveNext();
        }
    }
}