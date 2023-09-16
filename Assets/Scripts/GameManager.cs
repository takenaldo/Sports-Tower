using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject main;

    public GameObject box2Prefab;
    public GameObject box1Prefab;

    public GameObject container;
    public GameObject boxContainer;

    public GameObject x1, x2, z1, z2;

    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtTotalScore;

    public int score = 0;

    public GameObject trap;
    public GameObject DialogGameOver;
    public GameObject DialogPause;


    public TextMeshProUGUI txtGameOverTotalScore;
    public TextMeshProUGUI txtGameOverScore;
    public GameObject btnPause;


    public Material[] materials;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        Debug.Log(z1.transform.position);
        Debug.Log(main.transform.position.z + main.transform.localScale.z);
        Debug.Log(z2.transform.position);
        Debug.Log(main.transform.position.z - main.transform.localScale.z);

        txtGameOverTotalScore.text = Helper.getHighScore() + "";
/*        Debug.Log(x2);
        Debug.Log(z1);
        Debug.Log(z2);
*/        Debug.Log(">>>>>>>>>>>>>>>>>>>>");
    }
}
