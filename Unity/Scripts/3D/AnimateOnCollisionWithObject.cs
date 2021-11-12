using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnCollisionWithObject : MonoBehaviour
{
    public string TagOfObjectBeingHit = "";
    public string AnimationTriggerToFireWhenHit = "";
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


    private void OnCollisionEnter(Collision collision)
    {
        if (!isActiveAndEnabled)
            return;

        if (TagOfObjectBeingHit == string.Empty || collision.gameObject.tag.ToLower() == TagOfObjectBeingHit.ToLower())
        {
            if (anim == null)
            {
                anim = collision.gameObject.GetComponent<Animator>();
                if(anim!=null)
                    anim.SetTrigger(AnimationTriggerToFireWhenHit);
            }
        }
        
    }
}
