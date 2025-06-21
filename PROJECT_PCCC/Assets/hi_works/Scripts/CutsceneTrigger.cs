using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector director;
    public Camera cam1;
    public Camera cam2;
    public GameObject Minimap; // Đối tượng chứa cutscene

    void Start()
    {
        director.stopped += OnCutsceneEnd;
        director.Play();
    }

    void OnCutsceneEnd(PlayableDirector pd)
    {
        cam1.gameObject.SetActive(false); // Tắt camera khi cutscene kết thúc
        cam2.gameObject.SetActive(false); // Tắt camera khi cutscene kết thúc
        Minimap.SetActive(true); // Bật lại minimap
        // gameObject.SetActive(false); // Nếu bạn muốn tắt luôn đối tượng đang chứa script này
    }
}
