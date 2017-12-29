using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for anything that can trigger overworld dialogue.
[RequireComponent(typeof(BoxCollider2D))]
public class DialogueTrigger : MonoBehaviour {

    [Tooltip("If autotrigger is checked, the dialogue will play immediately upon entering the area, otherwise the player must press 'Z' to initiate the dialogue")]
    public bool autoTrigger = false;

    [Tooltip("Do we want this to go off every time we walk into the area? If not, then we destroy this trigger after it's been used")]
    public bool repeatable;

    public List<string> myText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && autoTrigger)
        {
            DialogueManager.instance.SetText(myText);
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //The canMove check is mainly so that we don't keep opening up new textboxes with each Z press.
        if (collision.gameObject.tag == "Player" && !autoTrigger && PlayerMovement.instance.canMove )
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                DialogueManager.instance.SetText(myText);
            }
        }
    }
}
