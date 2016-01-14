using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinNoiseGenerator
{
    private const int GRADIENT_COUNT = 256;

    private int m_width;
    private int m_height;

    private Vector2[] m_gradients;
    private int[] m_permutations;

    private System.Random m_randomizer;
    private float[] m_heightmap;
    private List<float[]> m_octaves;

    public PerlinNoiseGenerator(int _width, int _height)
    {
        m_randomizer = new System.Random();

        m_width = _width;
        m_height = _height;

        m_octaves = new List<float[]>();
        m_heightmap = new float[m_width * m_height];

        InitGradient();
        InitPermutation();
    }

    public float Min { get; private set; }
    public float Max { get; private set; }

    private int[] P
    {
        get { return m_permutations; }
    }

    private Vector2[] G
    {
        get { return m_gradients; }
    }

    private float Rnd
    {
        get { return (float)m_randomizer.NextDouble(); }
    }

    private void InitGradient()
    {
        m_gradients = new Vector2[GRADIENT_COUNT];
        int i = 0;
        while(i < GRADIENT_COUNT)
        {
            float x = (Rnd * 2.0f) - 1.0f, y = (Rnd * 2.0f) - 1.0f;
            if((x*x + y*y) < 1.0f && (x*x + y*y) > float.Epsilon)
            {
                m_gradients[i] = new Vector2(x, y);
                ++i;
            }
        }

        for(i=0; i<m_gradients.Length; ++i)
        {
            m_gradients[i] = m_gradients[i].normalized;
        }
    }

    private void InitHeightmap()
    {

    }

    private void InitPermutation()
    {
        m_permutations = new int[GRADIENT_COUNT * 2];
        for(int i=0; i< GRADIENT_COUNT; ++i)
        {
            m_permutations[i] = i;
        }

        for (int n = 0; n < 10000; ++n)
        {
            int i1 = Mathf.FloorToInt(Rnd * 256) % 256;
            int i2 = Mathf.FloorToInt(Rnd * 256) % 256;

            int temp = m_permutations[i1];
            m_permutations[i1] = m_permutations[i2];
            m_permutations[i2] = temp;
        }

        for(int i=256; i<m_permutations.Length; ++i)
        {
            m_permutations[i] = m_permutations[i - 256];
        }
    }

    private void ComputeOctave(int _octave, float _persistence, bool _absolute)
    {
        m_octaves.Clear();

        float ampl = 1.0f, freq = 2.0f;
        for(int i=0; i < _octave; ++i)
        {
            float[] octave = new float[m_width * m_height];
            for(int j = 0; j < octave.Length; ++j)
            {
                if (_absolute)
                    octave[j] = ampl * Mathf.Abs(Noise(((j % m_width) / (float)m_width) * freq, ((j / m_width) / (float) m_height) * freq) );
                else
                    octave[j] = ampl * Noise(((j % m_width) / (float)m_width) * freq, ((j / m_width) / (float)m_height) * freq);
            }

            ampl *= _persistence;
            freq *= 2.0f;

            m_octaves.Add(octave);
        }
    }

    private void ComputeHeightmap()
    {
        for(int i=0; i<m_heightmap.Length; ++i)
        {
            float sum = 0;
            for(int j = 0; j < m_octaves.Count; ++j)
            {
                sum += m_octaves[j][i];
            }

            m_heightmap[i] = sum;
        }
    }

    private void ComputeMinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;

        for(int i = 0; i<m_heightmap.Length; ++i)
        {
            if (m_heightmap[i] < Min)
                Min = m_heightmap[i];
            if (m_heightmap[i] > Max)
                Max = m_heightmap[i];
        }
    }

    private float Noise(float x, float y)
    {
        int X = Mathf.FloorToInt(x) & 255, Y = Mathf.FloorToInt(y) & 255;

        float u = s_curve(x - Mathf.Floor(x)), v = s_curve(y - Mathf.Floor(y));

        int A = P[X + P[Y]];
        int B = P[X + 1 + P[Y + 1]];
        int AA = P[X + P[Y + 1]];
        int BB = P[X + 1 + P[Y]];

        Vector2 gA = G[A % GRADIENT_COUNT], gD = G[AA % GRADIENT_COUNT], gB = G[BB % GRADIENT_COUNT], gC = G[B % GRADIENT_COUNT];
        Vector2 vA = new Vector2(X, Y), vB = new Vector2(X + 1, Y), vC = new Vector2(X + 1, Y + 1), vD = new Vector2(X, Y + 1), vP = new Vector2(x < 256.0f ? x : x - 256.0f, y < 256.0f ? y : y - 256.0f);
        vA = vP - vA; vB = vP - vB; vC = vP - vC; vD = vP - vD;
        float S = Vector2.Dot(gA, vA), T = Vector2.Dot(gD, vD), U = Vector2.Dot(gB, vB), V = Vector2.Dot(gC, vC);

        return lerp(lerp(S, U, u),
                    lerp(T, V, u), v);
    }

    private float s_curve(float a, float b, float t)
    {
        return (b - a) * (t * t * (3.0f - 2.0f * t)) + a;
    }

    private float s_curve(float t)
    {
        return (t * t * (3.0f - 2.0f * t));
    }

    private float lerp(float a, float b , float t)
    {
        return (b - a) * t + a;
    }

    public void GenerateHeightmap(int _octave, float _persistence)
    {
        ComputeOctave(_octave, _persistence, false);
        ComputeHeightmap();
        ComputeMinMax();
    }

    public Texture2D DumpHeightmapToTexture()
    {
        Texture2D texture = new Texture2D(m_width, m_height);

        Color[] colors = new Color[m_width * m_height];
        for(int i=0; i<m_heightmap.Length; ++i)
        {
            float rgb = (m_heightmap[i] + 1.0f) / 2.0f;
            colors[i] = new Color(rgb, rgb, rgb);
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    public void DumpHeightmapToTerrain(Terrain _terrain, float _maxheight)
    {
        int twidth = _terrain.terrainData.heightmapWidth, theight = _terrain.terrainData.heightmapHeight;
        float[,] heightmap = new float[twidth, theight];
        
        for(int x = 0; x < twidth; ++x)
        {
            for(int y = 0; y < theight; ++y)
            {
                int hx = x * m_width / twidth, hy = y * m_height / theight;
                heightmap[x, y] = ((m_heightmap[hx + hy * m_width] -Min) / (Max - Min)) * _maxheight;
            }
        }

        _terrain.terrainData.SetHeights(0, 0, heightmap);
        _terrain.Flush();
    }
}
