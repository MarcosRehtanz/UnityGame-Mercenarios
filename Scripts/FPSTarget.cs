using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTarget : MonoBehaviour
{
    private int target = 60;
    [SerializeField] private int fps;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }

    void Update()
    {
        fps = Application.targetFrameRate;

        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;
    }
}
