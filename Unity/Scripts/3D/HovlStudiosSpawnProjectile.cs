using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HovlStudiosSpawnProjectile : SpawnProjectile3D
{
   

    // Update is called once per frame
    override protected void Update()
    {
        if ((AutoFire || Input.GetAxis(InputAxisToInitateSpawnedObject) > 0) && (DateTime.Now - lastSpawnTime).TotalSeconds > secondsBetweenProjectiles)
        {
            //are we within range of an object to shoot at?
            if (TagsOfObjectsToAttack.Length > 0)
            {
                bool foundOne = false;
                //are we within range of one of the specified objects to attack?
                foreach (string tag in TagsOfObjectsToAttack)
                {
                    GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                    foreach (GameObject o in objects)
                    {
                        if (Vector3.Distance(gameObject.transform.position, o.transform.position) <= DistanceToStartAttacking)
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

            ProjectileMover mover = projectile.GetComponent<ProjectileMover>();
            if(mover != null)
            {
                mover.baseSpeed = rb.velocity.magnitude+projectTileSpeed;
            }

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
