using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElevation : MonoBehaviour
{
    private float _amplitude;
    private float _frequency;
    private float _phase;
    private float _speed;
    private float _amplitude2;
    private float _frequency2;
    private float _phase2;
    private float _speed2;
    [SerializeField] private Material waterMaterial;
    //debug
    public bool DrawGizmos;
    [Range(0.0f, 1000.0f)]
    public float WaterSize;
    [Range(1f, 10.0f)]
    public float DistanceBetweenPoints;

    private void OnValidate()
    {
        _amplitude = waterMaterial.GetFloat("_Amplitude");
        _frequency = waterMaterial.GetFloat("_Frequency");
        _speed = waterMaterial.GetFloat("_speed");
        _amplitude2 = waterMaterial.GetFloat("_Amplitude2");
        _frequency2 = waterMaterial.GetFloat("_Frequency2");
        _speed2 = waterMaterial.GetFloat("_speed2");
    }
    private void Update()
    {
        _amplitude = waterMaterial.GetFloat("_Amplitude");
        _frequency = waterMaterial.GetFloat("_Frequency");
        _speed = waterMaterial.GetFloat("_speed");
        _amplitude2 = waterMaterial.GetFloat("_Amplitude2");
        _frequency2 = waterMaterial.GetFloat("_Frequency2");
        _speed2 = waterMaterial.GetFloat("_speed2");
        waterMaterial.SetFloat("_CurrentTime", Time.time);
    }
    public float GetElevation(float xPos, float zPos)
    {
        float height = _amplitude * Mathf.Sin((Time.time * _speed + xPos) * _frequency + _phase)+_amplitude2* Mathf.Cos((Time.time * _speed2 + zPos) * _frequency2 + _phase2);
        return height;
    }
    //need to calculate second wave
    public Vector3 GetNormal(float xPos, float zPos)
    {
        // Calculate the partial derivatives
        float df_dx = _amplitude * _frequency * Mathf.Cos((Time.time * _speed + xPos) * _frequency + _phase);
        float df_dz = 0;  // Assuming there is no dependence on z in your elevation function

        // Create and normalize the normal vector
        Vector3 normal = new Vector3(-df_dx, 1.0f, -df_dz);

        return normal;
    }
    private void OnDrawGizmos()
    {
        waterMaterial.SetFloat("_CurrentTime", Time.time);
        Gizmos.color = Color.blue;
        if(DrawGizmos)
        {
            float numberOfPoints = WaterSize / DistanceBetweenPoints;
            for (int i = 0; i < numberOfPoints; i++)
            {
                for (int j = 0; j < numberOfPoints; j++)
                {
                    Vector3 pos = transform.position + Vector3.right * i * DistanceBetweenPoints + Vector3.forward * j * DistanceBetweenPoints;
                    pos -= Vector3.right * WaterSize / 2f + Vector3.forward * WaterSize / 2f;
                    pos.y = _amplitude * Mathf.Sin((Time.time * _speed + pos.x) * _frequency + _phase);
                    Gizmos.DrawSphere(pos, 0.2f);
                }
            }
        }
        
    }
}
