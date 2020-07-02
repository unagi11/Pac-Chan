using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject GetParticle;

    Camera main;

    private void Start()
    {
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(main.transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (CompareTag("Score"))
            {
                PlayerManager.instance.GetScore();
            }
            else if (CompareTag("Power"))
            {
                PlayerManager.instance.GetPower();
            }

            if (GetParticle != null)
                Instantiate(GetParticle, transform.position, Quaternion.identity);

            Destroy(transform.parent.gameObject, 0.2f);
        }
    }
}
