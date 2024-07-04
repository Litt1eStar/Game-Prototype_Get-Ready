using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectSize
{
    SMALL,
    MEDIUM,
    LARGE
}

[CreateAssetMenu(fileName = "objectSO", menuName = "ScriptableObjects/Object")]
public class ObjectSO : ScriptableObject
{
    public ObjectSize Size;
    public float Weight;
}
