using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Generic class for any option in a Menu
public class MenuOption : MonoBehaviour {

    public Text myText;
    [SerializeField] public UnityEvent myAction;
    public GameObject myObj;

    void Start()
    {
        myText = GetComponent<Text>();
    }
	
}
