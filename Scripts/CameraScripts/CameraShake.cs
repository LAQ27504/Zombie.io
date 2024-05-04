using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float shakerTimer;

    private void Awake()
    {
        Instance = this;
        shakerTimer = 0;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>(); 
    }

    public void ShakeCamera(float intensity, float timer)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakerTimer = timer;
    }

    private void Update()
    {
        if (shakerTimer > 0)
        {
            shakerTimer -= Time.deltaTime;
            if (shakerTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }

    }
}
