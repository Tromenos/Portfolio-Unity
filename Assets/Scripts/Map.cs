using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Tooltip("X is for the wait between chunks.\nY is for the wait between blocks.")]
    [SerializeField]
    private Vector2 routineWaits = new Vector2();
    [SerializeField]
    private Material[] materials = null;
    
    [SerializeField]
    private Vector3Int chunkCount = Vector3Int.zero;

    [SerializeField]
    private Vector3Int blockCount = Vector3Int.zero;

    [SerializeField]
    private Grid chunkGrid = new Grid(Vector3.one, Vector3.zero);

    [SerializeField]
    private Grid blockGrid = new Grid(Vector3.one);

    [SerializeField]
    private List<Chunk> chunks = new List<Chunk>();

    private IEnumerator _chunkWait;
    private IEnumerator _blockWait;

    private void Awake()
    {
        _chunkWait = new WaitForSecondsRealtime(routineWaits.x);
        _blockWait = new WaitForSecondsRealtime(routineWaits.y);
    }

    private void Start()
    {
        StartCoroutine(nameof(CreateMapRoutine));
    }

    private void MakeThisParent()
    {
        foreach (var chunk in chunks)
            chunk.Transform.SetParent(transform);
    }

    public IEnumerator CreateMapRoutine()
    {
        var max = chunkGrid.MaxIndex(chunkCount);

        for (int x = 0; x <= max.x; x++)
        {
            for (int y = 0; y <= max.y; y++)
                for (int z = 0; z <= max.z; z++)
                {
                    var mat = materials[(x + y + z) % materials.Length];
                    yield return CreateChunkRoutine(new Vector3Int(x, y, z), mat);
                    
                    yield return _chunkWait;
                }
        }
        
        MakeThisParent();
    }

    private IEnumerator CreateChunkRoutine(Vector3Int index, Material material)
    {
        var chunk = new Chunk(index);

        blockGrid.origin = chunkGrid.IndexToPosition(index);
        var max = blockGrid.MaxIndex(blockCount);

        var parent = chunk.Transform;
        for (int x = 0; x <= max.x; x++)
        {
            for (int y = 0; y <= max.y; y++)
            {
                for (int z = 0; z <= max.z; z++)
                {
                    var blockIndex = new Vector3Int(x, y, z);
                    var pos        = blockGrid.IndexToPosition(blockIndex);

                    var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    block.name               = $"Block {blockIndex.ToString()}";
                    block.transform.position = pos;
                    block.transform.SetParent(parent);

                    var blockRenderer = block.GetComponent<MeshRenderer>();
                    blockRenderer.material = material;

                    chunk.Blocks.Add(block);
                }
            }

            yield return _blockWait;
        }

        chunks.Add(chunk);

        blockGrid.origin = Vector3.zero;
    }

    public void CreateMap()
    {
        var max = chunkGrid.MaxIndex(chunkCount);

        for (int x = 0; x <= max.x; x++)
            for (int y = 0; y <= max.y; y++)
                for (int z = 0; z <= max.z; z++)
                {
                    var mat = materials[z % materials.Length];
                    Debug.Log($"Mat: {mat.name}");
                    CreateChunk(new Vector3Int(x, y, z), mat);
                }
    }

    private void CreateChunk(Vector3Int index, Material material)
    {
        var chunk = new Chunk(index);

        blockGrid.origin = chunkGrid.IndexToPosition(index);
        var max = blockGrid.MaxIndex(blockCount);

        var parent = chunk.Transform;
        for (int x = 0; x <= max.x; x++)
        {
            for (int y = 0; y <= max.y; y++)
            {
                for (int z = 0; z <= max.z; z++)
                {
                    var blockIndex = new Vector3Int(x, y, z);
                    var pos        = blockGrid.IndexToPosition(blockIndex);

                    var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    block.name               = $"Block {blockIndex.ToString()}";
                    block.transform.position = pos;
                    block.transform.SetParent(parent);

                    var blockRenderer = block.GetComponent<MeshRenderer>();
                    blockRenderer.material = material;

                    chunk.Blocks.Add(block);
                }
            }
        }

        chunks.Add(chunk);

        blockGrid.origin = Vector3.zero;
    }
}