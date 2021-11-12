/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls when a game object is destroyed when it is hit. Examples of how this may be used:
/// 1.) Platformer player jumps on another player
/// 2.) A projectile hits something.
/// 3.) Player punches or hits another object.
/// </summary>
public class TakeHitController : MonoBehaviour
{
    public float NumberOfHitsUntilDestroyMe = 1;
    public string AnimationTriggerToFireWhenHit = "";
    public string AnimationTriggerToFireWhenDead = "";
    public float TimeToDelayDestoryWhenDead = 0;
    public GameObject prefabForHitEffects;
    public string TagOfGameObjectToCauseDamage = "Player";
    public string AnimationOfCollidingObjectToCauseDamage = ""; //The damage only occurs if the player is performing a specific animation at the time of collision.
    public Component[] ComponentsToDisableWhileTakingDamage = null;
    public float timeBetweenHits = 500f;//duration between hits.
    private int numHits = 0;
    private System.DateTime lastHitTime;

    bool isRecoveringFromDamage = false;
    bool allowRecoverFromDamage = true;
    private Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        lastHitTime = System.DateTime.Now;
        myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if this object has been damaged and the damage features have been turned off, check if we need to turn them back on.
        if (allowRecoverFromDamage && isRecoveringFromDamage && (System.DateTime.Now - lastHitTime).TotalMilliseconds > timeBetweenHits)
        {
            foreach(Component c in ComponentsToDisableWhileTakingDamage)
                if (c is MonoBehaviour)
                     ((MonoBehaviour)c).enabled = true;
                else if (c is Collider)
                    ((Collider)c).enabled = true;

            isRecoveringFromDamage = false;
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        //if this script is disabled, return.
        if (!this.enabled)
            return;

        //did we collide with the object(or parent of the object) that causes damage?
        if(!isRecoveringFromDamage && IsDamageCollision(collision.gameObject))
        {
            //is the appropriate animation running?
            if(AnimationOfCollidingObjectToCauseDamage != string.Empty)
            {

                Animator anim = collision.gameObject.GetComponentInChildren<Animator>();
                if (anim == null)
                    anim = collision.GetComponentInParent<Animator>();
                if(anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationOfCollidingObjectToCauseDamage))
                {
                    TakeHit(collision.gameObject.transform);
                }
            }
            else
                TakeHit(collision.gameObject.transform);
        }
    }

    public void TakeHit(Transform hitTransform)
    {
        //has it been enough time since the last hit?
        if (!isRecoveringFromDamage && (System.DateTime.Now - lastHitTime).TotalMilliseconds > timeBetweenHits)
        {
            if(prefabForHitEffects != null)
            {
                GameObject effect = Instantiate(prefabForHitEffects, hitTransform.position, hitTransform.rotation,gameObject.transform);
            }

            //increase the number of hits on this object
            numHits++;
            isRecoveringFromDamage = true;
            //do we need to run an animation on this object to show a hit?
            if (myAnimator != null && AnimationTriggerToFireWhenHit != string.Empty)
            {
                myAnimator.SetTrigger(AnimationTriggerToFireWhenHit);
            }
            //update the last hit time
            lastHitTime = System.DateTime.Now;
            //do we need to temporarily disable this game objects attack ability?
            foreach (Component c in ComponentsToDisableWhileTakingDamage)
                if (c is MonoBehaviour)
                    ((MonoBehaviour)c).enabled = false;
                else if (c is Collider)
                    ((Collider)c).enabled = false;

            //check if we need to destroy this game object
            CheckIfDead();
        }
    }

    private bool IsDamageCollision(GameObject g)
    {
        string tagToCauseDamageLower = TagOfGameObjectToCauseDamage.ToLower();
        if (g.tag.ToLower() == tagToCauseDamageLower)
        {
            return true;
        }
        else
        {
            Transform current = g.transform.parent;
            while (current != null)
            {
                if (current.gameObject.tag.ToLower() == tagToCauseDamageLower)
                    return true;
                current = current.transform.parent;
            }
            return false;
        }
        
    }

    private void CheckIfDead()
    {
        if(numHits>= NumberOfHitsUntilDestroyMe)
        {
            allowRecoverFromDamage = false;
            //run the animation if there is one to run
            if (AnimationTriggerToFireWhenDead != string.Empty)
            {
                Animator anim = gameObject.GetComponentInChildren<Animator>();
                anim.SetTrigger(AnimationTriggerToFireWhenDead);
            }

            //disable components so while it is animating it can't cause harm.
            foreach(Component c in ComponentsToDisableWhileTakingDamage)
                if (c is MonoBehaviour)
                ((MonoBehaviour)c).enabled = false;
            else if (c is Collider)
                ((Collider)c).enabled = false;


            //destroy the game object
            Destroy(gameObject, TimeToDelayDestoryWhenDead);
        }
    }

}
