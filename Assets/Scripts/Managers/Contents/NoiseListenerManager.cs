using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseListenerManager
{
    private Dictionary<Collider2D, NoiseListener> noiseListenerDict = new();

    public void AddListener(Collider2D collider , NoiseListener listener) => 
        noiseListenerDict.Add(collider, listener);

    public NoiseListener GetListener(Collider2D collider) => noiseListenerDict[collider];
}
