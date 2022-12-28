using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SkillScene
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private SkillInfo skillInfo;
        public void BtnEvt_UpgradeSkill()
        {
            if (!skillInfo.CheckHaveSkillPoint()) return;
            skillInfo.UpgradeSkill();
        }
        public void BtnEvt_NextSkillInfo()
        {
            skillInfo.CurrentSkill++;
            if (skillInfo.CurrentSkill > 5) skillInfo.CurrentSkill = 0;
            skillInfo.ChangeSkillUI(skillInfo.CurrentSkill);
        }
        public void BtnEvt_PreviousSkillInfo()
        {
            skillInfo.CurrentSkill--;
            if (skillInfo.CurrentSkill < 0) skillInfo.CurrentSkill = 5;
            skillInfo.ChangeSkillUI(skillInfo.CurrentSkill);
        }
        public void BtnEvt_ActiveSkillInfoObj(int i)
        {
            if (!skillInfo.gameObject.activeSelf)
            {
                skillInfo.gameObject.SetActive(!skillInfo.gameObject.activeSelf);
            }
            if (skillInfo.CurrentSkill == i) return;
            skillInfo.CurrentSkill = i;
            skillInfo.ChangeSkillUI(i);
        }
        public void BtnEvt_ActiveObject(GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
