using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Animator animator;
    AudioSource audioSource;
    public AudioClip TitleSE;

    // UI Press A
    public GameObject PressA;
    // TransitionMenu (UI Frame,GameStart,Exit
    public GameObject TransitionMenu;
    // UI Frame
    public GameObject SelectFrame;

    bool SelectActiveFlag;

    public bool OnExit;
    public bool OnGameStart;

    public float Deray;

    public int SelectCount;

    // パッドの入力値
    private float PadVertical;

    private static int Tutorial = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PressA.SetActive(true);
        TransitionMenu.SetActive(false);
        SelectCount = 0;

        OnGameStart = true;

        Deray = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

        PadVertical = Input.GetAxis("D_Pad");

        // エンターキーまたはAボタンで選択フェーズへ
        if ((Input.GetKeyDown(KeyCode.Return) && !SelectActiveFlag) ||
            (Input.GetKeyDown(KeyCode.Joystick1Button0) && !SelectActiveFlag))
        {
            PressA.SetActive(false);
            TransitionMenu.SetActive(true);

            SelectActiveFlag = true;
        }

        if (SelectActiveFlag)
        {
            Deray -= 0.05f;
            if (Deray < 0.0)
            {
                Deray = 0.0f;
                TitleUIAction();
                FunctionTitleUI();
            }
        }
    }

    // 選択フレームUIの動き
    void TitleUIAction()
    {
        if (SelectCount == 0 && Input.GetKeyDown(KeyCode.DownArrow) ||
            SelectCount == 0 && PadVertical < 0)
        {
            OnGameStart = false;
            OnExit = true;
            SelectCount = 1;
        }
        if (SelectCount == 1 && Input.GetKeyDown(KeyCode.UpArrow) ||
            SelectCount == 1 && PadVertical > 0)
        {
            OnGameStart = true;
            OnExit = false;
            SelectCount = 0;
        }
    }

    // UIの機能
    void FunctionTitleUI()
    {
        if (OnExit)
        {
            SelectFrame.transform.position = new Vector3(960, 220, 0);
            if (Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                Application.Quit();
            }
        }
        if (OnGameStart)
        {
            SelectFrame.transform.position = new Vector3(960, 340, 0);

            if (Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                //1度SEを再生
                audioSource.PlayOneShot(TitleSE);
                //アニメーション再生フラグをtrue
                animator.SetTrigger("OUT_Animation");
                //シーン遷移関数を呼ぶ
                StartCoroutine("LoadGameScene");
            }
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
