using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowards : MonoBehaviour
{
	Transform target;
	public Transform partToRotate;
	public float turnSpeed = 10f;

    public string[] TagsOfObjectsToRotateTowards;
    public float DistanceToStartRotatingTowardsObject = 999f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled)
        {
            //are we within range of an object to shoot at?
            if (TagsOfObjectsToRotateTowards.Length > 0)
            {
                bool foundOne = false;
                //are we within range of one of the specified objects to attack?
                float closest_distance = int.MaxValue;
                foreach (string tag in TagsOfObjectsToRotateTowards)
                {
                    GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                    foreach (GameObject o in objects)
                    {
                        float dist = Vector3.Distance(gameObject.transform.position, o.transform.position);
                        if (dist <= DistanceToStartRotatingTowardsObject && dist < closest_distance)
                        {
                            closest_distance = dist;
                            foundOne = true;
                            target = o.transform;
                        }
                    }
                }
                if (!foundOne)
                    return;
            }
            else
            {
                return;

            }



            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z);
        }
	}
}
