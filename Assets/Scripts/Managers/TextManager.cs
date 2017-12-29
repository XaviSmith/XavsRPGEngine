using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : Singleton<TextManager> {

    public Text myTextObj;
    public List<string> myText;

    public bool textAdvanced = false;

    //for clearing text out and adding line
    public virtual void SetText(string text)
    {
        textAdvanced = false;
        StopAllCoroutines();
        myText.Clear();
        myText.Add(text);
        myTextObj.text = text;
        StartCoroutine(GetInput());
    }

    //for clearing text out and adding many lines
    public virtual void SetText(List<string> someText)
    {
        myText = new List<string>(someText);
        myTextObj.text = myText[0];
        StartCoroutine(GetInput());
    }

    //Adding to existing text
    public virtual void UpdateText(string text)
    {
        myText.Add(text);
    }

    public virtual void UpdateText(List<string> someText)
    {
        myText.AddRange(someText);
    }

    //Wrapper for our function to wait on the player to press "Z" to advance text
    public void GetInputWrapper()
    {
        StartCoroutine(GetInput());
    }

    public virtual IEnumerator GetInput()
    {
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
    }
}
