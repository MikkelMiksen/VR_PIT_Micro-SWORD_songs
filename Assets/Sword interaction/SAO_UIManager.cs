using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

/// <summary>Legacy SAO menu flow hosted on Unity 6.6 UI Toolkit PanelRenderer.</summary>
public class SAO_UIManager : MonoBehaviour
{
    public static SAO_UIManager Instance { get; private set; }

    [Header("UI Overlays")]
    public OVROverlayCanvas mainOverlay;
    public PanelRenderer panelRenderer;

    [Header("Visual Tree Assets")]
    public VisualTreeAsset scanningTemplate;
    public VisualTreeAsset mainMenuTemplate;
    public VisualTreeAsset hudTemplate;
    public VisualTreeAsset gameOverTemplate;

    private VisualElement _root;
    private VisualElement _currentScreen;
    private VisualElement _healthBarFill;
    private VisualElement _staminaBarFill;
    private Label _waveText;
    private Label _enemyCountText;
    private Label _scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (panelRenderer == null) panelRenderer = GetComponent<PanelRenderer>();
        if (panelRenderer == null && mainOverlay != null) panelRenderer = mainOverlay.GetComponent<PanelRenderer>();
    }

    private void OnEnable()
    {
        if (panelRenderer != null) panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        if (panelRenderer != null) panelRenderer.UnregisterUIReloadCallback(OnUIReload);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        _root = root;
    }

    private void SetScreen(VisualTreeAsset template)
    {
        if (_root == null || template == null)
        {
            Debug.LogWarning("[SAO] Cannot set screen - PanelRenderer root or template is null");
            return;
        }

        _root.Clear();
        _currentScreen = template.Instantiate();
        _currentScreen.style.flexGrow = 1;
        _currentScreen.style.width = Length.Percent(100f);
        _currentScreen.style.height = Length.Percent(100f);
        _root.Add(_currentScreen);
    }

    public void ShowScanning() => SetScreen(scanningTemplate);

    public void ShowMainMenu()
    {
        SetScreen(mainMenuTemplate);
        var playButton = _root?.Q<Button>("PlayButton");
        if (playButton != null)
        {
            playButton.clicked += () =>
            {
                /*
                if (ARGameManager.Instance != null)
                    ARGameManager.Instance.StartGame();
                */
            };
        }
    }

    public void ShowHUD()
    {
        SetScreen(hudTemplate);
        if (_root == null) return;
        _healthBarFill = _root.Q<VisualElement>("HealthBarFill");
        _staminaBarFill = _root.Q<VisualElement>("StaminaBarFill");
        _waveText = _root.Q<Label>("WaveText");
        _enemyCountText = _root.Q<Label>("EnemyCountText");
        _scoreText = _root.Q<Label>("ScoreText");
    }

    public void ShowGameOver(int score, int wave)
    {
        SetScreen(gameOverTemplate);
        if (_root == null) return;
        var scoreLabel = _root.Q<Label>("FinalScoreText");
        var waveLabel = _root.Q<Label>("FinalWaveText");
        var retryButton = _root.Q<Button>("RetryButton");
        if (scoreLabel != null) scoreLabel.text = $"Final Score: {score}";
        if (waveLabel != null) waveLabel.text = $"Wave Cleared: {wave}";
        if (retryButton != null)
            retryButton.clicked += () => UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void UpdateHUD(float healthPercent, float staminaPercent, int wave, int enemiesRemaining, int score)
    {
        float hpClamp = Mathf.Clamp01(float.IsNaN(healthPercent) ? 0f : healthPercent);
        float spClamp = Mathf.Clamp01(float.IsNaN(staminaPercent) ? 0f : staminaPercent);
        if (_healthBarFill != null) _healthBarFill.style.width = Length.Percent(hpClamp * 100f);
        if (_staminaBarFill != null) _staminaBarFill.style.width = Length.Percent(spClamp * 100f);
        if (_waveText != null) _waveText.text = $"Wave: {wave}";
        if (_enemyCountText != null) _enemyCountText.text = $"Enemies: {enemiesRemaining}";
        if (_scoreText != null) _scoreText.text = $"Score: {score}";
    }
}
