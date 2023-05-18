using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject prefabMessage = default!;
    [SerializeField] GameObject gameObjectCanvas = default!;
    [SerializeField] PlayDirector playDirector = default!;
    [SerializeField] UI_CountDown uiCountDown = default!;
    [SerializeField] UI_Objective_Quota uiObjectiveQuota = default!;
    [SerializeField] UI_Needles uiNeedles = default!;
    GameObject _message = null;

    public static int normaClear;
    public static int justTiming;

    //SE
    AudioSource audioSource;
    public AudioClip se_gameover;
    private double FadeOutSeconds = 1.0;
    bool IsFadeOut = true;
    double FadeDeltaTime = 0;

    // 画面にでる演出メッセージの表示
    void CreateMessage(string message)
    {
        Debug.Assert(_message == null);
        _message = Instantiate(prefabMessage, Vector3.zero, Quaternion.identity,
            gameObjectCanvas.transform);
        _message.transform.localPosition = new Vector3(0, 0, 0);// 画面中心に配置

        _message.GetComponent<TextMeshProUGUI>().text = message;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        IsFadeOut = true;
        FadeDeltaTime = 0;
        StartCoroutine("GameFlow");
        normaClear = 0;
        justTiming = 0;
    }

    private IEnumerator GameFlow()
    {
        //CreateMessage("Ready?");

        //yield return new WaitForSeconds(2.0f);
        //Destroy(_message); _message = null;

        while(!uiCountDown.GetGameStartFlag())
        {
            yield return null;
        }
        

        playDirector.EnableSpawn(true);// プレイ開始

        while (!playDirector.IsGameOver())// 終了待ち
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        CreateMessage("Game Over");
        audioSource.PlayOneShot(se_gameover);
        if (IsFadeOut)
        {
            FadeDeltaTime += Time.deltaTime;
            if (FadeDeltaTime > FadeOutSeconds)
            {
                FadeDeltaTime = FadeOutSeconds;
                IsFadeOut = false;
            }
            audioSource.volume = (float)(FadeDeltaTime / FadeOutSeconds);
        }

        normaClear = uiObjectiveQuota.GetObjectiveQuota() - 1;
        justTiming = uiNeedles.GetJustTiming();

        //while (!Input.anyKey)// 何か押すのを待つ
        //{
        //    yield return null;
        //}

        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("ResultScene");
    }
}