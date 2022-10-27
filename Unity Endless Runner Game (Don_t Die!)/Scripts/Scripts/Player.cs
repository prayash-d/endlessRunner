using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat mana;


    private float maxHealth = 100;

    private float maxMana = 50;

  

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2; //allows to use the right direction when casting

    private SpellBook spellBook;


    public Transform MyTarget { get; set; }
    

    // Start is called before the first frame update
    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();

        health.Initialize(maxHealth, maxHealth);
        mana.Initialize(maxMana, maxMana);

/*
        target = GameObject.Find("Bat Target").transform; //finda bat target
        */

        base.Start();
    }

    // Update is called once per frame
    protected override void Update() //virtual: overrided
    {
        GetInput();

        //Debug.Log(LayerMask.GetMask("Block"));
      
        base.Update(); //will execute character's movement
    }

    

    private void GetInput() {

        direction = Vector2.zero; //after every loop of moving a direction, the speed is reset to 0 so that the player doesn't keep accelerating faster and faster

        //THIS IS USED FOR DEBUGGING ONLY
        ///
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            direction += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }


       
    }

    private IEnumerator Attack(int spellIndex)
    {
        Spell newSpell = spellBook.CastSpell(spellIndex);

        isAttacking = true;
        myAnimator.SetBool("attack", isAttacking);

        yield return new WaitForSeconds(newSpell.MyCastTime); //this is a hardcoded cast time, for debugging

        SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>(); //keeps same rotation of the fireball

        s.MyTarget = MyTarget;


        stopAttack();

        Debug.Log("attack executed!");
    }   

    public void castSpell(int spellIndex)
    {
        Block();

        if ((MyTarget != null) && !isAttacking && !isMoving && InLineOfSight())
        {
            //coroutine is smmtg you do while something else is running 
            attackRoutine = StartCoroutine(Attack(spellIndex));
        } //end if statement 

      
        
    }
    
    private bool InLineOfSight() //function only executed when player attacks
    {

        Vector3 targetDirection = ((MyTarget.transform.position - transform.position).normalized);

        //Debug.DrawRay(transform.position, targetDirection, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256); //blocks only hits layer 8

        if (hit.collider == null)
        {
            return true; //if doesnt hit anything, we can cast our spell
        }

        return false; //if collider can hit something
    }

    private void Block() //blocks view
    {

        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate(); 

    }

    public override void stopAttack()
    {

        spellBook.StopCasting();
        base.stopAttack();
    }
}
