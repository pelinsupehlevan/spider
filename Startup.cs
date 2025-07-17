using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 200);
        slider.value = PlayerPrefs.GetFloat("MouseSensitivity", 200);
    }
}
