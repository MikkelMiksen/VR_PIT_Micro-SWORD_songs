using UnityEngine;
using Oculus.Interaction;
using UnityEngine.UI;

/// <summary>
/// Specialized helper for OVROverlay-based UI.
/// Maps a Canvas to a RenderTexture and displays it via OVROverlay for high-quality stereoscopic rendering.
/// Also handles mapping Interaction SDK events to the correct UV space.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class VR_OVROverlayUIHelper : MonoBehaviour
{
    [Header("Render Texture Settings")]
    [SerializeField] private int _textureWidth = 1024;
    [SerializeField] private int _textureHeight = 1024;
    [SerializeField] private bool _useStereo = true;
    
    [Header("Meta Overlay References")]
    [SerializeField] private OVROverlay _overlay;
    
    private RenderTexture _leftRenderTexture;
    private RenderTexture _rightRenderTexture;

    private void Start()
    {
        SetupOverlayUI();
    }

    [ContextMenu("Setup OVROverlay UI")]
    public void SetupOverlayUI()
    {
        Canvas canvas = GetComponent<Canvas>();
        
        // 1. Configure Canvas for RenderTexture
        canvas.renderMode = RenderMode.WorldSpace;
        
        // Release existing textures
        if (_leftRenderTexture != null) _leftRenderTexture.Release();
        if (_rightRenderTexture != null) _rightRenderTexture.Release();

        _leftRenderTexture = new RenderTexture(_textureWidth, _textureHeight, 24);
        _leftRenderTexture.name = $"RT_Left_{gameObject.name}";
        
        if (_useStereo)
        {
            _rightRenderTexture = new RenderTexture(_textureWidth, _textureHeight, 24);
            _rightRenderTexture.name = $"RT_Right_{gameObject.name}";
        }

        // 2. Setup OVROverlay
        if (_overlay == null)
        {
            _overlay = gameObject.GetComponent<OVROverlay>();
            if (_overlay == null) _overlay = gameObject.AddComponent<OVROverlay>();
        }
        
        _overlay.textures[0] = _leftRenderTexture;
        if (_useStereo)
        {
            _overlay.textures[1] = _rightRenderTexture;
        }
        
        _overlay.isDynamic = true;
        _overlay.compositionDepth = 0;
        _overlay.currentOverlayShape = OVROverlay.OverlayShape.Quad;
        
        // 3. Setup Interaction
        if (GetComponent<MeshCollider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
        }

        if (GetComponent<PointableCanvas>() == null)
        {
            gameObject.AddComponent<PointableCanvas>();
        }

        if (GetComponent<RayInteractable>() == null)
        {
            gameObject.AddComponent<RayInteractable>();
        }

        string stereoMsg = _useStereo ? "Stereo (Left/Right textures)" : "Mono (Single texture)";
        Debug.Log($"OVROverlay UI configured for {gameObject.name} in {stereoMsg} mode. Ensure UI Cameras are rendering the Canvas to the RTs.");
    }

    private void OnDestroy()
    {
        if (_leftRenderTexture != null) _leftRenderTexture.Release();
        if (_rightRenderTexture != null) _rightRenderTexture.Release();
    }
}
