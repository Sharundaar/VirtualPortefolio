using UnityEngine;
using System.Collections;
using System.Threading;

public class NoiseGeneratorComponent : MonoBehaviour {

    private PerlinNoiseGenerator m_NoiseGenerator;

    [Range(0, 1)]
    public float m_persistence = 0.5f;

    [Range(1, 8)]
    public int m_octave = 4;

    public float m_maxHeight = 0.25f;

    Renderer m_renderer;

    Thread thread;

    public Terrain terrain;

	// Use this for initialization
	void Start () {
        /*
        Debug.Log("Starting noise generation...");

        Debug.Log("Noise generation ended.");

        Debug.Log("Min Height : " + m_NoiseGenerator.Min);
        Debug.Log("Max Height : " + m_NoiseGenerator.Max);
        */

        thread = new Thread(new ThreadStart(ThreadFunction));
        thread.Start();
	}

    void ThreadFunction()
    {
        Debug.Log("Thread running...");
        m_NoiseGenerator = new PerlinNoiseGenerator(2048, 2048);
        m_NoiseGenerator.GenerateHeightmap(m_octave, m_persistence);
        m_NoiseGenerator.ForceZeroBorder(8, 1.1f);
    }

    void ThreadFinished()
    {
        Debug.Log("Thread finished.");
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.mainTexture = m_NoiseGenerator.DumpHeightmapToTexture();
        m_NoiseGenerator.DumpHeightmapToTerrain(terrain, m_maxHeight);
        thread = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (thread != null && !thread.IsAlive)
            ThreadFinished();
	}
}
