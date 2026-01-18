using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] KeyCode settings = KeyCode.Escape;

    [SerializeField] GameObject crosshair;

    [SerializeField] Movement movement;
    [SerializeField] CameraMovement camMovement;
    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject settingsScreen;

    [SerializeField] Slider xSlider;
    [SerializeField] Slider ySlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] TMP_InputField xInput;
    [SerializeField] TMP_InputField yInput;
    [SerializeField] TMP_InputField soundInput;

    [SerializeField] TMP_Text ammoText;

    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text healthBarText;


    private void Update()
    {
        if (Input.GetKeyDown(settings))
        {
            if (!settingsScreen.activeSelf)
            {
                UpdateValues();

                OpenSettingsScreen();
            }
            else if (settingsScreen.activeSelf)
            {
                ExitSettingsScreen();
            }
        }
    }

    private void UpdateValues()
    {
        xSlider.value = camMovement.sensitivityX;
        string x = camMovement.sensitivityX.ToString().Substring(0, (camMovement.sensitivityX.ToString().Length > 4 ? 4 : camMovement.sensitivityX.ToString().Length));
        xInput.text = x;

        ySlider.value = camMovement.sensitivityY;
        string y = camMovement.sensitivityY.ToString().Substring(0, (camMovement.sensitivityY.ToString().Length > 4 ? 4 : camMovement.sensitivityY.ToString().Length));
        yInput.text = y;

        soundSlider.value = audioSource.volume;
        string vol = audioSource.volume.ToString().Substring(0, (audioSource.volume.ToString().Length > 4 ? 4 : audioSource.volume.ToString().Length));
        soundInput.text = vol;
    }

    public void SetXSenseSlider()
    {
        camMovement.sensitivityX = xSlider.value;
        UpdateValues();
    }
    public void SetYSenseSlider()
    {
        camMovement.sensitivityY = ySlider.value;
        UpdateValues();
    }
    public void SetSoundSlider()
    {
        audioSource.volume = soundSlider.value;
        UpdateValues();
    }

    public void SetXSenseInput()
    {
        try
        {
            camMovement.sensitivityX = float.Parse(xInput.text.Replace('.',','));
            UpdateValues();
        }
        catch(FormatException x)
        {
            UpdateValues();
        }
    }
    public void SetYSenseInput()
    {
        try
        {
            camMovement.sensitivityY= float.Parse(yInput.text.Replace('.', ','));
            UpdateValues();
        }
        catch (FormatException x)
        {
            UpdateValues();
        }
    }

    public void SetSoundInput()
    {
        try
        {
            audioSource.volume = float.Parse(soundInput.text.Replace('.', ','));
            UpdateValues();
        }
        catch (FormatException x)
        {
            UpdateValues();
        }
    }

    public void OpenSettingsScreen() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        settingsScreen.SetActive(true);
        camMovement.active = false;
    }

    public void ExitSettingsScreen()
    {
        settingsScreen.SetActive(false);
        camMovement.active = true;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateAmmo(int _ammo, int _maxAmmo)
    {
        ammoText.text = _ammo.ToString()+" / "+_maxAmmo.ToString();
    }

    public void UpdateHealth(float _health, float _maxHealth)
    {
        healthBar.value = _maxHealth / _health;

        string health = _health.ToString().Substring(0, (_health.ToString().Length > 4 ? 4 : _health.ToString().Length));
        healthBarText.text = health;
    }

    public void SetCrosshairActive(bool _active)
    {
        crosshair.SetActive(_active);
    }
}
