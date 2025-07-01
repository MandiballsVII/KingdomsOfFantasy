using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public static float OuterRadius(float hexSize)
    {
        return hexSize;
    }
    public static float InnerRadius(float hexSize)
    {
        //return hexSize * Mathf.Sqrt(3) / 2f; // sqrt(3) / 2 * hexSize
        return hexSize * 0.866025404f; // 1 / sqrt(3)
    }
    public static Vector3[] Corners(float hexSize, HexOrientation orientation)
    {
        Vector3[] corners = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            corners[i] = Corner(hexSize, orientation, i);
        }
        return corners;
    }

    public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
    {
        float angle = 60f * index;
        if (orientation == HexOrientation.PointyTop)
        {
            angle += 30f; // Offset for flat top hexagons
        }
        Vector3 corner = new Vector3(hexSize * Mathf.Cos(angle * Mathf.Deg2Rad), 0f, hexSize * Mathf.Sin(angle * Mathf.Deg2Rad));
        return corner;
    }
    //public static Vector3 Center(float hexSize, int x, int z, HexOrientation orientation)
    //{
    //    Vector3 centrePosition;
    //    if(orientation == HexOrientation.PointyTop)
    //    {
    //        centrePosition.x = (x + z * 0.5f - z / 2) * (InnerRadius(hexSize) + 2f);
    //        centrePosition.y = 0f;
    //        centrePosition.z = z * (OuterRadius(hexSize) + 1.5f);
    //    }
    //    else
    //    {
    //        centrePosition.x = x * (OuterRadius(hexSize) + 1.5f);
    //        centrePosition.y = 0f;
    //        centrePosition.z = (z + x * 0.5f - x / 2) * (InnerRadius(hexSize) + 2f);
    //    }
    //    return centrePosition;
    //}
    public static Vector3 Center(float hexSize, int x, int z, HexOrientation orientation)
    {
        Vector3 centrePosition = Vector3.zero;

        if (orientation == HexOrientation.PointyTop)
        {
            float width = InnerRadius(hexSize) * 2f;
            float height = OuterRadius(hexSize) * 2f;
            float horizSpacing = width;
            float vertSpacing = height * 0.75f;

            centrePosition.x = x * horizSpacing + (z % 2 == 0 ? 0 : horizSpacing / 2f);
            centrePosition.z = z * vertSpacing;
        }
        else // Flat-topped
        {
            float width = OuterRadius(hexSize) * 2f;
            float height = InnerRadius(hexSize) * 2f;
            float horizSpacing = width * 0.75f;
            float vertSpacing = height;

            centrePosition.x = x * horizSpacing;
            centrePosition.z = z * vertSpacing + (x % 2 == 0 ? 0 : vertSpacing / 2f);
        }

        return centrePosition;
    }
}
