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
    [SerializeField] private string ASCIICharacters = "#%&Vi*,. ";
    private static int textureSize = 64;
    private Texture2D mainCameraTexture;
    private StringBuilder fullASCIIText = new StringBuilder(textureSize * textureSize + mspace.Length + (textureSize - 1 * newLine.Length));
    private static string mspace = "<mspace=mspace=10>";
    private static string newLine = "<br>";

    private WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();
    void Update()
    {
        StartCoroutine(CreateASCII());
    }

    IEnumerator CreateASCII()
    {
        yield return frameEnd;
        if (mainCamera.activeTexture == null)
            yield return null;

        fullASCIIText.Clear();

        mainCameraTexture = new Texture2D(textureSize, textureSize);
        RenderTexture.active = mainRenderTexture;
        mainCameraTexture.ReadPixels(new Rect(0, 0, mainCameraTexture.width, mainCameraTexture.height), 0, 0);
        mainCameraTexture.Apply();

        fullASCIIText.Append(mspace);

        for (int i = 0; i < textureSize; i++)
        {
            for (int j = 0; j < textureSize; j++)
            {
                char character = GetCharacterForRGBColor(GetBrightnessValueOfPixelInTexture(i, j));
                    
                fullASCIIText.Append(character);
                if (j == textureSize - 1)
                {
                    fullASCIIText.Append(newLine);
                }
            }
        }
        ASCIIText.text = fullASCIIText.ToString();
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
