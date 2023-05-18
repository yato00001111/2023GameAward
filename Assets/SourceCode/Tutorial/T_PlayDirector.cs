using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
interface T_IState
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

    E_State Initialize(T_PlayDirector parent);
    E_State Update(T_PlayDirector parent);
}


public class T_PlayDirector : MonoBehaviour
{
    public const int BOARD_WIDTH = 8;
    public const int BOARD_HEIGHT = 20;

    [SerializeField] UI_Dead_Gauge uiDeadGauge = default!;
    [SerializeField] GameObject[] player = { default!, default! };
    PlayerController[] _playerController = new PlayerController[2];
    LogicalInput _logicalInput = new();
    FieldController _fieldController = default!;

    NextQueue _nextQueue = new();
    [SerializeField] BlockPair[] nextBlockPairs = { default!, default! };// 次nextのゲームオブジェクトの制御

    // 得点
    [SerializeField] TextMeshProUGUI textScore = default!;
    [SerializeField] TextMeshProUGUI chainScore = default!;
    uint _score = 0;
    int _chainCount = -1;// 連鎖数（得点計算に必要）-1は初期化用 Magic number

    bool _canSpawn = false;

    // 音に合わせた操作
    // 1拍目到達通知
    private bool FirstBeat = false;

    // 操作可能フラグ
    public bool PlayFlag;
    public bool PenaltyFlag;
    public int PenaltyCount;

    public UI_NextBlock_Direction ui_NextBlock_Direction;

    //
    [SerializeField] private bool _eraseWave;
    [SerializeField] private bool _stateErase;

    [SerializeField] private int QuotaCount;

    private bool _gameStart;

    // 状態管理
    T_IState.E_State _current_state = T_IState.E_State.Falling;
    static readonly T_IState[] states = new T_IState[(int)T_IState.E_State.MAX]{
        new ControlState(),
        new GameOverState(),
        new FallingState(),
        new ErasingState(),
        new WaitingState(),
    };

