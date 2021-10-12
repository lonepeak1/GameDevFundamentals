using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndFollowAnotherGameObject2D_v2 : MonoBehaviour
{
    Transform target;
    // Angular speed in radians per sec.
    public float speed = 0.5f;
    public string TagToFollow = "Player";
    public bool FollowPlayer = false;
    Animator anim;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag(TagToFollow);
        if (player != null)
            target = player.transform;
        else
            Debug.Log("Please tag your player with '" + TagToFollow + "' to use the LookAtAnotherGameObject2D script.");

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.Log("Please attach a rigidbody2D to your game object to use the LookAtAnotherGameObject2D script.");
    }

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
            Vector3 direction = Vector3.zero;

            //if the center of this object is within so many units of the other player don't move it.

            float diff = Mathf.Abs(gameObject.transform.position.x - target.transform.position.x);
            Debug.LogWarning(diff);

            if (diff > 0.1f && gameObject.transform.position.x < target.transform.position.x)
            {
                direction = Vector3.right;
                try
                {
                    anim.SetBool("Moving", true);
                }
                catch (System.Exception exc)
                {
                    Debug.LogError("Create a 'Moving' parameter for the animation associated with " + gameObject.name);
                }
            }
            else if (diff > 0.1f && gameObject.transform.position.x > target.transform.position.x)
            {
                direction = Vector3.left;
                try
                {
                    anim.SetBool("Moving", true);
                }
                catch (System.Exception exc)
                {
                    Debug.LogError("Create a 'Moving' parameter for the animation associated with " + gameObject.name);
                }
            }
            else
            {
                try
                {
                    anim.SetBool("Moving", false);

                }
                catch (System.Exception exc)
                {
                    Debug.LogError("Create a 'Moving' parameter for the animation associated with " + gameObject.name);
                }
            }
            if (FollowPlayer)
                rb.velocity = direction * speed;
        }

    }
}
