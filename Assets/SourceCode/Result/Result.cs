using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Result : MonoBehaviour
{
    // �X�L�b�v�J�E���g
    [HideInInspector]
    public int _SkipCount = 0;
    
    // �I�u�W�F�N�g���X�g
    enum ObjList
    {
        Score,      // �X�R�A
        Wave,       // �E�F�[�u
        Combo,      // �R���{
        JustTiming, // �W���X�g�^�C�~���O

        MAX         // ���X�g��
    };

    // �X�R�A
    [SerializeField]
    GameObject _Score;

    // �E�F�[�u
    [SerializeField]
    GameObject _Wave;

    // �R���{
    [SerializeField]
    GameObject _Combo;

    // �W���X�g�^�C�~���O
    [SerializeField]
    GameObject _Just_Timing;

    // �I�u�W�F�N�g�̔z��
    [HideInInspector]
    public GameObject[] OBJList = new GameObject[(int)ObjList.MAX];

    // �g�����W�V����
    [SerializeField]
    New_Transition _transition;

    // ���g���C�{�^��
    [SerializeField]
    GameObject RetryButton;

    // �^�C�g���{�^��
    [SerializeField]
    GameObject TitleButton;

    // �^�C�}�[
    private float _Timer = 0f;

    // ���͒x���^�C�}�[
    [SerializeField]
    private float _lateTimer = 0f;

    // �v���C���[�̓��͎擾
    [SerializeField, HideInInspector]
    KeyCode PlayerInput = 0;

    // �{�^�������t���O
    [SerializeField]
    private bool PushFlg = false;

    // �X�L�b�v�֐��z��
    public delegate void Delegate(GameObject gameObject);
    Delegate[] SkipFunctions = new Delegate[2];

    // ������
    public void Start()
    {
        // �֐��z��̐ݒ�
        SkipFunctions[0] = SkipAnimation;
        SkipFunctions[1] = SkipCount;

        // �I�u�W�F�N�g�z��
        OBJList[(int)ObjList.Score]      = _Score;
        OBJList[(int)ObjList.Wave]       = _Wave;
        OBJList[(int)ObjList.Combo]      = _Combo;
        OBJList[(int)ObjList.JustTiming] = _Just_Timing;
    }

    // �X�V����
    public void Update()
    {
        // �J�ڃA�j���[�V�������I����Ă���
        if (_transition.GetEndINTransition())
        {
            // �X�R�A�̃A�j���[�V�����Đ�
            _Score.GetComponent<Animator>().SetBool("StartAnimation", true);

            // �^�C�}�[���P�b�ȏ�@�܂��́@�X�R�A�̃J�E���g���X�L�b�v����Ă�����
            if (_Timer > 1f || _SkipCount > 1)
                // �E�F�[�u�A�j���[�V�����Đ�
                _Wave.GetComponent<Animator>().SetBool("StartAnimation", true);

            // �^�C�}�[���Q�b�ȏ�@�܂��́@�E�F�[�u�̃J�E���g���X�L�b�v����Ă�����
            if (_Timer > 2f || _SkipCount > 3)
                // �R���{�A�j���[�V�����Đ�
                _Combo.GetComponent<Animator>().SetBool("StartAnimation", true);

            // �^�C�}�[���R�b�ȏ�@�܂��́@�R���{�̃J�E���g���X�L�b�v����Ă�����
            if (_Timer > 3f || _SkipCount > 5)
            {
                // �W���X�g�^�C�~���O�A�j���[�V�����Đ�
                _Just_Timing.GetComponent<Animator>().SetBool("StartAnimation", true);
            }

            // �X�L�b�v����
            Skip();

            // �^�C�}�[����
            _Timer += Time.deltaTime;
        }

        // �S�ẴJ�E���g���I����Ă�����
        if (_Score.transform.GetChild(1).GetComponent<Number>().GetEndCount() &&
            _Wave.transform.GetChild(1).GetComponent<Number>().GetEndCount() &&
            _Combo.transform.GetChild(1).GetComponent<Number>().GetEndCount() &&
            _Just_Timing.transform.GetChild(1).GetComponent<Number>().GetEndCount()
            )
        {
            // �J�ڃ{�^���\��
            RetryButton.SetActive(true);
            TitleButton.SetActive(true);

            if (_lateTimer > 3f)
            {
                PushFlg = true;
            }

            // A�{�^��(Retry)�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) && PushFlg)
            {
                // ��x�����̐���
                PushFlg = false;

                PlayerInput = KeyCode.Joystick1Button0;

                // �J�ڃA�j���[�V�����Đ�
                _transition.Start_OUTanimation();
            }

            //B�{�^��(Title)�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Joystick1Button1) && PushFlg)
            {
                // ��x�����̐���
                PushFlg = false;

                PlayerInput = KeyCode.Joystick1Button1;

                // �J�ڃA�j���[�V�����Đ�
                _transition.Start_OUTanimation();
            }

            _lateTimer += Time.deltaTime;
        }

        // �J�ڃA�j���[�V�������I�������
        if (_transition.GetEndOUTTransition())
        {
            // �v���C���[�̓��͂ɉ����đJ�ڕύX
            if (PlayerInput == KeyCode.Joystick1Button0)
                SceneManager.LoadScene("GameScene");

            if (PlayerInput == KeyCode.Joystick1Button1)
                SceneManager.LoadScene("Title");
        }
    }

    // �X�L�b�v����
    private void Skip()
    {
        // ���N���b�N�����Ƃ� �܂��́@�����ꂩ�̃{�^�����������Ƃ�
        if( 
            Input.GetKeyDown(KeyCode.Joystick1Button0) ||
            Input.GetKeyDown(KeyCode.Joystick1Button1) ||
            Input.GetKeyDown(KeyCode.Joystick1Button2) ||
            Input.GetKeyDown(KeyCode.Joystick1Button3) ||
            Input.GetKeyDown(KeyCode.Joystick1Button4) ||
            Input.GetKeyDown(KeyCode.Joystick1Button5)
            )
        {
            // �֐��̔z��̒����� _SkipCount �̐��l�ɉ����ā@�Ăяo���֐��A<=�֐��̈����@��ύX���Ċ֐����Ă�
            SkipFunctions[(_SkipCount % 2)](OBJList[(int)_SkipCount / 2]); 
        }
    }

    // �J�E���g���X�L�b�v����
    private void SkipCount(GameObject gameObject)
    {
        // �q����Number�X�N���v�g���擾
        Number obj = gameObject.transform.GetChild(1).GetComponent<Number>();

        // �J�E���g�㏸����������
        if (!obj.GetEndCount())
        {
            // �J�E���g�I���֐����Ă�
            obj.Finish();

            // �N���b�N�������ăG���[���o���Ȃ��悤�ɐ���
            if (_SkipCount < 7) _SkipCount++;
        }
    }

    // �Đ����̃A�j���[�V�������I���܂ŃX�L�b�v����
    private void SkipAnimation(GameObject gameObject)
    {
        // �A�j���[�V�����R���|�[�l���g�擾
        Animator animator = gameObject.GetComponent<Animator>();

        // �Đ����̃A�j���[�V�������擾
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // �A�j���[�V�������Đ�����Ă�����
        if (stateInfo.length > 0)
        {
            //�Đ����Ԃ�0�`1�͈̔͂Ȃ̂ŋ����I�ɂP�ɂ���
            animator.Play(stateInfo.fullPathHash, 0, 1);

            // �N���b�N�������ăG���[���o���Ȃ��悤�ɐ���
            if (_SkipCount < 7) _SkipCount++;
        }
    }

    // �ݒ�֐�
    public void SetResultNumber(int FinalScore, int Wave, int  Combo, int Just_Timing)
    {
        _Score.transform.GetComponent<Number>().SetNumber(FinalScore);
        _Wave.transform.GetComponent<Number>().SetNumber(Wave);
        _Combo.transform.GetComponent<Number>().SetNumber(Combo);
        _Just_Timing.transform.GetComponent<Number>().SetNumber(Just_Timing);
    }
}
