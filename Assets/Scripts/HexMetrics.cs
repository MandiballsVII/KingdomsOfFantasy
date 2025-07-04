using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    /// <summary>
    /// Calculates the outer radius of a hexagon based on its size.
    /// </summary>
    /// <param name="hexSize"></param>
    /// <returns></returns>
    public static float OuterRadius(float hexSize)
    {
        return hexSize;
    }

    /// <summary>
    /// Gets the inner radious of a hexagon based on its size.
    /// </summary>
    /// <param name="hexSize"></param>
    /// <returns></returns>
    public static float InnerRadius(float hexSize)
    {
        //return hexSize * Mathf.Sqrt(3) / 2f; // sqrt(3) / 2 * hexSize
        return hexSize * 0.866025404f; // 1 / sqrt(3)
    }

    /// <summary>
    /// Get the corners of a hexagon based on its size and orientation.
    /// </summary>
    /// <param name="hexSize"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
    public static Vector3[] Corners(float hexSize, HexOrientation orientation)
    {
        Vector3[] corners = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            corners[i] = Corner(hexSize, orientation, i);
        }
        return corners;
    }

    /// <summary>
    /// Get a specific corner of a hexagon based on its size and orientation.
    /// </summary>
    /// <param name="hexSize"></param>
    /// <param name="orientation"></param>
    /// <param name="index"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get the center position of a hexagon based on its size, coordinates, and orientation.
    /// </summary>
    /// <param name="hexSize"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Converts cube coordinates to axial coordinates for pointy-topped hexagons.
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector2 CubeToAxial(Vector3 cube)
    {
        return new Vector2(cube.x, cube.y);
    }

    /// <summary>
    /// Converts cube coordinates to axial coordinates.
    /// Cube coordinates calculate the S value from the Q and R values.
    /// </summary>
    /// <param name="q"></param>
    /// <param name="r"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public static Vector2 CubeToAxial(float q, float r, float s)
    {
        return new Vector2(q, r);
    }

    /// <summary>
    /// Converts cube coordinates to axial coordinates.
    /// Cube coordinates calculate the S value from the Q and R values.
    /// </summary>
    /// <param name="q"></param>
    /// <param name="r"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public static Vector2 CubeToAxial(int q, int r, int s)
    {
        return new Vector2(q, r);
    }

    /// <summary>
    /// Converts offset coordinates to axial coordinates for pointy-topped hexagons.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
    public static Vector2 OffsetToAxial(int x, int z, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return OffsetToAxialPointy(x, z);
        }
        else
        {
            return OffsetToAxialFlat(x, z);
        }
    }
    public static Vector3 OffsetToCube(int col, int row, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return AxialToCube(OffsetToAxialPointy(col, row));
        }
        else
        {
            return AxialToCube(OffsetToAxialFlat(col, row));
        }
    }
    public static Vector3 AxialToCube(Vector2Int axial)
    {
        float x = axial.x;
        float z = axial.y;
        float y = -x - z;
        return new Vector3(x, z, y);
    }

    /// <summary>
    /// Converts offset coordinates to axial coordinates.
    /// Axial coordinates are used for simplified calculations.
    /// Axial coordinates loose the S value of the cube coordinates.
    /// </summary>
    /// <param name="q"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Vector3 AxialToCube(float q, float r)
    {
        return new Vector3(q, r, -q - r);
    }

    /// <summary>
    /// Converts offset coordinates to axial coordinates.
    /// Axial coordinates are used for simplified calculations.
    /// Axial coordinates loose the S value of the cube coordinates.
    /// </summary>
    /// <param name="q"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Vector3 AxialToCube(int q, int r)
    {
        return new Vector3(q, r, -q - r);
    }

    /// <summary>
    /// Converts offset coordinates to axial coordinates.
    /// Axial coordinates are used for simplified calculations.
    /// Axial coordinates loose the S value of the cube coordinates.
    /// </summary>
    /// <param name="axialCoord"></param>
    /// <returns></returns>
    public static Vector3 AxialToCube(Vector2 axialCoord)
    {
        return AxialToCube(axialCoord.x, axialCoord.y);
    }

    /// <summary>
    /// Converts offset coordinates to axial coordinates for a flat orientation.
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public static Vector2Int OffsetToAxialFlat(int col, int row)
    {
        int q = col;
        int r = row - (col + (col & 1)) / 2;
        return new Vector2Int(q, r);
    }


    /// <summary>
    /// Converts offset coordinates to axial coordinates for a pointy orientation.
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public static Vector2Int OffsetToAxialPointy(int col, int row)

    {
        int q = col - (row + (row & 1)) / 2;
        int r = row;
        return new Vector2Int(q, r);
    }

    /// <summary>
    /// Converts cube coordinates to offset coordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
    public static Vector2 CubeToOffset(int x, int y, int z, HexOrientation orientation)
    {
        if (orientation == HexOrientation.PointyTop)
        {
            return CubeToOffsetPointy(x, y, z);
        }
        else
        {
            return CubeToOffsetFlat(x, y, z);
        }
    }


    /// <summary>
    /// Converts cube coordinates to offset coordinates.
    /// </summary>
    public static Vector2 CubeToOffset(Vector3 offsetCoord, HexOrientation orientation)
    {
        return CubeToOffset((int)offsetCoord.x, (int)offsetCoord.y, (int)offsetCoord.z, orientation);
    }

    /// <summary>
    /// Converts cube coordinates to offset coordinates for a pointy orientation.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private static Vector2 CubeToOffsetPointy(int x, int y, int z)
    {
        Vector2 offsetCoordinates = new Vector2(x + (y - (y & 1)) / 2, y);
        return offsetCoordinates;
    }

    /// <summary>
    /// Converts cube coordinates to offset coordinates for a flat orientation.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private static Vector2 CubeToOffsetFlat(int x, int y, int z)
    {
        Vector2 offsetCoordinates = new Vector2(x, y + (x - (x & 1)) / 2);
        return offsetCoordinates;
    }

    /// <summary>
    /// Rounds the cube coordinates to the nearest hexagon center.
    /// </summary>
    /// <param name="frac"></param>
    /// <returns></returns>
    private static Vector3 CubeRound(Vector3 frac)
    {
        Vector3 roundedCoordinates = new Vector3();
        int rx = Mathf.RoundToInt(frac.x);
        int ry = Mathf.RoundToInt(frac.y);
        int rz = Mathf.RoundToInt(frac.z);
        float xDiff = Mathf.Abs(rx - frac.x);
        float yDiff = Mathf.Abs(ry - frac.y);
        float zDiff = Mathf.Abs(rz - frac.z);
        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }
        roundedCoordinates.x = rx;
        roundedCoordinates.y = ry;
        roundedCoordinates.z = rz;
        return roundedCoordinates;
    }

    /// <summary>
    /// Rounds the axial coordinates to the nearest hexagon center.
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public static Vector2 AxialRound(Vector2 coordinates)
    {
        return CubeToAxial(CubeRound(AxialToCube(coordinates.x, coordinates.y)));
    }

    /// <summary>
    /// Converts a point in space to the nearest hexagon center.
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public static Vector2 CoordinateToAxial(float x, float z, float hexSize, HexOrientation orientation)
    {
        if(orientation == HexOrientation.PointyTop)
        {
            return CoordinateToPointyAxial(x, z, hexSize);
        }
        else // Flat-topped
        {
            return CoordinateToFlatAxial(x, z, hexSize);
        }
    }

    /// <summary>
    /// Helper function for CoordinateToAxial
    /// It gets a fractional axial coordinate from a point in space for a pointly top orientation.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="hexSize"></param>
    /// <returns></returns>
    private static Vector2 CoordinateToFlatAxial(float x, float z, float hexSize)
    {
        Vector2 pointlyHexCoordinates = new Vector2();
        pointlyHexCoordinates.x = (Mathf.Sqrt(3) / 3 * x - 1f / 3 * z) / hexSize;
        pointlyHexCoordinates.y = (2f / 3 * z) / hexSize;

        return AxialRound(pointlyHexCoordinates);
    }

    /// <summary>
    /// Helper function for CoordinateToAxial
    /// It gets a fractional axial coordinate from a point in space for a flat top orientation.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="hexSize"></param>
    /// <returns></returns>
    private static Vector2 CoordinateToPointyAxial(float x, float z, float hexSize)
    {
        Vector2 flatHexCoordinates = new Vector2();
        flatHexCoordinates.x = (2f / 3 *x) / hexSize;
        flatHexCoordinates.y = (-1 / 3 * x + Mathf.Sqrt(3) / 3 * z) / hexSize;

        return AxialRound(flatHexCoordinates);
    }

    public static Vector2 CoordinateToOffset(float x, float z, float hexSize, HexOrientation orientation)
    {
        return CubeToOffset(AxialToCube(CoordinateToAxial(x, z, hexSize, orientation)), orientation);
    }

}
