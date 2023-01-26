using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] EventChannelSO _event;

    void Start()
    {
        EventChannelSO.OnGameOver += ShowGameOverScreen;
        gameObject.transform.Find("Screen").gameObject.SetActive(false);
    }

    public void ShowGameOverScreen()
    {

        gameObject.transform.Find("Screen").gameObject.SetActive(true);
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        _event.DisableInput();
    }

    public void RestartGame()
    {
        gameObject.transform.Find("Screen").gameObject.SetActive(false);
        SceneManager.LoadScene("SchoolPlaygroundGame", LoadSceneMode.Single);
    }

    public void BackToMenu()
    {
        gameObject.transform.Find("Screen").gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        EventChannelSO.OnGameOver -= ShowGameOverScreen;
    }

}
