using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Chunk
{
    public Vector3Int Index;

    public List<GameObject> Blocks;

    public Chunk(Vector3Int index)
    {
        Index = index;
        Blocks = new List<GameObject>();
    }
}