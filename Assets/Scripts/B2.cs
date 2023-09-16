using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B2 : MonoBehaviour
{
    private GameObject Main;
    Rigidbody rb;
    BoxCollider coldr;
    Vector3 start;
    private bool movingBackwards = false;
    public bool move_f = false;

    private float margin = 1f;

    Vector3 forward;
    private bool dropped;
    private float droppedTime = 0f;
    private bool collisionOccured;

    public GameObject target;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coldr = GetComponent<BoxCollider>();
        start = transform.position;
        Main = GameManager.instance.main;

        if (target == null)
            target = Main;


        forward = new Vector3(
            transform.position.x - target.transform.position.x,
            0f,
            transform.position.z - target.transform.position.z
        );
        rb.AddForce(forward * 10f * -1, ForceMode.Force);

        move_f = true;

        containerStartingPos = GameManager.instance.container.transform.position;
    }

    Vector3 containerStartingPos;
    // Update is called once per frame
    void Update()
    {

        performBoxMove();
        checkUserInput();
        performMoveDown();


    }

    
    private void OnCollisionEnter(Collision collision)
    {


        if (collisionOccured)
            return;

        collisionOccured = true;

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(collision.contacts[i].point.z);
        }
        IntersectionResult res = LinesIntersectingNow(
              new Vector2(collision.contacts[0].point.x, collision.contacts[0].point.z),
              new Vector2(collision.contacts[1].point.x, collision.contacts[1].point.z),
              new Vector2(collision.contacts[2].point.x, collision.contacts[2].point.z),
              new Vector2(collision.contacts[3].point.x, collision.contacts[3].point.z)
         );

        Vector3 intersection = new Vector3(
            res.pos.x,
            collision.contacts[0].point.y,
            res.pos.y
        );


        transform.position = intersection;


        int index_one = 2;
        int index_two = 0;
        if (transform.position.x < Main.transform.position.x)
        {
            Debug.Log("AFTER HALF");
            index_one = 1;
            index_two = 2;
        }
        else
        {
            Debug.Log("BEFORE HALF");
        }


        float closest_distance = collision.contacts[0].point.z  - collision.contacts[1].point.z ;
        int closest = 1;

        for (int i = 1; i < 4; i++)
        {
//            float cpAt0x = collision.contacts[0].point.x;
            float cpAt0z = collision.contacts[0].point.z;

            if ((cpAt0z - collision.contacts[i].point.z) < closest_distance)
            {
                closest = i;
                closest_distance = (cpAt0z - collision.contacts[i].point.z);

//                break;
            }

        }

        Debug.Log("It's " + closest);
        index_one = 0;
        index_two = closest;



        float dis_x = Vector2.Distance(
             new Vector2(collision.contacts[index_one].point.x, collision.contacts[index_one].point.z),
             new Vector2(collision.contacts[index_two].point.x, collision.contacts[index_two].point.z)
        );


        Vector3 sc = transform.localScale;
        Debug.Log("DIS X:" + dis_x);
        sc.x = dis_x;
        transform.localScale = sc;

        drawLines(collision);

        dropped = true;
        transform.SetParent(GameManager.instance.container.transform);

    }

    void checkUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!collisionOccured)
            {
                coldr.isTrigger = false;
                rb.useGravity = true;
            }
        }
    }


    private void performBoxMove()
    {
        GameObject Main = GameManager.instance.main;
        bool passedMain = Mathf.Abs(transform.position.x) - Mathf.Abs(Main.transform.position.x) > margin && (transform.position.x < Main.transform.position.x);

        /*        Vector3 velocity = Vector3.zero;
                if(rb!=null)
                    velocity = rb.velocity;

                if (collisionOccured)
                    if(rb!=null)
                        Destroy(rb);*/

        Vector3 velocity = rb.velocity;
        if (collisionOccured)
        {
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.velocity = Vector3.zero;
//s            rb.useGravity = false;
        }


        if (passedMain)
        {
            /*            //            rb.velocity = Vector3.zero;
                        rb.isKinematic = true;
                        rb.isKinematic = false;
            */

            Debug.Log("Moving Backwards");
            Vector3 backward = new Vector3(
                    start.x - transform.position.x,
                    0f,
                    start.z - transform.position.z
                );
            rb.AddForce(backward * 10f, ForceMode.Force);
            movingBackwards = true;
            move_f = false;

        }


        if (movingBackwards && ((transform.position.x) - (Main.transform.position.x)) > margin)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.AddForce(forward * 40f * -1f, ForceMode.Force);
            movingBackwards = false;
            move_f = true;

            Debug.Log("Moving Forward");
        }
    }


    void drawLines(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            for (int j = i + 1; j < collision.contacts.Length; j++)
            {

                //For creating line renderer object
                LineRenderer lineRendererX = new GameObject("Line" + i + "," + j).AddComponent<LineRenderer>();
                lineRendererX.startColor = Color.black;
                lineRendererX.endColor = Color.black;
                lineRendererX.startWidth = 0.01f;
                lineRendererX.endWidth = 0.01f;
                lineRendererX.positionCount = 2;
                lineRendererX.useWorldSpace = true;

                //For drawing line in the world space, provide the x,y,z values
                lineRendererX.SetPosition(0, collision.contacts[i].point); //x,y and z position of the starting point of the line
                lineRendererX.SetPosition(1, collision.contacts[j].point); //x,y and z position of the end point of the line
            }
        }

    }

    public struct IntersectionResult
    {
        public Vector2 pos;
        public bool isIntersecting;
    }
    public IntersectionResult LinesIntersectingNow(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        IntersectionResult myRet = new IntersectionResult();
        Vector2 s1;
        Vector2 s2;
        s1.x = B.x - A.x;
        s1.y = B.y - A.y;
        s2.x = D.x - C.x;
        s2.y = D.y - C.y;
        float s = ((-1 * s1.y) * (A.x - C.x) + s1.x * (A.y - C.y)) / ((-1 * s2.x) * s1.y + s1.x * s2.y);
        float t = (s2.x * (A.y - C.y) - s2.y * (A.x - C.x)) / ((-1 * s2.x) * s1.y + s1.x * s2.y);
        if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
        {
            myRet.isIntersecting = true;
            myRet.pos.x = A.x + (t * s1.x);
            myRet.pos.y = A.y + (t * s1.y);
        }
        return myRet;
    }
    public Vector2 V_2(Vector3 input)
    {
        return new Vector2(input.x, input.y);
    }
    public void IntersectionSample()
    {

        LineRenderer line1 = new GameObject("Line1").AddComponent<LineRenderer>();
        LineRenderer line2 = new GameObject("Line2").AddComponent<LineRenderer>();

        line1.positionCount = 2;
        line2.positionCount = 2;
        line1.SetPositions(new Vector3[2] { new Vector3(0f, -0.5f, 0f), new Vector3(0f, 0.5f, 0f) });
        line2.SetPositions(new Vector3[2] { new Vector3(-0.5f, -0.6f, 0f), new Vector3(0.5f, 0.4f, 0f) });
        IntersectionResult myResult = LinesIntersectingNow(V_2(line1.GetPosition(0)), V_2(line1.GetPosition(1)), V_2(line2.GetPosition(0)), V_2(line2.GetPosition(1)));
        Debug.Log("crossing point x:" + myResult.pos.x);
        Debug.Log("crossing point y:" + myResult.pos.y);
        Debug.Log("was crossing:" + myResult.isIntersecting);
        //output Crossing point x:0,y:-0.1, crossing is true
    }


    void performMoveDown()
    {
        Vector3 velocity = rb.velocity;
        // this means the bloack is dropped and at rest , so let's move down all the blocks
        if (dropped && velocity == Vector3.zero)
        {
            //            Vector3 new_container_pos = GameManager.instance.container.transform.position;
            Vector3 new_container_pos = containerStartingPos;
            new_container_pos.y = new_container_pos.y - (2 * Mathf.Abs(transform.localScale.y));

            if (droppedTime == 0f)
                droppedTime = Time.time;


            if (Time.time - droppedTime > 3f)
            {
                if (!(GameManager.instance.container.transform.position.y <= new_container_pos.y))
                {
                    GameManager.instance.container.transform.Translate(Vector3.down * Time.deltaTime);
                }
                else
                {
                    //                    GameManager.instance.container.transform.position = new_container_pos;
                    dropped = false;
                    droppedTime = 0f;
                    containerStartingPos = GameManager.instance.container.transform.position;


                    createNextBox();

                }
            }



        }
    }

    void createNextBox()
    {

        GameObject box1 = GameObject.Instantiate(
             GameManager.instance.box1Prefab,
             GameManager.instance.boxContainer.transform
          );

        box1.transform.localScale = transform.localScale;
        box1.GetComponent<B1>().target = gameObject;

        Vector3 modified_pos = box1.transform.position;
        modified_pos.x = transform.position.x;
        box1.transform.position = modified_pos;


        //        drawCollisionLines(collision);
        transform.SetParent(GameManager.instance.container.transform);

    }


}





