using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    public enum SkillType
    {
        Active,
        Passive
    }
    [SerializeField] private SkillType skillType;
    [SerializeField] private float value;
    [SerializeField] private float mana;
    [SerializeField] private float coolTime;

    public SkillData(SkillType skillType, float value, float mana, float coolTime)
    {
        this.skillType = skillType;
        this.value = value;
        this.mana = mana;
        this.coolTime = coolTime;
    }
    
        
    
}
