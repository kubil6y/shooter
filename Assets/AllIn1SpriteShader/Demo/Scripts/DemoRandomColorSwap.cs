using UnityEngine;

namespace AllIn1SpriteShader
{
    public class DemoRandomColorSwap : MonoBehaviour
    {
        private Material mat;
        private readonly int colorSwapRed = Shader.PropertyToID("_ColorSwapRed");
        private readonly int colorSwapGreen = Shader.PropertyToID("_ColorSwapGreen");
        private readonly int colorSwapBlue = Shader.PropertyToID("_ColorSwapBlue");

        void Start()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                mat = GetComponent<Renderer>().material;
                if (mat != null) InvokeRepeating(nameof(NewColor), 0.0f, 0.6f);
                else
                {
                    Debug.LogError("No material found");
                    Destroy(this);
                }
            }
        }

        private void NewColor()
        {
            mat.SetColor(colorSwapRed, GenerateColor());
            mat.SetColor(colorSwapGreen, GenerateColor());
            mat.SetColor(colorSwapBlue, GenerateColor());
        }

        private Color GenerateColor()
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
}