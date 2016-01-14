using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    [SerializeField]
    Vector3 Axis = Vector3.up;

    [SerializeField]
    float Speed = 1.0f;

	// Update is called once per frame
	void Update () {
        transform.Rotate(Axis.normalized, Speed * 360.0f * Time.deltaTime);
	}
}
