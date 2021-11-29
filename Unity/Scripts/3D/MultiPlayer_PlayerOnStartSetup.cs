using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiPlayer_PlayerOnStartSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //attach the virtual camera to the network connected player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerInThisConnection = null;
        MultiPlayerMenu menu = GameObject.FindObjectOfType<MultiPlayerMenu>();
        if(menu!=null)
            this.name=menu.PlayerName;
        foreach(GameObject go in players)
        {
            NetworkObject no = go.GetComponent<NetworkObject>();

            if (no != null && no.IsLocalPlayer)
            {
                playerInThisConnection = no.gameObject;

                break;
            }
        }

        if (playerInThisConnection != null)
        {
            //update the cinemachine.
            Cinemachine.CinemachineVirtualCamera cvm = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
            if (cvm != null)
            {
                cvm.Follow = playerInThisConnection.transform;
                cvm.LookAt = playerInThisConnection.transform;
            }

            Radar[] radars = GameObject.FindObjectsOfType<Radar>();
            Radar radarInThisConnection = null;
            //find the right radar for this instance of the game.
            foreach (Radar radar in radars)
            {
                NetworkObject no = radar.GetComponent<NetworkObject>();

                if (no != null && no.IsLocalPlayer)
                {
                    radarInThisConnection = radar;
                    break;
                }
            }

            //find the map beacons and make sure they are rotating around the current player.
            if (radarInThisConnection != null)
            {
                MapBeacon[] beacons = GameObject.FindObjectsOfType<MapBeacon>();
                foreach (MapBeacon beacon in beacons)
                {
                    beacon.RadarCenterObject = gameObject;
                }
            }

            //point the minimap at the player
            Minimap minimap = GameObject.FindObjectOfType<Minimap>();
            if (minimap != null)
            {
                minimap.playerObject = playerInThisConnection;
                minimap.Radar = radarInThisConnection;
                if(menu!=null)
                    minimap.PlayerName = menu.PlayerName;
            }

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
