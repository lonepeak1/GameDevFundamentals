using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiPlayerCinemachine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //attach the virtual camera to the network connected player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in players)
        {
            NetworkObject no = go.GetComponent<NetworkObject>();

            if (no != null && no.IsLocalPlayer)
            {
                Cinemachine.CinemachineVirtualCamera cvm = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
                if (cvm != null && go != null)
                {
                    cvm.Follow = go.transform;
                    cvm.LookAt = go.transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
