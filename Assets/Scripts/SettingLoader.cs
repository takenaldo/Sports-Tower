using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingLoader : MonoBehaviour
{
    public GameObject btnVibrate;
    public GameObject btnSound;
    // Start is called before the first frame update
    void Start()
    {
        if (btnSound != null)
        {
            if (Helper.isMusicON() == false)
            {
                btnSound.GetComponent<Image>().sprite = btnSound.GetComponent<ButtoHelper>().spriteOFF_en;
            }
            else
            {
                btnSound.GetComponent<Image>().sprite = btnSound.GetComponent<ButtoHelper>().spriteON_en;
            }
        }


        if (btnVibrate != null)
        {

            if (Helper.isVibrationON() == false)
            {
                btnVibrate.GetComponent<Image>().sprite = btnVibrate.GetComponent<ButtoHelper>().spriteOFF_en;

            }
            else
            {
                btnVibrate.GetComponent<Image>().sprite = btnVibrate.GetComponent<ButtoHelper>().spriteON_en;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
