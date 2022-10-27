using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
   
    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float speed;


    public Transform MyTarget { get; set; }

    
    
    // Start is called before the first frame update
    void Start()
    {

        myRigidBody = GetComponent<Rigidbody2D>();

 
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        if (MyTarget!=null)
        {
            Vector2 direction = (MyTarget.position) - (transform.position); //target position - fireball positon

            myRigidBody.velocity = direction.normalized * speed; //moves fireball to enemy

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //angle in degrees

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox" && collision.transform ==MyTarget)
        {
            GetComponent<Animator>().SetTrigger("Impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
