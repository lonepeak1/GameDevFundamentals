using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadCastOnCollision : MonoBehaviour
{
    public string MessageToBroadcast = "";
    public string TagOfObjectBeingHit = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActiveAndEnabled)
        {
            if(TagOfObjectBeingHit == string.Empty || collision.gameObject.tag.ToLower() == TagOfObjectBeingHit.ToLower())
                collision.gameObject.BroadcastMessage(MessageToBroadcast);
        }
    }

}
