using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NewGameClick : MonoBehaviour
{
    [SerializeField] private string sceneName = "Level";

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(StartNewGame);
    }

    private void OnDestroy()
    {
        if (TryGetComponent<Button>(out var button))
        {
            button.onClick.RemoveListener(StartNewGame);
        }
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
