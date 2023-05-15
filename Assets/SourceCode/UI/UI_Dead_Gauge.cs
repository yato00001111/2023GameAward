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

    // Start is called before the first frame update
    void Start()
    {
        // ���S�Q�[�WUI�摜�̉����ϐ�������������
        Dead_Gauge_ScaleX = 0.0f;
        // ���̃J�E���g�ϐ�������������
        Clap_Count = 0;
        // �Q�[���J�n�܂ł̃J�E���g�ϐ�������������
        Start_Count = 5;

        // �X�P�[��������������
        Dead_Gauge_Image_Rect.localScale = new Vector3(0, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // ��������x�J�E���g����
        if (Music.IsJustChangedBeat()) Clap_Count++;

        // 8�񔏂����邽�тɃX�P�[����1�i�K���₷
        if (Music.IsJustChangedBeat() && (Clap_Count - Start_Count) % 8 == 0) 
        {
            // ���o
            DOTween
              .To(value => OnScale(value, Scale_Change_Count * 0.125f), 0, 1, 1.0f).SetEase(Ease.InOutQuad);
        }

    }

    // ���Y��UI�摜�̃X�P�[����ύX
    private void OnScale(float value,float scale_X)
    {
        var Scale = Mathf.Lerp(scale_X, scale_X + 0.125f, value);
        Dead_Gauge_Image_Rect.localScale = new Vector3(Scale, 1, 1);
        // �X�P�[���ύX�J�E���g
        if (Scale == scale_X + 0.125f) Scale_Change_Count++;
    }
}
