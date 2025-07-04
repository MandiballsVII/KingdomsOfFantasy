using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexGridMeshGenerator : MonoBehaviour
{
    [field: SerializeField] public LayerMask gridLayer { get; private set; }
    [field: SerializeField] public HexGrid hexGrid { get; private set; }
    public Transform explosionTest;

    private void Awake()
    {
        if (hexGrid == null)
        {
            hexGrid = GetComponentInParent<HexGrid>();
            Debug.LogError("HexGrid is not assigned. Attempting to find HexGrid component on the GameObject.");
        }

    }
    private void OnEnable()
    {
        MouseController.instance.OnLeftMouseClick += OnLeftMouseClick;
        MouseController.instance.OnRightMouseClick += OnRightMouseClick;
    }

    private void OnLeftMouseClick(RaycastHit hit)
    {
        Debug.Log("Hit object: " + hit.transform.name + " at position: " + hit.point);
        float localX = hit.point.x - hit.transform.position.x;
        float localZ = hit.point.z - hit.transform.position.z;
        Debug.Log("Offset position: " + HexMetrics.CoordinateToOffset(localX, localZ, hexGrid.hexSize, hexGrid.orientation));
    }

    private void OnRightMouseClick(RaycastHit hit)
    {
        float localX = hit.point.x - hit.transform.position.x;
        float localZ = hit.point.z - hit.transform.position.z;

        Vector2 location = HexMetrics.CoordinateToOffset(localX, localZ, hexGrid.hexSize, hexGrid.orientation);
        Vector3 center = HexMetrics.Center(hexGrid.hexSize, (int)location.x, (int)location.y, hexGrid.orientation);
        Debug.Log("Right clicked on hex: " + location);
        Instantiate(explosionTest, center, Quaternion.identity);
    }

    private void OnDisable()
    {
        MouseController.instance.OnLeftMouseClick -= OnLeftMouseClick;
        MouseController.instance.OnRightMouseClick -= OnRightMouseClick;
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
                for (int s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    vertices[(z * width + x) * 7 + s + 1] = centrePosition + HexMetrics.Corners(hexSize, orientation)[s % 6];
                }
            }
        }

        int[] triangles = new int[3 * 6 * width * height];
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    int next = (s + 1) % 6;
                    triangles[3 * 6 * (z * width + x) + s * 3 + 0] = (z * width + x) * 7;         // centro
                    triangles[3 * 6 * (z * width + x) + s * 3 + 1] = (z * width + x) * 7 + s + 1; // esquina actual
                    triangles[3 * 6 * (z * width + x) + s * 3 + 2] = (z * width + x) * 7 + next + 1; // esquina siguiente
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
