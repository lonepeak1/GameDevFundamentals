using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltTowards : MonoBehaviour
{
	Transform target;
	public Transform partToRotate;
	public float turnSpeed = 10f;

    public string[] TagsOfObjectsToTiltTowards;
    public float DistanceToStartTitlingTowardsObject = 999f;

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
            if (TagsOfObjectsToTiltTowards.Length > 0)
            {
                bool foundOne = false;
                //are we within range of one of the specified objects to attack?
                foreach (string tag in TagsOfObjectsToTiltTowards)
                {
                    GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                    foreach (GameObject o in objects)
                    {
                        if (Vector3.Distance(gameObject.transform.position, o.transform.position) <= DistanceToStartTitlingTowardsObject)
                        {
                            foundOne = true;
                            target = o.transform;
                            continue;
                        }
                    }
                }
                if (!foundOne)
                    return;
            }
            else
                return;

            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);
        }
	}
}
