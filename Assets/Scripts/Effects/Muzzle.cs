using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour
{
    SpriteRenderer muzzleRenderer;

    [SerializeField] Sprite[] muzzleSprites;
    
    void Start()
    {
        muzzleRenderer = GetComponentInChildren<SpriteRenderer>();
        muzzleRenderer.enabled = false;
    }

    public async UniTaskVoid ShowMuzzle()
    {
        int randIndex = Random.Range(0,muzzleSprites.Length);

        muzzleRenderer.sprite = muzzleSprites[randIndex];
        muzzleRenderer.enabled = true;
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.01));
        muzzleRenderer.enabled = false;
    }
}
