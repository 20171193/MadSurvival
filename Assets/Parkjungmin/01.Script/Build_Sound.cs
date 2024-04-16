using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Sound : MonoBehaviour
{
    [SerializeField] AudioSource buildsound;

    private void OnEnable()
    {
        buildsound?.Play();
    }
}
