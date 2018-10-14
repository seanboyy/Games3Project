using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WytriamSTD
{
    public class DayNightCycle : MonoBehaviour{
        public Light[] sceneLights;
        public Light sun;
        private Quaternion sunRotation;
        public float dayLengthMultiplier;
        public float speed;
        public float nightAngle;

        // Use this for initialization
        void Start(){
            sunRotation = sun.transform.rotation;
            foreach(Light light in sceneLights)
            {
                light.intensity = 0;
            }
        }

        void FixedUpdate(){
            sun.transform.rotation = sunRotation;
            if((sunRotation.eulerAngles.x < 180 - nightAngle || sunRotation.eulerAngles.x >= 180 + (180 - nightAngle))) {
                sunRotation *= Quaternion.Euler(speed * (1 + dayLengthMultiplier) * Time.fixedDeltaTime, 0, 0);
                foreach (Light light in sceneLights){
                    if (light.intensity < 1) light.intensity += Time.fixedDeltaTime;
                }
            }
            if((sunRotation.eulerAngles.x < 180 + (180 - nightAngle) && sunRotation.eulerAngles.x >= 180 - nightAngle)){
                sunRotation *= Quaternion.Euler(speed * (1 - dayLengthMultiplier) * Time.fixedDeltaTime, 0, 0);
                foreach (Light light in sceneLights){
                    if (light.intensity > 0) light.intensity -= Time.fixedDeltaTime;
                }
            }
        }
    }
}