using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Operate : MonoBehaviour
{

    [SerializeField]
    private Image[] Operate1_Image = new Image[3];      // �N�C�b�N�h���b�v������@UI�摜

    [SerializeField]
    private Image[] Operate2_Image = new Image[3];      // ���]������@UI�摜

    [SerializeField]
    private Image[] Operate3_Image = new Image[3];      // ����]������@UI�摜

    [SerializeField]
    private Animator Operate1_Animator;                 // �N�C�b�N�h���b�v������@UI�̃A�j���[�V����

    [SerializeField]
    private float Operate2_Image_Angle;                 // ���]������@UI�摜�̉�]�l

    [SerializeField]
    private float Operate3_Image_Angle;                 // ����]������@UI�摜�̉�]�l

    [SerializeField]
    private RectTransform Operate2_Image_Rect;          // ���]������@UI�摜��RectTransform
                                                        
    [SerializeField]                                    
    private RectTransform Operate3_Image_Rect;          // ����]������@UI�摜��RectTransform




    // Start is called before the first frame update
    void Start()
    {
        // ��]�l�ϐ�������������
        Operate2_Image_Angle = 0.0f;
        Operate3_Image_Angle = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // XBox�R���g���[���[��"A"�{�^����������Ă���Ԃ�"�N�C�b�N�h���b�v"UI���o
        if (Input.GetKey(KeyCode.Joystick1Button0) || /*�m�F�p*/ Input.GetKey(KeyCode.A)) 
        {
            // ���쒆�摜�݂̂�`�悷��
            Operate1_Image[1].enabled = true;
            Operate1_Image[2].enabled = true;
            // �ʏ�摜�͕`�悵�Ȃ�
            Operate1_Image[0].enabled = false;
            
        }
        // XBox�R���g���[���[��"A"�{�^����������Ă��Ȃ��Ԃ͒ʏ�摜��`��
        else
        {
            // �ʏ�摜�݂̂�`�悷��
            Operate1_Image[0].enabled = true;
            // ���쒆�摜�͕`�悵�Ȃ�
            Operate1_Image[1].enabled = false;
            Operate1_Image[2].enabled = false;

            // �A�j���[�V�����̍Đ����Ԃ�"0"�ɂ���
            Operate1_Animator.Play(0, -1, 0f);
        }

        // XBox�R���g���[���[��"LT RT"�{�^����������Ă���Ԃ�"���]"UI���o
        if (/*(Input.GetKey(KeyCode.Joystick1Button11) || Input.GetKey(KeyCode.Joystick1Button14)) ||*/ /*�m�F�p*/ Input.GetKey(KeyCode.S)) 
        {
            // ���쒆�摜�݂̂�`�悷��
            Operate2_Image[1].enabled = true;
            Operate2_Image[2].enabled = true;
            // �ʏ�摜�͕`�悵�Ȃ�
            Operate2_Image[0].enabled = false;

            // ������Ă���ԁA��]�l�𑝂₷
            Operate2_Image_Angle = Time.deltaTime * -200.0f;
            // ���]������@UI�A�C�R������]������
            // ���̍ہA�p�x(Operate2_Image_Angle)��0�`360�x�͈̔͂ɐ��K������
            var Normalized_Operate2_Angle = Mathf.Repeat(Operate2_Image_Angle, 360);
            Operate2_Image_Rect.Rotate(.0f, .0f, Normalized_Operate2_Angle);
        }
        // XBox�R���g���[���[��"LT RT"�{�^����������Ă��Ȃ��Ԃ͒ʏ�摜��`��
        else
        {
            // �摜�̉�]�l������������
            Operate2_Image_Angle = 0.0f;
            Vector3 Reset_Angle;
            // 0.0f���i�[�����Inspector����-90������ׁA���g��X�l���i�[����
            Reset_Angle.x = Operate2_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // �p�x���Z�b�g
            Operate2_Image_Rect.eulerAngles = Reset_Angle;

            // �ʏ�摜�݂̂�`�悷��
            Operate2_Image[0].enabled = true;
            // ���쒆�摜�͕`�悵�Ȃ�
            Operate2_Image[1].enabled = false;
            Operate2_Image[2].enabled = false;
        }

        // XBox�R���g���[���[��"LB RB"�{�^����������Ă���Ԃ�"����]"UI���o
        if ((Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5)) || /*�m�F�p*/ Input.GetKey(KeyCode.D)) 
        {
            // ���쒆�摜�݂̂�`�悷��
            Operate3_Image[1].enabled = true;
            Operate3_Image[2].enabled = true;
            // �ʏ�摜�͕`�悵�Ȃ�
            Operate3_Image[0].enabled = false;

            // ������Ă���ԁA��]�l�𑝂₷
            Operate3_Image_Angle = Time.deltaTime * -200.0f;
            // ���]������@UI�A�C�R������]������
            // ���̍ہA�p�x(Operate2_Image_Angle)��0�`360�x�͈̔͂ɐ��K������
            var Normalized_Operate3_Angle = Mathf.Repeat(Operate3_Image_Angle, 360);
            Operate3_Image_Rect.Rotate(.0f, .0f, Normalized_Operate3_Angle);

        }
        // XBox�R���g���[���[��"LB RB"�{�^����������Ă��Ȃ��Ԃ͒ʏ�摜��`��
        else
        {
            // �摜�̉�]�l������������
            Operate3_Image_Angle = 0.0f;
            Vector3 Reset_Angle;
            // 0.0f���i�[�����Inspector����-90������ׁA���g��X�l���i�[����
            Reset_Angle.x = Operate3_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // �p�x���Z�b�g
            Operate3_Image_Rect.eulerAngles = Reset_Angle;

            // �ʏ�摜�݂̂�`�悷��
            Operate3_Image[0].enabled = true;
            // ���쒆�摜�͕`�悵�Ȃ�
            Operate3_Image[1].enabled = false;
            Operate3_Image[2].enabled = false;

            // �摜�̉�]�l������������
            Operate3_Image_Angle = 0.0f;
        }
    }
}
