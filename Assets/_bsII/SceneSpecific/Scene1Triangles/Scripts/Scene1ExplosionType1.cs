using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Scene1ExplosionType1 : MonoBehaviour
{

    public Transform LightsOn;
    public Transform Center;
    public float factorFactor = 1;
    public float otherFactor = 0;

    public float curveSteepness = 3;
    public float curveOffset = 1.5f;
    public float maxFactor = 16;
    public float randomFactor = 0;
    public float speedFactor = 1;

    public GameObject AllTriangles;
    private MeshRenderer[] allTriangleRenderers;


    // Update is called once per frame
    void Update()
    {
        if (allTriangleRenderers == null)
        {
            return;
        }

        CalculateMaterials();
        AnimateElements();
    }

    private void AnimateElements()
    {
        LightsOn.position = new Vector3(LightsOn.position.x, LightsOn.position.y, LightsOn.position.z + Time.deltaTime * speedFactor);
        if (LightsOn.localPosition.z > 10)
        {
            randomFactor += 0.001f;
        }
        if (LightsOn.localPosition.z > 50)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    private void CalculateMaterials()
    {
        float distanceLigthsOnToCenter = LightsOn.position.z - Center.position.z;
        foreach (var renderer in allTriangleRenderers)
        {
            float distanceToCenter = Vector3.Distance(Center.transform.position, renderer.transform.position);
            float factor = Mathf.Abs(distanceToCenter - distanceLigthsOnToCenter) * factorFactor;

            factor = 1 - ((factor / maxFactor) + Random.Range(-1, 1) * randomFactor);
            factor = Mathf.Clamp(factor, 0, 1);
            var currentColor = renderer.material.GetColor("_EmissiveColor");
            renderer.material.SetColor("_EmissiveColor", new Color(factor, factor, factor, factor));
            renderer.material.SetColor("_BaseColor", new Color(0, 0, 0, factor));
        }
    }

    private float CalculateCurve(float x)
    {
        if (x < 0)
        {
            return 0;
        }
        else if (x >= 1)
        {
            return 1;
        }

        return 0.5f * ((Mathf.Atan(curveSteepness * Mathf.Sin(Mathf.PI * 2f * x + Mathf.PI * curveOffset))) / (Mathf.Atan(curveSteepness))) + 0.5f;
    }

    public void Start()
    {
        FindAllTriangles();
    }

    private void FindAllTriangles()
    {
        allTriangleRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in allTriangleRenderers)
        {
            renderer.material.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        }
    }
}
