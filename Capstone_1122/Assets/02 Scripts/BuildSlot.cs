using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildSlot : MonoBehaviour
{
    [SerializeField] GameObject panelMix;
    [SerializeField] TextMeshProUGUI txtItemExpanation;//아이템 설명
    [SerializeField] Image ItemImage; //선택되면 바뀔 이미지
    [SerializeField] public BuildItem buildItem;
    [SerializeField] Image[] mixIngredient;//재료가 들어갈 이미지 공간 3개
    [SerializeField] Sprite[] mixImage; //재료 이미지   0:나뭇가지, 1:돌, 2:나무줄기, 3:통나무
    [SerializeField] TextMeshProUGUI[] mixCount; //재료칸 필요한 개수 
    public Item item;
    private Preference preference;


    // Start is called before the first frame update
    void Start()
    {
        preference = transform.parent.parent.GetComponent<Preference>();
    }

    public void BtnOnClick()
    {
        preference.buildSlot = this;
        for (int i = 0; i < mixIngredient.Length; i++)
        {
            mixIngredient[i].gameObject.SetActive(false);
        }
        txtItemExpanation.text = buildItem.GetItemExplan();//item설명 text바꾸고 .text = "AA"
        ItemImage.sprite = buildItem.GetImg();
        panelMix.SetActive(true);
        setImage();
    }

    //이미지 3칸에 개수랑 이미지 변환 넣기
    private void setImage()
    {
        int cost_tree = buildItem.GetCostTree();
        int cost_stone = buildItem.GetCostStone();
        int cost_vine = buildItem.GetCostVine();
        int cost_bigtree = buildItem.GetCostBigTree();
        for (int i = 0; i < buildItem.GetTotalCount(); i++)
        {
            mixIngredient[i].gameObject.SetActive(true);
            if (cost_tree != 0)
            {
                mixIngredient[i].sprite = mixImage[0];
                mixCount[i].text = cost_tree.ToString();
                cost_tree = 0;
                continue;
            }
            else if (cost_stone != 0)
            {
                mixIngredient[i].sprite = mixImage[1];
                mixCount[i].text = cost_stone.ToString();
                cost_stone = 0;
                continue;
            }
            else if (cost_vine != 0)
            {
                mixIngredient[i].sprite = mixImage[2];
                mixCount[i].text = cost_vine.ToString();
                cost_vine = 0;
                continue;
            }
            else if (cost_bigtree != 0)
            {
                mixIngredient[i].sprite = mixImage[3];
                mixCount[i].text = cost_bigtree.ToString();
                cost_bigtree = 0;
                continue;
            }
        }
    }

}
