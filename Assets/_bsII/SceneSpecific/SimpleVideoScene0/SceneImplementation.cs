using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

namespace SimpleVideoScene
{
    public class SceneImplementation : MonoBehaviour
    {
        [Tooltip("persistentDataPath/videos/?")]
        [SerializeField]
        private string _videoFolderName;

        [SerializeField]
        private VideoPlayer videoPlayer;
        private string[] videoFilePaths;
        private long[] videoFramePositions;
        private int currentVideoIndex;

        [SerializeField]
        private Volume _sceneVolume;
        private SceneColorOverlayPostProcess _sceneColorOverlayPostProcessVolume;

        private UserInputsModel _userInputsModel;
        private MusicInputsModel _musicInputsModel;

        // Start is called before the first frame update
        void Start()
        {
            if (!_sceneVolume.sharedProfile.TryGet(out _sceneColorOverlayPostProcessVolume))
            {
                throw new NullReferenceException(nameof(_sceneColorOverlayPostProcessVolume));
            }


            _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
            _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();

            PrepareVideos();

            SubscribeUserInputs();

        }

        private void OnDisable()
        {
            UnsubscribeUserInputs();
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void PrepareVideos()
        {
            string folderPath = Application.persistentDataPath + "/" + "videos" + "/" + _videoFolderName;
            videoFilePaths = Directory.GetFiles(folderPath);

            videoFramePositions = new long[videoFilePaths.Length];

            videoPlayer.url = videoFilePaths[currentVideoIndex];
            videoPlayer.Play();
        }


        public void UnsubscribeUserInputs()
        {
            foreach (var key in _userInputsModel.MelodyKeys.Keys)
            {
                key.EmitTurnedOnOrOffEvent -= HandleMelodyKey;
            }

            foreach (var key in _userInputsModel.MoodKeys.Keys)
            {
                key.EmitCollectionKeyTriggeredEvent -= HandleMoodKey;
            }
        }

        public void SubscribeUserInputs()
        {

            foreach (var key in _userInputsModel.MelodyKeys.Keys)
            {
                key.EmitTurnedOnOrOffEvent += HandleMelodyKey;
            }

            foreach (var key in _userInputsModel.MoodKeys.Keys)
            {
                key.EmitCollectionKeyTriggeredEvent += HandleMoodKey;
            }
        }

        private void HandleMoodKey(int index)
        {
            videoFramePositions[currentVideoIndex] = videoPlayer.frame;
            videoPlayer.Stop();


            int newIndex = index;
            if (index >= videoFilePaths.Length)
            {
                newIndex = UnityEngine.Random.Range(0, videoFilePaths.Length);
            }
            currentVideoIndex = newIndex;
            videoPlayer.url = videoFilePaths[currentVideoIndex];
            videoPlayer.frame = videoFramePositions[currentVideoIndex];
            videoPlayer.Play();
        }

        private void HandleMelodyKey(bool hasTurnedOn, int index)
        {
            if (!hasTurnedOn)
            {
                return;
            }
            if (index == 0)
            {
                _sceneColorOverlayPostProcessVolume.intensity.value = 0;
            }
            else
            {
                _sceneColorOverlayPostProcessVolume.intensity.value = 1;
                _sceneColorOverlayPostProcessVolume.color.value = Color.HSVToRGB((index - 1) * 0.14285f, 1, 1);
            }
        }
    }
}