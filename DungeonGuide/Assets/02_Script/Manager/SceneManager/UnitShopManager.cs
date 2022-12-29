using UnityEngine;

namespace UnitShopScene
{
    public class UnitShopManager : MonoBehaviour
    {
        [SerializeField] private UnitInfo unitInfo;

        private void Start()
        {
            unitInfo.ChangeUnitInfo(0);
        }
        public void BtnEvt_UseUnitPoint()
        {
            if (!unitInfo.CheckHaveUnitPoint()) return;
            unitInfo.UpdateUnitExp();
        }
        public void BtnEvt_UpgradeAtk()
        {
            if (!unitInfo.CheckHaveGold()) return;

            unitInfo.UpgradeUnitStatus(true);
        }
        public void BtnEvt_UpgradeHp()
        {
            if (!unitInfo.CheckHaveGold()) return;

            unitInfo.UpgradeUnitStatus(false);
        }
        public void BtnEvt_ActiveObj(GameObject obj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        public void BtnEvt_ChangeUnitInfo(int i)
        {
            unitInfo.ChangeUnitInfo(i);
        }
    }
}
