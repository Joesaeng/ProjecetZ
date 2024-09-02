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

    public float minGlobalLightIntensity = 0.06f;
    public float maxGlobalLightIntensity = 0.86f;

    [SerializeField]
    [Range(0,1)]
    private float curTime;
    private float changeValue = 0.01f;

    [SerializeField]
    private float changeTimeSpeed;

    private void Start()
    {
        // StartCoroutine(CoTimer());
    }
    private void Update()
    {
        
        PlayerLightUpdate();
    }

    private void PlayerLightUpdate()
    {
        playerLight.pointLightOuterRadius = Mathf.Clamp(maxPlayerLightSize * curTime, minPlayerLightSize * 2, maxPlayerLightSize);
    }

    IEnumerator CoTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.05f);
            curTime += changeValue;

            if (curTime >= 1f)
                changeValue = -0.01f;
            else if (curTime <= 0f)
                changeValue = 0.01f;

            globalLight.intensity = Mathf.Clamp(curTime* maxGlobalLightIntensity,minGlobalLightIntensity,maxGlobalLightIntensity);
        }
    }
}
