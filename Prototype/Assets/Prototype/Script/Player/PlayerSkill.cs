using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillKind
{
    Attack,
    Ultimate,
    SpeedUp,
    Resauce,
    Shield,
    Healing,
    Teleport,
    Invincibility
}
[CreateAssetMenu(fileName ="PlayerSkill",menuName ="Scriptable Object/Player Skill")]
public class PlayerSkill : ScriptableObject //플레이어 스킬 오브젝트
{
    [SerializeField] private float mp;
    [SerializeField] private float value;
    [SerializeField] private float coolTime;
    [SerializeField] private Sprite icon;
    [SerializeField] private ESkillKind skillKind;

    /*public PlayerSkill(float mp, float value, float coolTime, Sprite icon, ESkillKind skillKind)
    {
        this.mp = mp;
        this.value = value;
        this.coolTime = coolTime;
        this.icon = icon;
        this.skillKind = skillKind;
    }*/

    public ESkillKind SkillKind { get => skillKind; }
    public float Value { get => value; set => this.value = value; }
    public float Mp { get => mp; set => mp = value; }
    public float CoolTime { get => coolTime; set => coolTime = value; }
}
