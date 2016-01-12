using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    Rigidbody m_rigidbody;
    public Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        // transform.position += velocity * Time.deltaTime;
	}

    void OnCollisionEnter(Collision collision)
    {
        // rigidbody.velocity = (transform.position - collision.transform.position).normalized * velocity.magnitude;
    }
}
