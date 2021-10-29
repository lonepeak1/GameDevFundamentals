using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTileMapScroller : MonoBehaviour
{
    public float HorizontalMoveSpeed = 0.5f;
    public float VerticalMoveSpeed = 0.5f;
    public GameObject target_gameobject;
    UnityEngine.Tilemaps.Tilemap map;
    public float offsetX = 0;
    public float offsetY = 0;

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<UnityEngine.Tilemaps.Tilemap>();
        if (target_gameobject == null)
        {
            target_gameobject = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (target_gameobject != null)
        {
            transform.position = new Vector3(offsetX + target_gameobject.transform.position.x * -HorizontalMoveSpeed, offsetY + target_gameobject.transform.position.y * -VerticalMoveSpeed);
            
        }


    }
}
