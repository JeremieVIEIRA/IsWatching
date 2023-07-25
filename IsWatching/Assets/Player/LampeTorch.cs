using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class LampeTorch : MonoBehaviour
{
    public AudioPlayerScript audioScript;
    [SerializeField] GameObject lampe;
    bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLamp();
    }

    void UpdateLamp()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isActive)
            {
                lampe.gameObject.SetActive(false);
                isActive = false;
                audioScript.FlashOff();
            }
            else
            {
                lampe.gameObject.SetActive(true);
                isActive = true;
                audioScript.FlashOn();
            }
        }
    }
}
