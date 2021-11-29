using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBeacon : MonoBehaviour
{
	internal float mapX=0;
	internal float mapY=0;
	public GameObject RadarCenterObject;
	public string MapText = "";
	

    // Start is called before the first frame update
    void Start()
    {
        Radar[] radars = GameObject.FindObjectsOfType<Radar>();
        if(radars.Length>0)
            RadarCenterObject = radars[0].gameObject;
        if (gameObject.tag == "Player")
            MapText = gameObject.name;

	}

    // Update is called once per frame
    void Update()
    {
        if (RadarCenterObject != null)
        {
            //calculate distance from the player.
            mapX = gameObject.transform.position.x - RadarCenterObject.transform.position.x;
            mapY = gameObject.transform.position.z - RadarCenterObject.transform.position.z;
            //calculate the direction from the player
        }
	}
}
