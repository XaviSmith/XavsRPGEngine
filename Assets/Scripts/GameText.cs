using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameText : MonoBehaviour {

    public List<string> myText;
    public Text myTextObj;
    public int textIndex = 0;
    [SerializeField] public UnityEvent myAction;


    void OnEnable()
    {
        StartCoroutine(GetInput());
    }

    // Use this for initialization
    void Start () {
        myTextObj = GetComponent<Text>();
    }

    IEnumerator GetInput()
    {
        while (gameObject.activeSelf)
        {
            yield return null;

            AdvanceText();

        }
    }

    public void AdvanceText()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (textIndex < myText.Count - 1)
            {
                textIndex++;
                myTextObj.text = myText[textIndex];
            }

            if (textIndex >= myText.Count - 1)
            {
                myAction.Invoke();
            }
        }
        
    }

}
