using UnityEngine;
using UnityEngine.UI;

public class MenuUIScript : MonoBehaviour
{
    [Header("UI Main Menu Objects")]
    public Button newGameBtn;
    public Button mainSettingBtn;
    public Button quitBtn;

    [Header("Panel Objects")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject containerPanel;
    public GameObject startGamePanel;

    [Header("Settings Panel Objects")]
    public Button backBtn;
    public Button Btn1;
    public Button Btn2;
    public Button Btn3;
    public Button Btn4;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public void Start()
    {
        Btn1.GetComponent<Image>().color = Color.red;
    }
    public void mainSettingBtn_Click()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // public void backBtn_Click()
    // {
    //     mainMenuPanel.SetActive(true);
    //     settingsPanel.SetActive(false);
    // }

    // public void backBtn2_Click()
    // {
    //     containerPanel.SetActive(true);
    //     startGamePanel.SetActive(false);
    // }

    public void BackButton_Click(GameObject backButton)
    {
        // Lấy tên của Parent của nút Back
        string parentName = backButton.transform.parent.name;

        if (parentName == "NavBar")
        {
            mainMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }
        else if (parentName == "StartGamePanel")
        {
            containerPanel.SetActive(true);
            startGamePanel.SetActive(false);
        }
    }

    public void quitBtn_Click()
    {
        Application.Quit();
    }

    public void Btn1_Click()
    {
        Btn1.GetComponent<Image>().color = Color.red;
        Btn2.GetComponent<Image>().color = Color.white;
        Btn3.GetComponent<Image>().color = Color.white;
        Btn4.GetComponent<Image>().color = Color.white;
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
    }
    public void Btn2_Click()
    {
        Btn2.GetComponent<Image>().color = Color.red;
        Btn1.GetComponent<Image>().color = Color.white;
        Btn3.GetComponent<Image>().color = Color.white;
        Btn4.GetComponent<Image>().color = Color.white;

        panel1.SetActive(false);
        panel2.SetActive(true);
        panel3.SetActive(false);
        panel4.SetActive(false);
    }
    public void Btn3_Click()
    {
        Btn3.GetComponent<Image>().color = Color.red;
        Btn1.GetComponent<Image>().color = Color.white;
        Btn2.GetComponent<Image>().color = Color.white;
        Btn4.GetComponent<Image>().color = Color.white;

        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(true);
        panel4.SetActive(false);
    }
    public void Btn4_Click()
    {
        Btn4.GetComponent<Image>().color = Color.red;
        Btn1.GetComponent<Image>().color = Color.white;
        Btn2.GetComponent<Image>().color = Color.white;
        Btn3.GetComponent<Image>().color = Color.white;

        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(true);
    }

    public void startGameBtn_Click()
    {
        containerPanel.SetActive(false);
        startGamePanel.SetActive(true);
    }
}