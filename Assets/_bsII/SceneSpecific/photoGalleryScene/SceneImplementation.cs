using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhotoGalleryScene
{
    public class SceneImplementation : MonoBehaviour
    {
        [SerializeField]
        private GameObject _leftQuad;
        [SerializeField]
        private GameObject _rightQuad;


        [SerializeField]
        private float _timeBetweenImagesInSeconds = 3.0f; // so 6 per image

        [Tooltip("persistentDataPath/image-sequences/?")]
        [SerializeField]
        private string _imgSeqDirectoryName;

        private ImageSequence _photoGallery;

        private Material _leftMaterial;
        private Material _rightMaterial;

        private int leftIndex = 0;
        private int rightIndex = 0;

        private float _iterationTimeInSeconds;

        private bool isLeftQuadsTurn = false;


        // Start is called before the first frame update
        void Start()
        {
            _leftMaterial = _leftQuad.GetComponent<Renderer>().material;
            _rightMaterial = _rightQuad.GetComponent<Renderer>().material;
            GetImageSequences(_imgSeqDirectoryName);
            SetImages(leftIndex, rightIndex);
        }

        // Update is called once per frame
        void Update()
        {

            _iterationTimeInSeconds += Time.deltaTime;

            if (_iterationTimeInSeconds > _timeBetweenImagesInSeconds)
            {
                _iterationTimeInSeconds = 0;
                HandleChange();
            }
        }

        private void HandleChange()
        {
            if (_photoGallery != null)
            {
                if (isLeftQuadsTurn)
                {
                    leftIndex = UnityEngine.Random.Range(0, _photoGallery.Length());
                }
                else
                {
                    rightIndex = UnityEngine.Random.Range(0, _photoGallery.Length());
                }
                isLeftQuadsTurn =!isLeftQuadsTurn;
                SetImages(leftIndex, rightIndex);
            }
        }

        private void SetImages(int leftIndex, int rightIndex)
        {
            if (_photoGallery != null && _photoGallery.Length() > leftIndex && _photoGallery.Length() > rightIndex)
            {
                _leftMaterial.mainTexture = _photoGallery.GetImageAtIndex(leftIndex);
                _rightMaterial.mainTexture = _photoGallery.GetImageAtIndex(rightIndex);
            }
        }
        private async void GetImageSequences(string directoryPath)
        {
            var loader = new ImageSequenceLoader();
            _photoGallery = (await loader.LoadImageSequenceInFolder(Application.persistentDataPath + "/image-sequences/" + directoryPath));

        }
    }
}