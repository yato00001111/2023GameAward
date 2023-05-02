using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level : MonoBehaviour
{

    [SerializeField]
    private float WaveTimer = 0.0f;       // �E�F�[�u�^�C�}�[

    [SerializeField]
    private int Now_Level = 0;            // ���݂̃��x��

    [SerializeField]                      
    private Image WaveFrame_Image;        // �E�F�[�u�t���[���摜
                                          
    [SerializeField]                      
    private Image WaveGauge_Image;        // �E�F�[�u�Q�[�W�摜

    [SerializeField]
    private RectTransform WaveGauge_Rect; // �E�F�[�u�Q�[�WRectTransform

    [SerializeField]
    private Image[] LevelNumber_Images
        = new Image[11];                  // ���݂̃��x���ԍ��摜

    [SerializeField]
    private Image[] LevelGauge_Images
    = new Image[11];                      // ���݂̃��x���Q�[�W�摜


    // Start is called before the first frame update
    void Start()
    {
        // �摜�����ݒ�
        //WaveFrame_Image = GetComponent<Image>();
        //WaveGauge_Image = GetComponent<Image>();

        // �E�F�[�u�Q�[�W�̏�������"0"�ɂ���
        // �ő�l��"266"
        // WaveGauge_Rect  = gameObject.Find("Wave").GetComponent<RectTransform>();
        WaveGauge_Rect.sizeDelta = new Vector2(0.0f, 9.0f);

        // ��Փx������
        Now_Level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // �E�F�[�u�^�C�}�[����
        UpdateWaveTimer();

        // �E�F�[�u�Q�[�W�̕�����
        UpdateWaveGauge();

        // ���x���֘A���o����
        UpdateLevelPerform();
    }

    void UpdateWaveTimer()
    {
        // �E�F�[�u�^�C���N��
        WaveTimer += Time.deltaTime;

        // 20�b�o�ߌ�
        if(WaveTimer>20.0f)
        {
            // �^�C�}�[�N��
            WaveTimer = 0.0f;

            //***<<���x���A�b�v>>***//
            Now_Level += 1;
        }
    }

    void UpdateWaveGauge()
    {
        // �ő啝�܂ŐL�΂�
        float Width = 13.3f * WaveTimer;
        WaveGauge_Rect.sizeDelta = new Vector2(Width, 9.0f);
    }


    void UpdateLevelPerform()
    {
        // ���݂̃��x���֘A�̉摜�`�揈��
        for (int Num = 0; Num < 11; ++Num) 
        {
            //***<<���x���ԍ�>>***//
            // �`�悷��
            if (Num == Now_Level - 1) 
            {
                LevelNumber_Images[Num].enabled = true;
            }
            // �`�悵�Ȃ�
            else
            {
                LevelNumber_Images[Num].enabled = false;
            }

            //***<<���x���Q�[�W>>***//
            // �`�悷��

            LevelGauge_Images[]

            if (Num == Now_Level - 1)
            {
                LevelNumber_Images[Num].enabled = true;
            }
            // �`�悵�Ȃ�
            else
            {
                LevelNumber_Images[Num].enabled = false;
            }
        }
       
    }
}
