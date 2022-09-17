using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ASCIIArtGenerator : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Texture2D mainCameraTexture;
    [SerializeField] private RenderTexture mainRenderTexture;
    [SerializeField] private TextMeshProUGUI ASCIIText;
    [SerializeField] private float alpha = 0;
    private string ASCIICharacters = "#%&Vi^*_-,.";
    private string fullScreenInASCII = "";

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(CreateASCII());
    }
    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    IEnumerator CreateASCII()
    {
        yield return frameEnd;
        if (mainCamera.activeTexture != null)
        {
            fullScreenInASCII = "";
            mainCameraTexture = new Texture2D(64, 64);
            RenderTexture.active = mainRenderTexture;
            mainCameraTexture.ReadPixels(new Rect(0, 0, mainCameraTexture.width, mainCameraTexture.height), 0, 0);
            mainCameraTexture.Apply();
            fullScreenInASCII += "<mspace=mspace=10>";
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    char character = GetCharacterForRGBColor(GetBrightnessValueOfPixelInTexture(i, j));
                    fullScreenInASCII += character.ToString();
                    if (j == 63)
                    {
                        fullScreenInASCII += "<br>";
                    }
                }
            }
            fullScreenInASCII += "</mspace>";
            ASCIIText.text = fullScreenInASCII;
        }
    }

    void GetCameraTexture()
    {
        
    }

    float GetBrightnessValueOfPixelInTexture(int x, int y)
    {
        Color color = mainCameraTexture.GetPixel(x, y);
        float r = color.r;
        float g = color.g;
        float b = color.b;
        var total = r + g + b;
        Debug.Log(total);
        return total / 3;
    }

    char GetCharacterForRGBColor(float brightnessValue)
    {
        int totalCharacterCount = ASCIICharacters.Length;
        int maxColorValue = 1;
        float steps = (float)maxColorValue / (float)totalCharacterCount;
        return ASCIICharacters[Mathf.RoundToInt(steps * brightnessValue * 10)];
    }
}
