using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadProperties : MonoBehaviour
{
    public float maxSpeed;
    public int direction;

    public void setMaxSpeed(float s)
    {
        maxSpeed = s;
    }

    public void setDirection(int d)
    {
        direction = d;
    }
}
