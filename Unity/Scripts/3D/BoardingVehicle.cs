using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Vehicles.Car;
public class BoardVehicle : MonoBehaviour
{
    public GameObject SeatPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.position, SeatPosition.transform.position);
        if (Input.GetKeyDown(KeyCode.E) && distance <= 30f)
        {
            playerIsOnSpeeder = !playerIsOnSpeeder;

            if (playerIsOnSpeeder == true)
            {
                //put the player on the barc speeder in the right position
                if (SeatPosition != null)
                    gameObject.transform.position = SeatPosition.transform.position;

                gameObject.transform.parent = SeatPosition.transform.parent;

                //turn off the first person rigidbody script
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                RigidbodyFirstPersonController rbScript = GetComponent<RigidbodyFirstPersonController>();
                rbScript.working = false;
                //Destroy(rbScript);
                //enable the car controller script on the barc speeder.


                //turn off the first person rigidbody script
                CarUserControl carScript = SeatPosition.transform.parent.gameObject.GetComponent<CarUserControl>();
                carScript.working = true;

            }
            else
            {
                gameObject.transform.parent = null;
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.isKinematic = false;
                //turn off the first person rigidbody script
                RigidbodyFirstPersonController rbScript = GetComponent<RigidbodyFirstPersonController>();
                rbScript.working = true;

                //turn off the barc speeder controller
                CarUserControl carScript = SeatPosition.transform.parent.gameObject.GetComponent<CarUserControl>();
                carScript.working = false;
            }

        }
    }
    bool playerIsOnSpeeder = false;

    private void OnTriggerStay(Collider other)
    {

    }
}