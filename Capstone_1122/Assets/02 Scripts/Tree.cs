using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private Player player;

    public GameObject[] twigs;
    Vector3[] tiwgPositions = new Vector3[3];
    int treeCount;
    int t_index = -1;
    int t_count = 0;

    void Awake()
    {

        //player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Invoke("initTree", 10f);
        tiwgPositions[0] = this.transform.position + new Vector3(0.74f, 5.0f, -1.34f);
        tiwgPositions[1] = this.transform.position + new Vector3(-1.6f, 5.0f, 0.34f);
        tiwgPositions[2] = this.transform.position + new Vector3(0.21f, 5.0f, 0.754f);
        treeCount = Random.Range(3, 10);
    }
    private void initTree()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        destroy();
    }

    void destroy()
    {
        if(treeCount <= 0 && t_count < tiwgPositions.Length)
        {
            t_index = Random.Range(0, twigs.Length);

            Instantiate(twigs[t_index], tiwgPositions[t_count], twigs[t_index].transform.rotation);

            t_count++;
        }

        else if(t_count == tiwgPositions.Length){
            Destroy(gameObject);
        }
    }
    IEnumerator shakeTree()
    {
        transform.Rotate(4, 0, -4);
        yield return new WaitForSeconds(0.02f);
        transform.Rotate(4, 0, 4);
        yield return new WaitForSeconds(0.02f);
        transform.Rotate(-4, 0, -4);
        yield return new WaitForSeconds(0.02f);
        transform.Rotate(-4, 0, 4);
        yield return new WaitForSeconds(0.02f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    private void OnTriggerEnter(Collider other)
    {
        ItemPickUp itempickup = other.GetComponent<ItemPickUp>();
        if (itempickup != null) {
            if (itempickup.item.itemName == "Axe" && !player.isFireReady)
            {
                player.PlayAudio("Wood");
                treeCount--;
                StartCoroutine("shakeTree");

            }
        }
    }


}
