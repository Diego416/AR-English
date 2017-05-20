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
		public string audioName;
		public bool question;

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

		public void SetAudioName(string name)
		{
			audioName = name;
		}

		public void SetQuestionBool(bool q)
		{
			question = q;
		}

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
			Canvas[] canvasObjects = GetComponentsInChildren<Canvas>();
			AudioSource[] audioObjects = GetComponentsInChildren<AudioSource>();
			UnityEngine.UI.Toggle[] checks = GetComponentsInChildren<UnityEngine.UI.Toggle>();

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
				if (audio.name == audioName)
					audio.Play ();
			}

			foreach(UnityEngine.UI.Toggle check in checks)
			{
				check.isOn = false;
			}

			if (question) {
				System.Random rnd = new System.Random();
				int pregunta = rnd.Next(0, 5);

				string[] preguntas = {"Select the correct option", "Select the true option", "What can horses do?", "Whales...", "What do elephants eat?"};
				string[] respuestas1 = {"Butterflies is a beautiful insect", "Tigers are very slow", "They can gallop", "are flying animals", "Animals"};
				string[] respuestas2 = {"Butterflies is not a beautiful insect", "Tigers cannot run", "They can trot", "are domesticated animals", "Humans"};
				string[] respuestas3 = {"Butterflies are a beautiful insect", "Tigers are very fast", "Both are correct", "are aquatic animals", "Plants"};

				GameObject goP = GameObject.Find("txtQuestion");
				TextMesh txtPregunta = (TextMesh) goP.GetComponent(typeof(TextMesh));
				txtPregunta.text = preguntas[pregunta];

				GameObject goR1 = GameObject.Find("txtA1");
				TextMesh txtRespuesta1 = (TextMesh) goR1.GetComponent(typeof(TextMesh));
				txtRespuesta1.text = respuestas1[pregunta];

				GameObject goR2 = GameObject.Find("txtA2");
				TextMesh txtRespuesta2 = (TextMesh) goR2.GetComponent(typeof(TextMesh));
				txtRespuesta2.text = respuestas2[pregunta];

				GameObject goR3 = GameObject.Find("txtA3");
				TextMesh txtRespuesta3 = (TextMesh) goR3.GetComponent(typeof(TextMesh));
				txtRespuesta3.text = respuestas3[pregunta];

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
			UnityEngine.UI.Toggle[] checks = GetComponentsInChildren<UnityEngine.UI.Toggle>();

			GameObject go = GameObject.Find("MicWidget");
			MicrophoneWidget mic = (MicrophoneWidget) go.GetComponent(typeof(MicrophoneWidget));
			mic.DeactivateMicrophone();


			foreach(UnityEngine.UI.Toggle check in checks)
			{
				check.isOn = false;
			}

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
				if (audio.name == audioName)
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
