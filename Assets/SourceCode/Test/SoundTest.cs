using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SoundTest : MonoBehaviour
{
    // �f�o�b�O�p�e�L�X�g
    public Text TestText;
    public int addText;

    // 1���ړ��B�ʒm
    private bool FirstBeat = false;

    // ����\�t���O
    public bool PlayFlag;
    // ���ɗ������_�ŋN������^�C�}�[
    public float BeatTimer;

    public bool PenaltyFlag;

    // �����r�[�g
    public bool EvenBeat;
    // ��r�[�g
    public bool OddBeat;

    public int BeatCount;
    bool Pena1;
    bool Pena2;


    // Start is called before the first frame update
    void Start()
    {
        addText = 0;
        TestText.text = "0";
        BeatTimer = 0.0f;
        PlayFlag = false;

        PenaltyFlag = false;
        BeatCount = 0;
        OddBeat = false;
        EvenBeat = false;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TestText.text = addText.ToString();


        // �����Ƃ�true
        if (Music.IsJustChangedBeat())
        {
            // �Q�[���J�n�t���O�N��
            FirstBeat = true;
            BeatCount += 1;
            //addText += 1;
        }

        // �Q�[�����N�������甏�Ԃ̎��Ԃ��v��
        if (FirstBeat) BeatTimer += Time.deltaTime;

        // ���̑O��0.15�t���[���͑�����󂯕t����
        if (!PenaltyFlag)
        {
            if (BeatTimer > 0.0f && BeatTimer < 0.15f)
            {
                PlayFlag = true;
            }
            if (BeatTimer > 0.45f && BeatTimer < 0.6f)
            {
                PlayFlag = true;
            }
            else
            {
                PlayFlag = false;
            }
        }

        // ���̔��ɍs������^�C�}�[�����Z�b�g
        if (BeatTimer > 0.6f)
        {
            BeatTimer = 0.0f;
        }

        PenaltyMethod();

        // ���u�� �w��t���[����������\
        if (PlayFlag && Input.GetMouseButtonDown(0)) addText += 1;
    }


    void PenaltyMethod()
    {
        // ��������\�t���[���ȊO�ő��삪�s��ꂽ��
        if (!PlayFlag && Input.GetMouseButtonDown(0))
        {
            // �y�i���e�B����
            PenaltyFlag = true;
        }

        // ���݂̔�������������𔻒�
        if (BeatCount % 2 == 0)
        {
            EvenBeat = true;
            OddBeat = false;
        }
        else
        {
            OddBeat = true;
            EvenBeat = false;
        }


        // �������������̎��Ƀy�i���e�B������������
        if (PenaltyFlag && EvenBeat)
        {
            // �����p�y�i���e�B�t���O�𗧂Ă�
            Pena2 = true;
        }
        // ����������̎��Ƀy�i���e�B������������
        if (PenaltyFlag && OddBeat)
        {
            // ��p�y�i���e�B�t���O�𗧂Ă�
            Pena1 = true;
        }

        // �����y�i���e�B(����)���������Ă��������ɉ���
        if (Pena2 && OddBeat)
        {
            PenaltyFlag = false;
            Pena2 = false;
        }
        // ���̋t
        if (Pena1 && EvenBeat)
        {
            PenaltyFlag = false;
            Pena1 = false;
        }
    }
}
// SceneGameBGM ����0.6frame
// TitleBGM     ����0.56frame