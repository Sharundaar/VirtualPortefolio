using UnityEngine;
using System.Collections;

public class Oscilate : MonoBehaviour {

    [SerializeField]
    float Amplitude = 0.5f;

    [SerializeField]
    float Frequency = 1;

    [SerializeField]
    Vector3 Axis = Vector3.up;

    Vector3 m_start;

    float m_timer = 0;

    void Start()
    {
        m_start = transform.position;
    }

    // Update is called once per frame
    void Update () {
        m_timer += Time.deltaTime;
        transform.position = m_start + (Axis.normalized * Amplitude * Mathf.Sin(m_timer / Frequency));
	}
}
