using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the base class for all functionable characters (players, enemies etc)

public abstract class Character : MonoBehaviour {

    [SerializeField] //only player can access speed since its private
    private float speed;
    protected Animator myAnimator;
    private Rigidbody2D myRigidbody;


    protected bool isAttacking = false;

    protected Coroutine attackRoutine;

    //players direction
    protected Vector2 direction; //when smtg is protected, everything that inherits from the script can access it

    public bool isMoving //used to figure out if were moving or not
    {
        get
        {
            return (direction.x != 0 || direction.y != 0);
        }
    }
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update() //virtual: overrides a function in the player class
        //to override something in an inherited class, you must use keyword "virtual" in the base class
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        myRigidbody.velocity = direction.normalized * speed; //using physics system in unity (more independent)
        //direction.normalized: returns ddirection as a unit vector (to normalize all movement speeds)

    }

    public void HandleLayers()
    {
        if (isMoving)
        {
            ActivateLayer("Walk Layer");

            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            stopAttack();
        }
        else if (isAttacking) {
            ActivateLayer("Attack Layer");
        }

        else
        {
            ActivateLayer("Idle Layer");
        } 
        
        /*else if (isAttacking)
        {
            ActivateLayer("Attack Layer");
        }*/

        //stopAttack();
    }

    public void ActivateLayer(string layerName) //allows to enable and disable layers of our choice
    {
        for(int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void stopAttack() {

        if (attackRoutine != null)
        {
            isAttacking = false;

            myAnimator.SetBool("attack", isAttacking);

            if(attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }
            

           
            
        }     
    }
}
