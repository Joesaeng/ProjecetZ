using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player가 시간을 조작하는 스킬을 사용할때 필요한 타임스케일
public class TimeManager
{
    public float PlayerTimeScale { get; private set; }
    public float GameTimeScale { get; private set; }

    public void SetPlayerTimeScale(float value) => PlayerTimeScale = value;
    public void SetGameTimeScale(float value) => GameTimeScale = value;
}
