using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public AudioClip GetSound;
    public GameObject GetParticle;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.CoinUp();
            if (GetSound != null)
                AudioManager.instance.PlaySEOnce(GetSound);
            if (GetParticle != null)
                Instantiate(GetParticle, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject, 0.2f);
        }
    }
}
