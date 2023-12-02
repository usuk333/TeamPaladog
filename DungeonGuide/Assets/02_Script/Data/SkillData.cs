using System.Collections.Generic;

namespace Data
{
    public class SkillData
    {
        protected int level;
        protected List<float> skillValueList = new List<float>();
        public int GetLevel()
        {
            return level;
        }
        public SkillData(int level)
        {
            this.level = level; 
            SetSkillValueArray();
        }
        public float[] GetSkillValueArray()
        {
            float[] skillValueArray = new float[skillValueList.Count];
            for (int i = 0; i < skillValueList.Count; i++)
            {
                skillValueArray[i] = skillValueList[i];
            }
            return skillValueArray;
        }
        public virtual void SetSkillValueArray()
        {
            if (skillValueList != null)
            {
                skillValueList.Clear();
            }
            //override something
        }
        public void LevelUp()
        {
            level++;
            SetSkillValueArray();
        }
    }
    public class SkillMaxHp : SkillData
    {

        public SkillMaxHp(int level) : base(level)
        {
        }
        public override void SetSkillValueArray()
        {
            base.SetSkillValueArray();
            skillValueList.Add(1000 + (100 * (level - 1)));
            skillValueList.Add(50 + (10 * ((level - 1) / 5)));
        }
    }
    public class SkillMaxMp : SkillData
    {
        public SkillMaxMp(int level) : base(level)
        {
        }
        public override void SetSkillValueArray()
        {
            base.SetSkillValueArray();
            skillValueList.Add(200 + (10 * (level - 1)));
            skillValueList.Add(30 + (5 * ((level - 1) / 5)));
        }
    }
    public class SkillAttack : SkillData
    {
        public SkillAttack(int level) : base(level)
        {
        }
        public override void SetSkillValueArray()
        {
            base.SetSkillValueArray();
            skillValueList.Add(200 + (20 * (level - 1)));
            skillValueList.Add(5);
            skillValueList.Add(3);
        }
    }
    public class SkillBarrior : SkillData
    {
        public SkillBarrior(int level) : base(level)
        {
        }
        public override void SetSkillValueArray()
        {
            base.SetSkillValueArray();
            if (level > 5) level = 5;
            skillValueList.Add(50 + (12.5f * (level - 1)));
            skillValueList.Add(50 + (5 * (level - 1)));
            skillValueList.Add(15 - (1.25f * (level - 1)));
            skillValueList.Add(3 + (0.5f * (level - 1)));
        }
    }
    public class SkillHeal : SkillData
    {
        public SkillHeal(int level) : base(level)
        {
        }
        public override void SetSkillValueArray()
        {
            base.SetSkillValueArray();
            if (level > 5) level = 5;
            skillValueList.Add(20 + (5 * (level - 1)));
            skillValueList.Add(20 + (2.5f * (level - 1)));
            skillValueList.Add(10 - (1.25f * (level - 1)));
        }
    }
    public class SkillPowerUp : SkillData
    {
        public SkillPowerUp(int level) : base(level)
        {
        }
        public override void SetSkillValueArray()
        {
            base.SetSkillValueArray();
            if (level > 5) level = 5;
            skillValueList.Add(3 + (0.5f * (level - 1)));
            skillValueList.Add(6 + (1 * (level - 1)));
            skillValueList.Add(1);
        }
    }
}

