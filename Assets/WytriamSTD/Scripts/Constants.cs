using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class Intended To Store Useful Information, like scene names. 
 * This is a singleton class.
 */ 

namespace WytriamSTD
{

    public class Constants : MonoBehaviour
    {
        //Singleton Implementation
        private static Constants instance;
        //private Constants() { } //private constructor to prevent extra copies
        public static Constants getInstance()
        {
            //if (instance == null)
            //    instance = new Constants();
            return instance;
        }

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
