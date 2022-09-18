using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ASCIIArtGenerator : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RenderTexture mainRenderTexture;
    [SerializeField] private TextMeshProUGUI ASCIIText;
    [SerializeField] private float alpha = 0;
    private string ASCIICharacters = "#%&Vi*.  ";
    private Texture2D mainCameraTexture;
    private StringBuilder fullASCIIText = new StringBuilder(64 * 64 + mspace.Length + (64 - 1 * newLine.Length));
    private static string mspace = "<mspace=mspace=10>";
    private static string newLine = "<br>";

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
            fullASCIIText.Clear();

            mainCameraTexture = new Texture2D(64, 64);
            RenderTexture.active = mainRenderTexture;
            mainCameraTexture.ReadPixels(new Rect(0, 0, mainCameraTexture.width, mainCameraTexture.height), 0, 0);
            mainCameraTexture.Apply();

            fullASCIIText.Append(mspace);

            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    char character = GetCharacterForRGBColor(GetBrightnessValueOfPixelInTexture(i, j));
                    
                    fullASCIIText.Append(character);
                    if (j == 63)
                    {
                        fullASCIIText.Append(newLine);
                    }
                }
            }
            ASCIIText.text = fullASCIIText.ToString();
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
        return total / 3;
    }

    char GetCharacterForRGBColor(float brightnessValue)
    {
        return ASCIICharacters[Mathf.RoundToInt(brightnessValue * (ASCIICharacters.Length - 1))];
    }
}
