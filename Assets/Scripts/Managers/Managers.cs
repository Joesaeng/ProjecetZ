using DG.Tweening;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region Core
    SaveAndLoadManager _saveLoad = new SaveAndLoadManager();
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    UIManager _UI = new UIManager();
    ComponentCacheManager _compCache = new ComponentCacheManager();
    SceneManagerEx _scene = new SceneManagerEx();

    // MonoBehaviour 상속 매니저
    // TimerManager _timer;

    public static DataManager Data { get => Instance._data;  }
    public static InputManager Input { get => Instance._input;  }
    public static PoolManager Pool { get => Instance._pool;  }
    public static ResourceManager Resource { get => Instance._resource;  }
    public static SoundManager Sound { get => Instance._sound;  }
    public static UIManager UI { get => Instance._UI;  }
    public static ComponentCacheManager CompCache { get => Instance._compCache;  }
    public static SaveAndLoadManager SAL { get => Instance._saveLoad;  }
    public static SceneManagerEx Scene { get => Instance._scene;  }

    // MonoBehaviour 상속 매니저
    // public static TimerManager Timer { get => Instance._timer; }
    #endregion

    #region Contents
    TimeManager _time = new();
    DamageableManager _damageable = new();
    NoiseListenerManager _noise = new();

    public static TimeManager Time { get =>  Instance._time; }
    public static DamageableManager Damageable { get =>  Instance._damageable; }
    public static NoiseListenerManager Noise { get =>  Instance._noise; }

    #endregion

    void Start()
    {
        Init();

        // TEMP
        DOTween.SetTweensCapacity(500, 125);
    }

    void Update()
    {
        // _input.OnUpdate();
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
            }

            s_instance._time.Init();
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
