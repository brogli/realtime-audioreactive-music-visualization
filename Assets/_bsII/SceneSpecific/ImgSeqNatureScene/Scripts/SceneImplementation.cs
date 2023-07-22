using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

namespace ImgSeqNatureScene
{
    public class SceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
    {
        [Tooltip("persistentDataPath/image-sequences/?")]
        [SerializeField]
        private string _imgSeqFolderName;
        [Tooltip("persistentDataPath/videos/?")]
        [SerializeField]
        private string _videoFolderName;

        [SerializeField]
        private GameObject _leftFourInFourElement;
        [SerializeField]
        private GameObject _rightFourInFourElement;

        [SerializeField]
        private GameObject _eightInFourElement0;
        [SerializeField]
        private GameObject _eightInFourElement1;
        [SerializeField]
        private GameObject _eightInFourElement2;
        [SerializeField]
        private GameObject _eightInFourElement3;

        private List<ImageSequence> _fourInFourSequences = new();
        private int _fourInFourRightSequenceIndex = 0;
        private int _fourInFourLeftSequenceIndex = 0;

        private Material _fourInFourLeftMaterial;
        private Material _fourInFourRightMaterial;


        private List<ImageSequence> _eightInFourSequences = new();
        private Material _eightInFourMaterial0;
        private Material _eightInFourMaterial1;
        private Material _eightInFourMaterial2;
        private Material _eightInFourMaterial3;

        private int _eightInFourSequenceIndex0 = 0;
        private int _eightInFourSequenceIndex1 = 0;
        private int _eightInFourSequenceIndex2 = 0;
        private int _eightInFourSequenceIndex3 = 0;


        [SerializeField]
        private Volume _sceneVolume;
        private SceneColorOverlayPostProcess _sceneColorOverlayPostProcessVolume;

        [SerializeField]
        private VideoPlayer videoPlayer;
        private string[] videoFilePaths;
        private long[] videoFramePositions;
        private int currentVideoIndex;


        private UserInputsModel _userInputsModel;
        private MusicInputsModel _musicInputsModel;

        // Start is called before the first frame update
        void Start()
        {
            _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
            _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
            LoadImgSequences();

            _fourInFourLeftMaterial = _leftFourInFourElement.GetComponent<Renderer>().sharedMaterial;
            _fourInFourRightMaterial = _rightFourInFourElement.GetComponent<Renderer>().sharedMaterial;

            //_eightInFourMaterial0 = _eightInFourElement0.GetComponent<Renderer>().sharedMaterial;
            //_eightInFourMaterial1 = _eightInFourElement1.GetComponent<Renderer>().sharedMaterial;
            //_eightInFourMaterial2 = _eightInFourElement2.GetComponent<Renderer>().sharedMaterial;
            //_eightInFourMaterial3 = _eightInFourElement3.GetComponent<Renderer>().sharedMaterial;

            if (!_sceneVolume.sharedProfile.TryGet(out _sceneColorOverlayPostProcessVolume))
            {
                throw new NullReferenceException(nameof(_sceneColorOverlayPostProcessVolume));
            }

            PrepareVideos();

            SubscribeMusicInputs();
            SubscribeUserInputs();
        }

        private void PrepareVideos()
        {
            string folderPath = Application.persistentDataPath + "/" + "videos" + "/" + _videoFolderName;
            videoFilePaths = Directory.GetFiles(folderPath);

            videoFramePositions = new long[videoFilePaths.Length];

            videoPlayer.url = videoFilePaths[currentVideoIndex];
            videoPlayer.Play();
        }

