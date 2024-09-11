using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D),typeof(SpriteRenderer))]
public abstract class Interactionable : MonoBehaviour
{
    private SpriteRenderer sr;
    private MaterialPropertyBlock mpb;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    public abstract void Interaction();

    public void SetCanInteration(float value)
    {
        if(value == 0f)
            transform.localScale = Vector3.one;
        else
            transform.localScale = Vector3.one * 1.1f;

        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_Thickness", value);
        sr.SetPropertyBlock(mpb);
    }

}
