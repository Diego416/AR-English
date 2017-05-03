/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using IBM.Watson.DeveloperCloud.Widgets;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES


        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
			Canvas[] canvasObjects = GetComponentsInChildren<Canvas>();
			AudioSource[] audioObjects = GetComponentsInChildren<AudioSource>();

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

			foreach(Canvas canvas in canvasObjects)
			{
				canvas.enabled = true;
			}

			foreach(AudioSource audio in audioObjects)
			{
				if (audio.name == "AudioTigre")
					audio.Play ();
			}

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
			Canvas[] canvasObjects = GetComponentsInChildren<Canvas>();
			AudioSource[] audioObjects = GetComponentsInChildren<AudioSource>();
			TextMesh[] texts = GetComponentsInChildren<TextMesh>();

			GameObject go = GameObject.Find("MicWidget");
			MicrophoneWidget mic = (MicrophoneWidget) go.GetComponent(typeof(MicrophoneWidget));
			mic.DeactivateMicrophone();

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
				component.enabled = false;
			}

			// Disable canvas
			foreach(Canvas canvas in canvasObjects)
			{
				canvas.enabled = false;
			}

			// Disable audios
			foreach(AudioSource audio in audioObjects)
			{
				if (audio.name == "AudioTigre")
					audio.Pause ();
			}

			foreach(TextMesh text in texts)
			{
				if (text.name == "txtSpeak")
					text.text = "";
			}
	
			/*GameObject btnNoMic = new GameObject();

			GameObject _spawnedObj;

			_spawnedObj =  Instantiate(btnNoMic) as GameObject;


			//and somewehre in code ...

			_spawnedObj.SendMessage("OnClick", SendMessageOptions.RequireReceiver);*/

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
