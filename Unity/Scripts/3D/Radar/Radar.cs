using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
	public static float radarDistance = 600;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		

	}

	/// <summary>
	/// Returns a list of x,y coordinates where there are collidable objects.
	/// </summary>
	/// <returns></returns>
	public List<Vector2> ScanTerrain()
	{
		List<Vector2> coords = new List<Vector2>();
		int layerMask = 1;
		
		//we are going to scan 360 degrees
		for (int i = 0; i < 360; i+=3)
		{
			var vector = new Vector3(transform.position.x,0,transform.position.y);// transform.TransformDirection(transform.position);
			vector = Quaternion.AngleAxis(i, Vector3.up) * vector;
			RaycastHit hit;
			// Does the ray intersect any objects excluding the player layer
			if (Physics.Raycast(transform.position, vector, out hit, radarDistance, layerMask))
			{
				Debug.DrawRay(transform.position, vector * hit.distance, Color.red);
				//Debug.Log("Did Hit at " + hit.distance.ToString() + " units");
				coords.Add(new Vector2(hit.point.x, hit.point.z));
			}
			else
			{
				coords.Add(Vector2.zero);
				//Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);
				//Debug.Log("Did not Hit");
			}
		}
		return coords;
	}

}
