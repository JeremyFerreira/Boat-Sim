using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTDIM_01 : MonoBehaviour
{
    [SerializeField]
    private Transform _center;
    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private float radius;
    [SerializeField]
    private Vector3 posRef;
    [SerializeField]
    [Range(-180f, 180f)]
    private float longitude, minLongitude, maxLongitude;
    [SerializeField]
    [Range(-90f, 90f)]
    private float latitude, minLatitude, maxLatitude;

    /*
    Latitude : La latitude mesure la distance nord-sud à partir de l'équateur. 
    Elle est généralement mesurée en degrés, allant de -90° (pôle sud) à 90° (pôle nord). 
    L'équateur est à 0° de latitude.

    Longitude : La longitude mesure la distance est-ouest à partir du méridien de Greenwich. 
    Elle est également mesurée en degrés, allant de -180° à 180°. 
    Le méridien de Greenwich est à 0° de longitude.

    En Radiant
    Latitude : arcsin(z/r)
    Longitude : Aten2(y,x)

    RadToDeg
    (180/pi)
    */

    private void FindAngelsOfPointOnSphere (Vector3 position)
    {
        float latitude = Mathf.Asin(position.y / radius) * (180 / Mathf.PI);
        float longitude = Mathf.Atan2(position.z, position.x) * (180 / Mathf.PI);

     //   Debug.Log("La latitude est de : " + latitude + "\n" +
          //  "La longitude est de : " + longitude);
    }

    private Vector3 FindPointWithAngels (float latitude, float longitude)
    {
        latitude *= Mathf.Deg2Rad;
        longitude *= Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(latitude) * Mathf.Cos(longitude);
        float y = radius * Mathf.Sin(latitude);
        float z = radius * Mathf.Cos(latitude) * Mathf.Sin(longitude);

        return new Vector3(x, y, z);
    }

    public struct MinMax
    {
        public float min;
        public float max;
        
        public MinMax (float min, float max)
        {
            this.min = min;
            this.max = max; 
        }
    }

    private Vector3 FindPointWithAngels(float latitude, float longitude, MinMax minMaxLat, MinMax minMaxLong)
    {
        latitude = Mathf.Clamp(latitude, minMaxLat.min, minMaxLat.max);
        longitude = Mathf.Clamp(longitude, minMaxLong.min, minMaxLong.max);

        return FindPointWithAngels(latitude,longitude);
    }

    private void Update()
    {
        FindAngelsOfPointOnSphere(posRef);
        FindPointWithAngels(latitude, longitude);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_center.position + _offset, radius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_center.position + _offset + FindPointWithAngels(latitude, longitude), 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_center.position + _offset + FindPointWithAngels(latitude, longitude, new MinMax(minLatitude,maxLatitude), new MinMax(minLongitude, maxLongitude)), 0.5f);
    }
}
