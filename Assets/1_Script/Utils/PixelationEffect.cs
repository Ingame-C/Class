using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PixelationEffect : MonoBehaviour
{
    public Material pixelationMaterial;
    [SerializeField, Range(0.001f, 0.1f)] private float pixelSize = 0.01f;  // 픽셀 크기
    [SerializeField, Range(0.001f, 1f)] private float darkness = 1f;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (pixelationMaterial != null)
        {
            pixelationMaterial.SetFloat("_PixelSize", pixelSize);
            pixelationMaterial.SetFloat("_Darkness", darkness);
            Graphics.Blit(source, destination, pixelationMaterial);
        }
        else
        {
            Graphics.Blit(source, destination); // 셰이더가 없으면 기본 렌더링
        }
    }

    public void SetDarkness(float darkness)
    {
        this.darkness = Mathf.Clamp01(darkness);
    }
}