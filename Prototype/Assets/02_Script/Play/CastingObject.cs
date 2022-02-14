using System;
using UnityEngine;

[Serializable]
public class CastingObject
{
    [SerializeField] private string playerTag;
    [SerializeField] private float castTime;
    public float CastTime { get => castTime; }
    public string PlayerTag { get => playerTag; }
}
