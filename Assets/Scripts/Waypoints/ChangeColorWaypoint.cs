using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorWaypoint : MonoBehaviour
{

    public Color setColor;
    private Renderer rend;
    private Color originalColor;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void resetColor()
    {
        rend.material.color = originalColor;
    }

    internal void applyColor()
    {
        rend.material.color = setColor;
    }
}
