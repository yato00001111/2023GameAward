using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Needles : MonoBehaviour
{
    [SerializeField]
    private float Clap_Time;                       // ���̊Ԋu�̕b��
                                                   
    [SerializeField]                               
    private float Current_Time;                    // �m�F�p���݂̃^�C��
                                                   
    [SerializeField]                               
    private int Clap_Count;                        // ���̃J�E���g�ϐ�
                                                   
    [SerializeField]                               
    private RectTransform Needle_Gauge_Image_Rect; // �j�Q�[�WUI�摜��RectTransform
    [SerializeField]
    private Image Needle_Gauge_Image_Color;        // �j�Q�[�WUI�摜��Color
    [SerializeField]
    private float Needle_Gauge_Image_AngleZ;       // �j�Q�[�WUI�摜��RectTransform��"Z"��]�l
    [SerializeField]
    private float Needle_Gauge_Image_RotateZ;      // �j�Q�[�WUI�摜��RectTransform�̏���Rotate"Z"�l
    [SerializeField]
    private float Save_AngleZ;                     // �O���"Z"��]�l��ۑ�����ϐ�
    [SerializeField]
    private int Needle_Gauge_Number;               // �j�Q�[�W�̌��݂̊p�x�ԍ�
    [SerializeField]
    private float Needle_Gauge_ElapsedTime;        // �j�Q�[�W�̌o�ߎ���
    [SerializeField]                               
    private bool Needle_Gauge_Is_Changing;         // �j�Q�[�W���ω����^�U�t���O


    [SerializeField]
    private RectTransform Rhythm_Gauge_Image_Rect; // ���Y���Q�[�WUI�摜��RectTransform
    [SerializeField]
    private float Rhythm_Gauge_Image_PosX;         // ���Y���Q�[�WUI�摜��RectTransform��"X"�ړ��l
    [SerializeField]
    private float Rhythm_Gauge_ElapsedTime;        // ���Y���Q�[�W�̌o�ߎ���
    [SerializeField]
    private bool  Rhythm_Gauge_Is_Changing;        // ���Y���Q�[�W���ω����^�U�t���O
    [SerializeField]
    private int   Rhythm_Transition_Count;         // ���Y���Q�[�W�̈ړ��J�E���g

    [SerializeField]
    private RectTransform Rhythm_Image_Rect;       // ���Y��UI�摜��RectTransform
    [SerializeField]
    private bool Succeed_Flag;                     // ���Y�����쐬���^�U�t���O
    [SerializeField]
    private float Save_Succeed_Time;               // ���Y�����쐬�����̎���


    [SerializeField]
    private RectTransform Phase_Gauge_Image_Rect;  // �t�F�[�Y�Q�[�WUI�摜��RectTransform

    [SerializeField]
    private int GameStart_ClapCount;               // �Q�[���J�n����̔��̃^�C�~���O

    bool beat;

    // Start is called before the first frame update
    void Start()
    {
        // ���̊Ԋu�̕b��������������
        Clap_Time                       = 0.0f;
        // �m�F�p���݂̃^�C���ϐ�������������
        Current_Time                    = 0.0f;
        // ���̃J�E���g�ϐ� ������������
        Clap_Count                      = 0;

        // �j�Q�[�WUI�摜��RectTransform��"Z"��]�l������������
        Needle_Gauge_Image_AngleZ       = 45.0f;
        // �j�Q�[�WUI�摜��RectTransform��Rotate"Z"�l
        Needle_Gauge_Image_RotateZ      = -157.5f;
        // �j�Q�[�WUI�摜��RectTransform�̉�]�l������������
        Vector3 Angle;
        Angle.x = Needle_Gauge_Image_Rect.eulerAngles.x;
        Angle.y = 0.0f;
        Angle.z = Needle_Gauge_Image_RotateZ;
        Needle_Gauge_Image_Rect.eulerAngles = Angle;
        // Z��]�l��ۑ�����
        Save_AngleZ = Needle_Gauge_Image_RotateZ;
        // �j�Q�[�W�̌o�ߎ��Ԃ�����������
        Needle_Gauge_ElapsedTime = 0.0f;
        // �j�Q�[�W���ω����^�U�t���O������������
        Needle_Gauge_Is_Changing = false;
        // �j�Q�[�WUI�摜��Color������������
        Needle_Gauge_Image_Color.color = new Color(1, 1, 1, 1);

        // ���Y���Q�[�WUI�摜��RectTransform��"X"�ړ��l������������
        Rhythm_Gauge_Image_PosX = 300.0f;
        // ���Y���Q�[�WUI�摜��RectTransform�̉�]�l������������
        Vector3 Pos;
        Pos.x =    0.0f;
        Pos.y =  432.0f;
        Pos.z = -200.0f;
        Rhythm_Gauge_Image_Rect.anchoredPosition = Pos;
        // ���Y���Q�[�W�̌o�ߎ��Ԃ�����������
        Rhythm_Gauge_ElapsedTime = 0.0f;
        // ���Y���Q�[�W���ω����^�U�t���O������������
        Rhythm_Gauge_Is_Changing = false;
        // ���Y���Q�[�W�̈ړ��J�E���g������������
        Rhythm_Transition_Count = 0;

        // ���Y�����쐬���^�U�t���O������������
        Succeed_Flag = false;
        // ���Y�����쐬�����̎��Ԃ�����������
        Save_Succeed_Time = 0.0f;

        // �Q�[���J�n����̔��̃^�C�~���O
        GameStart_ClapCount = 5;

        beat = false;
        StartCoroutine("Beat");
    }

    private IEnumerator Beat()
    {
        int count = 0;
        beat = true;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            count++;
            if(count == 30)
            {
                beat = true;
                count = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //********************************************//
        //<<����̂ݔ��̊Ԋu�����؂��ĕb�����擾����>>//
        //********************************************//
        {
            // ���̃^�C�~���O������ȊO�����擾����
            // "true" ...���̃^�C�~���O�^
            // "false"...���̃^�C�~���O�U
            if (Music.IsJustChangedBeat())
            {
                // �J�E���g��" "�̎��̂݌��؊J�n
                Clap_Count++;
            }
            // ���ؒ��̂݃^�C�}�[�N��
            if (Clap_Count == 3) Clap_Time += Time.deltaTime;

            // �Ȍ�AClap_Time�����Ɣ��܂ł̕b���ɂȂ�
        }

        // �t���O�������Ă���Ԃ�"Lerp"�œ�����
        if (Needle_Gauge_Is_Changing)
        {
            // �^�C�}�[�N��
            Needle_Gauge_ElapsedTime += Time.deltaTime;
            float Current_Time  = Mathf.Clamp01(Needle_Gauge_ElapsedTime / Clap_Time);
            float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);

            // ��]�֐�
            UI_Rotate(Current_Value);

            // ����l�܂ōs���΃t���O�I��
            if (Current_Time >= 1f) Needle_Gauge_Is_Changing = false;
        }

        // �j�Q�[�WUI�摜����]������ & �Q�[���J�n����̔��̃^�C�~���O
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount)   
        {
            // �ω����J�n����
            Needle_Gauge_Is_Changing = true;
            Needle_Gauge_ElapsedTime = 0f;
        }


        // �t���O�������Ă���Ԃ�"Lerp"�œ�����
        if (Rhythm_Gauge_Is_Changing)
        {
            // �^�C�}�[�N��
            Rhythm_Gauge_ElapsedTime += Time.deltaTime;
            float Current_Time  = Mathf.Clamp01(Rhythm_Gauge_ElapsedTime / (Clap_Time / 2.0f));
            float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);

            // �ړ��֐�
            UI_Position(Current_Value, Rhythm_Transition_Count);

            // ����l�܂ōs���΃t���O�I��
            if (Current_Time >= 1f && (Rhythm_Transition_Count == 1 || Rhythm_Transition_Count == 3))
            {
                Rhythm_Gauge_ElapsedTime = 0f;
            }
            else if (Current_Time >= 1f && (Rhythm_Transition_Count == 0 || Rhythm_Transition_Count == 2))
            {
                Rhythm_Gauge_Is_Changing = false;
            }
        }
        // ���Y���Q�[�WUI�摜���ړ������� & �Q�[���J�n����̔��̃^�C�~���O
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount) 
        {
            // �ω����J�n����
            Rhythm_Gauge_Is_Changing = true;
            Rhythm_Gauge_ElapsedTime = 0f;
        }

        // ���Y���摜�̃��Y�����o
        if (beat)
        {
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f) .SetEase(Ease.InQuad) .SetLoops(2, LoopType.Yoyo);
        }

        //***************************************//
        //***<< ���쐬�� or ���쎸�s�@�̉��o>>***//
        //***************************************//

        // "����"
        if      ((Rhythm_Gauge_Image_Rect.anchoredPosition.x >= -50.0f && Rhythm_Gauge_Image_Rect.anchoredPosition.x <= 50.0f) && (Input.GetKeyDown(KeyCode.Joystick1Button0) || /*�m�F�p*/ Input.GetKeyDown(KeyCode.A)))
        {
            // ���쐬���ɂ���
            Succeed_Flag = true;
            // ���݂̎��Ԃ�ۑ�����
            Save_Succeed_Time = Current_Time;
            // ���o
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo);
            Needle_Gauge_Image_Color.color = new Color(0.4f, 1, 1, 1);

        }
        // "���s"
        else if ((Rhythm_Gauge_Image_Rect.anchoredPosition.x < -50.0f || Rhythm_Gauge_Image_Rect.anchoredPosition.x > 50.0f) && (Input.GetKeyDown(KeyCode.Joystick1Button0) || /*�m�F�p*/ Input.GetKeyDown(KeyCode.A))) 
        {
            Needle_Gauge_Image_Color.color = new Color(1, 0.45f, 0.4f, 1);
        }
        // �L�[���͂����ꂽ��F�����ɖ߂�
        else if(Input.GetKeyUp(KeyCode.Joystick1Button0) || /*�m�F�p*/ Input.GetKeyUp(KeyCode.A))
        {
            Needle_Gauge_Image_Color.color = new Color(1, 1, 1, 1);
        }

        // ���쐬�������莞�Ԍo�߂�����
        if (Succeed_Flag && Current_Time >= Save_Succeed_Time + (Clap_Time / 2.0f)) 
        {
            Succeed_Flag = false;
        }


        // ���݂̎��Ԃ��m�F�p�^�C�}�[�N��
        Current_Time += 1.0f * Time.deltaTime;

        if (beat) beat = false;
    }





    // ���Y��UI�摜�̃X�P�[����ύX
    private void OnScale(float value)
    {
        var Scale = Mathf.Lerp(1, Succeed_Flag ? 1.7f : 1.2f, value);
        Rhythm_Image_Rect.localScale = new Vector3(Scale, Scale, Scale);
    }

    // ���̊Ԋu�̕b�����擾����֐�
    public float GetClapSecond() { return Clap_Time; }

    // ���݂̉�]�ԍ����擾����֐�
    public int GetCurrentNumber() { return Needle_Gauge_Number; }


    // �j�Q�[�WUI����]������֐�
    private void UI_Rotate(float timer)
    {
        var Angle = Mathf.Lerp(Save_AngleZ, Save_AngleZ - Needle_Gauge_Image_AngleZ, timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);

        // �p�x(Angle)��-180�`180�x�͈̔͂ɐ��K������
        var NormalizedAngleZ = Mathf.Repeat(Angle + 180, 360) - 180;

        // 45�x��]������ ���� ���̃^�C�~���O�������狭���I��
        if (Angle == Save_AngleZ - Needle_Gauge_Image_AngleZ || Music.IsJustChangedBeat()) 
        {
            // �ēx���݂̊p�x��ۑ�����
            Save_AngleZ = Save_AngleZ - Needle_Gauge_Image_AngleZ;
            // ���݂̊p�x�ԍ���ݒ肷��
            if (Needle_Gauge_Number == 7) Needle_Gauge_Number = 0;
            else                          Needle_Gauge_Number++;
        }
    }

    // ���Y���Q�[�WUI���ړ�������֐�
    private void UI_Position(float timer, int count)
    {
        //************************//
        //<<�J�E���g�ŕ��򂳂���>>//
        //************************//
        if      (count == 0) 
        {
            var Position = Mathf.Lerp(0.0f, -300.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == -300.0f) Rhythm_Transition_Count = 1;
        }
        else if (count == 1)
        {
            var Position = Mathf.Lerp(-300.0f, 0.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == 0.0f || Music.IsJustChangedBeat()) Rhythm_Transition_Count = 2;
        }
        else if (count == 2)
        {
            var Position = Mathf.Lerp(0.0f, 300.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == 300.0f) Rhythm_Transition_Count = 3;
        }
        else if (count == 3)
        {
            var Position = Mathf.Lerp(300.0f, 0.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == 0.0f || Music.IsJustChangedBeat()) Rhythm_Transition_Count = 0;
        }
    }
}
