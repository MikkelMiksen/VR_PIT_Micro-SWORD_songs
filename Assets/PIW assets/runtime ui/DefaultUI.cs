using UnityEngine;

public class DefaultUI : MonoBehaviour
{
    
    public static DefaultUI Instance; void Awake() => Instance = this;
    
    /// <summary>
    /// Is the UI currently showing?
    /// </summary>
    public bool isShowingUI = false;

    /// <summary>
    /// Shows the UI
    /// </summary>
    public void Show()
    {
        isShowingUI = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the UI
    /// </summary>
    public void Hide()
    {
        isShowingUI = false;
        gameObject.SetActive(false);
    }
}
