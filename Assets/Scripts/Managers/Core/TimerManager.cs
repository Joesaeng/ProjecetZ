using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor;

public class TimerManager : MonoBehaviour
{
    public async void StartTimer(float delay, Action callback)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        await UniTaskTimer(delay);
        callback?.Invoke();

        StartCoroutine(CoTimer(delay,callback));
    }

    public void StartTimerWithPlayerTimeScele(float delay, Action callback)
    {
        StartCoroutine(CoTimerWithPlayerTimeScale(delay, callback));
    }

    public void StartTimerUnscaled(float delay, Action callback)
    {
        StartCoroutine(CoiTimerUnscaled(delay, callback));
    }

    private async UniTask UniTaskTimer(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay * Managers.Time.GameTimeScale));
    }

    private IEnumerator CoiTimerUnscaled(float delay, Action callback)
    {
        yield return new WaitForSecondsRealtime(delay);
        callback?.Invoke();
    }

    private IEnumerator CoTimer(float delay, Action callback)
    {
        yield return YieldCache.WaitForSeconds(delay * Managers.Time.GameTimeScale);
        callback?.Invoke();
    }

    private IEnumerator CoTimerWithPlayerTimeScale(float delay, Action callback)
    {
        yield return YieldCache.WaitForSeconds(delay * Managers.Time.PlayerTimeScale);
        callback?.Invoke();
    }

    public void Clear()
    {
        StopAllCoroutines();
    }
}