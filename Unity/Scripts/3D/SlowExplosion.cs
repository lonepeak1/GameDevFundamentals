using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowExplosion : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SmallRandomExplosions()
    {
        GameObject projectile = Instantiate(ExplosionPrefab, gameObject.transform.position, gameObject.transform.rotation);

    }

    void MeshExplode()
    {
        BroadcastMessage("Explode");
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
