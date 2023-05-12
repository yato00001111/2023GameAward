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
    private float Needle_Gauge_Image_AngleZ;       // �j�Q�[�WUI�摜��RectTransform��"Z"��]�l
    [SerializeField]
    private float Needle_Gauge_Image_RotateZ;      // �j�Q�[�WUI�摜��RectTransform�̏���Rotate"Z"�l
    [SerializeField]
    private float Save_AngleZ;                     // �O���"Z"��]�l��ۑ�����ϐ�
    [SerializeField]
    private float Current_AngleZ;                  // ���݂�"Z"��]�l��ۑ�����ϐ�


    [SerializeField]
    private RectTransform Rhythm_Gauge_Image_Rect; // ���Y���Q�[�WUI�摜��RectTransform
    [SerializeField]
    private float Rhythm_Gauge_Image_PosX;         // ���Y���Q�[�WUI�摜��RectTransform��"X"�ړ��l
    [SerializeField]
    private float Rhythm_Gauge_Image_PositionX;    // ���Y���Q�[�WUI�摜��RectTransform�̏���Position"X"�l
    [SerializeField]                               
    private float Rhythm_Save_PositionX;           // �O���"X"�ړ��l��ۑ�����ϐ�
    [SerializeField]                               
    private float Rhythm_Current_PositionX;        // ���݂�"X"�ړ��l��ۑ�����ϐ�


    [SerializeField]
    private float Inside_Clap_Time;                // �����̃^�C��
    [SerializeField]
    private bool Inside_Clap_Setting_Flag;         // �����ݒ�t���O

    [SerializeField]
    private RectTransform Phase_Gauge_Image_Rect;  // �t�F�[�Y�Q�[�WUI�摜��RectTransform

    [SerializeField]
    private int Number;
    [SerializeField]
    private int Number_Count;

    [SerializeField]
    private int GameStart_ClapCount;               // �Q�[���J�n����̔��̃^�C�~���O


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


        // ���Y���Q�[�WUI�摜��RectTransform��"X"�ړ��l������������
        Rhythm_Gauge_Image_PosX      = 300.0f;
        // ���Y���Q�[�WUI�摜��RectTransform��Position"X"�l
        Rhythm_Gauge_Image_PositionX = 000.0f;
        // ���Y���Q�[�WUI�摜��RectTransform�̉�]�l������������
        Vector3 Pos;
        Pos.x = Rhythm_Gauge_Image_PositionX;
        Pos.y =  432.0f;
        Pos.z = -200.0f;
        Rhythm_Gauge_Image_Rect.anchoredPosition = Pos;
        // X�ړ��l��ۑ�����
        Rhythm_Save_PositionX = Rhythm_Gauge_Image_PositionX;


        // �Q�[���J�n����̔��̃^�C�~���O
        GameStart_ClapCount = 5;
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
        }

        // �j�Q�[�WUI�摜����]������ & �Q�[���J�n����̔��̃^�C�~���O
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount)   
        {
            DOTween
             .To(angle => UI_Rotate(angle), 0, 1, Clap_Time).SetEase(Ease.InOutQuad);
        }

        // ���Y���Q�[�WUI�摜���ړ������� & �Q�[���J�n����̔��̃^�C�~���O & �J�E���g�������̎�
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount && Clap_Count % 2 == 0) 
        {
            DOTween
             .To(position => UI_PositionL(position), 0, 1, Clap_Time / 2.0f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo);
        }
        // ���Y���Q�[�WUI�摜���ړ������� & �Q�[���J�n����̔��̃^�C�~���O & �J�E���g�������̎�
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount && Clap_Count % 2 != 0)
        {
            DOTween
             .To(position => UI_PositionR(position), 0, 1, Clap_Time / 2.0f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo);
        }

        // �m�F�p�^�C�}�[�N��
        Current_Time = 1.0f * Time.deltaTime;


        // �����֘A����
        {
            // ��������
            if (Clap_Count == 4 && !Inside_Clap_Setting_Flag)
            {
                Inside_Clap_Time += (Clap_Time / 2.0f);
            }

            // �����^�C�}�[�N��
            Inside_Clap_Time += Time.deltaTime;
        }


        // ���݂̊p�x�ԍ���ݒ肷��
        {
            if      (Number_Count == 0) Number = 7;
            else if (Number_Count == 1) Number = 0;
            else if (Number_Count == 2) Number = 1;
            else if (Number_Count == 3) Number = 2;
            else if (Number_Count == 4) Number = 3;
            else if (Number_Count == 5) Number = 4;
            else if (Number_Count == 6) Number = 5;
            else if (Number_Count == 7) Number = 6;
        }

    }

    // ���̊Ԋu�̕b�����擾����֐�
    public float GetClapSecond() { return Clap_Time; }

    // ���݂̉�]�ԍ����擾����֐�
    public int GetCurrentNumber() { return Number; }


    // UI����]������֐�
    private void UI_Rotate(float timer)
    {
        var Angle = Mathf.Lerp(Save_AngleZ, Save_AngleZ - Needle_Gauge_Image_AngleZ, timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);

        // �p�x(Angle)��-180�`180�x�͈̔͂ɐ��K������
        var NormalizedAngleZ = Mathf.Repeat(Angle + 180, 360) - 180;
        // ���݂̊p�x��ۑ�����
        Current_AngleZ = NormalizedAngleZ;

        // 45�x��]������
        if (Angle == Save_AngleZ - Needle_Gauge_Image_AngleZ)
        {
            // �ēx���݂̊p�x��ۑ�����
            Save_AngleZ = Save_AngleZ - Needle_Gauge_Image_AngleZ;
            if (Number_Count == 7) Number_Count = 0;
            else Number_Count++;
        }
    }

    // UI���ړ�������֐�
    private void UI_PositionL(float timer)
    {
        var Position = Mathf.Lerp(Rhythm_Save_PositionX, Rhythm_Save_PositionX - Rhythm_Gauge_Image_PosX, timer);
        Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 432.0f - 200.0f);
    }

    private void UI_PositionR(float timer)
    {
        var Position = Mathf.Lerp(Rhythm_Save_PositionX, Rhythm_Save_PositionX + Rhythm_Gauge_Image_PosX, timer);
        Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 432.0f - 200.0f);
    }
}
