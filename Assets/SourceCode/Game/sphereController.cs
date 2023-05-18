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

    // ���͂���荞��
    void UpdateInput()
    {
        LogicalInput.Key inputDev = 0;// �f�o�C�X�l

        // �L�[���͎擾
        for (int i = 0; i < (int)LogicalInput.Key.MAX; i++)
        {
            if (Input.GetKey(key_code_tbl[i]))
            {
                inputDev |= (LogicalInput.Key)(1 << i);
            }
        }

        logicalInput.Update(inputDev);
    }

    // ��]�A�j���[�V���������ǂ���
    //bool isAnimate;
    // ��]�̊p�x
    Vector3 angle;
    // ��]��
    float count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //// ��]�A�j���[�V�������s�����̔���
        //if (isAnimate)
        //{
        //    Animation();
        //}
        //if (isAnimate == false)
        //{
        //    //// ���L�[������
        //    //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        //    //{
        //    //    isAnimate = true;
        //    //    angle = new Vector3(0f, 0.5f, 0f);
        //    //    count = 0;
        //    //}
        //    //// ���L�[������
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
        // ���͂���荞��
        UpdateInput();

    }

    // 0.5�x����]��45��s��
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
