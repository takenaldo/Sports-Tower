using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public string nextScene= "Main";

    private float startTime;
    public float waitingSeconds = 5;

    public GameObject progressBar;

    // for preventing language loading redundancy
    private bool active = false;

    // project specific for progress bar
    public GameObject mover, destnation;

    // 
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        rotateProgressBar();
//        moveUp();
    
    }


    // Update is called once per frame
    void Update()
    {
        rotateProgressBar();
        float now = Time.time;

        if (now - startTime > waitingSeconds)
            SceneManager.LoadScene(nextScene);



    }


    void rotateProgressBar()
    {
        float speed = 100f;
        progressBar.gameObject.transform.Rotate(0, 0, -1 * speed * Time.deltaTime);

    }

    private void moveUp()
    {
        progressBar.gameObject.transform.Translate(Vector2.up * 2);
    }

    private void moveRight()
    {
        progressBar.gameObject.transform.Translate(Vector2.right * Time.deltaTime * 2);
    }



}
