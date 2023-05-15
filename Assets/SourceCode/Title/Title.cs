using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    public Animator animator;
    AudioSource audioSource;
    public AudioClip TitleSE;

    private static int Tutorial = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // �}�E�X���N���b�N��
        if (Input.GetMouseButtonDown(0)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            // 1�xSE���Đ�
            audioSource.PlayOneShot(TitleSE);
            // �A�j���[�V�����Đ��t���O��true
            animator.SetTrigger("OUT_Animation");
            // �V�[���J�ڊ֐����Ă�
            StartCoroutine("LoadGameScene");
        }
    }

    IEnumerator LoadGameScene()
    {
        if(Tutorial == 0)
        {
            Tutorial = 1;
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene("GameScene");
        }
    }
}
