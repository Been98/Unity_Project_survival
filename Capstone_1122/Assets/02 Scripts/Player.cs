using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    //public Camera followCamera;
    public LayerMask playerLayer;

    //float rotSpeed = 180f;
    float moveSpeed = 5f;
    float anim_speed = 0.0f;
    float hAxis;
    float vAxis;
    bool rDown; // run
    bool jDown; // jump
    bool fDown; // fire
    bool gDown; // gather

    bool isJump = false;
    public bool isFireReady = true;
    bool isGather = false;
    public bool isEquip = false;
    public bool isFishing = false;
    bool isFire = false; // 불 지피기
    bool isStartFire = false; // 불지피는 중
    bool isFinishFire = false;
    public bool isBuildSos = false; // 구조 신호 만들었는지

    Rigidbody rigid;
    Animator anim;
    GameObject nearObject;
    CameraFollow MainCamera;
    Fishing fishing;

    public Inventory inventory;
    public GameObject Hand;
    public string equippedItem;
    Transform inst;
    private Achivement ach;

    float fireDelay;
    private float damageDelay;

    [SerializeField] Image hpFill;
    [SerializeField] Image waterFill;
    [SerializeField] Image hungerFill;
    [SerializeField] Image brainFill;

    // 인벤토리 변수
    public float range; // 아이템 습득 가능 거리
    bool pickupActivated = false; // 아이템 습득 가능 여부
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    TextMeshProUGUI actionText;
    public GameObject MessagePanel;

    // 불 지피기
    [SerializeField] Image fireFill;
    [SerializeField] TMP_Text campFireText;
    public bool isSuccessedFire = false; // 성공!
    [SerializeField] GameObject CampFirePanel;
    [SerializeField] Image CampFireTextPanel;
    float t_fire = 0; // panel 반짝임 주기
    [SerializeField] Slot[] slot;
    [SerializeField] GameObject CantCampFire;
    bool isTrueTwig = false;
    float timeFire = 0.0f;
    GameObject fireParticle;

    // 캐릭터 선택
    [SerializeField] GameObject[] playerMaterial; // player material
    [SerializeField] Material[] changeMaterial; // 적용 시킬 material
    public static int selectedNum; // 고른 캐릭터

    public TMP_Text FishingText; // 물고기 게임 텍스트

    // hp 경고 창
    [SerializeField] GameObject HpWarningPanel;
    float t_hp = 0; // panel 반짝임 주기
    bool isSetHpWarning = false;

    // 죽음
    [SerializeField] GameObject DeathPanel; // 죽었을 때 활성화할 패널
    public bool isDeath = false;
    [SerializeField] ParticleSystem FX_Revival;

    // 게임 클리어
    [SerializeField] GameObject CearlPanel; // 게임 클리어 패널 
    bool isClearGame = false;
    [SerializeField] ParticleSystem FX_FireCracker; // 폭죽 파티클
    private int m_index; //material인덱스 잠시 보관하기 용도

    // 사운드
    AudioSource[] audioSource;
    [SerializeField] AudioClip audioWalk;
    [SerializeField] AudioClip audioRun;
    [SerializeField] AudioClip audioSwing;
    [SerializeField] AudioClip audioHit;
    [SerializeField] AudioClip audioWood;
    [SerializeField] AudioClip audioFishing;
    [SerializeField] AudioClip audioFishingSuccess;
    [SerializeField] AudioClip audioEat;
    [SerializeField] AudioClip audioWater;
    [SerializeField] AudioClip audioHelicopter;
    [SerializeField] AudioClip audioJump;
    bool isMoving = false;

    // 잠 자기
    public bool isSleep = false;
    Transform beforeSleepTrans;
    public GameObject bedding;
    public float bedEffect = 1.0f;

    GameObject Parent;

    private void Awake()
    {
        if (!photonView.IsMine)
            return;
        init();
        m_index = selectedNum;
        Debug.Log(m_index);
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        MainCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        MainCamera.target = this.transform;
        MainCamera.init();
        hpFill.fillAmount = GameManager.Instance.p_hp;
        ach = GameObject.Find("HUD").GetComponent<Achivement>();
        waterFill.fillAmount = GameManager.Instance.p_water;
        hungerFill.fillAmount = GameManager.Instance.p_hunger;
        brainFill.fillAmount = GameManager.Instance.p_brain;
        selectCharacter(); // 게임 시작할 때 먼저 실행되는 스크립트에서 값 넘겨줌 
        audioSource = GetComponents<AudioSource>();
        //Debug.Log(audioSource.Length);
    }
    private void init() //player prefab 초기화
    {
        Parent = GameObject.Find("PanelParent");
        inventory = GameObject.Find("InventoryPanel").GetComponent<Inventory>();
        hpFill = GameObject.Find("hp_fill").GetComponent<Image>();
        waterFill = GameObject.Find("water_fill").GetComponent<Image>();
        hungerFill = GameObject.Find("hunger_fill").GetComponent<Image>();
        brainFill = GameObject.Find("brain_fill").GetComponent<Image>();
        //GameObject a = GameObject.Find("Action");
        MessagePanel = Parent.transform.GetChild(0).gameObject;
        actionText = MessagePanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        CampFirePanel = Parent.transform.GetChild(1).gameObject;
        fireFill = CampFirePanel.transform.GetChild(0).GetComponent<Image>();
        CampFireTextPanel = CampFirePanel.transform.GetChild(1).GetComponent<Image>();
        campFireText = CampFireTextPanel.transform.GetChild(0).GetComponent<TMP_Text>();
        CantCampFire = Parent.transform.GetChild(2).gameObject;
        FishingText = Parent.transform.GetChild(4).GetChild(3).GetChild(0).GetComponent<TMP_Text>();
        HpWarningPanel = Parent.transform.GetChild(3).gameObject;
        DeathPanel = Parent.transform.GetChild(5).gameObject;
        CearlPanel = Parent.transform.GetChild(6).gameObject;
        GameObject[] a = GameObject.FindGameObjectsWithTag("Slot");
        for (int i = 0; i < a.Length; i++) //foreach보다 성능 좋음 
        {
            slot[i] = a[i].GetComponent<Slot>();
        }
        bedding = this.transform.GetChild(0).gameObject;
        Debug.Log(CearlPanel.name);
    }

    void selectCharacter()
    {
        photonView.RPC("MaterialSelect", RpcTarget.AllBuffered, playerMaterial[m_index].gameObject.GetComponent<PhotonView>().ViewID, m_index);
        //playerMaterial[selectedNum].SetActive(true);
        // 해당 캐릭터의 material 입히기
        //playerMaterial[selectedNum].GetComponent<Renderer>().material = changeMaterial[selectedNum];
    }
    [PunRPC]
    private void MaterialSelect(int viewID, int m)
    {
        var obj = PhotonView.Find(viewID);
        obj.gameObject.SetActive(true);
        obj.GetComponent<Renderer>().material = changeMaterial[m];
    }
    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (!isDeath && !isClearGame)
        {
            GetInput();
            Move();
            Jump();
            Hit();
            // Gather();
            Fishing();
            CampFire();
            CheckItem(); // 습득할 아이템 있는지 
            TryAction();
            CheckPlayerState();
            GameClear();
            fireTimeManage(); // 모닥불 관리
            if (isSleep) // 자고 있을 때 회복
                HealingPlayerState();
            else // 깨어있을 때 소모
                UpdatePlayerState();
            
        }
       
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("run");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        gDown = Input.GetButtonDown("Gather");

    }

    public void PlayAudio(string action)
    {
      
        switch (action)
        {

            case "Swing":
                audioSource[1].clip = audioSwing;
                break;
            case "Wood":
                audioSource[1].clip = audioWood;
                break;
            case "Hit":
                audioSource[1].clip = audioHit;
                break;
            case "Fishing":
                audioSource[1].clip = audioFishing;
                break;
            case "FishingSuccess":
                audioSource[1].clip = audioFishingSuccess;
                break;
            case "Eat":
                audioSource[1].clip = audioEat;
                break;
            case "Water":
                audioSource[1].clip = audioWater;
                break; 
            case "Jump":
                audioSource[1].clip = audioJump;
                audioSource[1].volume = 0.3f;
                break;
            case "Helicopter":
                audioSource[1].clip = audioHelicopter;
                audioSource[1].loop = true;
                break;

        }
        audioSource[1].Play();
       //audioSource[1].volume = 1.0f;
    }
    void Move()
    {
        if (rDown) // run
        {
            anim.SetBool("Run_b", true);
            moveSpeed = 10.0f;
            anim_speed = 0.6f;

        }
        else // walk
        {
            anim.SetBool("Run_b", false);
            moveSpeed = 5.0f;
            anim_speed = 0.3f;

        }

        if (isGather || !isFireReady || isFishing)
        {
            transform.Translate(Vector3.zero);
        }
        else
        {
            vAxis *= moveSpeed * Time.deltaTime;
            hAxis *= moveSpeed * Time.deltaTime;
            transform.Translate(hAxis, 0, vAxis);
        }


        if (vAxis != 0 || hAxis != 0) // 움직이고 있어
        { // 걷, 뛰
            //Debug.Log("걷 뛰");
            if (moveSpeed <= 5.0f)
            {
                audioSource[0].clip = audioWalk;
                audioSource[0].pitch = 1.3f;
            }
            else
            {
                audioSource[0].clip = audioRun;
                audioSource[0].pitch = 1.0f;
            }
            anim.SetFloat("Speed_f", anim_speed);
            isMoving = true;
        }
        else
        {
            //Debug.Log("멈춰");
            anim.SetFloat("Speed_f", 0.0f);
            isMoving = false;
        }

        if (isMoving && isFireReady)
        {
            if (!audioSource[0].isPlaying)
                audioSource[0].Play();
        }
        else
            audioSource[0].Stop();

    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            //anim.SetTrigger("Jump_t");
            audioSource[1].volume = 0.3f;
            //PlayAudio("Jump");
            photonView.RPC("AniTrigger", RpcTarget.All, "Jump_t");
            float jumpPower;

            isJump = true;
            jumpPower = rDown ? 12f : 7f;
            float jumptime = rDown ? 0.9f : 0.7f;
            StartCoroutine(jumpTime(jumptime));
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    IEnumerator jumpTime(float time)
    {
        yield return new WaitForSeconds(time);
        isJump = false;
    }

    void Hit()
    {
        fireDelay += Time.deltaTime;
        damageDelay += Time.deltaTime; // 아래에 있는 적에게 맞을 때 시간을 일단 여기서 처리 ㅋㅋ
        isFireReady = 0.6f < fireDelay;

        if (fDown && isFireReady && isEquip && !inventory.isBuild && equippedItem == "Axe")
        {
            //anim.SetTrigger("Attack_t");
            photonView.RPC("AniTrigger", RpcTarget.All, "Attack_t");
            PlayAudio("Swing");
            fireDelay = 0;
        }
    }
    void Fishing()
    {
        if (!photonView.IsMine)
            return;
        if (fDown && isFireReady && isEquip && !inventory.isBuild && equippedItem == "Fishing") // 낚시
        {

            fishing = inst.gameObject.GetComponent<Fishing>(); // 낚시대에 있는 스크립트

            if (!isFishing) // 낚시 중이 아니면
            {
                //anim.ResetTrigger("CatchFish_t"); // 물고기 걸린 애니메이션 리셋
                photonView.RPC("AniResetTrigger", RpcTarget.All, "CatchFish_t");
                PlayAudio("Fishing"); // 낚시 오디오 재생
                //anim.SetTrigger("Fishing_t"); // 낚시 시작 애니메이션
                photonView.RPC("AniTrigger", RpcTarget.All, "Fishing_t");
                anim.SetBool("Fishing_b", true); // 낚시 중 애니메이션
                isFishing = true; // 낚시 중

                fishing.RandomFishing(); // RandomFishing 함수 호출

            }
            else // 낚시 중
            {
                anim.SetBool("Fishing_b", false); // 낚시 중 애니메이션 false
                isFishing = false;
                fishing.Invoke("finishFish", 4.0f); // 낚시 끝 함수 호출

                if (fishing.checkSuccess()) // 낚시 성공
                    FishingText.text = "Success!"; // Fishing 미니게임 text 변경
                else
                    FishingText.text = "Fail..";

            }
        }
        if (fishing != null && fishing.isCatch) // 물고기가 걸렸다면
        {
            PlayAudio("FishingSuccess");
            //anim.SetTrigger("CatchFish_t"); // 물고기 걸린 애니메이션 
            photonView.RPC("AniTrigger", RpcTarget.All, "CatchFish_t");
        }
    }

    void Gather()
    {
        if (gDown && !isGather)// && mItemToPickup != null
        {
            isGather = true;
            //anim.SetTrigger("Gather_t");
            photonView.RPC("AniTrigger", RpcTarget.All, "Gather_t");
            Invoke("GatherOut", 0.3f);

        }
    }

    void GatherOut()
    {
        isGather = false;
    }

    void CampFire()
    {
        if (isStartFire) // 불 지피기 시작
        {
            fireFill.fillAmount -= 0.00005f; // ui fill 내려가
        }

        // 주기 늘리기
        t_fire += Time.deltaTime;
        if (t_fire > 0.1f) // panel 깜빡임
        {
            t_fire = 0;
            Color color = CampFireTextPanel.color;
            color.a *= -1;
            CampFireTextPanel.color = color;
        }

        if (isStartFire && !isFinishFire) // 불지피기 시작했으면 성공 체크
        {
            CheckCampFire();
        }

        if (gDown && nearObject != null && nearObject.name == "campFire"
            && !isFinishFire
            && !nearObject.transform.GetChild(0).gameObject.activeSelf)
        { // 불이 안 붙어 있을 때, 불 지피기가 끝나지 않앗을 때
            if (!isStartFire && !isTrueTwig) // 첫 시작에 twig 확인
                CheckTwig();

            if (!isTrueTwig) // twig가 없으면
            {
                CantCampFire.SetActive(true);  // 나뭇가지 부족하다
                Invoke("DisappearCampFirePanel", 2.0f); // panel 비활성화
                return;
            }

            if (!isStartFire) // 첫 시작
            {
                fireFill.fillAmount = 0.1f;
                Debug.Log("시작");
                isStartFire = true;
                CampFirePanel.SetActive(true); // Panel 활성화
                campFireText.text = "'G' key press!!";
            }

            fireGame();
        }
    }

    void DisappearCampFirePanel() // panel 비활성화
    {
        CantCampFire.SetActive(false);
    }

    void CheckTwig() // 나뭇가지 갖고 있으면 1개 소모
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].item != null)
            {
                if (slot[i].item.itemName == "Twig")
                {
                    inst = Hand.transform.Find("Twig");
                    inst.gameObject.SetActive(true);
                    slot[i].SetSlotCount(-1);
                    isTrueTwig = true;
                }
            }
        }
    }

    void CheckCampFire() // 불 지피기 성공 실패 체크
    {
        if (fireFill.fillAmount >= 0.9f) // 불 지피기 성공
        {
            isStartFire = false;
            isSuccessedFire = true;
            campFireText.text = "성공!!";
            anim.speed = 1.5f;
            isFinishFire = true;
        }
        else if (fireFill.fillAmount <= 0.0f) // 불 지피기 실패
        {
            isStartFire = false;
            isSuccessedFire = false;
            campFireText.text = "실패..";
            anim.speed = 1.5f;
            isFinishFire = true;
        }

        if (isSuccessedFire)
        {

            Invoke("SuccessCampFire", 2.0f);
            return;
        }
        else if (fireFill.fillAmount <= 0.0f && !isSuccessedFire)
        {
            Invoke("FailCampFire", 2.0f);
            return;
        }
    }
    private void SuccessCampFire()
    {
        // campFire 관련 변수들 초기화
        isTrueTwig = false;
        inst.gameObject.SetActive(false);
        isSuccessedFire = false;
        CampFirePanel.SetActive(false); // 캠프 파이터 패널 비활성화
        ach.fireCount++; // 업적 +
        ach.Fire();
        photonView.RPC("ObjectSetActive", RpcTarget.All, nearObject.gameObject.GetComponent<PhotonView>().ViewID);
        //fireParticle = nearObject.transform.GetChild(0).gameObject;
        //fireParticle.SetActive(true); // 파티클 활성화
        isFinishFire = false;
    }

    [PunRPC]
    private void ObjectSetActive(int viewID)
    {
        var obj = PhotonView.Find(viewID);
        //obj.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        fireParticle = obj.gameObject.transform.GetChild(0).gameObject;
        //fireParticle.GetComponent<ParticleSystem>().Play();
        fireParticle.SetActive(true);
        fireParticle.GetComponent<ParticleSystem>().Play();

    }
    private void FailCampFire()
    {
        isTrueTwig = false;
        inst.gameObject.SetActive(false);
        isSuccessedFire = false;
        CampFirePanel.SetActive(false); // 캠프 파이터 패널 비활성화
        isFinishFire = false;
    }

    private void fireGame()
    {
        if (!isFire && !isSuccessedFire) // 한 번 비비기
        {
            fireFill.fillAmount += 0.1f; // 한 번 누르면 플러스
            anim.speed = 1.5f; // 애니메이션 play
                               // anim.SetTrigger("Fire_t");
            photonView.RPC("AniTrigger", RpcTarget.All, "Fire_t");
            isFire = true;

            Invoke("fireOut", 1.0f); // 1초 뒤 중단
        }
    }

    private void fireOut()
    {
        if (!isSuccessedFire) // 애니메이션 멈추기
            anim.speed = 0.0f;

        isFire = false;
    }

    void fireTimeManage()
    {
        if (fireParticle != null && fireParticle.activeSelf) // 불켜졌으면
        {
            timeFire += Time.deltaTime; // 시간 늘리기

            if (timeFire >= 10.0f) // 불 켜진지 10초 지났을 때
                photonView.RPC("objSetFalse", RpcTarget.All, fireParticle.GetComponent<PhotonView>().ViewID);
            //fireParticle.SetActive(false);
        }
    }
    [PunRPC]
    private void objSetFalse(int viewID)
    { //파티클 포톤뷰 넣기
        var obj = PhotonView.Find(viewID);
        obj.gameObject.SetActive(false);
        timeFire = 0;
        // obj.gameObject.GetComponent<ParticleSystem>().Stop();
    }

    public void Equip(GameObject equipItem) //장비 장착
    {
        if (!photonView.IsMine)
            return;
        equippedItem = equipItem.GetComponent<ItemPickUp>().item.itemName;
        isEquip = true;
        inst = Hand.transform.Find(equippedItem);
        // inst.gameObject.SetActive(true);
        photonView.RPC("setEquip", RpcTarget.All, inst.gameObject.GetComponent<PhotonView>().ViewID, true);

    }
    public void unEquip() // 장착 해제
    {
        if (!photonView.IsMine)
            return;
        isEquip = false;
        photonView.RPC("setEquip", RpcTarget.All, inst.gameObject.GetComponent<PhotonView>().ViewID, false);
        equippedItem = null;

    }
    //동기화
    [PunRPC]
    private void setEquip(int viewID, bool flag)
    {
        var obj = PhotonView.Find(viewID);
        obj.gameObject.SetActive(flag);
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        FreezeRotation();
    }
    private void OnCollisionEnter(Collision collision)
    {

        /*if (other.gameObject.tag == "Enemy")
        {
            // health.OnDamage(EnemyCtrl.damage); 
        }*/
        if (collision.gameObject.tag == "Enemy" && damageDelay >= 2.0f)
        {
            GameManager.Instance.p_hp -= 0.2f;
            hpFill.fillAmount = GameManager.Instance.p_hp;
            damageDelay = 0.0f;
            StartCoroutine(MainCamera.Shake(0.1f, 0.4f));
            //if (GameManager.Instance.p_hp <= 0.0f)
            //    Die();

        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Item")
        {
            nearObject = other.gameObject;
        }

        else if (other.gameObject.name == "campFire")
        {

            nearObject = other.gameObject;
        }

        //else if (other.gameObject.tag == "Water")
        //{
        //    nearObject = other.gameObject;
        //}

    }
    private void OnTriggerExit(Collider other)
    {
        nearObject = null;
    }


    void TryAction() // 아이템 주우려고 시도
    {
        if (gDown)
        {
            if (isSleep)
                StartCoroutine(SleepOut(beforeSleepTrans));
            else
            {
                CheckItem();
                CanItemAction();
            } }
    }

    void CheckItem() // 내 앞에 아이템 있는지 체크
    {
        //if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        if (nearObject != null)
        {
            if (nearObject.transform.tag == "Item") // ㅣ있으면 획득 가능 UI
                ItemInfoAppear();
            //else if (nearObject.transform.tag == "Water")
            //    ItemInfoAppear();

        }
        else
            ItemInfoDisappear(); // 없으면 UI 없애
    }

    void ItemInfoAppear()
    {

        Gather(); // Gather 검사
        pickupActivated = true;
        MessagePanel.gameObject.SetActive(true);
        //actionText.gameObject.SetActive(true);
        if (isSleep)
            actionText.text = "'G'를 눌러 일어나기";
        else if (nearObject.GetComponent<ItemPickUp>().item.itemName == "Bucket"
             && !nearObject.GetComponent<BucketWater>().isWater) // 물이 생성됐을 때
            actionText.text = "'G'를 눌러 물 마시기";
        else if (nearObject.GetComponent<ItemPickUp>().item.itemName == "Bed"
                 || nearObject.GetComponent<ItemPickUp>().item.itemName == "House")
        {
            actionText.text = "'G'를 눌러 잠자기";
        }

        else
            actionText.text = "<color=yellow>" + "'G'" + "<color=white>" + "키를 눌러 " + nearObject.transform.GetComponent<ItemPickUp>().item.itemName + "를 획득하기!" + "</color>";
    }

    void ItemInfoDisappear()
    {
        pickupActivated = false;
        MessagePanel.gameObject.SetActive(false);
        //actionText.gameObject.SetActive(false);
    }

    void CanItemAction()
    {
        if (pickupActivated) // 픽업 가능
        {
            if (nearObject.transform != null) // 앞에 아이템 있어
            {
                if (nearObject.GetComponent<ItemPickUp>().item.itemName == "Book")
                {
                    Destroy(nearObject.gameObject);
                    return;
                }
                // 물 마시기
                if (nearObject.GetComponent<ItemPickUp>().item.itemName == "Bucket"
                    && !nearObject.GetComponent<BucketWater>().isWater) // 물이 생성됐을 때
                {
                    Debug.Log("물마시기");
                    PlayAudio("Water"); // 물 먹는 소리
                    ach.waterCount++;
                    ach.Water();
                    GameManager.Instance.p_water = GameManager.Instance.IncreaseStat(GameManager.Instance.p_water, 0.2f);
                    waterFill.fillAmount = GameManager.Instance.p_water;
                    ItemInfoDisappear();
                    if (photonView.IsMine)
                    {
                        photonView.RPC("DestroyRPC", RpcTarget.Others, nearObject.transform.GetChild(1).gameObject.transform.gameObject.GetComponent<PhotonView>().ViewID);
                        PhotonNetwork.Destroy(nearObject.transform.GetChild(1).gameObject);
                    }
                    else
                        return;
                    BucketWater a = nearObject.GetComponent<BucketWater>(); //생성하라고 전달
                    a.isWater = true;

                    return;
                }

                // 잠자기
                if (nearObject.GetComponent<ItemPickUp>().item.itemName == "Bed"
                   || nearObject.GetComponent<ItemPickUp>().item.itemName == "House")
                {
                    isSleep = true; // 자는 중
                    beforeSleepTrans = this.transform;
                    if (nearObject.GetComponent<ItemPickUp>().item.itemName == "Bed")
                    {
                        // 잠 자는 애니메이션
                        this.transform.position = new Vector3(nearObject.transform.position.x - 1.0f, 3.0f, nearObject.transform.position.z + 3.0f);
                        this.transform.rotation = Quaternion.Euler(-90.0f, nearObject.transform.rotation.y, nearObject.transform.rotation.z);
                    }
                    else
                    {
                        // 잠 자는 애니메이션
                        this.transform.position = new Vector3(nearObject.transform.position.x, 1.3f, nearObject.transform.position.z + 0.5f);
                        this.transform.rotation = Quaternion.Euler(-90.0f, nearObject.transform.rotation.y, nearObject.transform.rotation.z);
                    }

                    ItemInfoDisappear();
                    return;
                }

                Debug.Log(nearObject.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                inventory.AcquireItem(nearObject.transform.GetComponent<ItemPickUp>().item); // 인벤토리에 저장
                                                                                             // Destroy(nearObject.transform.gameObject);
                if (photonView.IsMine)
                {
                    photonView.RPC("DestroyRPC", RpcTarget.Others, nearObject.transform.gameObject.GetComponent<PhotonView>().ViewID);
                    PhotonNetwork.Destroy(nearObject.gameObject);
                }
                else
                    return;
                ItemInfoDisappear();
            }
        }
    }

    private IEnumerator SleepOut(Transform trans)
    {
        isSleep = false;
        bedding.SetActive(false);
        // 눕기 전 위치로 
        this.transform.position = trans.position;
        this.transform.rotation = trans.rotation;

        yield return null;
    }

    private void Die()
    {
        audioSource[0].Stop();

        isDeath = true;

        HpWarningPanel.SetActive(true); // 빨간 화면 활성화
        photonView.RPC("AniTrigger", RpcTarget.All, "Death_t");
        // anim.SetTrigger("Death_t"); // 죽는 애니메이션
        Invoke("SetActiveDeathPanel", 2.0f); // death panel 활성화
    }

    private void SetActiveDeathPanel() // death panel 활성화
    {
        anim.speed = 0.0f; // 애니메이션 멈춤
        DeathPanel.SetActive(true);
        bedEffect = 1.0f;
    }

    public void Revival() // 부활
    {
        Debug.Log(HpWarningPanel);
        HpWarningPanel.SetActive(false); // 빨간 화면 비활성화
        anim.speed = 1.0f; // 애니메이션 재생
        isDeath = false;
        FX_Revival.Play(); // 파티클 플레이
    }

    void UpdatePlayerState() // 플레이어 상태 업데이트
    {
        if (GameManager.Instance.p_hp <= 0.0f && !isDeath)
            Die();
        hpFill.fillAmount = GameManager.Instance.p_hp; // 수분량

        if (GameManager.Instance.p_water <= 0.0f)
            GameManager.Instance.p_water = 0.0f;
        else
            GameManager.Instance.p_water
                = GameManager.Instance.IncreaseStat(GameManager.Instance.p_water, -0.0015f * Time.deltaTime);
        waterFill.fillAmount = GameManager.Instance.p_water; // 수분량

        if (GameManager.Instance.p_brain <= 0.0f)
            GameManager.Instance.p_brain = 0.0f;
        else
            GameManager.Instance.p_brain
                = GameManager.Instance.IncreaseStat(GameManager.Instance.p_brain, -0.001f * Time.deltaTime);
        brainFill.fillAmount = GameManager.Instance.p_brain; // 정신력

        if (GameManager.Instance.p_hunger <= 0.0f)
            GameManager.Instance.p_hunger = 0.0f;
        else
            GameManager.Instance.p_hunger
                 = GameManager.Instance.IncreaseStat(GameManager.Instance.p_hunger, -0.002f * Time.deltaTime);
        hungerFill.fillAmount = GameManager.Instance.p_hunger; // 배고픔
    }

    void HealingPlayerState()
    {
        if (GameManager.Instance.p_brain >= 1.0f)
            GameManager.Instance.p_brain = 1.0f;
        else
            GameManager.Instance.p_brain
                = GameManager.Instance.IncreaseStat(GameManager.Instance.p_brain, +0.005f * bedEffect * Time.deltaTime);
        brainFill.fillAmount = GameManager.Instance.p_brain; // 정신력

        if (GameManager.Instance.p_hp >= 1.0f)
            GameManager.Instance.p_hp = 1.0f;
        else
            GameManager.Instance.p_hp
                 = GameManager.Instance.IncreaseStat(GameManager.Instance.p_hp, +0.005f * Time.deltaTime);
        hpFill.fillAmount = GameManager.Instance.p_hp; // 체력
    }

    void CheckPlayerState() // hp 감소할 때 화면 깜빡임 효과
    {
        if (GameManager.Instance.p_water <= 0.0f || GameManager.Instance.p_brain <= 0.0f
            || GameManager.Instance.p_hunger <= 0.0f)
        {
            GameManager.Instance.p_hp
               = GameManager.Instance.IncreaseStat(GameManager.Instance.p_hp, -0.003f * Time.deltaTime);

            // 깜빡 깜빡
            t_hp += Time.deltaTime;
            if (t_hp > 0.6f) // panel 깜빡임
            {
                isSetHpWarning = !isSetHpWarning;
                HpWarningPanel.SetActive(isSetHpWarning);
                t_hp = 0;
            }
        }
    }

    void GameClear() // 모든 업적을 클리어하고 구조 신호를 만들었으면 게임 클리어
    {
        if ((ach.checkClearAll() && isBuildSos))
        {
            anim.SetBool("Dance_b", true); // 승리의 세레모니 애니메이션
            //isClearGame = true;
            //CearlPanel.SetActive(true); // 클리어 패널 활성화
            photonView.RPC("Clear", RpcTarget.All,Parent.GetComponent<PhotonView>().ViewID);
            PlayAudio("Helicopter");

        }
    }
    [PunRPC]
    private void Clear(int viewID)
    {
        var obj = PhotonView.Find(viewID);
        obj.gameObject.transform.GetChild(6).gameObject.SetActive(true);
        isClearGame = true;
    }

    [PunRPC]
    private void DestroyRPC(int object_id)
    {
        PhotonNetwork.Destroy(PhotonView.Find(object_id));
    }

    [PunRPC]
    private void AniTrigger(string ani)
    {
        GetComponent<Animator>().SetTrigger(ani);
    }
    [PunRPC]
    private void AniResetTrigger(string ani)
    {
        GetComponent<Animator>().ResetTrigger(ani);
    }
}
