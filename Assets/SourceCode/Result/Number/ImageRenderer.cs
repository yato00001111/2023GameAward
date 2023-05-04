using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

// uGUI��Image�Ő�����\������N���X
public class ImageRenderer : MonoBehaviour
{
    // �F
    [SerializeField]
    private Color _color = Color.white;

    // �񂹂̐ݒ�
    public enum LayoutType
    {
        Center, Left, Right
    }

    [SerializeField]
    private LayoutType _layoutType = LayoutType.Center;

    // �����̊Ԋu
    [SerializeField]
    private float _textSpan = 50f;

    // �����ɂ���đ�������I�u�W�F�N�g(Image�R���|�[�l���g�t��)
    [SerializeField, HideInInspector]
    private List<Image> _Images = new List<Image>();

    //�e�����̃X�v���C�g
    [SerializeField]
    private List<Sprite> _spriteList = new List<Sprite>();

    // �C���X�y�N�^�[�����삳�ꂽ��
    private void OnValidate()
    {
        // spriteList�̐����Œ�
        SetSpriteList();
    }

    // �X�V����
     public void _Update(int Number)
    {
        // �t���[�����[�g��60�Œ�
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // NowNumber��string�^�ɕϊ�
        string Text_Number = Number.ToString();

        CreatNewObject(Text_Number);

        // ���̐������摜�X�V����
        for (int i = 0; i < _Images.Count; i++)
        {
            int no = int.Parse(Text_Number[i].ToString());

            ImageUpdate(_Images[i], _spriteList[no], _color);
        }

        // �ʒu�X�V
        PositonUpdate(Text_Number);
    }

    // ���l�̌��������V�����I�u�W�F�N�g���q���ɐ���
    private void CreatNewObject(string text)
    {
        // �e�L�X�g���ς���ĂȂ��ꍇ�͏������Ȃ�
        //if (_text == text)
        //{
        //    return;
        //}
        //_text = text;

        // �e�L�X�g�̌������q���̐��ƍ���Ȃ�������
        while (_Images.Count != text.Length)
        {
            // ����������폜
            if(_Images.Count > text.Length)
            {
                // �����������X�g����폜����
                var component = _Images[0];
                _Images.Remove(component);

                // ���������I�u�W�F�N�g���폜
                // ���쒆�������牼�폜
                if(Application.isPlaying)
                {
                    Destroy(component.gameObject);
                }
                // ���삵�ĂȂ������犮�S�폜
                else
                {
                    DestroyImmediate(component.gameObject);
                }
            }
            // ���Ȃ������琶��
            else
            {
                // ���X�g�̏��Ԃ𖼑O��
                GameObject child = new GameObject(_Images.Count.ToString());
                // �e�̈ʒu�Ɉˑ�
                child.transform.SetParent(transform, false);

                // �ǉ������q����Image�R���|�[�l���g��ǉ�
                var newRenderer = child.AddComponent<Image>();
                _Images.Add(newRenderer);
            }
        }
    }

    // image�̍X�V
    private void ImageUpdate(Image image, Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.color = color;
        image.SetNativeSize();
    }

    // �����P�P�̈ʒu�X�V
    private void PositonUpdate(string text)
    {
        // �����̐��������[�v
        for(int i = 0; i < _Images.Count; i++)
        {
            // �⊮�ʒu
            Vector3 position = Vector3.zero;

            // Center�̎�
            if(_layoutType == LayoutType.Center)
            {
                // �⊮
                position.x = ((float)i - (text.Length - 1) / 2f) * _textSpan;
            }
            // Left�̎�
            else if(_layoutType == LayoutType.Left)
            {
                // �⊮
                position.x = i * _textSpan;
            }
            // Right�̎�
            else if(_layoutType == LayoutType.Right)
            {
                // �⊮
                position.x = -(text.Length - 1 - i) * _textSpan;
            }

            _Images[i].transform.localPosition = position;
        }
    }

    // �����I��_SpriteList�̐���10�ɌŒ�
    private void SetSpriteList()
    {
        // SpriteList�̐���10����Ȃ�������
        while(_spriteList.Count != 10)
        {
            // �����ꍇ
            if(_spriteList.Count > 10)
            {
                // ����
                _spriteList.RemoveAt(_spriteList.Count - 1);
            }
            // ���Ȃ��ꍇ
            else
            {
                // ���₷
                _spriteList.Add(null);
            }
        }
    }
}