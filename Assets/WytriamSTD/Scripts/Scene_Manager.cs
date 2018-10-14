using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is intended to be inherited by every Scene_Manager class I use (not to be confused with SceneManger)
 * It includes some standard scene manager items, like access the Constants class. 
 * Scene_Manager's that inherit from this should be called SM_SceneName
 */

namespace WytriamSTD
{

    public class Scene_Manager : MonoBehaviour
    {
        //protected WytriamSTD.Constants constants = WytriamSTD.Constants.getInstance();
        //protected WytriamSTD.Announcements announcements = WytriamSTD.Announcements.getInstance();
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void announce(string announcement)
        {
            Announcements.getInstance().DisplayAnnouncement(announcement);
        }

        public void announce(string announcement, int duration)
        {
            Announcements.getInstance().DisplayAnnouncement(announcement, duration);
        }

        public void muteAnnoucements()
        {
            Announcements.getInstance().disableAnnouncements();
        }

        public void hearAnnouncements()
        {
            Announcements.getInstance().enableAnnouncements();
        }
    }
}
