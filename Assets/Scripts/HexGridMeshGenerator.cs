using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexGridMeshGenerator : MonoBehaviour
{
    [field: SerializeField] public LayerMask gridLayer { get; private set; }
    [field: SerializeField] public HexGrid hexGrid { get; private set; }

    private void Awake()
    {
        if (hexGrid == null)
        {
            hexGrid = GetComponent<HexGrid>();
            Debug.LogError("HexGrid is not assigned. Attempting to find HexGrid component on the GameObject.");
        }

    }
    public void CreateHexMesh()
    {
        CreateHexMesh(hexGrid.width, hexGrid.height, hexGrid.hexSize, hexGrid.orientation, gridLayer);
    }

    public void CreateHexMesh(HexGrid hexGrid, LayerMask layerMask)
    {
        this.hexGrid = hexGrid;
        this.gridLayer = layerMask;
        CreateHexMesh(hexGrid.width, hexGrid.height, hexGrid.hexSize, hexGrid.orientation, layerMask);
    }

    public void CreateHexMesh(int width, int height, float hexSize, HexOrientation orientation, LayerMask layerMask)
    {
        ClearHexGridMesh();
        Vector3[] vertices = new Vector3[7 * width * height];
        for(int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(hexSize, x, z, orientation);
                vertices[(z * width + x) * 7] = centrePosition; // Center vertex
                //Vector3[] corners = HexMetrics.Corners(hexSize, orientation);
                for (int s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    vertices[(z * width + x) * 7 + s + 1] = centrePosition + HexMetrics.Corners(hexSize, orientation)[s % 6];
                }
                vertices[(z * width + x) * 7 + 6] = centrePosition; // Center vertex
            }
        }

        int[] triangles = new int[3 * 6 * width * height];
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    int cornerIndex = s + 2 > 6 ? s + 2 - 6 : s + 2; // Wrap around the corners
                    triangles[3 * 6 * (z * width + x) + s * 3 + 0] = (z * width + x) * 7; // Center vertex
                    triangles[3 * 6 * (z * width + x) + s * 3 + 1] = (z * width + x) * 7 + s + 1; // First corner vertex
                    triangles[3 * 6 * (z * width + x) + s * 3 + 2] = (z * width + x) * 7 + cornerIndex;
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Hex Mesh";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        mesh.RecalculateUVDistributionMetrics();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        int gridLayerIndex = GetLayerIndex(layerMask);
        Debug.Log("Layer Index: " + gridLayerIndex);
        
        gameObject.layer = gridLayerIndex;
    }

    public void ClearHexGridMesh()
    {
        if (GetComponent<MeshFilter>().sharedMesh == null)
            return;
        GetComponent<MeshFilter>().sharedMesh.Clear();
        GetComponent<MeshCollider>().sharedMesh.Clear();
    }

    private int GetLayerIndex(LayerMask layerMask)
    {
        int layerMaskValue = layerMask.value;
        Debug.Log("Layer Mask Value: " + layerMaskValue);
        for(int i = 0; i < 32; i++)
        {
            if(((1 << i) & layerMaskValue) != 0)
            {
                Debug.Log("Layer index loop: " + i);
                return i; // Return the index of the first set bit
            }
        }
        return 0; // Default to 0 if no layer is set
    }
}
