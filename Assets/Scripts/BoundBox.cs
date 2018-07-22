using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// A bounding box on the x-z Surface
/// </summary>
public class BoundBox : MonoBehaviour
{
    public Vector2[] corners;

    private void Start()
    {
        corners = new Vector2[4];
    }

    private void test()
    {
        corners[0] = new Vector2(0, 0);
        corners[1] = new Vector2(1, 0);
        corners[2] = new Vector2(1, 1);
        corners[3] = new Vector2(0, 1);

        Vector2 trueP1 = new Vector2(0.5f, 0.5f);
        Vector2 falseP1 = new Vector2(2, 0);
        Vector2 falseP2 = new Vector2(1, 0);

        bool trueB1 = isInside(trueP1);
        bool falseB1 = isInside(falseP1);
        bool falseB2 = isInside(falseP2);

        bool res = trueB1 && !falseB1 && !falseB2;

        Debug.Log(res);
       
    }

    public bool isInside(BoundBox boundBox)
    {
        foreach (Vector2 c in boundBox.corners)
        {
            if (!isInside(c))
            {
                return false;
            }
        }
        return false;
    }

    public bool isInside(Vector2 point)
    {

        for (int i = 0; i < 4; i++)
        {
            Vector2 edge = corners[(i + 1) % 4] - corners[i % 4];
            Vector2 orthogonal = new Vector2(-edge.y, edge.x);

            float scalarPr = Vector2.Dot(orthogonal, point - corners[i % 4]);
            if (scalarPr <= 0)
            {
                return false;
            }
        }
        return true;
    }

    private Vector2 getOrtho(Vector2 original)
    {
        //To create a perpendicular vector switch X and Y, then make Y negative
        float x = original.x;
        float y = original.y;

        y = -y;

        return new Vector2(y, x);
    }
}
