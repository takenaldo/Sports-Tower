using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtoHelper : MonoBehaviour
{

    public Sprite spriteON_en, spriteOFF_en;
    public Sprite spriteON_ru, spriteOFF_ru;

    // for buttons that has only two options like on and off
    public void SwitchButtonONOFF()    {
        Image image = gameObject.GetComponent<Image>();
        if (image.sprite == spriteON_en || image.sprite == spriteON_ru)
            image.sprite = getSpriteOFF();
        else
            image.sprite = getSpriteON();

    }

    private Sprite getSpriteOFF()
    {
            return spriteOFF_en;
    }

    private Sprite getSpriteON()
    {
            return spriteON_en;

    }
}
