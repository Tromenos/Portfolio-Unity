using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Grid
{
    public Vector3 cellSize;
    public Vector3 cellExtents;
    public Vector3 origin;

    public Grid(Vector3 cellSize)
    {
        this.cellSize = cellSize;
        cellExtents   = cellSize * 0.5f;
        origin        = Vector3.zero;
    }

    public Grid(Vector3 cellSize, Vector3 origin)
    {
        this.cellSize = cellSize;
        cellExtents   = cellSize * 0.5f;
        this.origin   = origin;
    }

    public Vector3Int PositionToIndex(Vector3 position)
    {
        position -= origin;

        int x = Mathf.FloorToInt((position.x / cellSize.x));
        int y = Mathf.FloorToInt((position.y / cellSize.y));
        int z = Mathf.FloorToInt((position.z / cellSize.z));

        return new Vector3Int(x, y, z);
    }

    public Vector3 IndexToPosition(Vector3Int index)
    {
        var pos = new Vector3(index.x * cellSize.x, index.y * cellSize.y, index.z * cellSize.z);

        return pos + origin;
    }

    public Vector3 GetCellCenter(Vector3Int index)
    {
        return IndexToPosition(index) + cellExtents;
    }

    public Vector3 GetCellCenterXY(Vector3Int index)
    {
        var pos = IndexToPosition(index);

        pos.x += cellExtents.x;
        pos.y += cellExtents.y;

        return pos;
    }

    public Vector3 GetCellCenterXZ(Vector3Int index)
    {
        var pos = IndexToPosition(index);

        pos.x += cellExtents.x;
        pos.z += cellExtents.z;

        return pos;
    }

    public Vector3 GetCellCenterYZ(Vector3Int index)
    {
        var pos = IndexToPosition(index);

        pos.y += cellExtents.y;
        pos.z += cellExtents.z;

        return pos;
    }

    public Vector3Int MaxIndex(Vector3Int count)
    {
        count -= Vector3Int.one;
        var maxPos = new Vector3((cellSize.x * count.x), (cellSize.y * count.y), (cellSize.z * count.z)) + origin;
        return PositionToIndex(maxPos);
    }
}