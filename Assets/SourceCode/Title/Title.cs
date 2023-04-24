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
        // マウス左クリックで
        if (Input.GetMouseButtonDown(0))
        {
            // 1度SEを再生
            audioSource.PlayOneShot(TitleSE);
            // アニメーション再生フラグをtrue
            animator.SetTrigger("OUT_Animation");
            // シーン遷移関数を呼ぶ
            StartCoroutine("LoadGameScene");
        }
    }

    IEnumerator LoadGameScene()
    {

        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("GameScene");
    }
}
