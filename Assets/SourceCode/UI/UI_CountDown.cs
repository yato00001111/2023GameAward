using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CountDown : MonoBehaviour
{

    [SerializeField]
    private Animator CountDown_Animation;          // �J�E���g�_�E���A�j���[�V����

    [SerializeField]
    private AnimationClip CountDown_AnimationClip; // �J�E���g�_�E���A�j���[�V�����N���b�v

    [SerializeField]
    private int Animation_Start_Count;             // �J�E���g�_�E���A�j���[�V�����J�n�܂ł̃J�E���g
                                                   
    [SerializeField]                               
    private bool Game_Start_Flag;                  // �Q�[���J�n�t���O
                                                   
    [SerializeField]                               
    public float currentTime;                      // ���݂̃J�E���g�_�E���A�j���[�V�����̍Đ�����

    [SerializeField]
    private bool Tutorial_Start_Flag;              // �`���[�g���A���J�n�t���O


    // Start is called before the first frame update
    void Start()
    {
        // �J�E���g�_�E���A�j���[�V����������������
        CountDown_Animation = GetComponent<Animator>();
        // �J�E���g�_�E���A�j���[�V�����J�n�܂ł̃J�E���g������������
        Animation_Start_Count = 0;
        // �Q�[���J�n�t���O������������
        Game_Start_Flag = false;

        Tutorial_Start_Flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Music.IsJustChangedBeat())
        {
            // �J�E���g��" "�̎��̂݃A�j���[�V�����J�n
            Animation_Start_Count++;
        }

        // �Q�[����ʂɓ����Ĉ�莞�Ԍo�߂�����J�E���g�_�E���A�j���[�V�����J�n
        if (Animation_Start_Count >= 3)
        {
            CountDown_Animation.SetBool("CountDown_Flag", true);

            // �^�C�}�[�N��
            currentTime += Time.deltaTime;
        }

        // �A�j���[�V�����Đ����Ԃ�1�ɂȂ�΃Q�[���J�n�t���O�𗧂Ă�
        if (currentTime >= CountDown_AnimationClip.length) Game_Start_Flag = true;
        if(Tutorial_Start_Flag) Game_Start_Flag = true;
    }

    // �Q�[���J�n�t���O���擾�֐�
    public bool GetGameStartFlag() { return Game_Start_Flag; }

    public void SetTutorialStartFlag(bool flag) { Tutorial_Start_Flag = flag; }
}
