using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public GameObject open;
    public GameObject close;

    public void Open()
    {
        open.SetActive(true);
        close.SetActive(false);
    }
}
