using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public enum Lane
    {
        left,
        middle,
        right
    }
    public Lane lane;

    public int position;
}