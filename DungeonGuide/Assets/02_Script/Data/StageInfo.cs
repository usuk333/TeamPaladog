namespace Data
{
    public class StageInfo
    {
        public enum eDifficulty
        {
            Easy,
            Normal,
            Hard
        }
        private int bossIndex;
        private eDifficulty difficulty;

        public int BossIndex { get => bossIndex; set => bossIndex = value; }
        public eDifficulty Difficulty { get => difficulty; set => difficulty = value; }

        public int GetBossBGM()
        {
            return bossIndex + 3;
        }
    }
}
