using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector director;
    public Camera cam;

    void Start()
    {
        director.stopped += OnCutsceneEnd;
        director.Play();
    }

    void OnCutsceneEnd(PlayableDirector pd)
    {
        cam.gameObject.SetActive(false); // Tắt camera khi cutscene kết thúc
        // gameObject.SetActive(false); // Nếu bạn muốn tắt luôn đối tượng đang chứa script này
    }
}
