/*
 * The general idea behind this class is to have a Singleton Announcement Class that 
 *      - easily makes onscreen announcements appear
 *      - records all announcements made in a log
 * This script should be attached to a Canvas Object named "Announcements"
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WytriamSTD
{
    public class Announcements : MonoBehaviour
    {
        private static Announcements instance;
        public static Announcements getInstance()
        {
            return instance;
        }

        public float announcementDuration = 5.0f;
        public float defaultDuration = 5.0f;

        private List<string> announcementLog;
        private Text anncmntText;
        private bool allowAnnouncements = true;
        private IEnumerator ancmnt;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            anncmntText = GetComponentInChildren<Text>();
            if (anncmntText == null)
            anncmntText.enabled = false;
            announcementLog = new List<string>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Add an announcement to the announcement log. This will not display the announcement on screen. 
        public void WriteNewAnnoucement(string announcement)
        {
            if (!allowAnnouncements) return;
            announcementLog.Add(announcement);
        }

        // Display an announcement to the screen. If this announcement is not on the log, it will be added to the log
        public void DisplayAnnouncement(string announcement)
        {
            if (!allowAnnouncements) return;
            ancmnt = Announcment(announcement);
            StartCoroutine(ancmnt);
        }

        // Display an announcement to the screen for a fixed duration. If this announcement is not on the log, it will be added to the log
        public void DisplayAnnouncement(string announcement, float duration)
        {
            if (!allowAnnouncements) return;
            announcementDuration = duration;
            ancmnt = Announcment(announcement);
            StartCoroutine(ancmnt);
        }

        // Display an announcement from the announcementLog at location index to the screen.
        public void DisplayAnnouncement(int index)
        {
            if (!allowAnnouncements) return;
            ancmnt = Announcment(announcementLog[index]);
            StartCoroutine(ancmnt);
        }

        public void disableAnnouncements()
        {
            allowAnnouncements = false;
        }

        public void enableAnnouncements()
        {
            allowAnnouncements = true;
        }

        IEnumerator Announcment(string announcement)
        {
            anncmntText.enabled = true;
            anncmntText.text = announcement;
            if (!announcementLog.Contains(announcement))
                announcementLog.Add(announcement);
            yield return new WaitForSeconds(announcementDuration);
            anncmntText.enabled = false;
            StopCoroutine(ancmnt);
        }
    }
}
