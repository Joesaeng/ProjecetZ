using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField]
    Image ClockHand;

    [SerializeField]
    Light2D globalLight;
    [SerializeField]
    Light2D playerSight;
    [SerializeField]

    [Header("플레이어의 최소 시야 거리(밤)")]
    public float minPlayerSightDistance = 9.9f;
    [Header("플레이어의 최대 시야 거리(낮)")]
    public float maxPlayerSightDistance = 18.25f;

    [Header("플레이어의 현재 시야 거리")]
    public float curPlayerSightDistance;

    [SerializeField]
    [Header("0은 00시, 1은 24시를 의미합니다.")]
    [Range(0,1)]
    private float curTime;
    [Range(0.01f,0.1f) ,Header("시간 변화 속도")]
    public float GameTimeScale;

    [GradientUsage(true,ColorSpace.Gamma)]
    public Gradient LightColorByTime;

    [Header("시간 별 자연광 세기")]
    public float[] LightStrengthByTime =
    {
        0.02f,  0.02f,  0.02f,  0.02f,  0.02f,  0.1f,   // 00 ~ 06
        0.6f,   0.66f,  0.7f,   0.73f,  0.85f,  1f,     // 07 ~ 12
        1f,     1f,     1f,     0.85f,  0.75f,  0.6f,   // 13 ~ 18
        0.1f,   0.1f,   0.02f,  0.02f,  0.02f,  0.02f,  // 19 ~ 24
    };

    private void Update()
    {
        if (curTime >= 1f)
            curTime -= 1f;
        curTime += Time.deltaTime * GameTimeScale;

        ClockHand.rectTransform.rotation = Quaternion.Euler(0f, 0f, (1f - curTime) * 360f);
        GlobalLightUpdate();
        PlayerSightUpdate();
    }

    private void GlobalLightUpdate()
    {
        globalLight.color = LightColorByTime.Evaluate(curTime);
    }

    private void PlayerSightUpdate()
    {
        curPlayerSightDistance = CalculateCurrentSightDistance(curTime);
        float inner = curPlayerSightDistance * 0.9f;

        playerSight.pointLightOuterRadius = curPlayerSightDistance;
        playerSight.pointLightInnerRadius = inner;
    }

    public float CalculateCurrentSightDistance(float time)
    {
        // time은 0 ~ 1 사이의 값이어야 합니다.
        // 0은 자정(00:00), 1은 다음 자정(24:00)에 해당합니다.

        // 1. LightStrengthByTime 배열의 최소값과 최대값을 구합니다.
        float minLightStrength = Mathf.Min(LightStrengthByTime);
        float maxLightStrength = Mathf.Max(LightStrengthByTime);

        // 2. time 값을 사용해 LightStrengthByTime에서 샘플링할 인덱스를 계산합니다.
        float scaledTime = time * (LightStrengthByTime.Length - 1);
        int index = Mathf.FloorToInt(scaledTime); // 시간에 해당하는 배열 인덱스
        float t = scaledTime - index; // 인덱스 사이의 보간 계수

        // 3. 인덱스에 해당하는 빛의 세기를 보간하여 구합니다.
        float currentLightStrength = Mathf.Lerp(LightStrengthByTime[index], LightStrengthByTime[Mathf.Min(index + 1, LightStrengthByTime.Length - 1)], t);

        // 4. 구한 LightStrength 값을 이용해 시야 거리를 보간합니다.
        float normalizedLightStrength = (currentLightStrength - minLightStrength) / (maxLightStrength - minLightStrength);
        float curPlayerSightDistance = Mathf.Lerp(minPlayerSightDistance, maxPlayerSightDistance, normalizedLightStrength);

        return curPlayerSightDistance;
    }
}
