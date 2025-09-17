using System.Collections.Generic;
using UnityEngine;

public static class LaneData
{
    public static readonly Dictionary<Lane, float> Lanes = new Dictionary<Lane, float>
    {
        { Lane.Left, -1.5f },
        { Lane.Middle, 0f },
        { Lane.Right, 1.5f }
    };
}

public enum Lane
{
    Left = 0,
    Middle = 1,
    Right = 2
}