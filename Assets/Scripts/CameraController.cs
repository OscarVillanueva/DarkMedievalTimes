using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    // sacamos la camara virtual
    private CinemachineVirtualCamera vCam;

    // sacamos la propiedad noise para poder sacudir la camara
    private CinemachineBasicMultiChannelPerlin noise;

    // Setteamos las propiedades
    void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //noise.m_AmplitudeGain = 0;
        //noise.m_FrequencyGain = 0;
    }

    // Función para sacudir la camara
    public void Shake(float duration = 0.1f, float amplitude = 1.5f, float frecuency = 20)
    {
        // Detenemos todas las corutinas que esten pendientes dentro de este script
        StopAllCoroutines();
        StartCoroutine(ApplyNoiseRoutine(duration, amplitude, frecuency));


    }

    IEnumerator ApplyNoiseRoutine(float duration, float amplitude, float frecuency)
    {
        // cambiamos la amplitud de la camara
        noise.m_AmplitudeGain = amplitude;

        // la frecuencia de movimiento
        noise.m_FrequencyGain = frecuency;

        yield return new WaitForSeconds(duration);

        // reiniciamos
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
