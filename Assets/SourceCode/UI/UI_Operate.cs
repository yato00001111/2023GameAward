using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Operate : MonoBehaviour
{

    [SerializeField]
    private Image[] Operate1_Image = new Image[3];      // クイックドロップ操作方法UI画像

    [SerializeField]
    private Image[] Operate2_Image = new Image[3];      // 大回転操作方法UI画像

    [SerializeField]
    private Image[] Operate3_Image = new Image[3];      // 半回転操作方法UI画像

    [SerializeField]
    private Animator Operate1_Animator;                 // クイックドロップ操作方法UIのアニメーション

    [SerializeField]
    private float Operate2_Image_Angle;                 // 大回転操作方法UI画像の回転値

    [SerializeField]
    private float Operate3_Image_Angle;                 // 半回転操作方法UI画像の回転値

    [SerializeField]
    private RectTransform Operate2_Image_Rect;          // 大回転操作方法UI画像のRectTransform
                                                        
    [SerializeField]                                    
    private RectTransform Operate3_Image_Rect;          // 半回転操作方法UI画像のRectTransform




    // Start is called before the first frame update
    void Start()
    {
        // 回転値変数を初期化する
        Operate2_Image_Angle = 0.0f;
        Operate3_Image_Angle = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // XBoxコントローラーの"A"ボタンが押されている間は"クイックドロップ"UI演出
        if (Input.GetKey(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKey(KeyCode.A)) 
        {
            // 操作中画像のみを描画する
            Operate1_Image[1].enabled = true;
            Operate1_Image[2].enabled = true;
            // 通常画像は描画しない
            Operate1_Image[0].enabled = false;
            
        }
        // XBoxコントローラーの"A"ボタンが押されていない間は通常画像を描画
        else
        {
            // 通常画像のみを描画する
            Operate1_Image[0].enabled = true;
            // 操作中画像は描画しない
            Operate1_Image[1].enabled = false;
            Operate1_Image[2].enabled = false;

            // アニメーションの再生時間を"0"にする
            Operate1_Animator.Play(0, -1, 0f);
        }

        // XBoxコントローラーの"LT RT"ボタンが押されている間は"大回転"UI演出
        if (/*(Input.GetKey(KeyCode.Joystick1Button11) || Input.GetKey(KeyCode.Joystick1Button14)) ||*/ /*確認用*/ Input.GetKey(KeyCode.S)) 
        {
            // 操作中画像のみを描画する
            Operate2_Image[1].enabled = true;
            Operate2_Image[2].enabled = true;
            // 通常画像は描画しない
            Operate2_Image[0].enabled = false;

            // 押されている間、回転値を増やす
            Operate2_Image_Angle = Time.deltaTime * -200.0f;
            // 大回転操作方法UIアイコンを回転させる
            // その際、角度(Operate2_Image_Angle)を0～360度の範囲に正規化する
            var Normalized_Operate2_Angle = Mathf.Repeat(Operate2_Image_Angle, 360);
            Operate2_Image_Rect.Rotate(.0f, .0f, Normalized_Operate2_Angle);
        }
        // XBoxコントローラーの"LT RT"ボタンが押されていない間は通常画像を描画
        else
        {
            // 画像の回転値を初期化する
            Operate2_Image_Angle = 0.0f;
            Vector3 Reset_Angle;
            // 0.0fを格納するとInspector側で-90が入る為、自身のX値を格納する
            Reset_Angle.x = Operate2_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // 角度リセット
            Operate2_Image_Rect.eulerAngles = Reset_Angle;

            // 通常画像のみを描画する
            Operate2_Image[0].enabled = true;
            // 操作中画像は描画しない
            Operate2_Image[1].enabled = false;
            Operate2_Image[2].enabled = false;
        }

        // XBoxコントローラーの"LB RB"ボタンが押されている間は"半回転"UI演出
        if ((Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5)) || /*確認用*/ Input.GetKey(KeyCode.D)) 
        {
            // 操作中画像のみを描画する
            Operate3_Image[1].enabled = true;
            Operate3_Image[2].enabled = true;
            // 通常画像は描画しない
            Operate3_Image[0].enabled = false;

            // 押されている間、回転値を増やす
            Operate3_Image_Angle = Time.deltaTime * -200.0f;
            // 大回転操作方法UIアイコンを回転させる
            // その際、角度(Operate2_Image_Angle)を0～360度の範囲に正規化する
            var Normalized_Operate3_Angle = Mathf.Repeat(Operate3_Image_Angle, 360);
            Operate3_Image_Rect.Rotate(.0f, .0f, Normalized_Operate3_Angle);

        }
        // XBoxコントローラーの"LB RB"ボタンが押されていない間は通常画像を描画
        else
        {
            // 画像の回転値を初期化する
            Operate3_Image_Angle = 0.0f;
            Vector3 Reset_Angle;
            // 0.0fを格納するとInspector側で-90が入る為、自身のX値を格納する
            Reset_Angle.x = Operate3_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // 角度リセット
            Operate3_Image_Rect.eulerAngles = Reset_Angle;

            // 通常画像のみを描画する
            Operate3_Image[0].enabled = true;
            // 操作中画像は描画しない
            Operate3_Image[1].enabled = false;
            Operate3_Image[2].enabled = false;

            // 画像の回転値を初期化する
            Operate3_Image_Angle = 0.0f;
        }
    }
}
