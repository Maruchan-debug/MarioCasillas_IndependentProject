using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
        SceneManager.LoadScene("WorldView_Scene");
    }
    
    void StartGame()
    {
        SceneManager.LoadScene("WorldView_Scene");
    }
}
