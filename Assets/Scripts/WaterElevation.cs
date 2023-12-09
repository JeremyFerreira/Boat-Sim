using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct WaveParameters
{
    public float Amplitude;
    public float TimeScale;
    public Vector3 Direction;
}
public class WaterElevation : MonoBehaviour
{
    private WaveParameters _wave1;
    private WaveParameters _wave2;
    private WaveParameters _wave3;
    private WaveParameters _wave4;
    private WaveParameters _wave5;
    private WaveParameters _wave6;
    private WaveParameters _wave7;
    private WaveParameters _wave8;
    private float _gravity;
    private float _phase;
    private float _depth;
    private float _amplitude;
    private float _frequency;
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
        SetParameters();
    }
    private void SetParameters()
    {
        _phase = waterMaterial.GetFloat("_Phase");
        _depth = waterMaterial.GetFloat("_Depth");
        _gravity = waterMaterial.GetFloat("_Gravity");

        //wave1
        _wave1.Amplitude = waterMaterial.GetFloat("_Amplitude1");
        _wave1.Direction = waterMaterial.GetVector("_Direction1");
        _wave1.TimeScale = waterMaterial.GetFloat("_TimeScale1");
        //wave2
        _wave2.Amplitude = waterMaterial.GetFloat("_Amplitude1_1");
        _wave2.Direction = waterMaterial.GetVector("_Direction1_1");
        _wave2.TimeScale = waterMaterial.GetFloat("_TimeScale1_1");
        //wave3
        _wave3.Amplitude = waterMaterial.GetFloat("_Amplitude1_2");
        _wave3.Direction = waterMaterial.GetVector("_Direction1_2");
        _wave3.TimeScale = waterMaterial.GetFloat("_TimeScale1_2");
        //wav4
        _wave4.Amplitude = waterMaterial.GetFloat("_Amplitude1_3");
        _wave4.Direction = waterMaterial.GetVector("_Direction1_3");
        _wave4.TimeScale = waterMaterial.GetFloat("_TimeScale1_3");
        //wave5
        _wave5.Amplitude = waterMaterial.GetFloat("_Amplitude1_4");
        _wave5.Direction = waterMaterial.GetVector("_Direction1_4");
        _wave5.TimeScale = waterMaterial.GetFloat("_TimeScale1_4");
        //wave6
        _wave6.Amplitude = waterMaterial.GetFloat("_Amplitude1_5");
        _wave6.Direction = waterMaterial.GetVector("_Direction1_5");
        _wave6.TimeScale = waterMaterial.GetFloat("_TimeScale1_5");
        //wave7
        _wave7.Amplitude = waterMaterial.GetFloat("_Amplitude1_6");
        _wave7.Direction = waterMaterial.GetVector("_Direction1_6");
        _wave7.TimeScale = waterMaterial.GetFloat("_TimeScale1_6");
        //wave8
        _wave8.Amplitude = waterMaterial.GetFloat("_Amplitude1_7");
        _wave8.Direction = waterMaterial.GetVector("_Direction1_7");
        _wave8.TimeScale = waterMaterial.GetFloat("_TimeScale1_7");
    }
    private void Update()
    {
        waterMaterial.SetFloat("_CurrentTime", Time.time);
    }
    public float GetElevation(float xPos, float zPos)
    {
        Vector3 position = new Vector3(xPos,0, zPos);

        Vector3 sampleWave1 = SamplePosition(position, 5, _wave1);
        Vector3 sampleWave2 = SamplePosition(position, 5, _wave2);
        Vector3 sampleWave3 = SamplePosition(position, 5, _wave3);
        Vector3 sampleWave4 = SamplePosition(position, 5, _wave4);
        Vector3 sampleWave5 = SamplePosition(position, 5, _wave5);
        Vector3 sampleWave6 = SamplePosition(position, 5, _wave6);
        Vector3 sampleWave7 = SamplePosition(position, 5, _wave7);
        Vector3 sampleWave8 = SamplePosition(position, 5, _wave8);
        
        return sampleWave1.y + sampleWave2.y + sampleWave3.y + sampleWave4.y + sampleWave5.y + sampleWave6.y + sampleWave7.y + sampleWave8.y;
    }
    Vector3 SamplePosition(Vector3 position, int samples, WaveParameters wave)
    {
        Vector3 previousSample = GerstnerWave(position, wave);
        Vector3 sample = Vector3.zero;
        for (int i=0; i < samples-1; i++)
        {
            sample = GerstnerWave(position - previousSample, wave);
            previousSample = sample;
        }
        return sample;
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
    private Vector3 GerstnerWave(Vector3 position, WaveParameters wave)
    {
        float waveDirectionLength = wave.Direction.magnitude;
        float theta = Theta(position,_phase,Time.time * wave.TimeScale,_gravity,_depth,wave.Direction);
        float xComponent = -1 * Mathf.Sin(theta) * (wave.Direction.x / waveDirectionLength) * (wave.Amplitude / MathExtension.HyperbolicTangent(waveDirectionLength * _depth));
        float yComponent = Mathf.Cos(theta) * wave.Amplitude;
        float zComponent = -1 * Mathf.Sin(theta) * (wave.Direction.z / waveDirectionLength) * (wave.Amplitude / MathExtension.HyperbolicTangent(waveDirectionLength * _depth));
        return new Vector3(xComponent, yComponent, zComponent);
    }
    private float Theta(Vector3 position, float phase, float time, float gravity, float depth, Vector3 direction)
    {
        return  ((direction.x * position.x + direction.z * position.z) - Frequency(gravity, depth, direction) * time) - phase;
    }
    private float Frequency(float gravity, float depth, Vector3 direction)
    {
        float directionLength = direction.magnitude;
        return Mathf.Sqrt(MathExtension.HyperbolicTangent(directionLength*depth) * gravity * directionLength);
    }
    private void OnDrawGizmos()
    {
        SetParameters();
        waterMaterial.SetFloat("_CurrentTime", Time.time);
        Gizmos.color = Color.red;
        if(DrawGizmos)
        {
            float numberOfPoints = WaterSize / DistanceBetweenPoints;
            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                for (int j = 0; j < numberOfPoints - 1; j++)
                {
                    Vector3 pos = transform.position + Vector3.right * i * DistanceBetweenPoints + Vector3.forward * j * DistanceBetweenPoints;
                    Vector3 pos2 = transform.position + Vector3.right * (i+1) * DistanceBetweenPoints + Vector3.forward * (j+1) * DistanceBetweenPoints;
                    pos.y = GetElevation(pos.x,pos.z);
                    pos2.y = GetElevation(pos2.x,pos2.z);
                    Gizmos.DrawCube(pos, Vector3.one* 0.2f);
                    Gizmos.DrawLine(pos, pos2);
                }
            }
        }
        
    }
}
