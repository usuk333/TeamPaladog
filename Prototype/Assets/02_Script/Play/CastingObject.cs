using System;
using UnityEngine;

[Serializable]
public class CastingObject
{
    [SerializeField] private const string PLAYER_TAG = "PLAYER";
    [SerializeField] private float castTime;
    public float CastTime { get => castTime; }
    public string PlayerTag { get => PLAYER_TAG; }
}
