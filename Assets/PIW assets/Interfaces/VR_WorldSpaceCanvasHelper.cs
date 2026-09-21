using UnityEngine;
using Oculus.Interaction;

/// <summary>
/// Helper to convert a standard Canvas to a VR-ready World Space Canvas compatible with Meta Interaction SDK.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class VR_WorldSpaceCanvasHelper : MonoBehaviour
{
    private void Start()
    {
        SetupCanvas();
    }

    [ContextMenu("Setup for VR")]
    public void SetupCanvas()
    {
        Canvas canvas = GetComponent<Canvas>();
        
        // 1. Force World Space
        canvas.renderMode = RenderMode.WorldSpace;
        
        // 2. Ensure PointableCanvas is present for Ray Interaction
        if (GetComponent<PointableCanvas>() == null)
        {
            gameObject.AddComponent<PointableCanvas>();
        }

        // 3. Ensure RayInteractable is present for UI selection
        if (GetComponent<RayInteractable>() == null)
        {
            var ray = gameObject.AddComponent<RayInteractable>();
            // Typically UI canvases should have a surface defined, 
            // but PointableCanvas handles much of the mapping.
        }

        Debug.Log($"Canvas {gameObject.name} configured for VR World Space interaction.");
    }
}
