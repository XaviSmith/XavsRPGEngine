using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject cursor;
    public List<MenuOption> myItems;
    public int cursorPos = 0;
    public int menuCount;
    public int optionCount; //how many items in a column, so when we press left/right we know how far to advance

    void OnEnable()
    {
        StartCoroutine(GetInput());
        UpdateOptionCount();
    }


    public void RemoveOptions(int startIndex)
    {
        int count = myItems.Count - startIndex;
        myItems.RemoveRange(startIndex, count);
    }

    public void UpdateOptionCount()
    {
        optionCount = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                optionCount++;
            }
        }
        //Divide by 2 because a column will be half of our available options. Round up in case it's an odd number (e.g. if there's 5 options, there will be 3 on the left side, so move by 3 positions)
        optionCount = Mathf.CeilToInt(optionCount / 2);

    }

    // Use this for initialization
    void Start () {
        UpdateOptionCount();
	}
	
    IEnumerator GetInput()
    {
        while(gameObject.activeSelf)
        {
            yield return null;

            UpdateCursorPos();

        }
    }

    void UpdateCursorPos()
    {
        bool movedCursor = false;
        bool selectedOption = false;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movedCursor = true;
            if(cursorPos < myItems.Count - 1)
            {
                cursorPos++;
            }

            cursor.transform.position = new Vector2(myItems[cursorPos].transform.position.x - 110, myItems[cursorPos].transform.position.y + 10);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movedCursor = true;
            if (cursorPos > 0)
            {
                cursorPos--;
            }

            cursor.transform.position = new Vector2(myItems[cursorPos].transform.position.x - 110, myItems[cursorPos].transform.position.y + 10);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movedCursor = true;
            if (cursorPos + optionCount < myItems.Count)
            {
                cursorPos = cursorPos + optionCount;
            }

            cursor.transform.position = new Vector2(myItems[cursorPos].transform.position.x - 110, myItems[cursorPos].transform.position.y + 10);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movedCursor = true;
            if (cursorPos - optionCount >= 0)
            {
                cursorPos = cursorPos - optionCount;
            }

            cursor.transform.position = new Vector2(myItems[cursorPos].transform.position.x - 110, myItems[cursorPos].transform.position.y + 10);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            selectedOption = true;
            GetAction();
        }

        if(Input.GetKeyDown(KeyCode.X) && MenuManager.instance.previousMenu != null )
        {
            selectedOption = true;
            ChangeMenu(MenuManager.instance.previousMenu);
        }

        if(movedCursor)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.cursorSound);
        }
        else if(selectedOption)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.menuSelectSound);
        }
    }

    public void GetAction()
    {
        myItems[cursorPos].myAction.Invoke();
    }

    public void SetFighterAction()
    {

    }

	public void ChangeMenu(Menu otherMenu)
    {
        //OtherMenu.GetComponent<Menu>().previousMenu = this;
        otherMenu.gameObject.SetActive(true);
        ResetCursor();
        MenuManager.instance.currentMenu = otherMenu;
        MenuManager.instance.previousMenu = this; 
        gameObject.SetActive(false);
        
    }

    public void RenableMenu(Menu otherMenu)
    {
        otherMenu.gameObject.SetActive(false);
        //OtherMenu.GetComponent<Menu>().previousMenu = this;
        gameObject.SetActive(true);
    }

    public void ResetCursor()
    {
        cursorPos = 0;
        cursor.transform.position = new Vector2(myItems[0].transform.position.x - 110, myItems[0].transform.position.y + 10);
    }
}
