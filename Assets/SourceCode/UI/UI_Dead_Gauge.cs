using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Dead_Gauge : MonoBehaviour
{

    [SerializeField]
    private RectTransform Dead_Gauge_Image_Rect;          // ���S�Q�[�WUI�摜��RectTransform

    [SerializeField]
    private int Clap_Count;                               // ���̃J�E���g�ϐ�

    [SerializeField]
    private int Scale_Change_Count;                       // ���S�Q�[�WUI�摜�̃X�P�[���ύX�J�E���g�ϐ�

    [SerializeField]
    private int Start_Count;                              // �Q�[���J�n�܂ł̃J�E���g�ϐ�

    [SerializeField]
    private float Dead_Gauge_ScaleX;                      // ���S�Q�[�WUI�摜�̉����ϐ�

    [SerializeField]
    private bool Is_Disappear_Phase_Flag;                 // ������t�F�[�Y�t���O

    [SerializeField]
    private bool Is_Disappear_Phase_End_Flag;             // ������t�F�[�Y�̏I���t���O

    [SerializeField]
    private int Disappear_Phase_Clap_Count;               // ������t�F�[�Y���̔��̃J�E���g�ϐ�

    [SerializeField]
    private RectTransform Revers_Image_Rect;              // �F�ʔ��]UI�摜��RectTransform
    [SerializeField]
    private float Revers_Image_Scale;                     // �F�ʔ��]UI�摜�̃T�C�Y

    [SerializeField]
    private RectTransform Revers_Image_Rect2;             // �F�ʔ��]UI�摜��RectTransform(2����)
    [SerializeField]
    private float Revers_Image_Scale2;                    // �F�ʔ��]UI�摜�̃T�C�Y(2����)

    [SerializeField]
    public UI_CountDown uI_CountDown;                     // �Q�[���J�n�J�E���g�_�E���X�N���v�g

    [SerializeField]
    public UI_Needles uI_Needles;                         // �j�̃X�N���v�g

    [SerializeField]
    private float ElapsedTime;                            // ���S�Q�[�W�̏k���o�ߎ���

    [SerializeField]
    private int Quota_Count;                              // �m���}��

    [SerializeField]
    public UI_Objective_Quota uI_Objective_Quota;         // �m���}���X�N���v�g

    [SerializeField]
    private bool Beat_Flag;                               // ���̃^�C�~���O�t���O

    [SerializeField]
    private bool Before_Is_Disappear_Phase_Flag;

    [SerializeField]
    private PlayDirector playDirector;

    [SerializeField]
    private FieldController _fieldController;

    private const int normaSpeed = 8;

    // Start is called before the first frame update
    void Start()
    {
        // ���S�Q�[�WUI�摜�̉����ϐ�������������
        Dead_Gauge_ScaleX = 0.0f;
        // ���̃J�E���g�ϐ�������������
        Clap_Count = 0;
        // �Q�[���J�n�܂ł̃J�E���g�ϐ�������������
        Start_Count = 5;
        // ������t�F�[�Y���̔��̃J�E���g�ϐ�������������
        Disappear_Phase_Clap_Count = 0;
        // �X�P�[��������������
        Dead_Gauge_Image_Rect.localScale = new Vector3(0, 1, 1);

        // �F�ʔ��]�摜�̃X�P�[������������
        Revers_Image_Rect. localScale = new Vector3(0, 0, 0);
        Revers_Image_Rect2.localScale = new Vector3(0, 0, 0);
        // �F�ʔ��]UI�摜�̃T�C�Y�ϐ�������������
        Revers_Image_Scale  = 0.0f;
        Revers_Image_Scale2 = 0.0f;
        // ������t�F�[�Y�t���O������������
        Is_Disappear_Phase_Flag = false;
        // ������t�F�[�Y�̏I���t���O������������
        Is_Disappear_Phase_End_Flag = false;
        // ���S�Q�[�W�̏k���o�ߎ��Ԃ�����������
        ElapsedTime = 0.0f;
        // �m���}��������������
        Quota_Count = 1;

        // ���̃^�C�~���O�t���O������������
        Beat_Flag = false;

        //
        Before_Is_Disappear_Phase_Flag = false;

        // �X�^�[�g�R���`��
        StartCoroutine("BeatPlay");
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
        // ��������x�J�E���g����
        if (Beat_Flag) Clap_Count++;

        // �m���}�A�C�R���X�V
        uI_Objective_Quota.SetObjectiveQuota(Quota_Count);

        // 8�񔏂����邽�тɃX�P�[����1�i�K���₷ & ������t�F�[�Y�ɂȂ�܂�
        if ((Beat_Flag && (Clap_Count - Start_Count) % normaSpeed == 0) && !Is_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && Dead_Gauge_ScaleX < 1.0f)   
        {
            // ���o
            DOTween
              .To(value => OnScale(value, Scale_Change_Count * 0.125f), 0, 1, 0.25f).SetEase(Ease.InOutQuad);
        }

        // ������t�F�[�Y�ɓ������������t�F�[�Y�t���O�𗧂Ă�
        if (Scale_Change_Count == 8 || Dead_Gauge_ScaleX >= 1.0f) 
        {
            // 1���ڂ̐F�ʔ��]�摜�̃T�C�Y��傫�����Ă���
            if      (Revers_Image_Scale <  20.0f) Revers_Image_Scale += 0.25f;
            // 1���ڂ̐F�ʔ��]�摜�̃T�C�Y���ő�l�܂ł�����2���ڂ̐F�ʔ��]�摜�̃T�C�Y���傫�����Ă���
            else if (Revers_Image_Scale >= 20.0f && Revers_Image_Scale2 < 15.0f) Revers_Image_Scale2 += 0.25f;          
        }

        if(Dead_Gauge_ScaleX >= 1.0f && !Before_Is_Disappear_Phase_Flag) Before_Is_Disappear_Phase_Flag = true;

        // �F�ʔ��]���o���ɐj�̏ꏊ���ړ�������
        if ((Revers_Image_Scale2 > 2.97f && Revers_Image_Scale2 < 10.1f) && !Is_Disappear_Phase_End_Flag) uI_Needles.SetNeedlePos();

        // �F�ʔ��]���o���I�����������t�F�[�Y�ɓ��� �t���O�𗧂Ă�
        if (Revers_Image_Scale2 >= 15.0f) Is_Disappear_Phase_Flag = true;

        // ������t�F�[�Y���̔��̐����J�E���g����
        if (Is_Disappear_Phase_Flag && Beat_Flag) Disappear_Phase_Clap_Count++;

        // ������t�F�[�Y���͎��S�Q�[�W�̃X�P�[�������炷
        if (Is_Disappear_Phase_Flag)
        {
            // �^�C�}�[�N��
            ElapsedTime += Time.deltaTime;
            float Current_Time = Mathf.Clamp01(ElapsedTime / 4.8f);
            // ���o
            OnScale(Current_Time, 0);
        }

        // ������t�F�[�Y�̏I�����m�点���󂯎������
        if (Is_Disappear_Phase_End_Flag)
        {
            // 2���ڂ̐F�ʔ��]�摜�̃T�C�Y�����������Ă���
            if      (Revers_Image_Scale2 > 0.0f) Revers_Image_Scale2 -= 0.25f;
            // 2���ڂ̐F�ʔ��]�摜�̃T�C�Y���ŏ��l�܂ł�����1���ڂ̐F�ʔ��]�摜�̃T�C�Y�����������Ă���
            else if (Revers_Image_Scale2 <= 0.0f && Revers_Image_Scale > 0.0f) Revers_Image_Scale -= 0.25f;

            // �F�ʔ��]���o���I�������
            if (Revers_Image_Scale <= 0.0f) 
            {
                // �j�̃X�N���v�g���̏�����t�F�[�Y�I����m�点��
                uI_Needles.ResetDisappearPhaseFlag();
                // �t���O�֘A�����Z�b�g����
                Is_Disappear_Phase_Flag     = false;
                Is_Disappear_Phase_End_Flag = false;
                // �^�C�}�[���Z�b�g
                ElapsedTime = 0.0f;

                _fieldController._normaCount -= (Quota_Count);

                // �m���}�𑝂₷
                Quota_Count++;
            }
            Before_Is_Disappear_Phase_Flag = false;
            playDirector.SetStateErase(false);
        }

        // true�̃^�C�~���O��false�ɂ���
        if (Beat_Flag) Beat_Flag = false;

        // �F�ʔ��]�摜�̃T�C�Y�ݒ�
        Revers_Image_Rect.localScale  = new Vector3(Revers_Image_Scale,  Revers_Image_Scale,  Revers_Image_Scale);
        Revers_Image_Rect2.localScale = new Vector3(Revers_Image_Scale2, Revers_Image_Scale2, Revers_Image_Scale2);
    }

    // ���Y��UI�摜�̃X�P�[����ύX
    private void OnScale(float value,float scale_X)
    {
        // ������t�F�[�Y���Ȃ�360�x��]�@����ȊO�͒ʏ�ʂ�45�x��]
        var Scale = Is_Disappear_Phase_Flag ? Mathf.Lerp(1.0f, 0.0f, value) : Mathf.Lerp(scale_X, scale_X + 0.125f, value);
        // �X�P�[�����f
        Dead_Gauge_Image_Rect.localScale = new Vector3(Scale, 1, 1);
        // �X�P�[���ύX�J�E���g
        if (!Is_Disappear_Phase_Flag && Scale == scale_X + 0.125f) 
        {
            // �J�E���g
            Scale_Change_Count++;
            // �X�P�[���l�ۑ�����
            Dead_Gauge_ScaleX = Scale;
        }
        // ������t�F�[�Y���Ȃ烊�Z�b�g����
        if (Is_Disappear_Phase_Flag && Scale == 0.0f) 
        {
            // �J�E���g���Z�b�g
            Scale_Change_Count = 0;
            // �X�P�[���l�ۑ�����
            Dead_Gauge_ScaleX = Scale;
            // ������t�F�[�Y�̏I���t���O�𗧂Ă�
            Is_Disappear_Phase_End_Flag = true;
        }
    }


    // ������t�F�[�Y�^�U�t���O�擾�֐�
    public bool GetIsDisappearPhaseFlag()    { return Is_Disappear_Phase_Flag; }

    public bool GetBeforeIsDisappearPhaseFlag()    { return Before_Is_Disappear_Phase_Flag; }
    public void SetBeforeIsDisappearPhaseFlag(bool flag)    { Before_Is_Disappear_Phase_Flag = flag; }

}
