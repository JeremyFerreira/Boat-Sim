using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElevation : MonoBehaviour
{
    private float _amplitude;
    private float _frequency;
    private float _phase;
    private float _speed;
    [SerializeField] private Material waterMaterial;
    //debug
    [Range(0.0f, 1000.0f)]
    [SerializeField] public float WaterSize;
    [Range(1f, 10.0f)]
    [SerializeField] public float DistanceBetweenPoints;

    private void OnValidate()
    {
        _amplitude = waterMaterial.GetFloat("_Amplitude");
        _frequency = waterMaterial.GetFloat("_Frequency");
        _speed = waterMaterial.GetFloat("_speed");
    }
    private void Update()
    {
        _amplitude = waterMaterial.GetFloat("_Amplitude");
        _frequency = waterMaterial.GetFloat("_Frequency");
        _speed = waterMaterial.GetFloat("_speed");
        waterMaterial.SetFloat("_CurrentTime", Time.time);
    }
    public float GetElevation(float xPos, float zPos)
    {
        float height = _amplitude * Mathf.Sin((Time.time * _speed + xPos) * _frequency + _phase);
        return height;
    }
    public Vector3 GetNormal(float xPos, float zPos)
    {
        // Calculate the partial derivatives
        float df_dx = _amplitude * _frequency * Mathf.Cos((Time.time * _speed + xPos) * _frequency + _phase);
        float df_dz = 0;  // Assuming there is no dependence on z in your elevation function

        // Create and normalize the normal vector
        Vector3 normal = new Vector3(-df_dx, 1.0f, -df_dz);

        return normal;
    }
    private void OnDrawGizmosSelected()
    {
        waterMaterial.SetFloat("_CurrentTime", Time.time);
        Gizmos.color = Color.blue;
        float numberOfPoints = WaterSize / DistanceBetweenPoints;
        for (int i = 0; i < WaterSize / DistanceBetweenPoints; i++)
        {
            for (int j = 0; j < WaterSize / DistanceBetweenPoints; j++)
            {
                Vector3 pos = transform.position + Vector3.right * i * DistanceBetweenPoints + Vector3.forward * j * DistanceBetweenPoints;
                pos -= Vector3.right * WaterSize / 2f + Vector3.forward * WaterSize / 2f;
                pos.y = _amplitude * Mathf.Sin((Time.time * _speed + pos.x) * _frequency + _phase);
                Gizmos.DrawSphere(pos, 0.2f);
            }
        }
    }
}
