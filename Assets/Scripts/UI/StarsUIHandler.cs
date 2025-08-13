using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This script handles all the stars UI throughout the game
public class StarsUIHandler : MonoBehaviour
{
    #region VARIABLES
    [Header("Total Stars UI")]
    [SerializeField] private TextMeshProUGUI[] starsEarnedTMP;

    [Header("Level Stars UI")]
    [SerializeField] private LevelStarUI[] allLevels;
    [SerializeField] private Sprite starOn;
    [SerializeField] private Sprite starOff;

    [Header("Win Screen Stars UI")]
    [SerializeField] private Image[] winScreenStarImages;
    #endregion

    public void UpdateMenuStarsUI()
    {
        UpdateTotalStarsUI();
        UpdateLevelStarsUI();
    }

    private void UpdateTotalStarsUI()
    {
        int totalGameStars = GameManager.Instance.GetTotalGameStars();
        int totalPlayerStars = GameManager.Instance.GetTotalPlayerStars();

        for (int i = 0; i < starsEarnedTMP.Length; i++)
        {
            starsEarnedTMP[i].text = $"{totalPlayerStars}/{totalGameStars}";
        }
    }

    private void UpdateLevelStarsUI()
    {
        foreach (var levelUI in allLevels)
        {
            int earnedStars = GameManager.Instance.GetStarsForLevel(levelUI.levelName);

            for (int i = 0; i < levelUI.starImages.Length; i++)
            {
                levelUI.starImages[i].sprite = i < earnedStars ? starOn : starOff;
            }
        }
    }

    public void DisplayLevelEndingStars(int starsEarned)
    {
        for (int i = 0; i < winScreenStarImages.Length; i++)
        {
            winScreenStarImages[i].sprite = i < starsEarned ? starOn : starOff;
        }
    }

    [System.Serializable]
    public class LevelStarUI
    {
        public string levelName;
        public Image[] starImages;
    }
}