        // Update is called once per frame
        void Update()
        {
            if (_userInputsModel.FourInFourUserInput.IsPressed)
            {
                if (_fourInFourLeftSequenceIndex < _fourInFourSequences.Count && _fourInFourSequences[_fourInFourLeftSequenceIndex] != null)
                {
                    var imageSequence = _fourInFourSequences[_fourInFourLeftSequenceIndex];
                    var image = imageSequence.GetImageAtNormalizedIndex(_musicInputsModel.TwoInFourValue);
                    _fourInFourLeftMaterial.mainTexture = image;
                }
                if (_fourInFourRightSequenceIndex < _fourInFourSequences.Count && _fourInFourSequences[_fourInFourRightSequenceIndex] != null)
                {
                    var imageSequence = _fourInFourSequences[_fourInFourRightSequenceIndex];
                    var image = imageSequence.GetImageAtNormalizedIndex(BsIImath.AcutalModulo(_musicInputsModel.TwoInFourValue + 0.5f, 1));
                    _fourInFourRightMaterial.mainTexture = image;
                }
            }


            //if (_userInputsModel.EightInFourUserInput.IsPressed)
            //{
            //    if (_eightInFourSequenceIndex0 < _eightInFourSequences.Count && _eightInFourSequences[_eightInFourSequenceIndex0] != null)
            //    {
            //        var imageSequence = _eightInFourSequences[_eightInFourSequenceIndex0];
            //        var image = imageSequence.GetImageAtNormalizedIndex(_musicInputsModel.OneInFourValue);
            //        _eightInFourMaterial0.mainTexture = image;
            //    }
            //    if (_eightInFourSequenceIndex1 < _eightInFourSequences.Count && _eightInFourSequences[_eightInFourSequenceIndex1] != null)
            //    {
            //        var imageSequence = _eightInFourSequences[_eightInFourSequenceIndex1];
            //        var image = imageSequence.GetImageAtNormalizedIndex(BsIImath.AcutalModulo(_musicInputsModel.OneInFourValue + 0.25f, 1));
            //        _eightInFourMaterial1.mainTexture = image;
            //    }
            //    if (_eightInFourSequenceIndex2 < _eightInFourSequences.Count && _eightInFourSequences[_eightInFourSequenceIndex2] != null)
            //    {
            //        var imageSequence = _eightInFourSequences[_eightInFourSequenceIndex2];
            //        var image = imageSequence.GetImageAtNormalizedIndex(BsIImath.AcutalModulo(_musicInputsModel.OneInFourValue + 0.5f, 1));
            //        _eightInFourMaterial2.mainTexture = image;
            //    }
            //    if (_eightInFourSequenceIndex3 < _eightInFourSequences.Count && _eightInFourSequences[_eightInFourSequenceIndex3] != null)
            //    {
            //        var imageSequence = _eightInFourSequences[_eightInFourSequenceIndex3];
            //        var image = imageSequence.GetImageAtNormalizedIndex(BsIImath.AcutalModulo(_musicInputsModel.OneInFourValue + 0.75f, 1)); 
            //        _eightInFourMaterial3.mainTexture = image;
            //    }
            //}
        }

        void OnDisable()
        {
            UnsubscribeMusicInputs();
            UnsubscribeUserInputs();
        }



        private void LoadImgSequences()
        {
            string rootFolderPath = Application.persistentDataPath + "/" + "image-sequences" + "/" + _imgSeqFolderName;

            string[] subdirectories = Directory.GetDirectories(rootFolderPath);

            foreach (string subdir in subdirectories)
            {
                if (subdir.ToUpper().EndsWith("fourInFour".ToUpper()))
                {
                    Debug.Log("found four in four directory");
                    string[] fourInFourSubdirs = Directory.GetDirectories(subdir);
                    GetImageSequences(fourInFourSubdirs, _fourInFourSequences);
                }
                if (subdir.ToUpper().EndsWith("eightInFour".ToUpper()))
                {
                    //Debug.Log("eight four in four directory");
                    //string[] eightInFourSubdirs = Directory.GetDirectories(subdir);
                    //GetImageSequences(eightInFourSubdirs, _eightInFourSequences);
                }
            }
        }

        private async void GetImageSequences(string[] fourInFourSubdirs, List<ImageSequence> imageSequenceResult)
        {
            var loader = new ImageSequenceLoader();
            foreach (var dir in fourInFourSubdirs)
            {
                imageSequenceResult.Add(await loader.LoadImageSequenceInFolder(dir));
            }
        }

        #region userinputs

        public void UnsubscribeUserInputs()
        {
            _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= HandleFourInFourUserInput;
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
            _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent += HandleFourInFourUserInput;
            HandleFourInFourUserInput(_userInputsModel.FourInFourUserInput.IsPressed);
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

        private void HandleFourInFourUserInput(bool hasTurnedOn)
        {
            _leftFourInFourElement.SetActive(hasTurnedOn);
            _rightFourInFourElement.SetActive(hasTurnedOn);
            if (hasTurnedOn)
            {
                videoPlayer.Pause();
            } else
            {
                videoPlayer.Play();
            }
        }
        #endregion

        #region music inputs
        public void UnsubscribeMusicInputs()
        {
            _musicInputsModel.EmitOneInFourEvent -= HandleOneInFourMusicInput;
        }

        public void SubscribeMusicInputs()
        {
            _musicInputsModel.EmitOneInFourEvent += HandleOneInFourMusicInput;
        }

        private void HandleOneInFourMusicInput()
        {
            if (_userInputsModel.OneInFourUserInput.IsPressed)
            {
                _fourInFourLeftSequenceIndex = UnityEngine.Random.Range(0, _fourInFourSequences.Count);
                _fourInFourRightSequenceIndex = UnityEngine.Random.Range(0, _fourInFourSequences.Count);
            }
        }

        #endregion
    }
}

