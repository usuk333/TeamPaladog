using System;

namespace ETC
{
    public class GoldBar : StatusBar
    {
        private int unitSize = 10000;
        private string[] units = { null, "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극" };
        private (int value, int idx, int point) GetSize(int value) // 빅인티저로 받아온 값을 계산해 만 단위마다 단위 표기를 바꿔줌
        {
            var currentValue = value;
            var idx = 0;
            var lastValue = 0;
            while (currentValue > unitSize - 1)
            {
                var predCurrentValue = currentValue / unitSize;
                if (predCurrentValue <= unitSize - 1) lastValue = (int)currentValue;
                currentValue = predCurrentValue;
                idx++;
            }
            int point = (lastValue % unitSize) / 1000;
            return ((int)currentValue, idx, point);
        }
        private string GetUnit(int value) // 숫자를 단위 표기로 바꿀 때 호출, 알맞는 단위값을 리턴함
        {
            var sizeStruct = GetSize(value);
            return $"{sizeStruct.value}.{sizeStruct.point}{units[sizeStruct.idx]}";
        }
        public override void UpdateValueText()
        {
            int gold = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary[statusName]);
            valueText.text = gold < 1000000 ?
               gold.ToString() + " 골드" : GetUnit(gold) + " 골드";
        }
    }
}
