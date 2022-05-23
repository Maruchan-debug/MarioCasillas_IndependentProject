using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(restartGame);
        SceneManager.LoadScene("Start_Scene");
    }

    void restartGame()
    {
        SceneManager.LoadScene("Start_Scene");
    }
}
