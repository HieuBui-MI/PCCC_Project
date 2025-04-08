using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Main Menu Panels")]
    public GameObject Main1;
    public GameObject Main2;
    [Header("NewGame Panels")]
    public GameObject NewGamePanel;
    public TMP_InputField nameInputField;
    [Header("LoadSave Panel")]
    public GameObject LoadSavePanel;
    public Transform saveSlotContainer; // nơi chứa các button save
    public GameObject saveSlotPrefab;   // prefab slot

    public void StartGameBtn_Main1_CLick()
    {
        Main1.SetActive(false);
        Main2.SetActive(true);
    }
    public void BackBtn_Main2_Click()
    {
        Main1.SetActive(true);
        Main2.SetActive(false);
    }

    public void NewGameBtn_Main2_Click()
    {
        Main2.SetActive(false);
        NewGamePanel.SetActive(true);
    }
    public void BackBtn_NewGamePanel_Click()
    {
        Main2.SetActive(true);
        NewGamePanel.SetActive(false);
    }

    public void submitBtn_Click()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Tên nhân vật không được để trống.");
            return;
        }

        // Kiểm tra trùng tên
        string existing = PlayerPrefs.GetString("SaveList", "");
        string[] saves = existing.Split(',');

        foreach (string name in saves)
        {
            if (name == playerName)
            {
                Debug.LogWarning("Tên đã tồn tại, hãy chọn tên khác.");
                return;
            }
        }

        // Lưu dữ liệu
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("Save_" + playerName + "_Level", 0);
        PlayerPrefs.SetString("SaveList", string.Join(",", AddToList(saves, playerName)));
        PlayerPrefs.Save();

        // Load Scene 1
        SceneManager.LoadScene("hu_scene");
    }

    private string[] AddToList(string[] oldList, string newItem)
    {
        List<string> updated = new List<string>(oldList);
        updated.RemoveAll(string.IsNullOrEmpty);
        updated.Add(newItem);
        return updated.ToArray();
    }

    // Xử lý nút Load Game

    public void LoadSaveBtn_Main2_Click()
    {
        Main2.SetActive(false);
        LoadSavePanel.SetActive(true);
        ShowSaveFiles();
    }

    public void BackBtn_LoadSavePanel_Click()
    {
        LoadSavePanel.SetActive(false);
        Main2.SetActive(true);
    }

    void ShowSaveFiles()
    {
        // Xóa các slot cũ (nếu có)
        foreach (Transform child in saveSlotContainer)
        {
            Destroy(child.gameObject);
        }

        string existing = PlayerPrefs.GetString("SaveList", "");
        string[] saves = existing.Split(',');

        if (saves.Length == 0 || (saves.Length == 1 && string.IsNullOrEmpty(saves[0])))
        {
            Debug.Log("Không có file save nào.");
            return;
        }

        foreach (string name in saves)
        {
            if (string.IsNullOrEmpty(name)) continue;

            GameObject slot = Instantiate(saveSlotPrefab, saveSlotContainer);
            slot.GetComponentInChildren<TMP_Text>().text = name;

            // Gán sự kiện click
            Button btn = slot.GetComponentInChildren<Button>();
            string playerName = name; // tạo bản sao cho lambda

            btn.onClick.AddListener(() =>
            {
                LoadSavedGame(playerName);
            });
        }
    }

    void LoadSavedGame(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        int savedLevel = PlayerPrefs.GetInt("Save_" + playerName + "_Level", 0);
        // Load đến scene tương ứng (ví dụ Scene2)/
        if (savedLevel == 0)
        {
            SceneManager.LoadScene("hu_scene");
        }
        else if (savedLevel >= 1)
        {
            SceneManager.LoadScene("hi_scene");
        }

    }
}
