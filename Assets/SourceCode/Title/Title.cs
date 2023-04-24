using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    public Animator animator;
    AudioSource audioSource;
    public AudioClip TitleSE;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // �}�E�X���N���b�N��
        if (Input.GetMouseButtonDown(0))
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

        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("GameScene");
    }
}
