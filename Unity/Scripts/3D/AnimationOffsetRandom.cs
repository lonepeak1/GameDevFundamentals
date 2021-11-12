/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffsetRandom : MonoBehaviour
{

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(anim!=null)
        {
            anim.SetFloat("CycleOffset", Random.Range(0, 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
