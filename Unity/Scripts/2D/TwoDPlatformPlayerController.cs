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
    LayerMask ladderMaskLayer;
    LayerMask wallMaskLayer;
    bool hasMovingAnimation = false;
    bool hasJumpingAnimation = false;
    bool hasClimbingAnimation = false;

    public GameObject feetPosition;
    public float maxRunSpeed = 4f;//Replace with your max speed
    public float maxJumpSpeed = 10f;
    public float climbSpeed = 1f;
    public float jumpSpeed = 10f;
    public string GroundMaskLayerName = "Ground";
    public string LadderMaskLayerName = "Ladder";
    public string wallMaskLayerName = "Walls";
    public string AttackAnimationTrigger = "Attack";
    public string JumpingAnimationParam = "IsJumping";
    public string MovingAnimationParam = "IsMoving";
    public string ClimbingAnimationParam = "IsClimbing";
    public string ClimbingIdleAnimationParam = "IsClimbingIdle";
    public string AttackAxis = "Fire1";
    public float airMoveFactor = 0.2f;//this controls how much the player moves while in the air.
    GameObject ladderPlatform = null;
    bool isAttacking = true;
    bool isOnLadder = false;

    bool isClimbing = false;
    Collider2D[] playerColliders;
    // Start is called before the first frame update
    void Start()
    {
        playerColliders = GetComponentsInChildren<Collider2D>();

        rb = GetComponent<Rigidbody2D>();
        if(rb ==  null)
        {
            Debug.LogError("Please add a rigidbody to your player to use this platformer script.");
            return;
        }
        anim = GetComponentInChildren<Animator>();
        if (rb == null)
        {
            Debug.LogError("Please add an animation to your player to use this platformer script.");
            return;
        }
        groundMaskLayer = LayerMask.GetMask(GroundMaskLayerName);
        ladderMaskLayer = LayerMask.GetMask(LadderMaskLayerName);
        wallMaskLayer = LayerMask.GetMask(wallMaskLayerName);
        //check to see if there is an "IsMoving" animation.
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == MovingAnimationParam)
                    hasMovingAnimation = true;
            }
        }
        finally { }
        if (!hasMovingAnimation)
        {
            Debug.LogWarning("Please add a boolean " + MovingAnimationParam + " parameter to your player animation.");
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
        finally { }
        if (!hasJumpingAnimation)
        {
            Debug.LogWarning("Please add a boolean " + JumpingAnimationParam + " parameter to your player animation.");
        }

        //check to see if there is an "IsClimbing" animation.
        try
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == ClimbingAnimationParam)
                    hasClimbingAnimation = true;
            }

        }
        finally { }

        if (!hasClimbingAnimation)
            Debug.LogWarning("If you would like your player to climb, please add a " + ClimbingAnimationParam + " boolean parameter to your player animation.");


    }

    
    // Update is called once per frame
    void Update()
    {
        //check to see if the user is touching a ladder collider
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();


        //if the user is pushing up or down and they are touching a ladder, then disable the gravity and move the user
        if (Input.GetAxisRaw("Vertical") != 0 || isOnLadder)
        {
            //The player is allowed to jump if they are touching ground layers, they are giving a jump command, and their up velocity is less than the max jump speed.
            //find the ladder we are touching and put a platform on it.
            Collider2D climableLadder = isNearClimbableLadder(colliders);
            if (ladderPlatform == null)
            {

                if (climableLadder != null)
                {
                    isOnLadder = true;
                }
                else
                    isOnLadder = false;

                if (isOnLadder)
                {
                    ladderPlatform = new GameObject("LadderCollider");
                    ladderPlatform.transform.position = new Vector2(feetPosition.transform.position.x, feetPosition.transform.position.y);
                    BoxCollider2D playerLadderCollider = (BoxCollider2D)ladderPlatform.AddComponent(typeof(BoxCollider2D));
                    //apply a friction physics material so the player cannot fly off if they are on a swinging ladder.
                    PhysicsMaterial2D friction = new PhysicsMaterial2D();
                    friction.friction = 1;
                    friction.bounciness = 0;
                    playerLadderCollider.sharedMaterial = friction;
                    //change this later to be the max width of the player's collider
                    playerLadderCollider.size = new Vector2(climableLadder.bounds.extents.x * 2, 0.05f);
                    ladderPlatform.transform.parent = climableLadder.transform;

                    gameObject.transform.parent = climableLadder.transform;
                }
            }
            else
            {
                //are we still touching the ladder?
                if (climableLadder != null)
                {
                    isOnLadder = true;
                    //turn the ladder platform so it is facing left/right at all times.
                    Vector3 eulers = ladderPlatform.transform.eulerAngles;
                    eulers.z = 0;
                    ladderPlatform.transform.eulerAngles = eulers;
                    transform.eulerAngles = eulers;
                }
                else
                {
                    isOnLadder = false;
                    gameObject.transform.parent = null;
                }
            }

            if (ladderPlatform != null && isOnLadder)
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
            if (gameObject.transform.parent != null)
                gameObject.transform.parent = null;
        }

        //if they are not on the ladder and the ladder platform exists, remove it immediately.
        if (!isOnLadder && ladderPlatform != null)
        {
            Destroy(ladderPlatform);
            isOnLadder = false;
            isClimbing = false;
            ladderPlatform = null;
        }

        if (isClimbing && Input.GetAxisRaw("Vertical") != 0)
        {
            ladderPlatform.transform.position = new Vector2(feetPosition.transform.position.x, ladderPlatform.transform.position.y + climbSpeed * Input.GetAxisRaw("Vertical") * 0.005f);
        }
        //Attack animation (Don't set the attack trigger if we are already in the attack state.
        if (!isAttacking && Input.GetAxisRaw(AttackAxis) > 0 && anim != null && !anim.GetCurrentAnimatorStateInfo(0).IsName(AttackAnimationTrigger))
        {
            isAttacking = true;
            anim.SetTrigger(AttackAnimationTrigger);
        }
        else if (Input.GetAxisRaw(AttackAxis) == 0)
        {
            isAttacking = false;
        }

        bool grounded = isTouchingGround(colliders);
        bool allowAjump = (grounded || isOnLadder) && IsJumping() && rb.velocity.y < maxJumpSpeed;

        //proper rotation of the game object
        if (Input.GetAxis("Horizontal") < 0 && gameObject.transform.rotation.y != 0)
            gameObject.transform.rotation = Quaternion.identity;

        if (Input.GetAxis("Horizontal") > 0 && gameObject.transform.rotation.y == 0)
            gameObject.transform.Rotate(0, 180, 0);

        //run the move animation or idle animation if necessary.
        if ((hasMovingAnimation && Input.GetAxisRaw("Horizontal") == 0 && anim.GetBool(MovingAnimationParam)))
            anim.SetBool(MovingAnimationParam, false);
        else if (hasMovingAnimation && Input.GetAxisRaw("Horizontal") != 0 && !anim.GetBool(MovingAnimationParam))
            anim.SetBool(MovingAnimationParam, true);

        //run the jumping animation if necessary
        if (hasJumpingAnimation && (Input.GetAxisRaw("Jump") == 0 && anim.GetBool(JumpingAnimationParam) || grounded || isOnLadder))
            anim.SetBool(JumpingAnimationParam, false);
        else if (hasJumpingAnimation && (Input.GetAxisRaw("Jump") != 0 && !anim.GetBool(JumpingAnimationParam) || (!grounded && !isOnLadder)))
            anim.SetBool(JumpingAnimationParam, true);

        //run the climbing idle animation if necessary
        if (hasClimbingAnimation && ((Input.GetAxisRaw("Vertical") != 0 && anim.GetBool(ClimbingIdleAnimationParam)) || !isOnLadder))
            anim.SetBool(ClimbingIdleAnimationParam, false);
        else if (hasClimbingAnimation && isOnLadder && Input.GetAxisRaw("Vertical") == 0 && !anim.GetBool(ClimbingIdleAnimationParam))
            anim.SetBool(ClimbingIdleAnimationParam, true);

        //run the climbing animation if necessary
        if (hasClimbingAnimation && ((Input.GetAxisRaw("Vertical") == 0 && anim.GetBool(ClimbingAnimationParam)) || !isOnLadder))
            anim.SetBool(ClimbingAnimationParam, false);
        else if (hasClimbingAnimation && isOnLadder && Input.GetAxisRaw("Vertical") != 0 && !anim.GetBool(ClimbingAnimationParam))
            anim.SetBool(ClimbingAnimationParam, true);

        //move forward if we are not touching the ground.
        if (rb.IsTouchingLayers(groundMaskLayer.value))
            rb.AddForce(Vector2.right * Input.GetAxis("Horizontal"), ForceMode2D.Impulse);
        else if (!rb.IsTouchingLayers(wallMaskLayer.value))
        {
            //allow the player to turn but only add a smaller force amount.
            rb.AddForce(Vector2.right * Input.GetAxis("Horizontal") * airMoveFactor, ForceMode2D.Impulse);
        }

        //jump code (only allow jump if we are touching the ground and the collider is active.
        if (allowAjump)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }

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
        GameObject myFeetPosition = gameObject;
        if (feetPosition != null)
            myFeetPosition = feetPosition;
        foreach (Collider2D coll in colliders)
        {
            //check toi see if the collider is in the ground layer and if we are touching it.
            if (coll.gameObject != this.gameObject && 1 << coll.gameObject.layer == groundMaskLayer.value)
            {
                //Debug.Log(coll.gameObject.name + ":" + (coll.bounds.extents.y + coll.bounds.center.y).ToString());
                //are we above it, and are we over it
                bool isOver = myFeetPosition.transform.position.x < (coll.bounds.extents.x + coll.bounds.center.x) && myFeetPosition.transform.position.x > (-coll.bounds.extents.x + coll.bounds.center.x);
                bool isAbove = (-coll.bounds.extents.y + coll.bounds.center.y) > myFeetPosition.transform.position.y;
                if (isAbove && !coll.isTrigger && !rb.IsTouching(coll))
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

    /// <summary>
    /// Checks if the player is touching the ground
    /// </summary>
    /// <returns></returns>
    private bool isTouchingGround(Collider2D[] colliders)
    {
        GameObject myFeetPosition = gameObject;
        if (feetPosition != null)
            myFeetPosition = feetPosition;
        foreach (Collider2D coll in colliders)
        {
            //check toi see if the collider is in the ground layer and if we are touching it.
            if (coll.gameObject != this.gameObject && 1 << coll.gameObject.layer == groundMaskLayer.value)
            {
                foreach (Collider2D playerCollider in playerColliders)
                    if (coll.IsTouching(playerCollider) && coll.isTrigger == false)
                    {
                        return true;
                    }
            }
        }
        return false;
    }

    Collider2D isNearClimbableLadder(Collider2D[] colliders)
    {
        if (rb.IsTouchingLayers(ladderMaskLayer.value))
        {
            //we don't just want to be touching a ladder, we need to be within the bounds of its left and right collider
            foreach (Collider2D coll in colliders)
            {
                if (coll.gameObject != this.gameObject && 1 << coll.gameObject.layer == ladderMaskLayer.value && gameObject.transform.position.x > (-coll.bounds.extents.x + coll.bounds.center.x) && gameObject.transform.position.x < (coll.bounds.extents.x + coll.bounds.center.x))
                {
                    //we are climbing
                    //the ladder feature has a box collider under the player so they can move left and right
                    isOnLadder = true;
                    return coll;
                }
            }
        }
        return null;

    }

}


