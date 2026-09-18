using UnityEngine;

public class DefaultUI : MonoBehaviour
{
    public static DefaultUI Instance; void Awake() => Instance = this;
    public bool isShowingUI = false;

    public void Show()
    {
        isShowingUI = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        isShowingUI = false;
        gameObject.SetActive(false);
    }
}
