using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Vector3Int gridSize;

    [SerializeField]
    private Vector3Int subGridSize;

    [SerializeField]
    private Grid grid = new Grid(Vector3.one, Vector3.zero);

    [SerializeField]
    private Grid subGrid = new Grid(Vector3.one);

    [SerializeField]
    private List<Chunk> chunks = new List<Chunk>();

    private void Start()
    {
        CreateMap();
    }

    public void CreateMap()
    {
        var max = grid.MaxIndex(gridSize);

        for (int z = 0; z <= max.z; z++)
        {
            for (int y = 0; y <= max.y; y++)
            {
                for (int x = 0; x <= max.x; x++)
                {
                    CreateChunk(new Vector3Int(x, y, z));
                }
            }
        }
    }

    private void CreateChunk(Vector3Int index)
    {
        var chunk = new Chunk(index);

        subGrid.origin = grid.IndexToPosition(index);
        var max = subGrid.MaxIndex(subGridSize);

        for (int z = 0; z <= max.z; z++)
        {
            for (int y = 0; y <= max.y; y++)
            {
                for (int x = 0; x <= max.x; x++)
                {
                    var pos = subGrid.IndexToPosition(new Vector3Int(x, y, z));

                    var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    block.transform.position = pos;
                    
                    chunk.Blocks.Add(block);
                }
            }
        }
        
        chunks.Add(chunk);

        subGrid.origin = Vector3.zero;
    }
}