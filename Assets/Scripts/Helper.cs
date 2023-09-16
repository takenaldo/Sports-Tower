using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Helper : MonoBehaviour
{
    public static string SETTING_VIBRATION = "setting_vibration";
    public static string SETTING_MUSIC     = "setting_music";
    public static string BG_MUSIC = "bg_music";
    public static string LANGUAGE = "user_language";


    public static string STATUS_ON = "on";
    public static string STATUS_OFF = "off";

    public static string USER_LEVEL = "user_level";

    public static string TOTAL_SCORE = "total_score";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void pause()
    {
        Time.timeScale = 0;
    }

    public void resume()
    {
        Time.timeScale = 1;
    }



    public void triggMusicSetting()
    {
        AudioSource[] allAudioSources = getAllAudioSources();
         if (PlayerPrefs.GetString(SETTING_MUSIC, STATUS_ON) == STATUS_ON)
        {
            PlayerPrefs.SetString(SETTING_MUSIC, STATUS_OFF);
            foreach (AudioSource audioSource in allAudioSources)
                audioSource.Stop();
        }
        else if (PlayerPrefs.GetString(SETTING_MUSIC, STATUS_ON) == STATUS_OFF)
        {
            PlayerPrefs.SetString(SETTING_MUSIC, STATUS_ON);

            AudioSource bgMusic = GameObject.FindGameObjectWithTag("BG_MUSIC").GetComponent<AudioSource>();
            bgMusic.Play();
        }
    }



    public void triggMusicSettingWithBG(GameObject audioManager)
    {
        AudioSource[] allAudioSources = getAllAudioSources();
        if (PlayerPrefs.GetString(SETTING_MUSIC, STATUS_ON) == STATUS_ON)
        {
            PlayerPrefs.SetString(SETTING_MUSIC, STATUS_OFF);
            foreach (AudioSource audioSource in allAudioSources)
                audioSource.Stop();
        }
        else if (PlayerPrefs.GetString(SETTING_MUSIC, STATUS_ON) == STATUS_OFF)
        {
            PlayerPrefs.SetString(SETTING_MUSIC, STATUS_ON);
            audioManager.GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(audioManager);
        }
    }




    public AudioSource[] getAllAudioSources()
    {
      return  GameObject.FindObjectsOfType<AudioSource>();
    }


    public void changeVibrationSetting()
    {
        if(PlayerPrefs.GetString(SETTING_VIBRATION, STATUS_ON) == STATUS_ON)
            PlayerPrefs.SetString(SETTING_VIBRATION, STATUS_OFF);
        else
            PlayerPrefs.SetString(SETTING_VIBRATION, STATUS_ON);
    }


    public static void rotateAroundMyself(GameObject go)
    {
        go.transform.Rotate(new Vector3(0, 0, -1));
    }

    public void LoadLevel(int lvl)
    {
//        if ((lvl) <= PlayerPrefs.GetInt(Helper.USER_LEVEL, 1))
            LoadScene("Level" + lvl);
    }

    public void PlayCurrentLevel()
    {
        LoadScene("Level" + (PlayerPrefs.GetInt(Helper.USER_LEVEL, 1)));
    }


    public static int getHighScore()
    {
        return PlayerPrefs.GetInt(Helper.USER_LEVEL, 1);
    }
    public static void setHighScore(int hs)
    {
        if( hs > getHighScore() )
            PlayerPrefs.SetInt(Helper.USER_LEVEL,hs);
    }



    public static int getTotalScore()
    {
        return PlayerPrefs.GetInt(Helper.TOTAL_SCORE, 0);
    }

    public static void setTotalScore(int new_total_score)
    {
        PlayerPrefs.SetInt(Helper.TOTAL_SCORE, new_total_score);
    }

    public static bool isMusicON()
    {
        return PlayerPrefs.GetString(SETTING_MUSIC, STATUS_ON) == STATUS_ON;
    }

    public static bool isVibrationON()
    {
        return PlayerPrefs.GetString(SETTING_VIBRATION, STATUS_ON) == STATUS_ON;
    }





    // destroys an object , usefull to call from unity UI
    public void destroy(GameObject g)
    {
        Destroy(gameObject);
    }

    public void enable(GameObject g)
    {
        g.SetActive(true);
    }

    public void disable(GameObject g)
    {
        g.SetActive(false);
    }

    public static void vibrate()
    {
        if(isVibrationON())
            Handheld.Vibrate();
    }
 

}