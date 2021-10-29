using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To use this script you need the following pre-requisites
/// 1.) Create a sprite with the wrap mode set to "repeat"
/// 2.) Add a 3D quad to your 2D game.
/// 3.) Attach this script to the quad.
/// 4.) Set the background speed variable.
/// 5.) Define which game object to follow. (Ideally the player.)
/// </summary>
public class MoveBackground : MonoBehaviour
{
    
    public float moveSpeed=0.5f;
    public GameObject target_gameobject;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (target_gameobject==null)
        {
            target_gameobject = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target_gameobject != null)
        {
            Vector2 offset = new Vector2(target_gameobject.transform.position.x * moveSpeed, target_gameobject.transform.position.y * moveSpeed);
            renderer.material.SetTextureOffset("_MainTex", offset);
        }
        

    }
}
