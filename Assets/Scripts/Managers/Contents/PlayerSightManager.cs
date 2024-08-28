using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSightManager : MonoBehaviour
{
    [SerializeField]
    Light2D globalLight;
    [SerializeField]
    Light2D playerLight;

    public float minPlayerLightSize = 1.35f;
    public float maxPlayerLightSize = 6f;

    [SerializeField]
    [Range(0,1)]
    private float curTime;

    [SerializeField]
    private float changeTimeSpeed;

    private void Update()
    {
        globalLight.intensity = curTime;
        PlayerLightUpdate();
    }

    private void PlayerLightUpdate()
    {
        playerLight.pointLightOuterRadius = Mathf.Clamp(maxPlayerLightSize * curTime, minPlayerLightSize * 2, maxPlayerLightSize);
    }
}
