using System;

namespace ETC
{
    public class GoldBar : StatusBar
    {
        private int unitSize = 10000;
        private string[] units = { null, "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
        private (int value, int idx, int point) GetSize(int value) // ����Ƽ���� �޾ƿ� ���� ����� �� �������� ���� ǥ�⸦ �ٲ���
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
        private string GetUnit(int value) // ���ڸ� ���� ǥ��� �ٲ� �� ȣ��, �˸´� �������� ������
        {
            var sizeStruct = GetSize(value);
            return $"{sizeStruct.value}.{sizeStruct.point}{units[sizeStruct.idx]}";
        }
        public override void UpdateValueText()
        {
            int gold = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary[statusName]);
            valueText.text = gold < 1000000 ?
               gold.ToString() + " ���" : GetUnit(gold) + " ���";
        }
    }
}
