using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 14f; //f makes sure the number is a float
    public float jumpSpeed = 20f;

    public float groundedRadius = 0.5f;

    public bool canMove = true;
    [SerializeField]

    //public Animator anim;

    public bool walking = false;

    public bool faceLeft = false; //for facing left
    float someScale;

    private Vector2 moveDirection = Vector2.zero; //initialize the move vector. Vector2 = 2 axes, Vector3 = 4 axes etc. 

    public static PlayerMovement instance;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        someScale = transform.localScale.x;
        //anim = GetComponent<Animator>();
        StartCoroutine(GetInput());
    }

    IEnumerator GetInput()
    {

        while (true)
        {
            yield return null;

            if(canMove)
            {
                //Get our movement inputs in the horizontal and vertical planes
                moveDirection.x = Input.GetAxisRaw("Horizontal") * moveSpeed;

                moveDirection.y = Input.GetAxisRaw("Vertical") * moveSpeed;

                moveDirection = transform.TransformDirection(moveDirection); //set our movement direction

                if (Input.GetAxisRaw("Horizontal") < 0)
                {

                    faceLeft = true;
                    walking = true;
                }

                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    faceLeft = false;
                    walking = true;
                }

                else
                {
                    walking = false;
                }

                if (faceLeft)
                {
                    transform.localScale = new Vector2(-someScale, transform.localScale.y);
                }

                else
                {
                    transform.localScale = new Vector2(someScale, transform.localScale.y);
                }

                transform.Translate(moveDirection * Time.deltaTime); //actually move


                // anim.SetBool("Walking", walking);
            }

        }
    }


}
