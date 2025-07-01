using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum HexOrientation
    {
        FlatTop,
        PointyTop
    }

public class HexGrid : MonoBehaviour
{
    [field:SerializeField] public HexOrientation orientation = HexOrientation.FlatTop;
    [field: SerializeField] public int width { get; private set; }
    [field: SerializeField] public int height { get; private set; }
    [field: SerializeField] public int hexSize { get; private set; }
    [field: SerializeField] public GameObject hexPrefab { get; private set; }

    private void OnDrawGizmos()
    {
        for(int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(hexSize, x, z, orientation) + transform.position;
                for(int s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    Gizmos.DrawLine(
                        centrePosition + HexMetrics.Corners(hexSize, orientation)[s % 6],
                        centrePosition + HexMetrics.Corners(hexSize, orientation)[(s + 1) % 6]
                        );
                }
            }
        }
    }
}

