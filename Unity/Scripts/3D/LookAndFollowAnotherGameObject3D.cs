using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndFollowAnotherGameObject3D : MonoBehaviour
{
    Transform target;
    // Angular speed in radians per sec.
    public float speed = 0.5f;
    public string TagToFollow = "Player";
    public bool FollowPlayer = false;
    public string MovingAnimationParam="";
    bool hasMovingAnimation = false;
    Animator anim;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag(TagToFollow);
        if (player != null)
            target = player.transform;
        else
            Debug.Log("Please tag your player with '" + TagToFollow + "' to use the LookAtAnotherGameObject script.");

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.Log("Please attach a rigidbody to your game object to use the LookAtAnotherGameObject script.");

        if (anim != null)
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == MovingAnimationParam)
                    hasMovingAnimation = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //code to look at the player
        if (target != null && rb != null)
        {
            //var dir = target.transform.position - transform.position;
            //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Radeg - 90f;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //what direction is the player relative to this one?
            Vector3 direction = Vector3.zero;

            //if the center of this object is within so many units of the other player don't move it.

            float diff = Vector3.Distance(gameObject.transform.position, target.position);
            if (diff > 10 )
            {
                if (FollowPlayer)
                {
                    Vector3 dir = target.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * speed).eulerAngles;
                    transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
                    rb.velocity = transform.forward * speed;
                }

                if (hasMovingAnimation)
                    anim.SetBool(MovingAnimationParam, true);
            }
            else
            {
                rb.velocity = Vector3.zero;
                if (hasMovingAnimation)
                    anim.SetBool(MovingAnimationParam, false);
            }
            
        }

    }
}
