using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PixelationEffect : MonoBehaviour
{
    public Material pixelationMaterial;
    [Range(0.001f, 0.1f)]
    public float pixelSize = 0.01f;  // 픽셀 크기

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (pixelationMaterial != null)
        {
            pixelationMaterial.SetFloat("_PixelSize", pixelSize);
            Graphics.Blit(source, destination, pixelationMaterial);
        }
        else
        {
            Graphics.Blit(source, destination); // 셰이더가 없으면 기본 렌더링
        }
    }
}