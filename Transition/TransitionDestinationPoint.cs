using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestinationPoint : MonoBehaviour
{
    public enum DestinationTag //终点的标签
    {
        ENTER,//场景的入口
        A,
        B,
        C,
    }
    public DestinationTag destionationTag;
}
