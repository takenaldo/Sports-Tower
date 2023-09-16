using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1trial : MonoBehaviour
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
    private float speed = 1f;

    Vector3 direction = new Vector3(0, 0, -1f );
    private bool inMovement= true;

    public GameObject x1, x2, z1, z2;
    private bool stopped = false;

    private bool shouldCheckGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        start = transform.position;


        if (Main == null)
            Main = GameManager.instance.main;

/*        Renderer renderer = GetComponent<Renderer>();
        int random = new System.Random().Next(0, GameManager.instance.materials.Length);
        renderer.material = GameManager.instance.materials[random];
        Debug.Log("RANDOM: " + random);*/

        /*MeshRenderer ms = GetComponent<MeshRenderer>();
        ms.materials[0] = GameManager.instance.materials[new System.Random().Next(GameManager.instance.materials.Length - 1)];
*/


        direction = new Vector3(0, 0, -1f * speed);

        prepareSides();

/*        if (target == null)
            target = Main;
*/
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
            transform.Translate(direction * 1f * speed * Time.deltaTime);
        handleBoxMovement();
        checkUserInput();
        checkGameStatus();


    }

    private void checkGameStatus()
    {
        if (shouldCheckGameOver)
        {
            if (transform.position.y < GameManager.instance.trap.transform.position.y)
            {
                GameManager.instance.DialogGameOver.gameObject.SetActive(true);
                shouldCheckGameOver = false;

//                Helper.setTotalScore(GameManager.instance.score + Helper.getTotalScore());
                GameManager.instance.txtGameOverScore.text = GameManager.instance.score + "";
                GameManager.instance.txtGameOverTotalScore.text = Helper.getTotalScore() + "";
                GameManager.instance.DialogGameOver.SetActive(true);
            }
        }
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

    private void handleBoxMovement()
    {
        float z_min = (GameManager.instance.main.transform.position.z + margin) * -1;
        float z_max = (GameManager.instance.main.transform.position.z + margin);
        if (transform.position.z > z_max || transform.position.z < z_min)
            direction *= -1;
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
                        Debug.Log("--------> " + Mathf.Abs(mousePosition.y - pause.y));
                        Debug.Log("========> " + Mathf.Abs(mousePosition.x - pause.x));
                        Debug.Log("##############################");*/
            /*            if (Mathf.Abs(mousePosition.y - pause.y) < 500f || Mathf.Abs(mousePosition.x - pause.x) < 500f)
                        {
            */
            if (Mathf.Abs(Input.mousePosition.y - pause.y) < 100 && Mathf.Abs(Input.mousePosition.x - pause.x) < 100)
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

        if (stopped)
            return;
        
//        Debug.Log(z(z1) + " : " + Main_z2());
        if (z(z2) > Main_z1() || z(z1)  < Main_z2() )
        {
            rb.useGravity = true;
            shouldCheckGameOver = true;
        }
        else
        {

            if (z(gameObject) > Main.transform.position.z)
            {
                Debug.Log("%%%%%%%%%%%%%%%%%%%%%%");
                Debug.Log("BEFORE ");
                rb.isKinematic = true;
//                DrawSimplLine(GameManager.instance.z1.transform.position, GameManager.instance.z2.transform.position, "Main z1 to Mains Z2 ", true);
               //float scale_z = DrawSimpeLine(GameManager.instance.z1.transform.position, z2.transform.position, "Main z1 to Z2 ", true);
                Vector3 a = GameManager.instance.z1.transform.position;
                Vector3 b = z2.transform.position;
                a.y = 0;
                b.y = 0;
                Debug.Log("AZ " + a.z + "BZ " + b.z);

                //                float scale_mine = Vector2.Distance(a, b);
                float scale_mine =  Mathf.Abs(a.z - b.z);


//                DrawSimpeLine(a, b, "MINE", true);
                
                
                Vector3 modifiedScale = transform.localScale;
//                modifiedScale.z = scale_z ;
                modifiedScale.z = scale_mine;

                transform.localScale = modifiedScale;

                Vector3 mod = transform.position;
                mod.z = Main_z1() - (modifiedScale.z / 2f);
                transform.position = mod;

                Debug.Log("SCALE Z : " + scale_mine);
//                Debug.Log("z scale z " + scale_z);
                
            }
            else
            {
                Debug.Log("%%%%%%%%%%%%%%%%%%%%%%");
                rb.isKinematic = true;
                //float scale_z = DrawSimpeLine(z1.transform.position, GameManager.instance.z2.transform.position, "Z1 to Main z2 ", true);


                Vector3 a = z1.transform.position;
                Vector3 b = GameManager.instance.z2.transform.position;
                a.y = 0;
                b.y = 0;


                Debug.Log("AZ " + a.z+"BZ "+b.z);
                float scale_mine = Mathf.Abs(a.z - b.z);


                Vector3 modifiedScale = transform.localScale;
//                modifiedScale.z = scale_z;
                modifiedScale.z = scale_mine;

                transform.localScale = modifiedScale;

                Vector3 mod = transform.position;
                mod.z = Main_z2() + (modifiedScale.z / 2f);
                transform.position = mod;

            }

            if (!stopped)
            {

                Helper.vibrate();

                GameManager.instance.score++;
                GameManager.instance.txtScore.text = GameManager.instance.score+"";
                GameManager.instance.txtScore.text = GameManager.instance.score + "";
                Helper.setTotalScore(Helper.getTotalScore() + 1);
                GameManager.instance.txtTotalScore.text = Helper.getTotalScore() + "";


                InstantiateNewBox();

                GameManager.instance.z1 = z1;
                GameManager.instance.z2 = z2;

                transform.SetParent(GameManager.instance.container.transform);

            }

            Debug.Log("-------------------------------------------");
            Debug.Log("X scale: " +transform.localScale.x);
            Debug.Log("Z scale: " + transform.localScale.z);
            Debug.Log("-------------------------------------------");

            stopped = true;

        }
    }

    private void InstantiateNewBox()
    {
        GameObject box2 = GameObject.Instantiate(
             GameManager.instance.box2Prefab,
             GameManager.instance.boxContainer.transform
        );

        box2.transform.localScale = transform.localScale;
//        box2.GetComponent<B2trial>().target = gameObject;

        Vector3 modified_pos = box2.transform.position;
        modified_pos.z = transform.position.z;
        box2.transform.position = modified_pos;
        box2.GetComponent<B2trial>().Main = gameObject;

    }




    //return the 'z' postion of a gameobject
    float z(GameObject gameObject)
    {
        return gameObject.transform.position.z;
    }

    //return the 'x' postion of a gameobject
    float X(GameObject gameObject)
    {
        return gameObject.transform.position.x;
    }

    float Main_z1()
    {
        return GameManager.instance.z1.transform.position.z;
    }

    float Main_z2()
    {
        return GameManager.instance.z2.transform.position.z;
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