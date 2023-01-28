using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    PlayerInput _playerInput;

    [SerializeField] EventChannelSO _event;

    [SerializeField] OptionsSO _optionsSO;
    [SerializeField] GameObject PauseScreen;
    [SerializeField] bool _paused;
    public Slider mouseSensivity;
    public Slider volume;
    bool _muted = false;
    public Sprite _mutedSprite;
    public Sprite _unMutedSprite;
    [SerializeField] Image _muteImage;



    void Start()
    {
        PauseScreen.SetActive(false);
        _playerInput = new PlayerInput();
        _playerInput.InGame.ESC.started += OnEscape;
    }

    void Update()
    {

    }

    void OnEscape(InputAction.CallbackContext context)
    {
        

        if (_paused)
        {

            _paused = false;
            ResumeGame();
            return;
        }
        else
        {
            _paused = true;
            ShowPause();
            _event.DisableInput();
            return;
        }
    }

    public void EnableInput()
    {
        _playerInput.Enable();
    }

    public void DisableInput()
    {
        _playerInput.Disable();
    }

    public void ShowPause()
    {
        _event.PauseScreenToggled();
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;

    }

    public void ResumeGame()
    {
        _event.PauseScreenToggledOff();
        PauseScreen.SetActive(false);
        _event.EnableInput();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SchoolPlaygroundGame", LoadSceneMode.Single);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void AdjustMouseSensivity()
    {
        _optionsSO.mouseSensetivity = mouseSensivity.value;
    }

    public void AdjustVolume()
    {
        _optionsSO.Volume = volume.value;
        _muted = false;
        _muteImage.GetComponent<Image>().sprite = _unMutedSprite;
    }

    public void HandleMute()
    {
        if (_muted)
        {
            _muted = false;
            _muteImage.GetComponent<Image>().sprite = _unMutedSprite;
            _optionsSO.Volume = volume.value;
        }
        else
        {
            _muted = true;
            _muteImage.GetComponent<Image>().sprite = _mutedSprite;

            _optionsSO.Volume = 0;
        }
    }

    private void OnEnable()
    {
        volume.value = _optionsSO.Volume;
        mouseSensivity.value = _optionsSO.mouseSensetivity;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.InGame.ESC.started -= OnEscape;
    }
}
