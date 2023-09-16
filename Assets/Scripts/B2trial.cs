using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B2trial : MonoBehaviour
{
    public GameObject Main;
    public GameObject target;

    Rigidbody rb;
    private BoxCollider coll;
    Vector3 start;
    private bool movingForward = true;
    private bool movingBackward = false;


    private float margin = 1f;

    Vector3 forward;
    private bool collisionOccured = false;
    Vector3 direction = new Vector3(-1, 0, 0f);
    private float speed = 1f;

    private bool inMovement= true;

    public GameObject x1, x2, z1, z2;
    private bool stopped = false;
    private bool dropped;

    Vector3 containerStartingPos;
    private float droppedTime = 0f;

    private bool shouldCheckGameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        start = transform.position;


        /*Renderer renderer = GetComponent<Renderer>();
        int random = new System.Random().Next(0, GameManager.instance.materials.Length);
        renderer.material = GameManager.instance.materials[random];
        Debug.Log("RANDOM: " + random);
*/

        /*        MeshRenderer ms = GetComponent<MeshRenderer>();
                int random = new System.Random().Next(0, GameManager.instance.materials.Length - 1);
                Debug.Log("RANDOM: " + random);
                ms.materials[0] = GameManager.instance.materials[random];
        */
        if (Main == null)
            Main = GameManager.instance.main;

        containerStartingPos = GameManager.instance.container.transform.position;

        direction = new Vector3(-1 * speed,0, 0);


    

        prepareSides();

        if (target == null)
            target = Main;

        /*forward = new Vector3(
            target.transform.position.x - transform.position.x,
            0f,
            target.transform.position.z - transform.position.z
        );
        rb.AddForce(forward * 10f, ForceMode.Force);*/

    }

    // Update is called once per frame
    void Update()
    {
        if(inMovement)
            transform.Translate(direction * 1f* speed * Time.deltaTime);


        handleBoxMovement();
        checkUserInput();
        checkGameStatus();

        //        if (dropped)
        //            performMoveDown();


    }

    private void checkGameStatus()
    {
        if (shouldCheckGameOver)
        {
            if (transform.position.y < GameManager.instance.trap.transform.position.y)
            {
                GameManager.instance.DialogGameOver.gameObject.SetActive(true);
                shouldCheckGameOver = false;

                GameManager.instance.txtGameOverScore.text = GameManager.instance.score + "";
                GameManager.instance.txtGameOverTotalScore.text = Helper.getTotalScore() + "";
                GameManager.instance.DialogGameOver.SetActive(true);
            }
        }
    }

    private void checkUserInput()
    {
        if (Input.GetMouseButtonDown(0) && !stopped)
        {
//            rb.useGravity = true;
//            rb.isKinematic = true;

            inMovement = false;


            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pause = (GameManager.instance.btnPause.transform.position);

            Debug.Log("MOUSE POS X => " + Input.mousePosition.x);
            Debug.Log(" BTN PAUSE X => " + pause.x);


            Debug.Log("MOUSE POS Y  => " + Input.mousePosition.y);
            Debug.Log(" BTN PAUSE Y => " + pause.y);

            /*
                        Vector3 pause = Camera.main.ScreenToWorldPoint(GameManager.instance.btnPause.transform.position);
                        Debug.Log("///////////////// " + pause);
                        Debug.Log("--------> " + Mathf.Abs(mousePosition.y - pause.y));
                        Debug.Log("========> " + Mathf.Abs(mousePosition.x - pause.x));
                        Debug.Log("##############################");*/
            //            if ( Mathf.Abs(mousePosition.y - GameManager.instance.btnPause.transform.position.y) < 1f || Mathf.Abs(mousePosition.x - GameManager.instance.btnPause.transform.position.x) < 1f)
            //            if (Mathf.Abs(mousePosition.y - pause.y) < 4000f || Mathf.Abs(mousePosition.x - pause.x) < 3000f){



            if ( Mathf.Abs(Input.mousePosition.y - pause.y)  < 100 && Mathf.Abs(Input.mousePosition.x - pause.x) < 100)
            {

                    inMovement = true;
                Debug.Log("PAUSE CLICKED");
                return;
            }

            if (GameManager.instance.DialogGameOver.activeSelf || GameManager.instance.DialogPause.activeSelf)
            {
                inMovement = true;
                Debug.Log("NOO");
                return;
            }

            PerformCut();
        }
    }

    private void PerformCut()
    {
//        Debug.Log(x(x2) + " <> " + Main_x1());
//        if (z(z2) > Main_z1() || z(z1) < Main_z2())

        if (x(x2) > Main_x1() || (x(x1) < Main_x2()))
        {
            rb.useGravity = true;
            shouldCheckGameOver = true;
        }
        else
        {
          if (x(gameObject) > Main.transform.position.x)
            {
                rb.isKinematic = true;
                Vector3 a = GameManager.instance.x1.transform.position;
                Vector3 b = x2.transform.position;
                a.y = 0;
                b.y = 0;


                float scale_mine = Mathf.Abs(a.x - b.x);

                Vector3 modifiedScale = transform.localScale;
                modifiedScale.x = scale_mine;
                transform.localScale = modifiedScale;

                Vector3 mod = transform.position;
                mod.x = Main_x1() - (modifiedScale.x / 2f);
                transform.position = mod;

  //              Debug.Log("x scale x " + scale_x);

            }
            else
            {
//                Debug.Log(">>>>>>>>> AFTER>>>>>>>>>>>>");
                Debug.Log(">>>>>>>>>>>>>>>>>AFTER " + x(gameObject) + "---" + Main.transform.position.x);
                rb.isKinematic = true;
                //float scale_x = DrawSimpeLine(x1.transform.position, GameManager.instance.x2.transform.position, "x1 to Main x2 ", true);

                Vector3 a = x1.transform.position;
                Vector3 b = GameManager.instance.x2.transform.position;
                a.y = 0;
                b.y = 0;

                float scale_mine = Mathf.Abs(a.x - b.x);

                Vector3 modifiedScale = transform.localScale;
                modifiedScale.x = scale_mine;
                transform.localScale = modifiedScale;

                Vector3 mod = transform.position;
                mod.x = Main_x2() + (modifiedScale.x / 2f);
                transform.position = mod;

            }


            dropped = true;
            transform.SetParent(GameManager.instance.container.transform);


            if (!stopped)
            {

                Helper.vibrate();

                GameManager.instance.score++;

                GameManager.instance.txtScore.text = GameManager.instance.score + "";
                Helper.setTotalScore(Helper.getTotalScore()+1 );
                GameManager.instance.txtTotalScore.text = Helper.getTotalScore() + "";


                InstantiateNewBox();
                performMoveDownNoAnim();

                GameManager.instance.x1 = x1;
                GameManager.instance.x2 = x2;

                //performMoveDown();
            }                

            stopped = true;




        }

    }

    void performMoveDownNoAnim()
    {
        if (dropped)
        {
            Vector3 new_pos = GameManager.instance.container.transform.position;
            new_pos.y = GameManager.instance.container.transform.position.y - (2 * Mathf.Abs(transform.localScale.y));
            GameManager.instance.container.transform.position = new_pos;
            dropped = false;
        }
    }

    void performMoveDown()
    {

        // this means the bloack is dropped and at rest , so let's move down all the blocks
        Debug.Log("HERE BF");
        if (dropped)
        {
            Debug.Log("HERE AF");

            //Vector3 new_container_pos = GameManager.instance.container.transform.position;
            Vector3 new_container_pos = containerStartingPos;
            new_container_pos.y = new_container_pos.y - (2 * Mathf.Abs(transform.localScale.y));

            if (droppedTime == 0f)
                droppedTime = Time.time;


            if (Time.time - droppedTime > 3f)
            {
                Debug.Log("in if");
                if (!(GameManager.instance.container.transform.position.y < new_container_pos.y))
                {
                    GameManager.instance.container.transform.Translate(Vector3.down * 10 * Time.deltaTime);
                }
                else
                {
                    Debug.Log("in else");
                    //                    GameManager.instance.container.transform.position = new_container_pos;
                    dropped = false;
                    droppedTime = 0f;
                    containerStartingPos = GameManager.instance.container.transform.position;
                    InstantiateNewBox();
                }
            }



        }
    }
    //return the 'z' postion of a gameobject
    float z(GameObject gameObject)
    {
        return gameObject.transform.position.z;
    }

    //return the 'x' postion of a gameobject
    float x(GameObject gameObject)
    {
        return gameObject.transform.position.x;
    }

    float Main_x1()
    {
        return GameManager.instance.x1.transform.position.x;
    }

    float Main_x2()
    {
        return GameManager.instance.x2.transform.position.x;
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


    private void prepareSides()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject childObject = transform.GetChild(i).gameObject;
            if (childObject.name == "X1")
                x1 = childObject;
            if (childObject.name == "X2")
                x2 = childObject;
            if (childObject.name == "Z1")
                z1 = childObject;
            if (childObject.name == "Z2")
                z2 = childObject;
        }
    }


    float DrawSimpeLine(Vector3 line1, Vector3 line2, string name, bool ignoreY)
    {
        //For creating line renderer object
        LineRenderer lineRendererZ = new GameObject(name).AddComponent<LineRenderer>();
        lineRendererZ.startColor = Color.black;
        lineRendererZ.endColor = Color.black;
        lineRendererZ.startWidth = 0.01f;
        lineRendererZ.endWidth = 0.01f;
        lineRendererZ.positionCount = 2;
        lineRendererZ.useWorldSpace = true;

        if (ignoreY)
        {
            line1.y = 0;
            line2.y = 0;
        }

        //For drawing line in the world space, provide the x,y,z values
        lineRendererZ.SetPosition(0, line1); //x,y and z position of the starting point of the line
        lineRendererZ.SetPosition(1, line2); //x,y and z position of the end point of the line

        Debug.Log("===> " + lineRendererZ.transform.localScale.z);

        return Mathf.Abs(lineRendererZ.GetPosition(0).z) + Mathf.Abs(lineRendererZ.GetPosition(lineRendererZ.positionCount - 1).z);
    }

    private void InstantiateNewBox()
    {
        GameObject box1 = GameObject.Instantiate(
             GameManager.instance.box1Prefab,
             GameManager.instance.boxContainer.transform
        );

        Debug.Log(gameObject.transform.position+" : ........");
        box1.transform.localScale = transform.localScale;
//        box1.GetComponent<B2trial>().target = transform.gameObject;
        Vector3 modified_pos = box1.transform.position;
        modified_pos.x = transform.position.x;
        box1.transform.position = modified_pos;
        box1.GetComponent<B1trial>().Main = gameObject;

    }

    private void handleBoxMovement()
    {
        float x_min = (GameManager.instance.main.transform.position.x + margin) * -1;
        float x_max = (GameManager.instance.main.transform.position.x + margin);
        if (transform.position.x > x_max || transform.position.x < x_min)
            direction *= -1;
    }


}