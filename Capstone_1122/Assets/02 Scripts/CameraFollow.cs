using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 pos_offset; // 보정값
    public float damping = 1;
    Vector3 targetPos;

    private bool isShake = false;

    float rotSpeed = 10.0f;
    float x_rot = -2f; // 카메라 조정

    Quaternion targetRotation;


    public void init()
    {
        pos_offset = -new Vector3(0f, 8.5f, -10f);
    }

    void LateUpdate()
    {
            if (!target.gameObject.GetComponent<Player>().isSleep) // 자고 있지 않을 때만
                FixRotation();
            if (!isShake && !target.GetComponent<Player>().isDeath && !target.gameObject.GetComponent<Player>().isSleep)
            {
                float horizontal = Input.GetAxis("Mouse X") * rotSpeed;
                target.Rotate(0, horizontal, 0);


                float desiredAngle = target.eulerAngles.y;

                Quaternion rotation = Quaternion.Euler(x_rot, desiredAngle, 0);
                transform.position = target.position - (rotation * pos_offset);

                targetPos = target.position + new Vector3(0, 2.5f, 0); // 카메라 조정 위로 // 2.5
                transform.LookAt(targetPos + new Vector3(0.0f, 2.0f, 0.0f));
            }
        
    }

    //Player에서 맞을 때 부르는 함수 (시간, 범위)
    public IEnumerator Shake(float duration, float ShakeArrange)
    {
        isShake = true;
        float time = 0.0f;

        while (time < duration)
        {

            float posX = Random.Range(-1f, 1f) * ShakeArrange;
            float posY = Random.Range(-1f, 1f) * ShakeArrange;

            this.transform.position = this.transform.position + new Vector3(posX, posY, 0f);

            time += Time.deltaTime;

            yield return null;
        }


        isShake = false;
    }

    private void FixRotation()
    {
        target.rotation = new Quaternion(0.0f, target.rotation.y, 0.0f, target.rotation.w);
    }
}
