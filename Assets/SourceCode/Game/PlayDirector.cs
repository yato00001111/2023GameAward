using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
interface IState
{
    public enum E_State
    {
        Control = 0,
        GameOver = 1,
        Falling = 2,
        Erasing = 3,
        Waiting = 4,

        MAX,

        Unchanged,
    }

    E_State Initialize(PlayDirector parent);
    E_State Update(PlayDirector parent);
}

[RequireComponent(typeof(FieldController))]
public class PlayDirector : MonoBehaviour
{
    public const int BOARD_WIDTH = 8;
    public const int BOARD_HEIGHT = 20;

    [SerializeField] GameObject beatTest = default!;
    [SerializeField] GameObject[] player = { default!, default! };
    PlayerController[] _playerController = new PlayerController[2];
    LogicalInput _logicalInput = new();
    FieldController _fieldController = default!;

    NextQueue _nextQueue = new();
    [SerializeField] BlockPair[] nextBlockPairs = { default!, default! };// ��next�̃Q�[���I�u�W�F�N�g�̐���

    // ���_
    [SerializeField] TextMeshProUGUI textScore = default!;
    [SerializeField] TextMeshProUGUI chainScore = default!;
    uint _score = 0;
    int _chainCount = -1;// �A�����i���_�v�Z�ɕK�v�j-1�͏������p Magic number

    bool _canSpawn = false;

    // ���ɍ��킹������
    // 1���ړ��B�ʒm
    private bool FirstBeat = false;

    // ����\�t���O
    public bool PlayFlag;
    public bool PenaltyFlag;
    public int PenaltyCount;

    // ���ɗ������_�ŋN������^�C�}�[
    public float BeatTimer;
    // �����r�[�g
    public bool EvenBeat;
    // ��r�[�g
    public bool OddBeat;

    public int BeatCount;
    bool Pena1;
    bool Pena2;


    public UI_NextBlock_Direction ui_NextBlock_Direction;

    // ��ԊǗ�
    IState.E_State _current_state = IState.E_State.Falling;
    static readonly IState[] states = new IState[(int)IState.E_State.MAX]{
        new ControlState(),
        new GameOverState(),
        new FallingState(),
        new ErasingState(),
        new WaitingState(),
    };

    // 60FPS ���w�肷��ꍇ
    private void Awake()
    {
        // Vsync Count �� 0�ɂ��邱�Ƃɂ��AFPS ���Œ�ł���悤�ɂȂ�
        // �v���W�F�N�g�ݒ肩��0�ɂ��Ă���
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

    }

    // Start is called before the first frame update
    void Start()
    {
        _playerController[0] = player[0].GetComponent<PlayerController>();
        _playerController[1] = player[1].GetComponent<PlayerController>();
        _fieldController = GetComponent<FieldController>();
        _logicalInput.Clear();
        _playerController[0].SetLogicalInput(_logicalInput);
        _playerController[1].SetLogicalInput(_logicalInput);

        _nextQueue.Initialize();
        UpdateNextsView();
        // ��Ԃ̏�����
        InitializeState();

        SetScore(0);

        BeatTimer = 0.0f;
        PlayFlag = false;
        PenaltyCount = 0;
        PenaltyFlag = false;
        BeatCount = 0;
        OddBeat = false;
        EvenBeat = false;

        StartCoroutine("BeatPlay");
    }

    void UpdateNextsView()
    {
        _nextQueue.Each((int idx, Vector2Int n) =>
        {
            nextBlockPairs[idx++].SetBlockType((BlockType)n.x, (BlockType)n.y);
        });
    }

    static readonly KeyCode[] key_code_tbl = new KeyCode[(int)LogicalInput.Key.MAX]{
        KeyCode.RightArrow, // Right
        KeyCode.LeftArrow,  // Left
        KeyCode.D,          // D
        KeyCode.A,          // A
        KeyCode.UpArrow,    // QuickDrop
        KeyCode.DownArrow,  // Down
        KeyCode.Joystick1Button0,  // A
        KeyCode.Joystick1Button4,  // LB
        KeyCode.Joystick1Button5,  // RB
    };

    // ���͂���荞��
    void UpdateInput()
    {
        LogicalInput.Key inputDev = 0;// �f�o�C�X�l

        // �L�[���͎擾
        for (int i = 0; i < (int)LogicalInput.Key.MAX; i++)
        {
            if (Input.GetKey(key_code_tbl[i]))
            {
                inputDev |= (LogicalInput.Key)(1 << i);
            }
        }

        _logicalInput.Update(inputDev);
    }

    class WaitingState : IState
    {
        public IState.E_State Initialize(PlayDirector parent) { return IState.E_State.Unchanged; }
        public IState.E_State Update(PlayDirector parent)
        {
            return parent._canSpawn ? IState.E_State.Control : IState.E_State.Unchanged;
        }
    }

    class ControlState : IState
    {
        public IState.E_State Initialize(PlayDirector parent)
        {
            //if (!parent.Spawn(parent._nextQueue.Update()))
            if (parent._fieldController.CheckDead())
            {
                return IState.E_State.GameOver;
            }
            parent.Spawn(parent._nextQueue.Update());

            

            parent.UpdateNextsView();
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayDirector parent)
        {
            //if(parent.PlayFlag)
                parent._fieldController.TransUpdate(parent._logicalInput);

            return parent.player[0].activeSelf || parent.player[1].activeSelf ? IState.E_State.Unchanged : IState.E_State.Falling;
        }
    }

