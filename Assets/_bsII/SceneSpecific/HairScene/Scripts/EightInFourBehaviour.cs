using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HairScene
{
    public class EightInFourBehaviour : MonoBehaviour
    {
        public float FinalZdistance = 1.14f;
        public float StepSize = 0.1f;

        private float distanceTraveled = 0f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(MoveEightInFour());
        }

        private IEnumerator MoveEightInFour()
        {
            while (distanceTraveled < FinalZdistance)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + StepSize, transform.position.z);
                distanceTraveled += StepSize;
                yield return null;
            }

            Destroy(this.gameObject);
        }
    }
}

