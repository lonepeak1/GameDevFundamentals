/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateComponentWhenHit : MonoBehaviour
{
    public Component ComponentToActivateOnCollision;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string TagOfObjectToCauseActivation = "Player";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled)
        {
            if (collision.gameObject.tag.ToLower() == TagOfObjectToCauseActivation.ToLower())
            {
                if (ComponentToActivateOnCollision is Collider2D)
                {
                    ((Collider2D)ComponentToActivateOnCollision).enabled = true;
                }
                else if (ComponentToActivateOnCollision is Animator)
                {
                    ((Animator)ComponentToActivateOnCollision).enabled = true;
                }
            }
        }
    }

}
