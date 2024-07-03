using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 给每个敌人分配不同路线
/// </summary>
public class WayPointManager : MonoBehaviour
{
    private static WayPointManager _instance;

    /* 属性封装 */
    public static WayPointManager Instance
    {
        get
        {
        return _instance;
        }
    }

    //用2个 List 随机生成不同路线赋给敌人，防止敌人出现走同一路线的情况
    //相当于给每个敌人分配不同的路线ID
    public List<int> usingIndex = new List<int>();//每个敌人分配用到的路线ID
    public List<int> rawIndex = new List<int>();//辅助的 List， 将0， 1， 2打乱，重新分配

    private void Awake()
    {
        _instance = this;
        //分配路线ID
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);//遵循左闭右开, 随机3条路线的位置
            usingIndex.Add(rawIndex[tempIndex]);//分配路线
            rawIndex.RemoveAt(tempIndex);//分配路线之后，删除编号（防止重复）
        }
    }
}
