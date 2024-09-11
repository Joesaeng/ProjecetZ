using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시간 진행속도 관리
public class TimeManager
{
    [Tooltip("플레이어의 시간 진행 속도")]
    public float PlayerTimeScale { get; private set; }
    [Tooltip("게임의 시간 진행 속도")]
    public float GameTimeScale { get; private set; }

    private float prevPlayerTimeScale;
    private float prevGameTimeScale;

    public void Init()
    {
        SetPlayerTimeScale(1f);
        SetGameTimeScale(1f);
    }

    public void SetPlayerTimeScale(float value)
    {
        PlayerTimeScale = value;
        prevPlayerTimeScale = value;
    }

    public void SetGameTimeScale(float value)
    {
        GameTimeScale = value;
        prevGameTimeScale = value;
    }

    public void GamePause()
    {
        SetPlayerTimeScale(0f);
        SetGameTimeScale(0f);
    }

    public void GameResume()
    {
        SetPlayerTimeScale(prevPlayerTimeScale);
        SetGameTimeScale(prevGameTimeScale);
    }
}
