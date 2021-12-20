using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillKind
{
    Attack,
    PowerUp,
    Resource,
    Shield,
    SpeedUp,
    Revival,
    Invincibility
}
[CreateAssetMenu(fileName ="PlayerSkill",menuName ="Scriptable Object/Player Skill")]
public class PlayerSkill : ScriptableObject 
{
    [SerializeField] private float mp;
    [SerializeField] private float value;
    [SerializeField] private ESkillKind skillKind;

    public ESkillKind SkillKind { get => skillKind; }
    public float Value { get => value; }
    public float Mp { get => mp; set => mp = value; }
}
