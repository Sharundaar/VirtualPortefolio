using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

    Renderer m_renderer;

	// Use this for initialization
	void Start () {
        m_renderer = GetComponent<Renderer>();

        m_renderer.material.mainTexture = GenerateTexture();
	}

    private Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(512, 512);
        Color[] colors = new Color[512 * 512];

        for(int x=0; x < 512; ++x)
        {
            for(int y = 0; y < 512; ++y)
            {
                float grey = ComputeGreyValue(x, y);
                colors[x + y * 512] = new Color(grey, grey, grey);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    private float ComputeGreyValue(int x, int y)
    {
        float rx = (x - 256.0f) / 256.0f * 1.1f;
        float ry = (y - 256.0f) / 256.0f * 1.1f;

        return Mathf.Clamp01(Mathf.Pow(rx, 6) + Mathf.Pow(ry, 6));
    }
}
