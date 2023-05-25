using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Dead_Gauge_Tutorial : MonoBehaviour
{

    [SerializeField]
    private RectTransform Dead_Gauge_Image_Rect;          // ���S�Q�[�WUI�摜��RectTransform

    [SerializeField]
    private RectTransform Needle_Gauge_Image_Rect;        // �j�Q�[�WUI�摜��RectTransform

    [SerializeField]                                     
    private Image Tutorial_Content_01_Image;              // �`���[�g���A��01�摜
    [SerializeField]                                     
    private Image Tutorial_Content_02_Image;              // �`���[�g���A��02�摜
    [SerializeField]                                     
    private Image Tutorial_Content_03_Image;              // �`���[�g���A��03�摜

    [SerializeField]
    private Image[] Step_Image                            // �X�e�b�v�摜
        = new Image[4];

    [SerializeField]
    private RectTransform Revers_Image_Rect;              // �F�ʔ��]UI�摜��RectTransform
    [SerializeField]
    private float Revers_Image_Scale;                     // �F�ʔ��]UI�摜�̃T�C�Y

    [SerializeField]
    private RectTransform Revers_Image_Rect2;             // �F�ʔ��]UI�摜��RectTransform(2����)
    [SerializeField]
    private float Revers_Image_Scale2;                    // �F�ʔ��]UI�摜�̃T�C�Y(2����)


    [SerializeField]
    private Image[] Arrow_Image                           // ���摜
        = new Image[3];                    

    [SerializeField]
    private Image Back_Black_Image;                       // �w�i���摜

    [SerializeField]
    private Animator Arrow_Animator;                      // ���A�j���[�V����

    [SerializeField]
    private Image A_Image;                                // A�{�^���摜

    [SerializeField]
    private bool Tutorial04_Start_Flag;                   // No04�̃`���[�g���A���J�n�t���O

    [SerializeField]
    private bool Gauge_Stop_Flag;                         // �Q�[�W�X�g�b�v�t���O

    [SerializeField]
    private bool[] Input_Stop_Flag
        =new bool[2];                                     // �N���b�N����t���O

    [SerializeField]
    private bool Explanation_End_Flag;                    // �`���[�g���A��04���̐����I���t���O

    [SerializeField]
    private float ElapsedTime;                            // ���S�Q�[�W�̊g��o�ߎ���

    [SerializeField]
    private float ElapsedTime2;                           // ���S�Q�[�W�̏k���o�ߎ���

    [SerializeField]
    private bool Is_Disappear_Phase_Flag;                 // ������t�F�[�Y�t���O

    [SerializeField]
    private float Dead_Gauge_ScaleX;                      // ���S�Q�[�WUI�摜�̉����ϐ�

    [SerializeField]
    private bool Is_Disappear_Phase_End_Flag;             // ������t�F�[�Y�̏I���t���O

    [SerializeField]
    private bool TutorialEndFlag;                         // "�`���[�g���A��"�̏I���t���O

    private const float Stepalpha = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // �`���[�g���A���摜������������
        Tutorial_Content_01_Image.enabled = false;
        Tutorial_Content_02_Image.enabled = false;
        Tutorial_Content_03_Image.enabled = false;

        // ���摜������������
        Arrow_Image[0].enabled = Arrow_Image[1].enabled = Arrow_Image[2].enabled = false;

        // A�{�^���摜������������
        A_Image.enabled = false;

        // ���S�Q�[�W�̊g��o�ߎ��Ԃ�����������
        ElapsedTime = 0.0f;

        // �X�P�[��������������
        Dead_Gauge_Image_Rect.localScale = new Vector3(0, 1, 1);

        // �j�Q�[�WUI�摜��RectTransform�̉�]�l������������
        Vector3 Angle;
        Angle.x = Needle_Gauge_Image_Rect.eulerAngles.x;
        Angle.y = 0.0f;
        Angle.z = -157.5f;
        Needle_Gauge_Image_Rect.eulerAngles = Angle;

        // No04�̃`���[�g���A���J�n�t���O������������
        Tutorial04_Start_Flag = false;

        // �Q�[�W�X�g�b�v�t���O������������
        Gauge_Stop_Flag = false;

        // �N���b�N����t���O������������
        Input_Stop_Flag[0] = Input_Stop_Flag[1] = false;

        // �`���[�g���A��04���̐����I���t���O������������
        Explanation_End_Flag = false;

        // �w�i���摜������������
        Back_Black_Image.enabled = false;

        // �X�e�b�v�摜������������
        Step_Image[0].enabled = Step_Image[1].enabled = Step_Image[2].enabled = Step_Image[3].enabled = false;
        Step_Image[0].color   = Step_Image[1].color   = Step_Image[2].color   = Step_Image[3].color   = new Vector4(1, 1, 1, Stepalpha);

        // �F�ʔ��]�摜�̃X�P�[������������
        Revers_Image_Rect.localScale = new Vector3(0, 0, 0);
        Revers_Image_Rect2.localScale = new Vector3(0, 0, 0);
        // �F�ʔ��]UI�摜�̃T�C�Y�ϐ�������������
        Revers_Image_Scale = 0.0f;
        Revers_Image_Scale2 = 0.0f;
        // ���S�Q�[�WUI�摜�̉����ϐ�������������
        Dead_Gauge_ScaleX = 0.0f;
        // ������t�F�[�Y�̏I���t���O������������
        Is_Disappear_Phase_End_Flag = false;
        // "�`���[�g���A��"�̏I���t���O������������
        TutorialEndFlag = false;
        // ���S�Q�[�W�̏k���o�ߎ��Ԃ�����������
        ElapsedTime2 = 0.0f;
    }


    // Update is called once per frame
    void Update()
    {
        // �`���[�g���A��04���J�n���ꂽ��e�摜�̈ʒu��߂�
        if (Tutorial04_Start_Flag)
        {
            // �Q�[�W��0.2�n�_�܂ł����ΐ����t�F�[�Y�ɓ���
            if (Gauge_Stop_Flag)
            {
                // ���`��
                Arrow_Image[2].enabled = true;
                // "A"�{�^���`��
                A_Image.enabled = true;
                // ���A�j���[�V����
                Arrow_Animator.SetBool("Up",   false);
                Arrow_Animator.SetBool("Down", false);
                Arrow_Animator.SetBool("Side",  true);
                // �`���[�g���A��01�摜��`�悷��
                Tutorial_Content_01_Image.enabled = true;
                // �w�i���摜��`�悷��
                Back_Black_Image.enabled = true;


                // XBox�R���g���[���[��"A"�{�^���������ꂽ
                if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*�m�F�p*/ Input.GetKeyDown(KeyCode.Return)) && !Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // �`���[�g���A��01�摜���\���ɂ���
                    Tutorial_Content_01_Image.enabled = false;
                    // �`���[�g���A��02�摜��`�悷��
                    Tutorial_Content_02_Image.enabled = true;
                }
                // XBox�R���g���[���[��"A"�{�^���������ꂽ
                else if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || /*�m�F�p*/   Input.GetKeyUp(KeyCode.Return)) && !Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // �N���b�N����
                    Input_Stop_Flag[0] = true;
                }
                // XBox�R���g���[���[��"A"�{�^���������ꂽ
                else if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*�m�F�p*/  Input.GetKeyDown(KeyCode.Return)) && Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // �`���[�g���A��02�摜���\���ɂ���
                    Tutorial_Content_02_Image.enabled = false;
                    // �`���[�g���A��03�摜��`�悷��
                    Tutorial_Content_03_Image.enabled = true;
                }
                // XBox�R���g���[���[��"A"�{�^���������ꂽ
                else if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || /*�m�F�p*/  Input.GetKeyUp(KeyCode.Return)) && Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // �N���b�N����
                    Input_Stop_Flag[1] = true;
                }
                // XBox�R���g���[���[��"A"�{�^���������ꂽ
                else if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*�m�F�p*/ Input.GetKeyDown(KeyCode.Return)) && Input_Stop_Flag[0] && Input_Stop_Flag[1])
                {
                    // �������I������
                    Explanation_End_Flag = true;
                    // �Q�[�W���ĊJ������
                    Gauge_Stop_Flag = false;
                    // �K�v�ȉ摜�ȊO�S�Ĕ�\��
                    Arrow_Image[2].enabled = false;
                    A_Image.enabled = false;
                    Tutorial_Content_01_Image.enabled = false;
                    Tutorial_Content_02_Image.enabled = false;
                    Tutorial_Content_03_Image.enabled = false;
                    // �w�i���摜���\���ɂ���
                    Back_Black_Image.enabled = false;

                }
            }

            // �O�̂��ߏ�ɃQ�[�W���ĊJ������
            if (Explanation_End_Flag) Gauge_Stop_Flag = false;

            // �Q�[�W���~�܂��Ă���Ԃ͂����Ń��^�[��
            if (Gauge_Stop_Flag) return;

            // ������t�F�[�Y���ȊO�͒ʏ�ʂ蓮����
            if (!Is_Disappear_Phase_Flag)
            {
                // �^�C�}�[�N��
                ElapsedTime += Time.deltaTime;
                float Current_Time = Mathf.Clamp01(ElapsedTime / 4.8f);
                // ���o
                OnScale(Current_Time);
                float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);
                // ��]�֐�
                UI_Rotate(Current_Value);
            }
           
            // ������t�F�[�Y�ɓ������������t�F�[�Y�t���O�𗧂Ă�
            if (Dead_Gauge_ScaleX >= 1.0f)
            {
                // 1���ڂ̐F�ʔ��]�摜�̃T�C�Y��傫�����Ă���
                if (Revers_Image_Scale < 20.0f) Revers_Image_Scale += 0.25f;
                // 1���ڂ̐F�ʔ��]�摜�̃T�C�Y���ő�l�܂ł�����2���ڂ̐F�ʔ��]�摜�̃T�C�Y���傫�����Ă���
                else if (Revers_Image_Scale >= 20.0f && Revers_Image_Scale2 < 15.0f) Revers_Image_Scale2 += 0.25f;
            }


            // �F�ʔ��]���o���I�����������t�F�[�Y�ɓ��� �t���O�𗧂Ă�
            if (Revers_Image_Scale2 >= 15.0f) Is_Disappear_Phase_Flag = true;

            // ������t�F�[�Y���͎��S�Q�[�W�̃X�P�[�������炷
            if (Is_Disappear_Phase_Flag)
            {
                // �^�C�}�[�N��
                ElapsedTime2 += Time.deltaTime;
                float Current_Time = Mathf.Clamp01(ElapsedTime2 / 4.8f);
                // ���o
                OnScale(Current_Time);
                float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);
                // ��]�֐�
                UI_Rotate(Current_Value);
            }

            // ������t�F�[�Y�̏I�����m�点���󂯎������
            if (Is_Disappear_Phase_End_Flag)
            {
                // 2���ڂ̐F�ʔ��]�摜�̃T�C�Y�����������Ă���
                if (Revers_Image_Scale2 > 0.0f) Revers_Image_Scale2 -= 0.25f;
                // 2���ڂ̐F�ʔ��]�摜�̃T�C�Y���ŏ��l�܂ł�����1���ڂ̐F�ʔ��]�摜�̃T�C�Y�����������Ă���
                else if (Revers_Image_Scale2 <= 0.0f && Revers_Image_Scale > 0.0f) Revers_Image_Scale -= 0.25f;

                // �F�ʔ��]���o���I�������
                if (Revers_Image_Scale <= 0.0f)
                {
                    // �`���[�g���A���I���t���O�𗧂Ă�
                    TutorialEndFlag = true;
                }
            }


            // �F�ʔ��]�摜�̃T�C�Y�ݒ�
            Revers_Image_Rect.localScale  = new Vector3(Revers_Image_Scale,  Revers_Image_Scale,  Revers_Image_Scale);
            Revers_Image_Rect2.localScale = new Vector3(Revers_Image_Scale2, Revers_Image_Scale2, Revers_Image_Scale2);
        }
    }

    // ���S�Q�[�WUI�摜�̃X�P�[����ύX
    private void OnScale(float value)
    {
        // ������t�F�[�Y���Ȃ�360�x��]�@����ȊO�͒ʏ�ʂ�45�x��]
        var Scale = Is_Disappear_Phase_Flag ? Mathf.Lerp(1.0f, 0.0f, value) : Mathf.Lerp(0.0f, 1.0f, value);
        // �X�P�[�����f
        Dead_Gauge_Image_Rect.localScale = new Vector3(Scale, 1, 1);
        // 0.2�܂ōs�������U�~�߂�
        if (Scale >= 0.2f && !Explanation_End_Flag) Gauge_Stop_Flag = true;

        // �X�P�[���l�ۑ�����
        Dead_Gauge_ScaleX = Scale;

        // ������t�F�[�Y�̃Q�[�W��0�ɂȂ��
        if (Is_Disappear_Phase_Flag && Scale <= 0.0f) 
        {
            // ������t�F�[�Y�̏I���t���O�𗧂Ă�
            Is_Disappear_Phase_End_Flag = true;
        }
    }

    // �j�Q�[�WUI����]������֐�
    private void UI_Rotate(float timer)
    {
        var Angle = Mathf.Lerp(-157.5f, -517.5f, timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);
    }

    // No04�̃`���[�g���A���J�n�t���O�̐^�U�ݒ�֐�
    public void SetTutorial04StartFlag() { Tutorial04_Start_Flag = true; }

    // ���A�j���[�V�����J�n�֐�(UP)
    public void SetArrowUpAnimation() 
    {
        // ���`��
        Arrow_Image[0].enabled = true;
        Arrow_Image[1].enabled = false;
        Arrow_Image[2].enabled = false;
        // ���A�j���[�V����
        Arrow_Animator.SetBool("Up",    true);
        Arrow_Animator.SetBool("Down", false);
        Arrow_Animator.SetBool("Side", false);
    }

    // No04�̃`���[�g���A���J�n�t���O�̐^�U�ݒ�֐�
    public void SetArrowDownAnimation() 
    {
        // ���`��
        Arrow_Image[1].enabled = true;
        Arrow_Image[0].enabled = false;
        Arrow_Image[2].enabled = false;
        // ���A�j���[�V����
        Arrow_Animator.SetBool("Up",   false);
        Arrow_Animator.SetBool("Down",  true);
        Arrow_Animator.SetBool("Side", false);
    }

    public void SetArrowResetAnimation()
    {
        // ���`��
        Arrow_Image[0].enabled = false;
        Arrow_Image[1].enabled = false;
        Arrow_Image[2].enabled = false;
    }

    // ���񐬌��������̉摜�ݒ�֐�
    public void SetStepImage(int Num)
    {
        //*************************************//
        // ������"5"�Ȃ炻�������̕`������Ȃ� //
        //*************************************//
        if (Num == 5) 
        {
            for (int index = 0; index < 4; ++index)
            {
                // �O�̂��ߔ������ɂ��Ă���
                Step_Image[index].color = new Vector4(1, 1, 1, Stepalpha);
                // ��\���ɂ���
                Step_Image[index].enabled = false;
            }
            return;
        }

        // ���̊֐����Ă΂ꂽ�i�K�ŕ\������
        for (int index = 0; index < 4; ++index)
        {
            Step_Image[index].enabled = true;
        }

        // �w��̃X�e�b�v�܂ŕ`�悷��
        if      (Num == 0) 
        {
            Step_Image[0].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[1].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[2].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 1)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[2].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 2)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[2].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 3)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[2].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 4)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[2].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[3].color = new Vector4(1, 1, 1, 1.0f);
        }
    }

    // �`���[�g���A���I���t���O�擾�֐�
    public bool GetTutorialEndFlag() { return TutorialEndFlag; }
}
