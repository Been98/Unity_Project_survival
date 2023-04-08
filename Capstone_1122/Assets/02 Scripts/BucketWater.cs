using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketWater : MonoBehaviour
{
    //0.06에서 0.28까지
    //물 마시면 isWater= true로 만들고 코루틴 다시 부르기
    // isWater가 false일 경우만 물을 get할 수 있음
    [SerializeField] private GameObject water;
    public bool isWater = true;
    private float GetwaterY;
    private Vector3 T_water;
    private GameObject Ins_water; //생성된 프리팹
    private float plusWaterHigh = 0.14f;

    public bool isBuildFinish = false;

    //물의 높이 결정 물이 꽉 차면 get하도록 설정
    public IEnumerator SetWaterHigh()
    {
        T_water = this.gameObject.transform.position;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (isWater)
            {
                Instantiate(water, T_water, Quaternion.identity, transform);
                isWater = false;
                Ins_water = transform.GetChild(0).gameObject;
                GetwaterY = Ins_water.transform.position.y;
            }
            else if (Ins_water.transform.position.y < GetwaterY + plusWaterHigh && Ins_water.activeSelf)
            {
                Ins_water.transform.Translate(0, 0.01f, 0);
            }
            else if (Ins_water.transform.position.y >= GetwaterY + plusWaterHigh && Ins_water.activeSelf)
            {
                Ins_water.gameObject.tag = "Water";
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Rain" && isBuildFinish) // 비가 오는 곳
        {
            StartCoroutine(SetWaterHigh());
            isBuildFinish = false;
        }
    }
}
