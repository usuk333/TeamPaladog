using System;
using UnityEngine;

[Serializable]
public class CastingObject
{
    private const string UNIT_TAG = "UNIT";
    [SerializeField] private const string PLAYER_TAG = "PLAYER";
    [SerializeField] private float castTime;
    public float CastTime { get => castTime; }
    public string PlayerTag { get => PLAYER_TAG; }
    public string UnitTag { get => UNIT_TAG; }
}
