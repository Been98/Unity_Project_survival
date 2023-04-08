using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Build Item", menuName = "New Item/builditem")]

public class BuildItem : ScriptableObject
{
    [TextArea(10, 14)] [SerializeField] string txtItemExplan;
    [SerializeField] int total_count;
    [SerializeField] int cost_tree;
    [SerializeField] int cost_bigtree;
    [SerializeField] int cost_stone;
    [SerializeField] int cost_vine;
    [SerializeField] Sprite img;
    public string GetItemExplan() { return txtItemExplan; }
    public int GetCostTree() { return cost_tree; }
    public int GetCostBigTree() { return cost_bigtree; }
    public int GetCostStone() { return cost_stone; }
    public int GetCostVine() { return cost_vine; }
    public int GetTotalCount() { return total_count; }
    public Sprite GetImg() { return img; }
}

