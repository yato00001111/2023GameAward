using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Needles : MonoBehaviour
{
    [SerializeField]
    private float Clap_Time;                           // ���̊Ԋu�̕b��
                                                       
    [SerializeField]                                   
    private float Current_Time;                        // �m�F�p���݂̃^�C��
                                                       
    [SerializeField]                                   
    private int Clap_Count;                            // ���̃J�E���g�ϐ�

    [SerializeField]
    private RectTransform Needle_Gauge_Image_Rect;     // �j�Q�[�WUI�摜��RectTransform
    [SerializeField]                                   
    private float Needle_Gauge_Image_AngleZ;           // �j�Q�[�WUI�摜��RectTransform��"Z"��]�l
    [SerializeField]                                   
    private float Needle_Gauge_Image_RotateZ;          // �j�Q�[�WUI�摜��RectTransform�̏���Rotate"Z"�l
    [SerializeField]                                   
    private float Save_AngleZ;                         // �O���"Z"��]�l��ۑ�����ϐ�
    [SerializeField]                                   
    private int Needle_Gauge_Number;                   // �j�Q�[�W�̌��݂̊p�x�ԍ�
    [SerializeField]                                   
    private float Needle_Gauge_ElapsedTime;            // �j�Q�[�W�̌o�ߎ���
    [SerializeField]                                   
    private bool Needle_Gauge_Is_Changing;             // �j�Q�[�W���ω����^�U�t���O


    [SerializeField]
    private RectTransform[] Rhythm_Gauge_Image_Rect    // ���Y���Q�[�WUI�摜��RectTransform
        = new RectTransform[3];                        
    [SerializeField]                                   
    private GameObject[] Rhythm_Gauge_Image_OBJ        // ���Y���Q�[�WUI�摜��RectTransform
        = new GameObject[3];                           
    [SerializeField]                                   
    private float Rhythm_Gauge_Image_PosX;             // ���Y���Q�[�WUI�摜��RectTransform��"X"�ړ��l
    [SerializeField]                                   
    private float Rhythm_Gauge_ElapsedTime;            // ���Y���Q�[�W�̌o�ߎ���
    [SerializeField]                                   
    private bool  Rhythm_Gauge_Is_Changing;            // ���Y���Q�[�W���ω����^�U�t���O
    [SerializeField]                                   
    private int   Rhythm_Transition_Count;             // ���Y���Q�[�W�̈ړ��J�E���g
                                                       
    [SerializeField]                                   
    private RectTransform Rhythm_Image_Rect;           // ���Y��UI�摜��RectTransform
    [SerializeField]                                   
    private bool Succeed_Flag;                         // ���Y�����쐬���^�U�t���O
    [SerializeField]                                   
    private float Save_Succeed_Time;                   // ���Y�����쐬�����̎���
                                                       
                                                       
    [SerializeField]                                   
    private RectTransform Phase_Gauge_Image_Rect;      // �t�F�[�Y�Q�[�WUI�摜��RectTransform
                                                       
    [SerializeField]                                   
    private int GameStart_ClapCount;                   // �Q�[���J�n����̔��̃^�C�~���O
                                                       
    [SerializeField]                                   
    private AudioSource Game_BGM;                      // �Q�[����BGM
                                                       
    [SerializeField]                                   
    private bool Beat_Flag;                            // ���̃^�C�~���O�t���O
                                                       
    [SerializeField]                                   
    public UI_CountDown uI_CountDown;                  // �Q�[���J�n�J�E���g�_�E���X�N���v�g
                                                       
    [SerializeField]                                   
    public UI_Dead_Gauge uI_Dead_Gauge;                // ���S�Q�[�W�X�N���v�g
                                                       
    [SerializeField]                                   
    private bool _Disappear_Phase_Flag;                // ������t�F�[�Y�t���O

    [SerializeField]
    private bool Disappear_Roatet_End_Flag;            // ������t�F�[�Y��360��]�̏I���t���O

    [SerializeField]
    private AudioClip SE_Metronome;                    // ���g���m�[�����ʉ�
    [SerializeField] PlayDirector playDirector = default!;

    [SerializeField]
    private AudioSource audioSource;                   // �I�[�f�B�I�\�[�X

    [SerializeField]
    private AudioSource BGM_AudioSource;               // BGM�̃I�[�f�B�I�\�[�X

    [SerializeField]
    private float BGM_Time;                            // ���݂�BGM�̎���

    [SerializeField]
    private float BGM_Length;                          // BGM�̒���


    [SerializeField] UI_Rythm_Effect uiRythmEffect = default!;

    [SerializeField]
    private int justTiming;                   // ���U���g��ʂɓn��

    bool beat;
    bool TriggerFlag;                                   // �g���K�[�̒������������Ȃ�����p

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

        // ���Y���Q�[�WUI�摜��RectTransform��"X"�ړ��l������������
        Rhythm_Gauge_Image_PosX = 300.0f;
        // ���Y���Q�[�WUI�摜��RectTransform�̈ʒu������������
        Vector3 Pos;
        for (int Num = 0; Num < 3; Num++) 
        {
            Pos.x = 0.0f;
            Pos.y = 432.0f;
            Pos.z = -200.0f;
            Rhythm_Gauge_Image_Rect[Num].anchoredPosition = Pos;
        }
        // ���Y���Q�[�W�̑��ݐ^�U������������
        Rhythm_Gauge_Image_OBJ[0].SetActive(true);
        Rhythm_Gauge_Image_OBJ[1].SetActive(false);
        Rhythm_Gauge_Image_OBJ[2].SetActive(false);
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

        // ���̃^�C�~���O�t���O������������
        Beat_Flag = false;

        // ������t�F�[�Y�̏����t���O������������
        _Disappear_Phase_Flag = false;

        // ������t�F�[�Y��360��]�̏I���t���O������������
        Disappear_Roatet_End_Flag = false;

        //Component���擾
        audioSource = GetComponent<AudioSource>();

        // ���݂�BGM�̎��Ԃ�����������
        BGM_Time = 0.0f;

        // BGM�̒���������������
        BGM_Length = 0.0f;


        justTiming = 0;

        TriggerFlag = false;

        // �X�^�[�g�R���`��
        StartCoroutine("BeatPlay");
    }

    private void Awake()
    {
        // �ڕW�t���[�����[�g��60�ɐݒ�
        Application.targetFrameRate = 60; 
    }

    private IEnumerator BeatPlay()
    {
        int Count = 0;
        Beat_Flag = true;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            // 0.02�b��
            Count++;
            if (Count == 30)
            {
                Beat_Flag = true;
                Count = 0;
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
            if (Beat_Flag)
            {
                // �J�E���g��" "�̎��̂݌��؊J�n
                Clap_Count++;
            }
            // ���ؒ��̂݃^�C�}�[�N��
            if (Clap_Count == 2) Clap_Time += Time.deltaTime;

            // �Ȍ�AClap_Time�����Ɣ��܂ł̕b���ɂȂ�
        }

        // ���0.6�b�ɒ�������
        Clap_Time = 0.6f;

        // �ŏ��ɋl�܂�ׁA�X�N���v�g������Đ�����
        if (Clap_Count == 3) Game_BGM.Play();
        // BGM�̃^�C�}�[�N��
        if (Clap_Count > 3) BGM_Time += Time.deltaTime;

        BGM_Length = Game_BGM.clip.length;

        if (Game_BGM.clip.length <= BGM_Time && Beat_Flag/* && Rhythm_Transition_Count == 0*/)
        {
            // �j�̈ړ��ԍ����Z�b�g
            Rhythm_Transition_Count = 0;
            // BGM�ēx�Đ�
            Game_BGM.Play();
            // �^�C�}�[���Z�b�g
            BGM_Time = 0.0f;
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
            if (Current_Time >= 1f || _Disappear_Phase_Flag) Needle_Gauge_Is_Changing = false;
        }

        // �j�Q�[�WUI�摜����]������ & �Q�[���J�n����̔��̃^�C�~���O
        if (Beat_Flag && Clap_Count > GameStart_ClapCount && uI_CountDown.GetGameStartFlag() && !_Disappear_Phase_Flag)      
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
        if (Beat_Flag && Clap_Count > GameStart_ClapCount && uI_CountDown.GetGameStartFlag())  
        {
            // �ω����J�n����
            Rhythm_Gauge_Is_Changing = true;
            Rhythm_Gauge_ElapsedTime = 0f;
        }

        // ���Y���摜�̃��Y�����o
        if (Rhythm_Gauge_Image_Rect[0].anchoredPosition.x == 0.0f && !Succeed_Flag && uI_CountDown.GetGameStartFlag()) 
        {
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo);
        }

        //***************************************//
        //***<< ���쐬�� or ���쎸�s�@�̉��o>>***//
        //***************************************//
        float TrigerInput = Input.GetAxisRaw("Trigger");
        if (TriggerFlag)
        {
            if (TrigerInput == 0) TriggerFlag = false;
        }

        // "����"
        if (!_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && (playDirector.GetPlayFlag())
            && (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick1Button5) || (TrigerInput < 0.0f && !TriggerFlag )|| (TrigerInput > 0.0f && !TriggerFlag)
            || /*�m�F�p*/ Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            // ���쐬���ɂ���
            Succeed_Flag = true;
            // ���݂̎��Ԃ�ۑ�����
            Save_Succeed_Time = Current_Time;
            // ���o
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo);
            Rhythm_Gauge_Image_OBJ[0].SetActive(false);
            Rhythm_Gauge_Image_OBJ[1].SetActive(false);
            Rhythm_Gauge_Image_OBJ[2].SetActive(true);

            //
            uiRythmEffect.PlayEffect(true);

            justTiming++;
            TriggerFlag = true;
        }
        // "���s"
        else if (!_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && (!playDirector.GetPlayFlag())
            && (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick1Button5) || (TrigerInput < 0.0f && !TriggerFlag) || (TrigerInput > 0.0f && !TriggerFlag) || /*�m�F�p*/ Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Rhythm_Gauge_Image_OBJ[0].SetActive(false);
            Rhythm_Gauge_Image_OBJ[1].SetActive(true);
            Rhythm_Gauge_Image_OBJ[2].SetActive(false);

            uiRythmEffect.PlayEffect(false);
            TriggerFlag = true;
        }
        // �L�[���͂����ꂽ��F�����ɖ߂�
        else if (!_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.Joystick1Button5) || TrigerInput == 0.0f || /*�m�F�p*/ Input.GetKeyUp(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow)
            || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow)) 
        {
            Rhythm_Gauge_Image_OBJ[0].SetActive(true);
            Rhythm_Gauge_Image_OBJ[1].SetActive(false);
            Rhythm_Gauge_Image_OBJ[2].SetActive(false);
        }

        // ���쐬�������莞�Ԍo�߂�����
        if (Succeed_Flag && Current_Time >= Save_Succeed_Time + (Clap_Time / 2.0f)) 
        {
            Succeed_Flag = false;
        }

        // ������t�F�[�Y�p�̉�]����
        if (uI_Dead_Gauge.GetIsDisappearPhaseFlag() && _Disappear_Phase_Flag && !Disappear_Roatet_End_Flag)  
        {
            // �^�C�}�[�N��
            Needle_Gauge_ElapsedTime += Time.deltaTime;
            float Current_Time  = Mathf.Clamp01(Needle_Gauge_ElapsedTime / (Clap_Time * 8));
            float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);

            // ��]�֐�
            UI_Rotate(Current_Value);
        }

        // true�̃^�C�~���O��false�ɂ���
        if (Beat_Flag) Beat_Flag = false;

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
        var Angle = Mathf.Lerp(Save_AngleZ, Save_AngleZ - ((uI_Dead_Gauge.GetIsDisappearPhaseFlag() && _Disappear_Phase_Flag) ? 360.0f : Needle_Gauge_Image_AngleZ), timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);
        // �p�x(Angle)��-180�`180�x�͈̔͂ɐ��K������
        var NormalizedAngleZ = Mathf.Repeat(Angle + 180, 360) - 180;

        // 45�x��]������ ���� ���̃^�C�~���O�������狭���I��
        if (!_Disappear_Phase_Flag && (Angle == Save_AngleZ - Needle_Gauge_Image_AngleZ || Beat_Flag)) 
        {
            // �ēx���݂̊p�x��ۑ�����
            Save_AngleZ = Save_AngleZ - Needle_Gauge_Image_AngleZ;
            // ���݂̊p�x�ԍ���ݒ肷��
            if (Needle_Gauge_Number == 7) Needle_Gauge_Number = 0;
            else                          Needle_Gauge_Number++;
        }

        // �j�̊p�x�ԍ��ݒ�
        if(_Disappear_Phase_Flag)
        {
            if      (Angle <= -157.5f && Angle > Save_AngleZ - 45.0f * 1) Needle_Gauge_Number = 0;
            else if (Angle <= -202.5f && Angle > Save_AngleZ - 45.0f * 2) Needle_Gauge_Number = 1;
            else if (Angle <= -247.5  && Angle > Save_AngleZ - 45.0f * 3) Needle_Gauge_Number = 2;
            else if (Angle <= -292.5f && Angle > Save_AngleZ - 45.0f * 4) Needle_Gauge_Number = 3;
            else if (Angle <= -337.5f && Angle > Save_AngleZ - 45.0f * 5) Needle_Gauge_Number = 4;
            else if (Angle <= -382.5f && Angle > Save_AngleZ - 45.0f * 6) Needle_Gauge_Number = 5;
            else if (Angle <= -427.5f && Angle > Save_AngleZ - 45.0f * 7) Needle_Gauge_Number = 6;
            else if (Angle <= -472.5f && Angle > Save_AngleZ - 45.0f * 8) Needle_Gauge_Number = 7;
        }

        // 360�x��]�����狭���I��
        if (_Disappear_Phase_Flag && (Angle == Save_AngleZ - 360.0f)) 
        {
            // �ēx���݂̊p�x��ۑ�����
            Save_AngleZ = Save_AngleZ - 360.0f;
            // ������t�F�[�Y��360��]�̏I���t���O�𗧂Ă�
            Disappear_Roatet_End_Flag = true;
            
        }
    }

    // ���Y���Q�[�WUI���ړ�������֐�
    private void UI_Position(float timer, int count)
    {
        //************************//
        //<<�J�E���g�ŕ��򂳂���>>//
        //************************//
        if (count == 0)
        {
            var Position = Mathf.Lerp(-300.0f, 0.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == 0.0f) Rhythm_Transition_Count = 1;
        }
        else if (count == 1)
        {
            var Position = Mathf.Lerp(0.0f, 300.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == 300.0f || Beat_Flag)
            {
                //��(Metronome)��炷
                audioSource.PlayOneShot(SE_Metronome);
                Rhythm_Transition_Count = 2;
            }

        }
        else if (count == 2)
        {
            var Position = Mathf.Lerp(300.0f, 0.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == 0.0f) Rhythm_Transition_Count = 3;

        }
        else if (count == 3)
        {
            var Position = Mathf.Lerp(0.0f, -300.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // �w��ʒu�ɓ��B�����烊�Y���Q�[�W�ړ��J�E���g�ݒ�
            if (Position == -300.0f || Beat_Flag)
            {
                //��(Metronome)��炷
                audioSource.PlayOneShot(SE_Metronome);
                Rhythm_Transition_Count = 0;
            }
        }

    }

    // �j�̈ʒu��"0"�Ԗڂ̈ʒu�ɐݒ肷��֐�
    public void SetNeedlePos()
    {
        // ��]�l�Œ�
        Vector3 Angle;
        Angle.x = Needle_Gauge_Image_Rect.eulerAngles.x;
        Angle.y = 0.0f;
        Angle.z = Needle_Gauge_Image_RotateZ;
        Needle_Gauge_Image_Rect.eulerAngles = Angle;
        // �p�x�ۑ�����
        Save_AngleZ = Needle_Gauge_Image_RotateZ;
        // �J�E���g���Z�b�g
        Needle_Gauge_Number = 0;
        // ������t�F�[�Y�t���O�𗧂Ă�
        _Disappear_Phase_Flag = true;
        // �^�C�}�[���Z�b�g
        Needle_Gauge_ElapsedTime = 0.0f;
        // ������t�F�[�Y��360��]�̏I���t���O�����Z�b�g
        Disappear_Roatet_End_Flag = false;
    }

    // ������t�F�[�Y�t���O�����Z�b�g����
    public void ResetDisappearPhaseFlag() { 
        _Disappear_Phase_Flag = false;
        // �p�x�ԍ����Z�b�g
        Needle_Gauge_Number = 0;

        playDirector.EnableSpawn(true);

    }

    public int GetJustTiming()
    {
        return justTiming;
    }
}
