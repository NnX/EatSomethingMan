using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartNewGame);
    }

    // Update is called once per frame
    private void StartNewGame()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }
}
