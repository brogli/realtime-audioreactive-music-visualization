using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1OneInFourTriangle : MonoBehaviour
{
    public float _growthFactor = 0.5f;
    private Scene1ColorSwitcher _scene1ColorSwitcher;
    private float _startTime = 0f;
    private readonly float _maxDuration = 5f;
    private bool _shouldDie = false;
    // Start is called before the first frame update
    void Start()
    {
        _scene1ColorSwitcher = GameObject.FindGameObjectWithTag("SceneImplementation").GetComponent<Scene1ColorSwitcher>();
        _startTime = Time.timeSinceLevelLoad;
        StartCoroutine(GrowAndDie());
    }


    private IEnumerator GrowAndDie()
    {
        while (!_shouldDie)
        {
            transform.localScale = new Vector3(
                transform.localScale.x + _growthFactor * Time.deltaTime,
                transform.localScale.y + _growthFactor * Time.deltaTime,
                transform.localScale.z + _growthFactor * Time.deltaTime);

            var currentTargetColor = _scene1ColorSwitcher.OneInFourColor;
            var currentColor = GetComponent<Renderer>().material.GetColor("_BaseColor");
            var newAlpha = currentColor.a * .95f;
            GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha));
            GetComponent<Renderer>().material.SetColor("_EmissiveColor", new Color(currentTargetColor.r, currentTargetColor.g, currentTargetColor.b, 1));

            if (Time.timeSinceLevelLoad - _startTime >= _maxDuration || newAlpha <= 0.00001)
            {
                _shouldDie = true;
            }
            else
            {
                yield return null;
            }
        }
        Die();
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
