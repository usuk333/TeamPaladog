using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StageSection
{
    public class StageReward
    {
        public int warriorPoint;
        public int assassinPoint;
        public int magicianPoint;
        public int archorPoint;

        public StageReward(int warriorPoint, int assassinPoint, int magicianPoint, int archorPoint)
        {
            this.warriorPoint = warriorPoint;
            this.assassinPoint = assassinPoint;
            this.magicianPoint = magicianPoint;
            this.archorPoint = archorPoint;
        }
        public int[] GetReward()
        {
            int[] rewardArray = { warriorPoint, assassinPoint, magicianPoint, archorPoint };
            return rewardArray;
        }
    }

    public class StagePanel : MonoBehaviour
    {
        //public static StagePanel instance;

        Tower t = new Tower();
        [SerializeField] private GameObject Reward;
        [SerializeField] private GameObject[] rewardObjectarray;
        private StageReward stagereward;

        private void Awake()
        {
            //instance = this;
        }

        private void RewardCheck(StageReward s)
        {
            for(int k = 0; k < rewardObjectarray.Length; k++)
            {
                rewardObjectarray[k].SetActive(true);
            }

            int[] stagerewardarray = s.GetReward();
            for ( int i = 0; i < rewardObjectarray.Length; i++ )
            {
                if(stagerewardarray[i] <= 0)
                {
                    rewardObjectarray[i].SetActive(false);
                }
                rewardObjectarray[i].GetComponentInChildren<Text>().text = "x" + stagerewardarray[i];
            }
        }

        public void SetReward(int nowstage, int nowdifficult)
        {
            switch(nowstage)
            {
                case 0:
                    {
                        Debug.Log("비스트 스테이지 선택");
                        RewardBeast(nowdifficult);
                    }
                    break;
                case 1:
                    {
                        Debug.Log("버섯 스테이지 선택");
                        RewardMushroom(nowdifficult);
                    }
                    break;
                case 2:
                    {
                        Debug.Log("수도승 스테이지 선택");
                        RewardMonk(nowdifficult);
                    }
                    break;
                case 3:
                    {
                        Debug.Log("가고일 스테이지 선택");
                        RewardGargoyle(nowdifficult);
                    }
                    break;
            }
        }

        public void RewardBeast(int nowdifficult)
        {
            switch (nowdifficult)
            {
                case 0:
                    Debug.Log("비스트 쉬움 스테이지 선택");
                    stagereward = new StageReward(0, 1, 0, 1);
                    RewardCheck(stagereward);
                    break;
                case 1:
                    Debug.Log("비스트 보통 스테이지 선택");
                    stagereward = new StageReward(0, 2, 0, 2);
                    RewardCheck(stagereward);
                    break;
                case 2:
                    Debug.Log("비스트 어려움 스테이지 선택");
                    stagereward = new StageReward(1, 3, 1, 3);
                    RewardCheck(stagereward);
                    break;
            }
        }
        public void RewardGargoyle(int nowdifficult)
        {
            switch (nowdifficult)
            {
                case 0:
                    Debug.Log("가고일 쉬움 스테이지 선택");
                    stagereward = new StageReward(1, 0, 0, 1);
                    RewardCheck(stagereward);
                    break;
                case 1:
                    Debug.Log("가고일 보통 스테이지 선택");
                    stagereward = new StageReward(2, 0, 0, 2);
                    RewardCheck(stagereward);
                    break;
                case 2:
                    Debug.Log("가고일 어려움 스테이지 선택");
                    stagereward = new StageReward(3, 1, 1, 3);
                    RewardCheck(stagereward);
                    break;
            }
        }
        public void RewardMushroom(int nowdifficult)
        {
            switch (nowdifficult)
            {
                case 0:
                    Debug.Log("버섯 쉬움 스테이지 선택");
                    stagereward = new StageReward(1, 0, 1, 0);
                    RewardCheck(stagereward);
                    break;
                case 1:
                    Debug.Log("버섯 보통 스테이지 선택");
                    stagereward = new StageReward(2, 0, 2, 0);
                    RewardCheck(stagereward);
                    break;
                case 2:
                    Debug.Log("버섯 어려움 스테이지 선택");
                    stagereward = new StageReward(3, 1, 3, 1);
                    RewardCheck(stagereward);
                    break;
            }
        }
        public void RewardMonk(int nowdifficult)
        {
            switch (nowdifficult)
            {
                case 0:
                    Debug.Log("수도승 쉬움 스테이지 선택");
                    stagereward = new StageReward(0, 1, 1, 0);
                    RewardCheck(stagereward);
                    break;
                case 1:
                    Debug.Log("수도승 보통 스테이지 선택");
                    stagereward = new StageReward(0, 2, 2, 0);
                    RewardCheck(stagereward);
                    break;
                case 2:
                    Debug.Log("수도승 어려움 스테이지 선택");
                    stagereward = new StageReward(1, 3, 3, 1);
                    RewardCheck(stagereward);
                    break;
            }
        }
    }
}
