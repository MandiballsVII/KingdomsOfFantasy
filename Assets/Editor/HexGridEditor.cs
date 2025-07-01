using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    private void OnSceneGUI()
    {
        HexGrid hexGrid = (HexGrid)target;

        if (hexGrid == null)
        {
            Debug.LogError("HexGrid is not assigned.");
            return;
        }

        Handles.color = Color.green;

        for (int z = 0; z < hexGrid.height; z++)
        {
            for (int x = 0; x < hexGrid.width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(hexGrid.hexSize, x, z, hexGrid.orientation) + hexGrid.transform.position;

                int centerX = x;
                int centerZ = z;
                //Show the coordinates in a label
                Vector3 cubeCoord = HexMetrics.OffsetToCube(centerX, centerZ, hexGrid.orientation);
                Handles.Label(centrePosition + Vector3.forward * 0.5f, $"({centerX}, {centerZ}]");
                Handles.Label(centrePosition, $"({cubeCoord.x}, {cubeCoord.y}, {cubeCoord.z})");
            }
        }
    }
}
