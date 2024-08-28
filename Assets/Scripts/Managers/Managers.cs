using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region Core
    PlayerManager _player = new PlayerManager();
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    UIManager _UI = new UIManager();
    ComponentCacheManager _compCache = new ComponentCacheManager();

    // MonoBehaviour 상속 매니저
    SceneManagerEx _scene;
    TimerManager _timer;

    public static DataManager Data { get => Instance._data;  }
    public static InputManager Input { get => Instance._input;  }
    public static PoolManager Pool { get => Instance._pool;  }
    public static ResourceManager Resource { get => Instance._resource;  }
    public static SoundManager Sound { get => Instance._sound;  }
    public static UIManager UI { get => Instance._UI;  }
    public static ComponentCacheManager CompCache { get => Instance._compCache;  }
    public static PlayerManager Player { get => Instance._player;  }

    // MonoBehaviour 상속 매니저
    public static SceneManagerEx Scene { get => Instance._scene;  }
    public static TimerManager Timer { get => Instance._timer; }
    #endregion

    #region Contents
    TimeManager _time = new();

    public static TimeManager Time { get=>  Instance._time; }

    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    public static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            // MonoBehaviour 상속 매니저
            {
                s_instance._scene = go.GetOrAddComponent<SceneManagerEx>();
                s_instance._timer = go.GetOrAddComponent<TimerManager>();
            }

            //s_instance._data.Init();
            //s_instance._pool.Init();
            //s_instance._player.Init();
            //s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        //Sound.Clear();
        //Input.Clear();
        //Scene.Clear();
        //UI.Clear();

        //CompCache.Clear();
        //Pool.Clear();
    }
}