    class GameOverState : IState
    {
        public IState.E_State Initialize(PlayDirector parent) { return IState.E_State.Unchanged; }
        public IState.E_State Update(PlayDirector parent) { return IState.E_State.Unchanged; }
    }

    class FallingState : IState
    {
        public IState.E_State Initialize(PlayDirector parent)
        {
            return parent._fieldController.CheckFall() ? IState.E_State.Unchanged : IState.E_State.Erasing;
        }
        public IState.E_State Update(PlayDirector parent)
        {
            return parent._fieldController.Fall() ? IState.E_State.Unchanged : IState.E_State.Erasing;
        }
    }

    class ErasingState : IState
    {
        public IState.E_State Initialize(PlayDirector parent)
        {
            // CheckErase-������u���b�N�������true
            if (parent._fieldController.CheckErase(parent._chainCount++))
            {
                return IState.E_State.Unchanged;// �����A�j���[�V�����ɓ˓�
            }
            parent._chainCount = 0;// �A�����r�؂ꂽ
            return parent._canSpawn ? IState.E_State.Control : IState.E_State.Waiting;// �������̂͂Ȃ�
        }
        public IState.E_State Update(PlayDirector parent)
        {
            return parent._fieldController.Erase() ? IState.E_State.Unchanged : IState.E_State.Falling;
        }
    }

    void InitializeState()
    {
        Debug.Assert(condition: _current_state is >= 0 and < IState.E_State.MAX);

        var next_state = states[(int)_current_state].Initialize(this);

        if (next_state != IState.E_State.Unchanged)
        {
            _current_state = next_state;
            InitializeState();// �������ŏ�Ԃ��ς��悤�Ȃ�A�ċA�I�ɏ������Ăяo��
        }
    }

    void UpdateState()
    {
        Debug.Assert(condition: _current_state is >= 0 and < IState.E_State.MAX);

        var next_state = states[(int)_current_state].Update(this);
        if (next_state != IState.E_State.Unchanged)
        {
            // ���̏�ԂɑJ��
            _current_state = next_state;
            InitializeState();
        }
    }

    void PenaltyMethod()
    {
        // ��������\�t���[���ȊO�ő��삪�s��ꂽ��
        if (!PlayFlag)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                // �y�i���e�B����
                PenaltyFlag = true;
            }
        }

        // ���݂̔�������������𔻒�
        if (BeatCount % 2 == 0)
        {
            EvenBeat = true;
            OddBeat = false;
        }
        else
        {
            OddBeat = true;
            EvenBeat = false;
        }


        // �������������̎��Ƀy�i���e�B������������
        if (PenaltyFlag && EvenBeat)
        {
            // �����p�y�i���e�B�t���O�𗧂Ă�
            Pena2 = true;
        }
        // ����������̎��Ƀy�i���e�B������������
        if (PenaltyFlag && OddBeat)
        {
            // ��p�y�i���e�B�t���O�𗧂Ă�
            Pena1 = true;
        }

        // �����y�i���e�B(����)���������Ă��������ɉ���
        if (Pena2 && OddBeat)
        {
            PenaltyFlag = false;
            Pena2 = false;
        }
        // ���̋t
        if (Pena1 && EvenBeat)
        {
            PenaltyFlag = false;
            Pena1 = false;
        }
    }

    private IEnumerator BeatPlay()
    {
        while (true)
        {
            PlayFlag = true;
            if (PenaltyFlag)
            {
                PlayFlag = false;
                PenaltyCount++;
            }
            yield return new WaitForSeconds(0.15f);
            PlayFlag = false;
            yield return new WaitForSeconds(0.3f);
            PlayFlag = true;
            if(PenaltyFlag)
            {
                PlayFlag = false;
                PenaltyCount++;
            }
            yield return new WaitForSeconds(0.15f);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayFlag) beatTest.SetActive(true);
        if (!PlayFlag) beatTest.SetActive(false);

        if (!PlayFlag)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                // �y�i���e�B����
                PenaltyFlag = true;
            }
        }
        if (PenaltyCount ==2)
        {
            PenaltyFlag = false;
            PenaltyCount = 0;
        }

        // ���͂���荞��
        UpdateInput();

        UpdateState();


        AddScore(_playerController[0].popScore());
        AddScore(_playerController[1].popScore());
        AddScore(_fieldController.popScore());
        SetChainScore(_chainCount);
    }

    bool Spawn(Vector2Int next)
    {
        //Vector2Int position = new(Random.Range(0, BOARD_WIDTH), 9);// �����ʒu
        Vector2Int position = new(6, 18);// �����ʒu
        ui_NextBlock_Direction.ResetNextBlockAnimation();
        //return _playerController[0].Spawn((BlockType)next[0], (BlockType)next[0], position) && 
        //    _playerController[1].Spawn((BlockType)next[1], (BlockType)next[1], new Vector2Int(position.x, position.y - 3));
        return _playerController[0].Spawn((BlockType)next[0], (BlockType)next[1], position) ;
    }

    void SetScore(uint score)
    {
        _score = score;

        textScore.text = _score.ToString();
    }
    void AddScore(uint score)
    {
        if (0 < score) SetScore(_score + score);
    }
    void SetChainScore(int score)
    {
        chainScore.text = score.ToString();
    }

    public bool GetPlayFlag()
    {
        return PlayFlag;
    }

    public void EnableSpawn(bool enable)
    {
        _canSpawn = enable;
    }

    public bool IsGameOver()
    {
        return _current_state == IState.E_State.GameOver;
    }
}
