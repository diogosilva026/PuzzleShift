using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUIHandler : MonoBehaviour
{
    [SerializeField] private Button deleteProgressButton;

    private void Start()
    {
        deleteProgressButton.onClick.AddListener(GameManager.Instance.DeleteProgress);
    }
}
