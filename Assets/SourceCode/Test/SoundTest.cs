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

    // Start is called before the first frame update
    void Start()
    {
        addText = 0;
        TestText.text = "0";
        BeatTimer = 0.0f;
        PlayFlag = false;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        TestText.text = addText.ToString();


        // �����Ƃ�true
        if (Music.IsJustChangedBeat())
        {
            // �Q�[���J�n�t���O�N��
            FirstBeat = true;
            //addText += 1;
        }
        // �� 32frame ��

        // �Q�[�����N�������甏�Ԃ̎��Ԃ��v��
        //if (FirstBeat) BeatTimer++;
        if (FirstBeat) BeatTimer += Time.deltaTime;

        // ���̑O��0.048�t���[���͑�����󂯕t����
        if ((BeatTimer > 0.000f && BeatTimer < 0.112f))// || (BeatTimer > 0.448f && BeatTimer < 0.56f))
        {
            PlayFlag = true;
        }
        else if((BeatTimer > 0.448f && BeatTimer < 0.56f))
        {
            PlayFlag = true;
        }
        else
        {
            PlayFlag = false;
        }

        // ���̔��ɍs������^�C�}�[�����Z�b�g
        if (BeatTimer > 0.56f)
        {
            BeatTimer = 0.0f;
        }




        // ���u�� �w��t���[����������\
        if (PlayFlag && Input.GetMouseButtonDown(0)) addText += 1;
    }
}
