using UnityEngine;
using System.Collections;

public class Telepad : MonoBehaviour {

    [SerializeField]
    Transform m_destination;

	void OnTriggerEnter(Collider _collider)
    {
        _collider.transform.position = m_destination.position;
        _collider.transform.rotation = m_destination.rotation;

        if(_collider.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>() != null)
        {
            _collider.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().SetLookRotation(m_destination);
        }
    }
}
