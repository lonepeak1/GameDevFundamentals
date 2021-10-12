using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile2D_v2
    : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject projectTilePrefab;
    public float projectTileSpeed = 1;
    DateTime lastSpawnTime = DateTime.Now;
    public int millisecondsBetweenProjectiles = 1;
    public bool AutoFire = false;
    public float secondsBeforeProjectileDies = 10;
    public float secondsBeforeShotEffectsDestroy = 1;

    public GameObject shotEffectsPrefab;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if ((AutoFire || Input.GetAxis("Fire1") > 0) && (DateTime.Now-lastSpawnTime).TotalMilliseconds > millisecondsBetweenProjectiles)
        {
            //Debug.Log(rb.velocity.magnitude);
            //spawn the projectile
            GameObject projectile = Instantiate(projectTilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            //add force to the projectile so it moves
            projectile.GetComponent<Rigidbody2D>().AddForce(spawnPoint.transform.up * (projectTileSpeed + (rb.velocity.magnitude)), ForceMode2D.Impulse);
            //set the destroy time on the projectile
            Destroy(projectile, secondsBeforeProjectileDies);
            lastSpawnTime = DateTime.Now;

            //do we have a "fire" image to play?
            if (shotEffectsPrefab != null)
            {
                GameObject image = Instantiate(shotEffectsPrefab, spawnPoint.transform);
                Destroy(image, secondsBeforeShotEffectsDestroy);
            }
        }

    }
}
