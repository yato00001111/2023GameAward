using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereController : MonoBehaviour
{
    LogicalInput logicalInput = new();

    [SerializeField] FieldController _fieldController = default!;
    [SerializeField] PlayDirector _playDirector = default!;


    static readonly KeyCode[] key_code_tbl = new KeyCode[(int)LogicalInput.Key.MAX]{
        KeyCode.RightArrow, // Right
        KeyCode.LeftArrow,  // Left
        KeyCode.D,          // RotR
        KeyCode.A,          // RotL
        KeyCode.UpArrow,    // QuickDrop
        KeyCode.DownArrow,  // Down
        KeyCode.Joystick1Button0,  // A
        KeyCode.Joystick1Button4,  // LB
        KeyCode.Joystick1Button5,  // RB
    };

    // 入力を取り込む
    void UpdateInput()
    {
        LogicalInput.Key inputDev = 0;// デバイス値

        // キー入力取得
        for (int i = 0; i < (int)LogicalInput.Key.MAX; i++)
        {
            if (Input.GetKey(key_code_tbl[i]))
            {
                inputDev |= (LogicalInput.Key)(1 << i);
            }
        }

        logicalInput.Update(inputDev);
    }

    // 回転アニメーション中かどうか
    //bool isAnimate;
    // 回転の角度
    Vector3 angle;
    // 回転回数
    float count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //// 回転アニメーションを行うかの判定
        //if (isAnimate)
        //{
        //    Animation();
        //}
        //if (isAnimate == false)
        //{
        //    //// →キー押下時
        //    //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        //    //{
        //    //    isAnimate = true;
        //    //    angle = new Vector3(0f, 0.5f, 0f);
        //    //    count = 0;
        //    //}
        //    //// ←キー押下時
        //    //if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        //    //{
        //    //    isAnimate = true;
        //    //    angle = new Vector3(0f, -0.5f, 0f);
        //    //    count = 0;
        //    //}
        //    if (logicalInput.IsRepeat(LogicalInput.Key.Right) || logicalInput.IsRepeat(LogicalInput.Key.D) ||
        //        logicalInput.IsRepeat(LogicalInput.Key.RB))
        //    {
        //        angle = new Vector3(0f, 22.5f, 0f);
        //        transform.Rotate(angle, Space.World);

        //        //isAnimate = true;
        //        //angle = new Vector3(0f, 0.5f, 0f);
        //        //count = 0;
        //    }
        //    if (logicalInput.IsRepeat(LogicalInput.Key.Left) || logicalInput.IsRepeat(LogicalInput.Key.A) ||
        //        logicalInput.IsRepeat(LogicalInput.Key.LB))
        //    {
        //        angle = new Vector3(0f, -22.5f, 0f);
        //        transform.Rotate(angle, Space.World);

        //        //isAnimate = true;
        //        //angle = new Vector3(0f, -0.5f, 0f);
        //        //count = 0;
        //    }
        //}
        float TrigerInput = Input.GetAxisRaw("Trigger");

        if (_fieldController.GetControl())
        {
            if(_playDirector.GetPlayFlag())
            {
                if (logicalInput.IsTrigger(LogicalInput.Key.Right) || logicalInput.IsTrigger(LogicalInput.Key.D) ||
                    logicalInput.IsTrigger(LogicalInput.Key.RB) || TrigerInput > 0.0f)
                {
                    angle = new Vector3(0f, 22.5f, 0f);
                    transform.Rotate(angle, Space.World);

                    //isAnimate = true;
                    //angle = new Vector3(0f, 0.5f, 0f);
                    //count = 0;
                }
                if (logicalInput.IsTrigger(LogicalInput.Key.Left) || logicalInput.IsTrigger(LogicalInput.Key.A) ||
                    logicalInput.IsTrigger(LogicalInput.Key.LB) || TrigerInput > 0.0f)
                {
                    angle = new Vector3(0f, -22.5f, 0f);
                    transform.Rotate(angle, Space.World);

                    //isAnimate = true;
                    //angle = new Vector3(0f, -0.5f, 0f);
                    //count = 0;
                }
            }
        }
    }

    void FixedUpdate()
    {
        // 入力を取り込む
        UpdateInput();

    }

    // 0.5度ずつ回転を45回行う
    private void Animation()
    {
        for (int i = 0; i < 15; i++)
        {
            transform.Rotate(angle, Space.World);
        }
        count++;
        if (count == 3)
        {
            //isAnimate = false;
        }
    }

}
