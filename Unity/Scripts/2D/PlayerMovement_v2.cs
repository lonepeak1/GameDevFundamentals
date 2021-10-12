using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement_v2 : MonoBehaviour
{
	public float DriveSpeed = 1;
	Rigidbody2D rigidBody;

	
	// Start is called before the first frame update
	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

			
			rigidBody.AddTorque(-Input.GetAxis("Horizontal"));
			rigidBody.AddForce(transform.up * Input.GetAxis("Vertical") * DriveSpeed, ForceMode2D.Force);


	}
}
