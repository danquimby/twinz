using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class LevelModel
    {
        public string tag;
        public int timeLeft;
        public GameRulesType gameRulesType;
        public int TopScore;
        public int decreaseTimeLeft;
        public int minimumTimeLeft;
        public int addTimeLeft; // for AddedTimeLeft
    }
    public static class LevelCardExtension
    {
        public static void SaveTopPerLevel(this LevelModel model, int topScore)
        {
            PlayerPrefs.SetInt(model.tag, topScore);
        }

        public static int GetTopPerLevel(this LevelModel model)
        {
            if (!PlayerPrefs.HasKey(model.tag))
                PlayerPrefs.SetInt(model.tag, 0);
            return PlayerPrefs.GetInt(model.tag);
        }
    }
}