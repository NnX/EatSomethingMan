using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    public void SetVisible(bool isVisible)
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(isVisible);
        }
    }
}
