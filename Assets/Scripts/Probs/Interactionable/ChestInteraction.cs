using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : Interactionable
{
    public override void Interaction()
    {
        Debug.Log("보물상자를 열었다!");
        gameObject.SetActive(false);
    }
}
