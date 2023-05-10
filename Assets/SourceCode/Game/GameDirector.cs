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
    GameObject _message = null;

    //SE
    AudioSource audioSource;
    public AudioClip se_gameover;
    private double FadeOutSeconds = 1.0;
    bool IsFadeOut = true;
    double FadeDeltaTime = 0;

    // ��ʂɂł鉉�o���b�Z�[�W�̕\��
    void CreateMessage(string message)
    {
        Debug.Assert(_message == null);
        _message = Instantiate(prefabMessage, Vector3.zero, Quaternion.identity,
            gameObjectCanvas.transform);
        _message.transform.localPosition = new Vector3(0, 0, 0);// ��ʒ��S�ɔz�u

        _message.GetComponent<TextMeshProUGUI>().text = message;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        IsFadeOut = true;
        FadeDeltaTime = 0;
        StartCoroutine("GameFlow");
    }

    private IEnumerator GameFlow()
    {
        CreateMessage("Ready?");

        yield return new WaitForSeconds(2.0f);
        Destroy(_message); _message = null;

        playDirector.EnableSpawn(true);// �v���C�J�n

        while (!playDirector.IsGameOver())// �I���҂�
        {
            yield return null;
        }

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

        while (!Input.anyKey)// ���������̂�҂�
        {
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Title");
    }
}