using UnityEngine;

public class MouseSwitching : MonoBehaviour
{
    [SerializeField] private Renderer overlayRenderer;
    private Material overlayMaterial;
    
    void Awake()
    {
        overlayMaterial = overlayRenderer.material;
        overlayRenderer.enabled = false;
    }

    public void SetTint(Color color)
    {
        overlayRenderer.enabled = true;
        overlayMaterial.color = color;
    }

    public void ResetTint()
    {
        overlayRenderer.enabled = false;
    }
}