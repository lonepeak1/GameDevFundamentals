using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile3D
    : MonoBehaviour
{

    public string InputAxisToInitateSpawnedObject = "Fire1";
    public GameObject spawnPoint;
    public GameObject projectTilePrefab;
    public float projectTileSpeed = 30f;
    protected DateTime lastSpawnTime = DateTime.Now;
    //tags of gameobjects to shoot at
    public string[] TagsOfObjectsToAttack;
    public float DistanceToStartAttacking = 999f;
    public double secondsBetweenProjectiles = 1;
    public bool AutoFire = false;
    public float secondsBeforeProjectileDies = 10;
    public float secondsBeforeShotEffectsDestroy = 1;

    public GameObject shotEffectsPrefab;

    protected Rigidbody rb;
    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.GetComponentInParent<Rigidbody>();
        if (spawnPoint == null)
            spawnPoint = gameObject;
    }

    //if you come with in the specified range of an object, start attacking.

    // Update is called once per frame
    protected virtual void Update()
    {


        if ((AutoFire || Input.GetAxis(InputAxisToInitateSpawnedObject) > 0) && (DateTime.Now - lastSpawnTime).TotalSeconds > secondsBetweenProjectiles)
        {
            //are we within range of an object to shoot at?
            if(TagsOfObjectsToAttack.Length>0)
            {
                bool foundOne = false;
                //are we within range of one of the specified objects to attack?
                foreach(string tag in TagsOfObjectsToAttack)
                {
                    GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                    foreach(GameObject o in objects)
                    {
                        if(Vector3.Distance(gameObject.transform.position,o.transform.position)<=DistanceToStartAttacking)
                        {
                            foundOne = true;
                            continue;
                        }
                    }
                }
                if (!foundOne)
                    return;
            }

            //Debug.Log(rb.velocity.magnitude);
            //spawn the projectile
            GameObject projectile = Instantiate(projectTilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            //add force to the projectile so it moves
            Rigidbody projectileRigidBody = projectile.GetComponent<Rigidbody>();

            if (projectileRigidBody != null)
            {
                if (rb != null)
                    projectileRigidBody.AddForce(spawnPoint.transform.forward * (projectTileSpeed + (rb.velocity.magnitude)), ForceMode.Impulse);
                else
                    projectileRigidBody.AddForce(spawnPoint.transform.forward * (projectTileSpeed), ForceMode.Impulse);
            }
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