    // 60FPS を指定する場合
    private void Awake()
    {
        // Vsync Count を 0にすることにより、FPS を固定できるようになる
        // プロジェクト設定から0にしてある
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
        // 状態の初期化
        InitializeState();

        SetScore(0);

        PlayFlag = false;
        PenaltyCount = 0;
        PenaltyFlag = false;

        _eraseWave = false;
        _stateErase = false;

        _gameStart = false;

        QuotaCount = 0;

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

    // 入力を取り込む
    void UpdateInput()
    {
        LogicalInput.Key inputDev = 0;// デバイス値

        // キー入力取得
        for (int i = 0; i < (int)LogicalInput.Key.MAX; i++)
        {
            if (Input.GetKey(key_code_tbl[i]))
            {
                inputDev |= (LogicalInput.Key)(1 << i);
            }
        }

        _logicalInput.Update(inputDev);
    }

    class WaitingState : T_IState
    {
        public T_IState.E_State Initialize(T_PlayDirector parent) { return T_IState.E_State.Unchanged; }
        public T_IState.E_State Update(T_PlayDirector parent)
        {
            return parent._canSpawn ? T_IState.E_State.Control : T_IState.E_State.Unchanged;
        }
    }

    class ControlState : T_IState
    {
        public T_IState.E_State Initialize(T_PlayDirector parent)
        {
            //if (!parent.Spawn(parent._nextQueue.Update()))
            //if (parent._fieldController.CheckDead())
            //if (parent._fieldController.CheckDead())
            //{
            //    return T_IState.E_State.GameOver;
            //}
            parent.Spawn(parent._nextQueue.Update());
            parent._gameStart = true;

            parent._stateErase = false;

            parent.UpdateNextsView();
            return T_IState.E_State.Unchanged;
        }
        public T_IState.E_State Update(T_PlayDirector parent)
        {
            //if(parent.PlayFlag)
                parent._fieldController.TransUpdate(parent._logicalInput);

            return parent.player[0].activeSelf || parent.player[1].activeSelf ? T_IState.E_State.Unchanged : T_IState.E_State.Falling;
            //return parent.player[0].activeSelf ? T_IState.E_State.Unchanged : T_IState.E_State.Waiting;
        }
    }

    class GameOverState : T_IState
    {
        public T_IState.E_State Initialize(T_PlayDirector parent) { return T_IState.E_State.Unchanged; }
        public T_IState.E_State Update(T_PlayDirector parent) { return T_IState.E_State.Unchanged; }
    }

    class FallingState : T_IState
    {
        public T_IState.E_State Initialize(T_PlayDirector parent)
        {
            return parent._fieldController.CheckFall() ? T_IState.E_State.Unchanged : T_IState.E_State.Erasing;
        }
        public T_IState.E_State Update(T_PlayDirector parent)
        {
            return parent._fieldController.Fall() ? T_IState.E_State.Unchanged : T_IState.E_State.Erasing;
        }
    }

    class ErasingState : T_IState
    {
        public T_IState.E_State Initialize(T_PlayDirector parent)
        {
            parent._fieldController.CheckDead();
            if (parent.player[0].activeSelf) parent.player[0].SetActive(false);
            // CheckErase-消えるブロックがあればtrue
            if (parent._fieldController.CheckErase(parent._chainCount++))
            {
                return parent._stateErase ? T_IState.E_State.Unchanged : T_IState.E_State.Control;// 消すアニメーションに突入
            }
            parent._chainCount = 0;// 連鎖が途切れた
            return parent._canSpawn ? T_IState.E_State.Control : T_IState.E_State.Waiting;// 消すものはない
        }
        public T_IState.E_State Update(T_PlayDirector parent)
        {
            return parent._fieldController.Erase(parent.uiDeadGauge.GetIsDisappearPhaseFlag()) ? T_IState.E_State.Unchanged : T_IState.E_State.Falling;
        }
    }

    void InitializeState()
    {
        Debug.Assert(condition: _current_state is >= 0 and < T_IState.E_State.MAX);

        var next_state = states[(int)_current_state].Initialize(this);

        if (next_state != T_IState.E_State.Unchanged)
        {
            _current_state = next_state;
            InitializeState();// 初期化で状態が変わるようなら、再帰的に初期を呼び出す
        }
    }

    void UpdateState()
    {
        Debug.Assert(condition: _current_state is >= 0 and < T_IState.E_State.MAX);

        var next_state = states[(int)_current_state].Update(this);
        if ((_current_state == T_IState.E_State.Waiting) && _gameStart)
        {
            if (!uiDeadGauge.GetIsDisappearPhaseFlag()) EnableSpawn(true);
        }
        if (uiDeadGauge.GetBeforeIsDisappearPhaseFlag())
        {
            next_state = T_IState.E_State.Erasing;
            EnableSpawn(false);
            _stateErase = true;
            uiDeadGauge.SetBeforeIsDisappearPhaseFlag(false);
        }
        if (next_state != T_IState.E_State.Unchanged)
        {
            // 次の状態に遷移
            _current_state = next_state;
            InitializeState();
        }
    }


    private IEnumerator BeatPlay()
    {
        int count = 0;
        while (true)
        {
            if(count==0)
            {
                PlayFlag = true;
                if (PenaltyFlag)
                {
                    PlayFlag = false;
                    PenaltyCount++;
                }
            }
            yield return new WaitForFixedUpdate();
            // 0.02秒後
            count++;
            if(count == 10)
            {
                PlayFlag = false;
            }
            if(count == 20)
            {
                PlayFlag = true;
                if (PenaltyFlag)
                {
                    PlayFlag = false;
                    PenaltyCount++;
                }
            }
            if(count == 30)
            {
                count = 0;
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PlayFlag)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                // ペナルティ処理
                PenaltyFlag = true;
            }
        }
        if (PenaltyCount ==2)
        {
            PenaltyFlag = false;
            PenaltyCount = 0;
        }

        // 入力を取り込む
        UpdateInput();

        if (!(_current_state == T_IState.E_State.Erasing)) _fieldController.EraseBlockSound();

        UpdateState();


        AddScore(_playerController[0].popScore());
        AddScore(_playerController[1].popScore());
        AddScore(_fieldController.popScore());
        QuotaCount = _fieldController._eraseCount / 3;
        SetChainScore(_fieldController._eraseCount / 3);
    }

    bool Spawn(Vector2Int next)
    {
        //Vector2Int position = new(Random.Range(0, BOARD_WIDTH), 9);// 初期位置
        Vector2Int position = new(6, 18);// 初期位置
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
    
    public int GetQuotaCount()
    {
        return QuotaCount;
    }

    public void EnableSpawn(bool enable)
    {
        _canSpawn = enable;
    }

    public bool IsGameOver()
    {
        return _current_state == T_IState.E_State.GameOver;
    }
}
