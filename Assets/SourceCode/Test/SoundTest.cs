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
    int PenaltyCount;


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

        PenaltyCount = 0;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        StartCoroutine("BeatPlay");
    }




    private IEnumerator BeatPlay()
    {
        while (true)
        {
            PlayFlag = true;
            if (PenaltyFlag)
            {
                PlayFlag = false;
                PenaltyCount++;
            }
            yield return new WaitForSeconds(0.15f);
            PlayFlag = false;
            yield return new WaitForSeconds(0.3f);
            PlayFlag = true;
            if (PenaltyFlag)
            {
                PlayFlag = false;
                PenaltyCount++;
            }
            yield return new WaitForSeconds(0.15f);
        }

    }




    // Update is called once per frame
    void FixedUpdate()
    {

        if (!PlayFlag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �y�i���e�B����
                PenaltyFlag = true;
            }
        }
        if (PenaltyCount == 2)
        {
            PenaltyFlag = false;
            PenaltyCount = 0;
        }

        TestText.text = addText.ToString();

        //// ���u�� �w��t���[����������\
        if (PlayFlag && Input.GetMouseButtonDown(0)) addText += 1;
    }
}
// SceneGameBGM ����0.6frame
// TitleBGM     ����0.56frame