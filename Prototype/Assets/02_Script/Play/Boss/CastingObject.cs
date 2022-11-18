using System;
using UnityEngine;

[Serializable]
public class CastingObject : MonoBehaviour
{
    [SerializeField] private float castTime;
    public float CastTime { get => castTime; }
    public bool CastFinish { get; set; }
}
