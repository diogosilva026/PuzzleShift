using System;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class BestTimesUIHandler : MonoBehaviour
{
    [SerializeField] private string[] levelNames;
    [SerializeField] private TextMeshProUGUI[] bestTimesTMP;

    public void LoadBestTimes()
    {
        for (int i = 0; i < levelNames.Length; i++)
        {
            string levelName = levelNames[i];
            float bestTime = GameManager.Instance.GetLevelTime(levelName);

            if (bestTime > 0)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(bestTime);
                string formattedTime = timeSpan.ToString(@"mm\:ss\.ff");
                bestTimesTMP[i].text = $"<color=#198207>Best Time:</color> {formattedTime}";
            }
            else
            {
                bestTimesTMP[i].text = "<color=#198207>Best Time:</color> --:--.--";
            }
        }
    }
}
