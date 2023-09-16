using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1 : MonoBehaviour
{
    private GameObject Main;
    public GameObject target;

    Rigidbody rb;
    private BoxCollider coll;
    Vector3 start;
    private bool movingBackwards = false;

    private float margin = 1f;

    Vector3 forward;
    private bool collisionOccured = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        start = transform.position;
        Main = GameManager.instance.main;

        if (target == null)
            target = Main;

        forward = new Vector3(
            target.transform.position.x - transform.position.x,
            0f,
            target.transform.position.z - transform.position.z
        );
        rb.AddForce(forward * 10f, ForceMode.Force);

    }

    // Update is called once per frame
    void Update()
    {
        bool passedMain = Mathf.Abs(transform.position.z) - Mathf.Abs(Main.transform.position.z) > margin;

        if (passedMain)
        {
            /*            //            rb.velocity = Vector3.zero;
                        rb.isKinematic = true;
                        rb.isKinematic = false;
            */
            Vector3 backward = new Vector3(
                    start.x - transform.position.x,
                    0f,
                    start.z - transform.position.z
                );
            rb.AddForce(backward * 10f, ForceMode.Force);
            movingBackwards = true;

        }


        if (movingBackwards && Mathf.Abs(Main.transform.position.z) - Mathf.Abs(transform.position.z) > margin)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.AddForce(forward * 40f, ForceMode.Force);
            movingBackwards = false;
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (!collisionOccured)
            {
                coll.isTrigger = false;
                rb.useGravity = true;
            }

            /*            GameObject box2 = GameObject.Instantiate(
                            GameManager.instance.box2Prefab,
                            GameManager.instance.box2Prefabparent.transform
                         );*/

            //            rb.isKinematic = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("box2"))
            return;
        if (collisionOccured)
            return;

        collisionOccured = true;
//        rb.constraints = RigidbodyConstraints.FreezeAll;


        Debug.Log("len:" + collision.contacts.Length);



        /*        IntersectionResult res = LinesIntersectingNow(
                      new Vector2(collision.contacts[0].point.x, collision.contacts[0].point.z),
                      new Vector2(collision.contacts[1].point.x, collision.contacts[1].point.z),
                      new Vector2(collision.contacts[2].point.x, collision.contacts[2].point.z),
                      new Vector2(collision.contacts[3].point.x, collision.contacts[3].point.z)
                 );

                Vector3 intersection = new Vector3(
                    res.pos.x,
                    collision.contacts[0].point.y,
                    res.pos.y

                );*/
      
        IntersectionResult res = FindDiagonalsIntersection(collision.contacts);
        Vector3 intersection = new Vector3(res.pos.x, collision.contacts[0].point.y, res.pos.y);

        // 1,3 & 1,2 old
        Debug.Log("INTERSECTION : " + intersection);


        DrawLine(intersection, 6f);
        transform.position = intersection;
        float dis_x = Vector2.Distance(
             new Vector2(collision.contacts[0].point.x, collision.contacts[0].point.z),
             new Vector2(collision.contacts[2].point.x, collision.contacts[2].point.z)
        );

        Vector3 sc = transform.localScale;
        Debug.Log("DIS X:" + dis_x);
        //        transform.localScale = sc;

        int index_one = 3;
        int index_two  = 1;
        if (transform.position.z > Main.transform.position.z)
        {
            Debug.Log("BEFORE HALF");
            index_one = 1;
            index_two = 2;
        }
        else
        {
            Debug.Log("AFTER HALF");
        }


        index_one = 0;
        index_two = res.sideZ;

        float dis_z = Vector2.Distance(
             new Vector2(collision.contacts[index_one].point.x, collision.contacts[index_one].point.z),
             new Vector2(collision.contacts[index_two].point.x, collision.contacts[index_two].point.z)
        );

        Debug.Log("DIS Z:" + dis_z);
        sc.z = dis_z;
        transform.localScale = sc;







        GameObject box2 = GameObject.Instantiate(
             GameManager.instance.box2Prefab,
             GameManager.instance.boxContainer.transform
        );

        box2.transform.localScale = transform.localScale;
        box2.GetComponent<B2>().target = gameObject;

        Vector3 modified_pos = box2.transform.position;
        modified_pos.z = transform.position.z;
        box2.transform.position = modified_pos;


        drawCollisionLines(collision);
        transform.SetParent(GameManager.instance.container.transform);

    }

  
    private void drawCollisionLines(Collision collision)
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
        public int sideX;
        public int sideZ;

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
  
    void DrawLine(Vector3 point, float distance)
    {
        LineRenderer lineRendererX = new GameObject("LineX").AddComponent<LineRenderer>();
        lineRendererX.startColor = Color.black;
        lineRendererX.endColor = Color.black;
        lineRendererX.startWidth = 0.01f;
        lineRendererX.endWidth = 0.01f;
        lineRendererX.positionCount = 2;
        lineRendererX.useWorldSpace = true;

        //For drawing line in the world space, provide the x,y,z values
        lineRendererX.SetPosition(0, point); //x,y and z position of the starting point of the line
        lineRendererX.SetPosition(1, point * distance); //x,y and z position of the end point of the line

    }


    // return a diagonal intersecting point of 4 points which constructs a 
    private IntersectionResult FindDiagonalsIntersection(ContactPoint[] contacts)
    {
        float zero_diagonal_index = contacts[0].point.x - contacts[1].point.x;

        //        int point2 = 1;
        Vector3 point0 = contacts[0].point;
        Vector3 point1 = contacts[1].point;

        Vector3 point2 = Vector3.zero;
        Vector3 point3 = Vector3.zero;

        int sideX = 0;
        int sideZ = 0;
        
        int diag_2_counter = 0;
        Debug.Log(">>>>>>>>>>>>>>>>>");

        for (int i = 1; i < contacts.Length; i++)
        {
            Debug.Log(contacts[i].point);
            float diff = contacts[0].point.x - contacts[i].point.x;
            if (contacts[0].point.x != contacts[i].point.x && contacts[0].point.z != contacts[i].point.z)
            {
                point1 = contacts[i].point;
                zero_diagonal_index = i;
            }
            else
            {
                if (diag_2_counter == 0)
                    point2 = contacts[i].point;
                else if (diag_2_counter == 1)
                    point3 = contacts[i].point;

                if (contacts[0].point.x != contacts[i].point.x)
                {
                    sideX = i;
                    Debug.Log("SIDE Z is " + i);
                }
                if (contacts[0].point.z != contacts[i].point.z)
                {
                    sideZ = i;
                    Debug.Log("SIDE Z is " + i);
                }


                diag_2_counter++;

            }
        }

        Debug.Log("Zero's Diagonal is " + zero_diagonal_index);


        IntersectionResult res = LinesIntersectingNow(
          new Vector2(point0.x, point0.z),
          new Vector2(point1.x, point1.z),
          new Vector2(point2.x, point2.z),
          new Vector2(point3.x, point3.z)
     );

        Vector3 intersection = new Vector3(
            res.pos.x,
            point0.y,
            res.pos.y
        );
        res.sideX = sideX;
        res.sideZ = sideZ;


        return res;


    }

}