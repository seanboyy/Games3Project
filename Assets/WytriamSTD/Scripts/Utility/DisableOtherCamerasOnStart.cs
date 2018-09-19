using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WytriamSTD
{

    public class DisableOtherCamerasOnStart : MonoBehaviour
    {
        private GameObject[] otherCameras;

        // Use this for initialization
        void Start()
        {
            otherCameras = GameObject.FindGameObjectsWithTag("MainCamera");
            foreach (GameObject camera in otherCameras)
                camera.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
