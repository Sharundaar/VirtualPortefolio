using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

    public GameObject AsteroidTemplate;
    public int AsteroidCount = 10;
    public float padding = 0.1f;

    private GameObject[] m_asteroids;
    private BoxCollider m_spawnArea;

    private int m_frameCounter = 0;

	// Use this for initialization
	void Start () {
        m_spawnArea = GetComponent<BoxCollider>();

        m_asteroids = new GameObject[AsteroidCount];
        for(int i=0 ; i<AsteroidCount; ++i)
        {
            m_asteroids[i] = Instantiate(AsteroidTemplate);
            m_asteroids[i].name = "Asteroid" + i;
            m_asteroids[i].tag = "Asteroids";

            Initialize(m_asteroids[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        m_frameCounter++;
        if(m_frameCounter >= 10) // Every 10 Frames or so
        {
            for(int i=0 ; i<AsteroidCount ; ++i)
            {
                if (m_asteroids[i].GetComponent<Rigidbody>().velocity.magnitude < 1f)
                    Initialize(m_asteroids[i]);
            }
        }
	}

    void Initialize(GameObject asteroid)
    {
        asteroid.transform.position = RandomizePosition();
        asteroid.GetComponent<Rigidbody>().isKinematic = false;
        asteroid.GetComponent<Rigidbody>().velocity = (m_spawnArea.bounds.center - asteroid.transform.position) * Mathf.Lerp(0.1f, 1.0f, Random.value);
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Asteroids")
        {
            for(int i=0 ; i<AsteroidCount; ++i)
            {
                if(m_asteroids[i] == collider.gameObject)
                {
                    Initialize(m_asteroids[i]);
                    break;
                }
            }
        }
    }

    Vector3 RandomizePosition()
    {
        Vector3 pos = new Vector3();
        int loopCount = 0;

        do
        {
            float side = Random.value;
            float offset = Random.value;

            if (side < .25f) // left side
            {
                pos.x = m_spawnArea.bounds.center.x;
                pos.y = Mathf.Lerp(m_spawnArea.bounds.min.y + padding, m_spawnArea.bounds.max.y - padding, offset);
                pos.z = m_spawnArea.bounds.min.z + padding;
            }
            else if (side < .5f) // top side
            {
                pos.x = m_spawnArea.bounds.center.x;
                pos.y = m_spawnArea.bounds.max.y - padding;
                pos.z = Mathf.Lerp(m_spawnArea.bounds.min.z + padding, m_spawnArea.bounds.max.z - padding, offset);
            }
            else if (side < .75f) // right side
            {
                pos.x = m_spawnArea.bounds.center.x;
                pos.y = Mathf.Lerp(m_spawnArea.bounds.min.y + padding, m_spawnArea.bounds.max.y - padding, offset);
                pos.z = m_spawnArea.bounds.max.z - padding;
            }
            else // bottom side
            {
                pos.x = m_spawnArea.bounds.center.x;
                pos.y = m_spawnArea.bounds.min.y + padding;
                pos.z = Mathf.Lerp(m_spawnArea.bounds.min.z + padding, m_spawnArea.bounds.max.z - padding, offset);
            }

            ++loopCount;
        } while (Physics.OverlapSphere(pos, AsteroidTemplate.transform.localScale.x).Length > 1 && loopCount < 10);

        Debug.Log("Loop Count Exeeded !");

        return pos;
    }
}
