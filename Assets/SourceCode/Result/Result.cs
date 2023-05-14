using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Result : MonoBehaviour
{
    // �X�L�b�v�J�E���g
    private int _SkipCount = 0;
    
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
    private GameObject[] OBJList = new GameObject[(int)ObjList.MAX];

    // �g�����W�V����
    [SerializeField]
    ResultTransition _transition;

    // �p�[�e�B�N��
    [SerializeField]
    GameObject Particle;

    // �^�C�}�[
    private float _Timer = 0f;

    // �X�L�b�v�֐��z��
    public delegate void Delegate(GameObject gameObject);
    Delegate[] SkipFunctions = new Delegate[2];

    // ������
    private void Start()
    {
        // �J�ڃA�j���[�V�����J�n
        _transition.Start_INanimation();

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
            if (_Timer > 1f || _SkipCount > 2)
                // �E�F�[�u�A�j���[�V�����Đ�
                _Wave.GetComponent<Animator>().SetBool("StartAnimation", true);

            // �^�C�}�[���Q�b�ȏ�@�܂��́@�E�F�[�u�̃J�E���g���X�L�b�v����Ă�����
            if (_Timer > 2f || _SkipCount > 4)
                // �R���{�A�j���[�V�����Đ�
                _Combo.GetComponent<Animator>().SetBool("StartAnimation", true);

            // �^�C�}�[���R�b�ȏ�@�܂��́@�R���{�̃J�E���g���X�L�b�v����Ă�����
            if (_Timer > 3f || _SkipCount > 6)
                // �W���X�g�^�C�~���O�A�j���[�V�����Đ�
                _Just_Timing.GetComponent<Animator>().SetBool("StartAnimation", true);

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
            // �J�ڃA�j���[�V�����Đ�
            _transition.Start_OUTanimation();

            // �p�[�e�B�N�����B��
            Particle.SetActive(false);
        }

        // �J�ڃA�j���[�V�������I�������
        if (_transition.GetEndOUTTransition())
        {
            // �V�[���J��
            //SceneManager.LoadScene("");
        }
    }

    // �X�L�b�v����
    private void Skip()
    {
        // ���N���b�N�����Ƃ�
        if(Input.GetMouseButtonDown(0))
        {
            // �֐��̔z��̒����� _SkipCount �̐��l�ɉ����ā@�Ăяo���֐��A<=�֐��̈����@��ύX���Ċ֐����Ă�
            SkipFunctions[(_SkipCount % 2)](OBJList[(int)_SkipCount / 2]); 
            // �N���b�N�������ăG���[���o���Ȃ��悤�ɐ���
             if(_SkipCount < 7) _SkipCount++;
        }
    }

    // �J�E���g���X�L�b�v����
    public static void SkipCount(GameObject gameObject)
    {
        // �q����Number�X�N���v�g���擾
        Number obj = gameObject.transform.GetChild(1).GetComponent<Number>();
        // �J�E���g�I���֐����Ă�
        obj.Finish();
    }

    // �Đ����̃A�j���[�V�������I���܂ŃX�L�b�v����
    public static void SkipAnimation(GameObject gameObject)
    {
        // �A�j���[�V�����R���|�[�l���g�擾
        Animator animator = gameObject.GetComponent<Animator>();

        // �Đ����̃A�j���[�V�������擾
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //�Đ����Ԃ�0�`1�͈̔͂Ȃ̂ŋ����I�ɂP�ɂ���
        animator.Play(stateInfo.fullPathHash, 0, 1);
    }
}
