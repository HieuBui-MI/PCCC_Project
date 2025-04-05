using UnityEngine;
using UnityEngine.UI;

public class MainMenuScriptController : MonoBehaviour
{
    [SerializeField]
    [Header("Main Menu UI Elements")]
    [Header("Panels")]
    public GameObject SettingsPanel;
    public GameObject BackgroundPanel;
    public GameObject MainMenuPanel;


    public void settingBtn_clicked()
    {
        SettingsPanel.SetActive(true);
        BackgroundPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
    }
    
    [Header("Settings Panel UI Elements")]
    [Header("Panels")]
    public GameObject AudioPanel;
    public GameObject GraphicsPanel;
    public GameObject ControlsPanel;

    public void audioBtn_clicked()
    {
        AudioPanel.SetActive(true);
        GraphicsPanel.SetActive(false);
        ControlsPanel.SetActive(false);
    }

    public void graphicsBtn_clicked()
    {
        AudioPanel.SetActive(false);
        GraphicsPanel.SetActive(true);
        ControlsPanel.SetActive(false);
    }

    public void controlsBtn_clicked()
    {
        AudioPanel.SetActive(false);
        GraphicsPanel.SetActive(false);
        ControlsPanel.SetActive(true);
    }
}