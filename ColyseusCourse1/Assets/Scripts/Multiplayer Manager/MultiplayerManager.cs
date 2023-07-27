using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    // Start is called before the first frame update
    void Start()
    {
        Instance.InitializeClient();
        Instance.client.JoinOrCreate<State>("state_handler");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
