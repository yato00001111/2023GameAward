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
    private Image[] Operate4_Image = new Image[2];      // ����ւ�������@UI�摜

    [SerializeField]
    private Image Select_Operate_Image;                 // ���ݑI�𒆂̑�����@UI�摜
    [SerializeField]
    private RectTransform Select_Operate_Image_Rect;    // ���ݑI��UI�摜��RectTransform
    [SerializeField]
    private int[] Select_Operate_Number = new int[4];   // ���ݑI�𒆂̑�����@UI�摜�̔ԍ�
    [SerializeField]
    private float Select_Operate_Alpha;                 // ���ݑI�𒆂̑�����@UI�摜�̓����l
    [SerializeField]
    private bool Select_Operate_Alpha_Flag;             // ���ݑI�𒆂̑�����@UI�摜�̓����l�t���O


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

    [SerializeField]
    private RectTransform Operate4_Image_Rect;          // ����ւ�������@UI�摜��RectTransform

    [SerializeField]
    private float Operate4_Time;                        // ����ւ�������@UI�摜�̃A�j���[�V��������

    [SerializeField]
    private bool Operate4_Swap_Flag;                    // ����ւ�������@UI�摜�̃A�j���[�V�����t���O

    [SerializeField]
    public UI_CountDown uI_CountDown;                   // �Q�[���J�n�J�E���g�_�E���X�N���v�g


    // Start is called before the first frame update
    void Start()
    {
        // ��]�l�ϐ�������������
        Operate2_Image_Angle = 0.0f;
        Operate3_Image_Angle = 0.0f;
        // ����ւ�������@UI�摜�̃A�j���[�V�������Ԃ�����������
        Operate4_Time = 0.0f;
        // ����ւ�������@UI�摜�̃A�j���[�V�����t���O������������
        Operate4_Swap_Flag = false;
        // ���ݑI�𒆂̑�����@UI�摜�̔ԍ�������������
        Select_Operate_Number[0] = Select_Operate_Number[1] =
        Select_Operate_Number[2] = Select_Operate_Number[3] = 0;
        // ���ݑI�𒆂̑�����@UI�摜�̓����l������������
        Select_Operate_Alpha = 0.0f;
        // ���ݑI�𒆂̑�����@UI�摜�̓����l�t���O������������
        Select_Operate_Alpha_Flag = false;
    }

    // Update is called once per frame
    void Update()
    {

        // �I�𒆂̑�����@UI�摜�̉��o�ݒ�
        if (Select_Operate_Number[0] != 0 || Select_Operate_Number[1] != 0 ||
            Select_Operate_Number[2] != 0 || Select_Operate_Number[3] != 0 )
        {
            // �����x���o
            if      (!Select_Operate_Alpha_Flag && Select_Operate_Alpha < 0.3f) Select_Operate_Alpha += 0.025f;
            else if (!Select_Operate_Alpha_Flag && Select_Operate_Alpha >= 0.3f) Select_Operate_Alpha_Flag = true;
            else if (Select_Operate_Alpha_Flag  && Select_Operate_Alpha > 0.0f) Select_Operate_Alpha -= 0.025f;
            else if (Select_Operate_Alpha_Flag  && Select_Operate_Alpha <= 0.0f) Select_Operate_Alpha_Flag = false;

            // �����l�ݒ�
            Select_Operate_Image.color = new Color(1, 1, 1, Select_Operate_Alpha);

            if      (Select_Operate_Number[0] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-780.0f, 324.0f - 200.0f);
            else if (Select_Operate_Number[1] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-780.0f,  54.0f - 200.0f);
            else if (Select_Operate_Number[2] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-585.0f,  54.0f - 200.0f);
            else if (Select_Operate_Number[3] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-585.0f, 324.0f - 200.0f);

            // �X�P�[���ύX
            if      (Select_Operate_Number[1] == 1 || Select_Operate_Number[2] == 1) Select_Operate_Image_Rect.localScale = new Vector3(1, 0.9f, 1);
            else if (Select_Operate_Number[0] == 1 || Select_Operate_Number[3] == 1) Select_Operate_Image_Rect.localScale = new Vector3(1, 1, 1);
        }
        // ���I�𒆂̓��Z�b�g����
        else if (Select_Operate_Number[0] == 0 && Select_Operate_Number[1] == 0 && 
                 Select_Operate_Number[2] == 0 && Select_Operate_Number[3] == 0 ) 
        {
            Select_Operate_Alpha_Flag = false;
            Select_Operate_Alpha = 0.0f;
            Select_Operate_Image.color = new Color(1, 1, 1, Select_Operate_Alpha);
            Select_Operate_Image_Rect.localScale = new Vector3(1, 1, 1);
        }

        float TrigerInput = Input.GetAxisRaw("Trigger");

        // XBox�R���g���[���[��"A"�{�^����������Ă���Ԃ�"�N�C�b�N�h���b�v"UI���o
        if (Input.GetKey(KeyCode.Joystick1Button0) || /*�m�F�p*/ Input.GetKey(KeyCode.DownArrow) && uI_CountDown.GetGameStartFlag())  
        {
            // ���쒆�摜�݂̂�`�悷��
            Operate1_Image[1].enabled = true;
            Operate1_Image[2].enabled = true;
            // �ʏ�摜�͕`�悵�Ȃ�
            Operate1_Image[0].enabled = false;
            // �I�𒆐ݒ�
            Select_Operate_Number[0] = 1;
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
            // �I�𒆐ݒ胊�Z�b�g
            Select_Operate_Number[0] = 0;
        }

        // XBox�R���g���[���[��"LT RT"�{�^����������Ă���Ԃ�"���]"UI���o
        if ((TrigerInput < 0.0f || TrigerInput > 0.0f) || /*�m�F�p*/ Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) && uI_CountDown.GetGameStartFlag()) 
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
            // �I�𒆐ݒ�
            Select_Operate_Number[1] = 1;
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
            // �I�𒆐ݒ胊�Z�b�g
            Select_Operate_Number[1] = 0;
        }

        // XBox�R���g���[���[��"LB RB"�{�^����������Ă���Ԃ�"����]"UI���o
        if ((Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5)) || /*�m�F�p*/ Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) && uI_CountDown.GetGameStartFlag()) 
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
            // �I�𒆐ݒ�
            Select_Operate_Number[2] = 1;
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
            // �I�𒆐ݒ胊�Z�b�g
            Select_Operate_Number[2] = 0;
        }

        // XBox�R���g���[���[��"X"�{�^����������Ă���Ԃ�"����ւ�"UI���o
        if (Input.GetKey(KeyCode.Joystick1Button2) || /*�m�F�p*/ Input.GetKey(KeyCode.UpArrow) && uI_CountDown.GetGameStartFlag())
        {
            // ���쒆�摜�݂̂�`�悷��
            Operate4_Image[1].enabled = true;
            // �ʏ�摜�͕`�悵�Ȃ�
            Operate4_Image[0].enabled = false;

            // ������Ă���ԁA���Ԃ𑝂₷
            Operate4_Time += Time.deltaTime;
            // ��莞�Ԍo�߂�����
            if (Operate4_Time > 0.25f) 
            {
                // �t���O�؂�ւ�
                Operate4_Swap_Flag = !Operate4_Swap_Flag;
                // �^�C�����Z�b�g
                Operate4_Time = 0.0f;
            }

            // ���]������@UI�A�C�R������]������
            if       (Operate4_Swap_Flag && Operate4_Time == .0f) Operate4_Image_Rect.Rotate(.0f, 180.0f, .0f);
            else if (!Operate4_Swap_Flag && Operate4_Time == .0f) Operate4_Image_Rect.Rotate(.0f,    .0f, .0f);
            // �I�𒆐ݒ�
            Select_Operate_Number[3] = 1;

        }
        // XBox�R���g���[���[��"X"�{�^����������Ă��Ȃ��Ԃ͒ʏ�摜��`��
        else
        {
            // �摜�̉�]�l������������
            Vector3 Reset_Angle;
            // 0.0f���i�[�����Inspector����-90������ׁA���g��X�l���i�[����
            Reset_Angle.x = Operate4_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // �p�x���Z�b�g
            Operate4_Image_Rect.eulerAngles = Reset_Angle;

            // �ʏ�摜�݂̂�`�悷��
            Operate4_Image[0].enabled = true;
            // ���쒆�摜�͕`�悵�Ȃ�
            Operate4_Image[1].enabled = false;
            // ���Ԃƃt���O�����Z�b�g
            Operate4_Swap_Flag = false;
            Operate4_Time = 0.0f;
            // �I�𒆐ݒ胊�Z�b�g
            Select_Operate_Number[3] = 0;
        }
    }
}
