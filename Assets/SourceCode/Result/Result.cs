using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    // �X�R�A
    [SerializeField]
    GameObject _Score;

    // ���x��
    [SerializeField]
    GameObject _Level;

    // �R���{
    [SerializeField]
    GameObject _Combo;

    // ����
    [SerializeField]
    GameObject _Time;

    // �g�����W�V����
    [SerializeField]
    Result_Transition _transition;

    // �^�C�}�[
    private float _Timer = 0f;

    // ������
    private void Start()
    {
        // �J�ڃA�j���[�V�����J�n
        _transition.Start_INanimation();
    }

    // �X�V����
    public void Update()
    {
        // �J�ڃA�j���[�V�������I����Ă��ā@�S�ẴJ�E���g���I����Ă��Ȃ�������
        if (_transition.GetEndOUTTransition() && 
            !_Score.transform.GetChild(1).GetComponent<ScoreNumber>().GetEndCount() &&
            !_Level.transform.GetChild(1).GetComponent<LevelNumber>().GetEndCount() &&
            !_Combo.transform.GetChild(1).GetComponent<ComboNumber>().GetEndCount() &&
            !_Time.transform.GetChild(1).GetComponent<TimeNumber>().GetEndCount()
            )

        {
            _Score.GetComponent<Animator>().SetBool("StartAnimation", true);

            if (_Timer > 1f)
                _Level.GetComponent<Animator>().SetBool("StartAnimation", true);

            if (_Timer > 2f)
                _Combo.GetComponent<Animator>().SetBool("StartAnimation", true);

            if (_Timer > 3f)
                _Time.GetComponent<Animator>().SetBool("StartAnimation", true);

            // �^�C�}�[����
            _Timer += Time.deltaTime;
        }

        // �S�ẴJ�E���g���I����Ă�����
        if (_Score.transform.GetChild(1).GetComponent<ScoreNumber>().GetEndCount() &&
            _Level.transform.GetChild(1).GetComponent<LevelNumber>().GetEndCount() &&
            _Combo.transform.GetChild(1).GetComponent<ComboNumber>().GetEndCount() &&
            _Time.transform.GetChild(1).GetComponent<TimeNumber>().GetEndCount()
            )
        {
            // �J�ڃA�j���[�V�����Đ�
            _transition.Start_OUTanimation();
        }

        // �J�ڃA�j���[�V�������I�������
        if (_transition.GetEndINTransition())
        {
            // �V�[���J��
            //SceneManager.LoadScene("ResultScene");
        }
    }
}
