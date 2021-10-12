/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDPlatformPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator anim;
    LayerMask groundMaskLayer;
    bool hasMovingAnimation = false;
    bool hasJumpingAnimation = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        groundMaskLayer = LayerMask.GetMask(GroundMaskLayerName);

        //check to see if there is an "IsMoving" animation.
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if(param.name == MovingAnimationParam)
                    hasMovingAnimation = true;
            }
        }
        catch(System.Exception e)
        {
            Debug.LogWarning("Please add a boolean "+ MovingAnimationParam + " trigger to your player animation.");
        }

        //check to see if there is an "IsJumping" animation.
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == JumpingAnimationParam)
                    hasJumpingAnimation = true;
            }
            
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Please add a boolean " + JumpingAnimationParam + " trigger to your player animation.");
        }

    }

    public GameObject feetPosition;
    public float maxRunSpeed = 2f;//Replace with your max speed
    public float maxJumpSpeed = 5f;
    public float jumpSpeed = 5f;
    public string GroundMaskLayerName = "Ground";
    public string AttackAnimationTrigger = "Attack";
    public string JumpingAnimationParam = "IsJumping";
    public string MovingAnimationParam = "IsMoving";
    public string AttackAxis = "Fire1";


    // Update is called once per frame
    void Update()
    {
        //Attack animation (Don't set the attack trigger if we are already in the attack state.
        if (Input.GetAxisRaw(AttackAxis) > 0 && anim != null && !anim.GetCurrentAnimatorStateInfo(0).IsName(AttackAnimationTrigger))
        {
            anim.SetTrigger(AttackAnimationTrigger);
        }

        //proper rotation of the game object
            if (Input.GetAxis("Horizontal") < 0 && gameObject.transform.rotation.y != 0)
            gameObject.transform.rotation = Quaternion.identity;

        if (Input.GetAxis("Horizontal") > 0 && gameObject.transform.rotation.y == 0)
            gameObject.transform.Rotate(0, 180, 0);

        //run the move animation or idle animation if necessary.
        if (hasMovingAnimation && Input.GetAxisRaw("Horizontal") == 0 && anim.GetBool(MovingAnimationParam))
            anim.SetBool(MovingAnimationParam, false);
        else if (hasMovingAnimation && Input.GetAxisRaw("Horizontal") != 0 && !anim.GetBool(MovingAnimationParam))
            anim.SetBool(MovingAnimationParam, true);

        if(hasJumpingAnimation && Input.GetAxisRaw("Jump") == 0 && anim.GetBool(JumpingAnimationParam))
            anim.SetBool(JumpingAnimationParam, false);
        else if (hasJumpingAnimation && Input.GetAxisRaw("Jump") != 0 && !anim.GetBool(JumpingAnimationParam))
            anim.SetBool(JumpingAnimationParam, true);


        //move forward if we are not touching the ground.
        if (rb.IsTouchingLayers(groundMaskLayer.value))
            rb.AddForce(Vector2.right * Input.GetAxis("Horizontal"), ForceMode2D.Impulse);

        if (rb.IsTouchingLayers(groundMaskLayer.value) && IsJumping() && rb.velocity.y < maxJumpSpeed)
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

        //max speed
        //if (rb.velocity.magnitude > maxRunSpeed)
        //    rb.velocity = rb.velocity.normalized * maxRunSpeed;

        if (Mathf.Abs(rb.velocity.x) > maxRunSpeed)
        {
            // and finally asign the new vel
            rb.velocity = new Vector2(maxRunSpeed * Mathf.Sign(rb.velocity.x), rb.velocity.y);
        }

        if (Mathf.Abs(rb.velocity.y) > maxJumpSpeed)
        {
            // and finally asign the new vel
            rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed * Mathf.Sign(rb.velocity.y));
        }

        //turn off any ground colliders which are not below the FootLocation of the player.
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        GameObject myFeetPosition = gameObject;
        if (feetPosition != null)
            myFeetPosition = feetPosition;
        foreach (Collider2D coll in colliders)
        {
            if (coll.gameObject != this.gameObject && 1 << coll.gameObject.layer == groundMaskLayer.value)
            {
                //Debug.Log(coll.gameObject.name + ":" + (coll.bounds.extents.y + coll.bounds.center.y).ToString());
                if ((-coll.bounds.extents.y + coll.bounds.center.y) > myFeetPosition.transform.position.y && !coll.isTrigger && !rb.IsTouching(coll))
                {
                    coll.isTrigger = true;
                }
                else if ((coll.bounds.extents.y + coll.bounds.center.y) <= myFeetPosition.transform.position.y)
                {
                    coll.isTrigger = false;
                }
            }
        }

        //do this at the end of update so as not to break code above it
        if (canJump && Input.GetAxisRaw("Jump") > 0)
            canJump = false;
        else if (!canJump && Input.GetAxisRaw("Jump") == 0)
            canJump = true;
    }

    /// <summary>
    /// This method prevents the player from holding down the jump key and "skipping" up ground.  
    /// Once the jump key is pressed they must let go of it before pressing it again.
    /// </summary>
    bool canJump = true;
    private bool IsJumping()
    {
        return canJump && Input.GetAxisRaw("Jump") > 0;
    }

}


