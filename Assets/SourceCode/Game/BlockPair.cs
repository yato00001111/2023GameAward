using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPair : MonoBehaviour
{
    [SerializeField] BlockController[] blocks = { default!, default! };

    public void SetBlockType(BlockType axis, BlockType child)
    {
        blocks[0].SetBlockType(axis);
        blocks[1].SetBlockType(child);
    }
}
