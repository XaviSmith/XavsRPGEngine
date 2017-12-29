using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour {

    public Transform player;
    public Vector2 offset;  //offset for the camera in case you don't want the player in the center of the screen
    public bool camLock = false;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(UpdateCameraPos());
    }

    IEnumerator UpdateCameraPos()
    {
        while(true)
        {
            yield return null;

            if (!camLock)
            {
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            }
        }      
    }

}
