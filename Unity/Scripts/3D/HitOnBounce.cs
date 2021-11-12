/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnBounce : MonoBehaviour
{
    public string TagOfObjectToGiveHit = "Player";
    TakeHitController hitController;
    // Start is called before the first frame update
    void Start()
    {
        hitController = GetComponentInParent<TakeHitController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
         if(hitController!=null && collision.gameObject.name.ToLower() == TagOfObjectToGiveHit.ToLower())
            hitController.TakeHit(collision.gameObject.transform);
    }

}
