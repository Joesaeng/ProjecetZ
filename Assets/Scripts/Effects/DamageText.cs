using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public bool isInit = false;
    TextMeshPro text;
    Color originColor;

    public void Init()
    {
        text = GetComponent<TextMeshPro>();
        originColor = text.color;
        isInit = true;
    }


    public void ShowDamageEffect(Vector3 pos,DamageMessage msg ,float duration)
    {
        text.text = msg.damage.ToString();
        gameObject.transform.localScale = Vector3.one;
        text.color = originColor;
        
        Vector3 direction = (pos - msg.dealerPos).normalized;
        Vector3 targetPos = pos + direction * 2f;

        transform.position = pos;

        DOTween.Sequence()
            .Append(text.DOScale(0.2f, duration))
            .Join(text.DOFade(0, duration))
            .Join(transform.DOMove(targetPos, duration));
        
        Managers.Resource.DestroyDelay(gameObject, duration + 0.5f);
        
    }
}
