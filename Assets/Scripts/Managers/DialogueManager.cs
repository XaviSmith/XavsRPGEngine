using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : TextManager {

    public GameObject textBox;

    void Start()
    {
        textBox = transform.GetChild(0).gameObject;
        myTextObj = textBox.GetComponentInChildren<Text>();
    }

    //for clearing text out and adding line
    public override void SetText(string text)
    {
        PlayerMovement.instance.canMove = false;
        AudioManager.instance.PlaySound(AudioManager.instance.dialogueSound);
        textBox.SetActive(true);
        base.SetText(text);
    }

    //for clearing text out and adding many lines
    public override void SetText(List<string> someText)
    {
        PlayerMovement.instance.canMove = false;
        AudioManager.instance.PlaySound(AudioManager.instance.dialogueSound);
        textBox.SetActive(true);
        base.SetText(someText);
    }

    public override IEnumerator GetInput()
    {
        //Due to how unity works, we can't just use base.GetInput() here so I had to copy over the function
        bool buttonPressed = false;
        while (!textAdvanced && myText.Count > 0)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Z) && !buttonPressed)
            {
                myText.RemoveAt(0);
                buttonPressed = true;
                if (myText.Count > 0)
                    myTextObj.text = myText[0];

                AudioManager.instance.PlaySound(AudioManager.instance.menuSelectSound);
                yield return new WaitForSeconds(0.1f);


                if (myText.Count <= 0)
                {
                    textAdvanced = true;
                }
                buttonPressed = false;
            }
        }
        //Delay so everything else has time to process the change
        yield return new WaitForSeconds(0.1f);
        //FightManager.instance.textAdvanced = true;
        textAdvanced = false;

        textBox.SetActive(false);
        PlayerMovement.instance.canMove = true;
    }
}
