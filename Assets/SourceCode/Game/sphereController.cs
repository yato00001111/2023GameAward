using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereController : MonoBehaviour
{
    LogicalInput logicalInput = new();

    static readonly KeyCode[] key_code_tbl = new KeyCode[(int)LogicalInput.Key.MAX]{
        KeyCode.RightArrow, // Right
        KeyCode.LeftArrow,  // Left
        KeyCode.D,          // RotR
        KeyCode.A,          // RotL
        KeyCode.UpArrow,    // QuickDrop
        KeyCode.DownArrow,  // Down
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
    bool isAnimate;
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
        // ��]�A�j���[�V�������s�����̔���
        if (isAnimate)
        {
            Animation();
        }
        if (isAnimate == false)
        {
            //// ���L�[������
            //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            //{
            //    isAnimate = true;
            //    angle = new Vector3(0f, 0.5f, 0f);
            //    count = 0;
            //}
            //// ���L�[������
            //if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            //{
            //    isAnimate = true;
            //    angle = new Vector3(0f, -0.5f, 0f);
            //    count = 0;
            //}
            if (logicalInput.IsRepeat(LogicalInput.Key.Right) || logicalInput.IsRepeat(LogicalInput.Key.D))
            {
                angle = new Vector3(0f, 22.5f, 0f);
                transform.Rotate(angle, Space.World);

                //isAnimate = true;
                //angle = new Vector3(0f, 0.5f, 0f);
                //count = 0;
            }
            if (logicalInput.IsRepeat(LogicalInput.Key.Left) || logicalInput.IsRepeat(LogicalInput.Key.A))
            {
                angle = new Vector3(0f, -22.5f, 0f);
                transform.Rotate(angle, Space.World);

                //isAnimate = true;
                //angle = new Vector3(0f, -0.5f, 0f);
                //count = 0;
            }
        }

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    angle = new Vector3(0f, 22.5f, 0f);
        //    transform.Rotate(angle, Space.World);
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    angle = new Vector3(0f, -22.5f, 0f);
        //    transform.Rotate(angle, Space.World);
        //}
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
            isAnimate = false;
        }
    }

}
