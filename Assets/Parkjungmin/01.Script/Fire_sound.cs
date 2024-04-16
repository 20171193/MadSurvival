using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Sound : MonoBehaviour
{
    [SerializeField] public AudioSource firesound;

    public void PlayFire()
    {
        firesound?.Play();
    }

}
