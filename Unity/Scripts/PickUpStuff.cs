using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUpStuff : MonoBehaviour
{
    /// <summary>
    /// This class allows the player to pick up objects which are within "distanceToAllowPickup" units of the object being clicked by the mouse.
    /// </summary>

    public float speed = 10;
    public float distanceToAllowPickup = 2;
    List<GameObject> holdingStuff = new List<GameObject>();
    Transform guide;//this is the position where the object is placed when it is picked up.

    private void Start()
    {
       if (guide==null)
        {
            guide = transform;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pickup();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            throw_drop();
        }
    }//update

    private void Pickup()
    {
        //what are we pointing at
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if(hitObject.tag == "takeable" && System.Math.Abs(Vector3.Distance(hitObject.transform.position,transform.position))<=distanceToAllowPickup)
            {
                hitObject.GetComponent<Rigidbody>().isKinematic = true;
                //turn off any colliders
                foreach(Collider c in hitObject.GetComponents<Collider>())
                {
                    c.enabled = false;
                }
                foreach (Collider c in hitObject.GetComponentsInChildren<Collider>())
                {
                    c.enabled = false;
                }

                //turn off any renderers
                foreach (Renderer c in hitObject.GetComponents<Renderer>())
                {
                    c.enabled = false;
                }
                foreach (Renderer c in hitObject.GetComponentsInChildren<Renderer>())
                {
                    c.enabled = false;
                }

                //We set the object parent to our guide empty object.
                hitObject.transform.SetParent(guide);

                //Set gravity to false while holding it
                hitObject.GetComponent<Rigidbody>().useGravity = false;

                //we apply the same rotation our main object (Camera) has.
                hitObject.transform.localRotation = transform.rotation;
                //We re-position the ball on our guide object 
                hitObject.transform.position = guide.position;
                holdingStuff.Add(hitObject);
            }
        }

        
    }

    private void throw_drop()
    {
        //drop the first item in our bag
        if(holdingStuff.Count>0)
        {
            GameObject objectToThrow = holdingStuff[0];
            holdingStuff.RemoveAt(0);
            //Set our Gravity to true again.
            objectToThrow.GetComponent<Rigidbody>().useGravity = true;

            //Apply velocity on throwing
            objectToThrow.GetComponent<Rigidbody>().velocity = transform.forward * speed;
            objectToThrow.GetComponent<Rigidbody>().isKinematic = false;
            //Unparent our object
            objectToThrow.transform.parent = null;

            objectToThrow.transform.position = transform.position;
            //turn on any colliders
            foreach (Collider c in objectToThrow.GetComponents<Collider>())
            {
                c.enabled = true;
            }
            foreach (Collider c in objectToThrow.GetComponentsInChildren<Collider>())
            {
                c.enabled = true;
            }

            //turn on any renderers
            foreach (Renderer c in objectToThrow.GetComponents<Renderer>())
            {
                c.enabled = true;
            }
            foreach (Renderer c in objectToThrow.GetComponentsInChildren<Renderer>())
            {
                c.enabled = true;
            }

        }
        
    }
}//class