using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnTriggerCollision : MonoBehaviour
{
    public string TagOfObjectBeingHeld = "";
    public string AnimationTriggerOnHitObject = "";
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!isActiveAndEnabled)
            return;

        if (anim != null)
        {
            foreach (Transform child in collision.gameObject.transform)
            {
                if (child.gameObject.tag == TagOfObjectBeingHeld)
                {
                    anim.SetTrigger(AnimationTriggerOnHitObject);
                    Destroy(child.gameObject);
                }
            }
        }
        else
        {
            //get the anmiator on the thing we hit and call it.
            anim = collision.gameObject.GetComponent<Animator>();
            anim.SetTrigger(AnimationTriggerOnHitObject);
        }
    }

  
}
