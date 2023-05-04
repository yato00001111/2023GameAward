using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

// uGUIのImageで数字を表現するクラス
public class ImageRenderer : MonoBehaviour
{
    // 色
    [SerializeField]
    private Color _color = Color.white;

    // 寄せの設定
    public enum LayoutType
    {
        Center, Left, Right
    }

    [SerializeField]
    private LayoutType _layoutType = LayoutType.Center;

    // 文字の間隔
    [SerializeField]
    private float _textSpan = 50f;

    // 桁数によって増減するオブジェクト(Imageコンポーネント付き)
    [SerializeField, HideInInspector]
    private List<Image> _Images = new List<Image>();

    //各数字のスプライト
    [SerializeField]
    private List<Sprite> _spriteList = new List<Sprite>();

    // インスペクターが操作された時
    private void OnValidate()
    {
        // spriteListの数を固定
        SetSpriteList();
    }

    // 更新処理
     public void _Update(int Number)
    {
        // フレームレートを60固定
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // NowNumberをstring型に変換
        string Text_Number = Number.ToString();

        CreatNewObject(Text_Number);

        // 桁の数だけ画像更新処理
        for (int i = 0; i < _Images.Count; i++)
        {
            int no = int.Parse(Text_Number[i].ToString());

            ImageUpdate(_Images[i], _spriteList[no], _color);
        }

        // 位置更新
        PositonUpdate(Text_Number);
    }

    // 数値の桁数だけ新しいオブジェクトを子供に生成
    private void CreatNewObject(string text)
    {
        // テキストが変わってない場合は処理しない
        //if (_text == text)
        //{
        //    return;
        //}
        //_text = text;

        // テキストの桁数が子供の数と合わなかったら
        while (_Images.Count != text.Length)
        {
            // 多かったら削除
            if(_Images.Count > text.Length)
            {
                // 多い文をリストから削除して
                var component = _Images[0];
                _Images.Remove(component);

                // 生成したオブジェクトを削除
                // 動作中だったら仮削除
                if(Application.isPlaying)
                {
                    Destroy(component.gameObject);
                }
                // 動作してなかったら完全削除
                else
                {
                    DestroyImmediate(component.gameObject);
                }
            }
            // 少なかったら生成
            else
            {
                // リストの順番を名前に
                GameObject child = new GameObject(_Images.Count.ToString());
                // 親の位置に依存
                child.transform.SetParent(transform, false);

                // 追加した子供にImageコンポーネントを追加
                var newRenderer = child.AddComponent<Image>();
                _Images.Add(newRenderer);
            }
        }
    }

    // imageの更新
    private void ImageUpdate(Image image, Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.color = color;
        image.SetNativeSize();
    }

    // 文字１つ１つの位置更新
    private void PositonUpdate(string text)
    {
        // 文字の数だけループ
        for(int i = 0; i < _Images.Count; i++)
        {
            // 補完位置
            Vector3 position = Vector3.zero;

            // Centerの時
            if(_layoutType == LayoutType.Center)
            {
                // 補完
                position.x = ((float)i - (text.Length - 1) / 2f) * _textSpan;
            }
            // Leftの時
            else if(_layoutType == LayoutType.Left)
            {
                // 補完
                position.x = i * _textSpan;
            }
            // Rightの時
            else if(_layoutType == LayoutType.Right)
            {
                // 補完
                position.x = -(text.Length - 1 - i) * _textSpan;
            }

            _Images[i].transform.localPosition = position;
        }
    }

    // 強制的に_SpriteListの数を10個に固定
    private void SetSpriteList()
    {
        // SpriteListの数が10個じゃなかったら
        while(_spriteList.Count != 10)
        {
            // 多い場合
            if(_spriteList.Count > 10)
            {
                // 消す
                _spriteList.RemoveAt(_spriteList.Count - 1);
            }
            // 少ない場合
            else
            {
                // 増やす
                _spriteList.Add(null);
            }
        }
    }
}