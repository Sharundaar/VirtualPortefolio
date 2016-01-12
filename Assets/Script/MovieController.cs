using UnityEngine;
using System.Collections;

public class MovieController : MonoBehaviour {

    public MovieTexture Movie;

    void Start()
    {
        Movie.loop = true;
    }

	void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
            Movie.Play();
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
            Movie.Pause();
    }
}
