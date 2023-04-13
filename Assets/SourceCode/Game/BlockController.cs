using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType
{
    Blank = 0,

    Blue = 1,
    Green = 2,
    Pink = 3,
    Purple = 4,
    Red = 5,
    Yellow = 6,

    Invalid = 7,
};

public class BlockController : MonoBehaviour
{
    static readonly Color[] color_table = new Color[] {
        Color.black,

        Color.blue,
        Color.green,
        Color.cyan,
        Color.magenta,
        Color.red,
        Color.yellow,

        Color.gray,
    };

    // �c���W
    private float[] BLOCK_SCALE = { 0.5733f, 0.6332f, 0.7003f, 0.7664f, 0.8419f, 0.9251f, 1.0100f, 1.10818f, 1.20f, 1.333f, 1.466f, 1.61f };
    // �����W(360��-22.5f�͒[�ɍs�����Ƃ��Ɏ���ł���悤�ɗp�ӂ����p�x)
    private float[] BLOCK_ROTATE = { 0, 22.5f, 45.0f, 67.5f, 90.0f, 112.5f, 135.0f, 157.5f,
                                     180.0f, 202.5f, 225.0f, 247.5f, 270.0f, 292.5f, 315.0f, 337.5f, 360.0f, -22.5f};


    [SerializeField, Header("�ʒu�����_�̐e�I�u�W�F�N�g")] GameObject parentObject;
    [SerializeField] Renderer my_renderer = default!;// �������g�̃}�e���A����o�^���Ă���(GetComponent���Ȃ���)
    BlockType _type = BlockType.Invalid;

    // Start is called before the first frame update
    //    void Start()
    //    {
    //        // ����͎g��Ȃ�
    //    }

    // Update is called once per frame
    //    void Update()
    //    {
    //        // ����͎g��Ȃ�
    //    }

    public void SetBlockType(BlockType type)
    {
        _type = type;

        my_renderer.material.color = color_table[(int)_type];
    }
    public BlockType GetBlockType()
    {
        return _type;
    }

    public void SetPos(Vector3Int pos)
    {
        parentObject.transform.localRotation = Quaternion.Euler(0, BLOCK_ROTATE[pos.x], 0);
        parentObject.transform.localScale = new Vector3(BLOCK_SCALE[pos.y], BLOCK_SCALE[pos.y], BLOCK_SCALE[pos.y]);
    }
    
    public void SetPosInterpolate(Vector2Int pos, Vector2Int pos_last, float rate, float fall_y)
    {
        //Debug.Log("pos" + pos);
        //Debug.Log("pos_last" + pos_last);
        //Debug.Log("fall_y" + fall_y);
        Vector3 p = Vector3.Lerp(
    new Vector3((float)BLOCK_ROTATE[pos.x], (float)BLOCK_SCALE[pos.y]/* + fall_y*/, 0.0f),
    new Vector3((float)BLOCK_ROTATE[pos_last.x], (float)BLOCK_SCALE[pos_last.y]/* + fall_y*/, 0.0f), rate);
        p = Vector3.Lerp(new Vector3(p.x, (float)BLOCK_SCALE[pos_last.y], p.z), new Vector3(p.x, (float)BLOCK_SCALE[pos.y], p.z), (1 - fall_y));

        parentObject.transform.localRotation = Quaternion.Euler(0, p.x, 0);
        parentObject.transform.localScale = new Vector3(p.y, p.y, p.y);
    }
}
