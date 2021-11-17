using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        timeOfContact = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool isPlayerCrouch ;
    public static int timeOfContact = 0;
}
