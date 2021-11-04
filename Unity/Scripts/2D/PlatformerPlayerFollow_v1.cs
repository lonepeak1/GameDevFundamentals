/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerFollow_v1 : MonoBehaviour
{
    enum state { idle,jumping,climbing,idlecimbing,sleeping,movingslow,movingfast}

    state animationState = state.idle;
    Transform target;
    // Angular speed in radians per sec.
    public float speed = 0.5f;
    public string TagToFollow = "Player";
    public bool FollowPlayer = false;
    public float DistanceFromPlayerToAttack = 1f;
    //0 idle
    //1 jumping
    //2 climbing
    //3 idleclimbing
    //4 sleeping
    //5 moving slow
    //6 moving fast

    //triggers (turn left,turn right, attack, take hit, die)


    Animator anim;
    Rigidbody2D rb;
    bool hasStateAnimationParameter = false;
    bool hasTurnRightTrigger = false;
    public string turnRightAnimTriggerName = "TurnRight";
    bool hasTurnLeftTrigger = false;
    bool hasAttackAnimationTrigger = false;
    public string turnLeftAnimTriggerName = "TurnLeft";
    public string AttackAnimationTriggerName = "Attack";
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag(TagToFollow);
        if (player != null)
        {
            target = player.transform;
        }
        else
            Debug.Log("Please tag your player with '" + TagToFollow + "' to use the LookAtAnotherGameObject2D script.");

        
        if (rb == null)
            Debug.Log("Please attach a rigidbody2D to your game object to use the LookAtAnotherGameObject2D script.");

        //test if has state animation parameter exists.
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == "State")
                    hasStateAnimationParameter = true;
            }

        }
        finally { }
        if (!hasStateAnimationParameter)
        {
            Debug.LogWarning("Please add an integer 'State' parameter to your '"+gameObject.name+"' animation.");
        }

        //test if turn right animation trigger exists
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == turnRightAnimTriggerName)
                    hasTurnRightTrigger = true;
            }

        }
        finally { }

        if (!hasTurnRightTrigger)
        {
            Debug.LogWarning("Please add a 'TurnRight' trigger parameter to your '" + gameObject.name + "' animation.");
        }

        //test if turn left animation trigger exists
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == turnLeftAnimTriggerName)
                    hasTurnLeftTrigger = true;
            }

        }
        finally { }
        if (!hasTurnLeftTrigger)
        {
            Debug.LogWarning("Please add a 'TurnLeft' trigger parameter to your '" + gameObject.name + "' animation.");
        }

        //test if attack animation trigger exists
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == AttackAnimationTriggerName)
                    hasAttackAnimationTrigger = true;
            }

        }
        finally { }
        if (!hasAttackAnimationTrigger)
        {
            Debug.LogWarning("Please add an 'Attack' animation trigger parameter to your '" + gameObject.name + "' animator.");
        }

    }

    Vector3 direction = Vector3.right;
    // Update is called once per frame
    void Update()
    {
        //code to look at the player
        if (target != null && rb != null)
        {
            //var dir = target.transform.position - transform.position;
            //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //what direction is the player relative to this one?


            //if the center of this object is within so many units of the other player don't move it.

            float diff = Mathf.Abs(gameObject.transform.position.x - target.transform.position.x);


            if (diff > 0.1f && gameObject.transform.position.x < target.transform.position.x)
            {
                //if we are going a different direction, set the animation trigger
                if (direction != Vector3.right)
                {
                    direction = Vector3.right;
                    if (hasTurnRightTrigger)
                        anim.SetTrigger(turnRightAnimTriggerName);

                }


                if (hasStateAnimationParameter)
                    anim.SetInteger("State", (int)state.movingslow);


            }
            else if (diff > 0.1f && gameObject.transform.position.x > target.transform.position.x)
            {
                if (direction != Vector3.left)
                {
                    direction = Vector3.left;
                    if (hasTurnLeftTrigger)
                        anim.SetTrigger(turnLeftAnimTriggerName);
                }
                if (hasStateAnimationParameter)
                    anim.SetInteger("State", (int)state.movingslow);
            }
            else
            {
                if (hasStateAnimationParameter)
                    anim.SetInteger("State", (int)state.idle);

            }

            if (FollowPlayer)
            {
                rb.velocity = direction * speed;
            }


            //are we close enough to attack?
            if (hasAttackAnimationTrigger && target != null && Vector2.Distance(target.position, transform.position) < DistanceFromPlayerToAttack)
            {
                anim.SetTrigger(AttackAnimationTriggerName);
            }
        }

    }
}
