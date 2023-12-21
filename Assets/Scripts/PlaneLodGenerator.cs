using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlaneLodGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Vector2 _offset;
    [SerializeField]
    private int _width, _depth;
    [SerializeField]
    [Tooltip("Number of vertex per unit value = 16 32 64 ...")]
    [Range(32, 512)]
    private float _numberOfVertex;

    private MeshFilter _filter;
    private Mesh _mesh;

    [SerializeField]
    private float _lod01Dist, _lod02Dist, _lod03Dist;
    [SerializeField]
    private float _lod01, _lod02, _lod03;

    [SerializeField]
    private bool _showDebug;
    [SerializeField]
    private bool _showTriangle;

    private void Start()
    {

    }
    private void FixedUpdate()
    {
        //transform.position = new Vector3(_player.transform.position.x - (transform.localScale.x/2), 0, transform.position.z - (transform.localScale.z / 2));
    }

    IEnumerator CoroutineTest()
    {
        yield return new WaitForSecondsRealtime(1);

        GeneratePlane();

        StartCoroutine(CoroutineTest());
    }
    public void GeneratePlane ()
    {
        Debug.Log("je genere");
        List<Vector3> verticesFullTemp = new List<Vector3>();

        List<Vector2> uvFullTemp = new List<Vector2>();

        List<int> trianglesMod2 = new List<int>();

        for (int x = 0; x <= _width * _numberOfVertex; x++)
        {
            for (int z = 0; z <= this._depth * _numberOfVertex; z++)
            {
                Vector3 toAdd = new Vector3(x / _numberOfVertex + _offset.x, 0, z / _numberOfVertex + _offset.y);

                if (x >= (((_width -  _lod01Dist) * _numberOfVertex) / 2) && z >= (((_width - _lod01Dist) * _numberOfVertex) / 2) 
                    && x <= (((_width +  _lod01Dist) * _numberOfVertex) / 2) && z <= (((_width +  _lod01Dist) * _numberOfVertex) / 2))
                {
                    verticesFullTemp.Add(toAdd);
                    uvFullTemp.Add(new Vector2(toAdd.x, toAdd.z));
                }
                else if ((x % _lod02 == 0 && z % _lod02 == 0) 
                    && x >= (((_width - _lod02Dist) * _numberOfVertex) / 2) && z >= (((_width -  _lod02Dist) * _numberOfVertex) / 2) 
                    && x <= (((_width +  _lod02Dist) * _numberOfVertex) / 2) && z <= (((_width + _lod02Dist) * _numberOfVertex) / 2))
                {
                    verticesFullTemp.Add(toAdd);
                    uvFullTemp.Add(new Vector2(toAdd.x, toAdd.z));
                }
                else if ((x % _lod03 == 0 && z % _lod03 == 0) 
                    && x >= (((_width - _lod03Dist) * _numberOfVertex) / 2) && z >= (((_width - _lod03Dist) * _numberOfVertex) / 2) 
                    && x <= (((_width + _lod03Dist) * _numberOfVertex) / 2) && z <= (((_width + _lod03Dist) * _numberOfVertex) / 2))
                {
                    verticesFullTemp.Add(toAdd);
                    uvFullTemp.Add(new Vector2(toAdd.x, toAdd.z));
                }
            }
        }

        for (int x = 0; x < _width * _numberOfVertex; x++)
        {
            for (int z = 0; z < _depth * _numberOfVertex; z++)
            {
                Vector3 toAdd = new Vector3(x / _numberOfVertex + _offset.x, 0, z / _numberOfVertex + _offset.y);

                if (x >= (((_width -  _lod01Dist) * _numberOfVertex) / 2) && z >= (((_width -  _lod01Dist) * _numberOfVertex) / 2) 
                    && x  < (((_width +  _lod01Dist) * _numberOfVertex) / 2) && z  < (((_width + _lod01Dist) * _numberOfVertex) / 2))
                {
                    Vector3 topR = toAdd + new Vector3((1 / _numberOfVertex), 0, (1 / _numberOfVertex));
                    Vector3 topL = toAdd + new Vector3((1 / _numberOfVertex) , 0, 0);
                    Vector3 botR = toAdd + new Vector3(0, 0, (1 / _numberOfVertex) );
                    Vector3 botL = toAdd;

                    trianglesMod2.Add(verticesFullTemp.IndexOf(botL));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(botR));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(topL));

                    trianglesMod2.Add(verticesFullTemp.IndexOf(topL));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(botR));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(topR));
                }
                else if ((x % _lod02 == 0 && z % _lod02 == 0) 
                    && x >= (((_width -  _lod02Dist) * _numberOfVertex) / 2) && z >= (((_width -  _lod02Dist) * _numberOfVertex) / 2) 
                    && x < (((_width +  _lod02Dist) * _numberOfVertex) / 2) && z < (((_width +  _lod02Dist) * _numberOfVertex) / 2))
                {
                    Vector3 topR = toAdd + new Vector3((1 / _numberOfVertex) * _lod02, 0, (1 / _numberOfVertex) * _lod02);
                    Vector3 topL = toAdd + new Vector3((1 / _numberOfVertex) * _lod02, 0, 0);
                    Vector3 botR = toAdd + new Vector3(0, 0, (1 / _numberOfVertex) * _lod02);
                    Vector3 botL = toAdd;


                    trianglesMod2.Add(verticesFullTemp.IndexOf(botL));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(botR));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(topL));

                    trianglesMod2.Add(verticesFullTemp.IndexOf(topL));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(botR));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(topR));
                }
                else if ((x % _lod03 == 0 && z % _lod03 == 0) 
                    && x >= (((_width -  _lod03Dist) * _numberOfVertex) / 2) && z >= (((_width -  _lod03Dist) * _numberOfVertex) / 2) 
                    && x <= (((_width +  _lod03Dist) * _numberOfVertex) / 2) && z <= (((_width + _lod03Dist) * _numberOfVertex) / 2))
                {
                    Vector3 topR = toAdd + new Vector3((1 / _numberOfVertex) * _lod03, 0, (1 / _numberOfVertex) * _lod03);
                    Vector3 topL = toAdd + new Vector3((1 / _numberOfVertex) * _lod03, 0, 0);
                    Vector3 botR = toAdd + new Vector3(0, 0, (1 / _numberOfVertex) * _lod03);
                    Vector3 botL = toAdd;


                    trianglesMod2.Add(verticesFullTemp.IndexOf(botL));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(botR));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(topL));

                    trianglesMod2.Add(verticesFullTemp.IndexOf(topL));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(botR));
                    trianglesMod2.Add(verticesFullTemp.IndexOf(topR));
                }
            }
        }

        _filter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _mesh.vertices = verticesFullTemp.ToArray();
        _mesh.uv = uvFullTemp.ToArray();
        _mesh.triangles = trianglesMod2.ToArray();
        _mesh.RecalculateNormals();
        _filter.mesh = _mesh;
      //  _filter.GetComponent<MeshRenderer>().
    }

    private void OnDrawGizmos()
    {
        if (!_showDebug) { return; }

        for (int x = 0; x <= _width * _numberOfVertex; x++)
        {
            for (int z = 0; z <= this._depth * _numberOfVertex; z++)
            {
                Vector3 toAdd = new Vector3(x / _numberOfVertex + _offset.x, 0, z / _numberOfVertex + _offset.y);


                if (x >= (((_width - _lod01Dist) * _numberOfVertex) / 2) && z >= (((_width - _lod01Dist) * _numberOfVertex) / 2) && x <= (((_width + _lod01Dist) * _numberOfVertex) / 2) && z <= (((_width + _lod01Dist) * _numberOfVertex) / 2))
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(toAdd, 0.09f);
                }
                else if ((x % 2 == 0 && z % 2 == 0) && x >= (((_width - _lod02Dist) * _numberOfVertex) / 2) && z >= (((_width -  _lod02Dist) * _numberOfVertex) / 2) && x <= (((_width + _lod02Dist) * _numberOfVertex) / 2) && z <= (((_width + _lod02Dist) * _numberOfVertex) / 2))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(toAdd, 0.09f);
                }
                else if ((x % 4 == 0 && z % 4 == 0) && x >= (((_width - _lod03Dist) * _numberOfVertex) / 2) && z >= (((_width - _lod03Dist) * _numberOfVertex) / 2) && x <= (((_width + _lod03Dist) * _numberOfVertex) / 2) && z <= (((_width + _lod03Dist) * _numberOfVertex) / 2))
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(toAdd, 0.1f);
                }
            }
        }

        if (!_showTriangle) { return; }
        for (int x = 0; x < _width * _numberOfVertex; x++)
        {
            for (int z = 0; z < _depth * _numberOfVertex; z++)
            {
                Vector3 toAdd = new Vector3(x / _numberOfVertex + _offset.x, 0, z / _numberOfVertex + _offset.y);

                if (x >= (((_width -_lod01Dist) * _numberOfVertex) / 2) && z >= (((_width -  _lod01Dist) * _numberOfVertex) / 2) && x+1 < (((_width + _lod01Dist) * _numberOfVertex) / 2) && z+1 < (((_width + _lod01Dist) * _numberOfVertex) / 2))
                {
                    Vector3 topR = toAdd + new Vector3((1 / _numberOfVertex) * 2, 0, (1 / _numberOfVertex) * 2);
                    Vector3 topL = toAdd + new Vector3((1 / _numberOfVertex) * 2, 0, 0);
                    Vector3 botR = toAdd + new Vector3(0, 0, (1 / _numberOfVertex) * 2);
                    Vector3 botL = toAdd;

                    Gizmos.color = Color.cyan;

                    Gizmos.DrawLine(botL, botR);
                    Gizmos.DrawLine(botR, topL);
                    Gizmos.DrawLine(topL, botL);

                    Gizmos.DrawLine(topL, botR);
                    Gizmos.DrawLine(botR, topR);
                    Gizmos.DrawLine(topR, topL);

                }
                else if ((x % 2 == 0 && z % 2 == 0) && x >= (((_width - _lod02Dist) * _numberOfVertex) / 2) && z >= (((_width - _lod02Dist) * _numberOfVertex) / 2) && x < (((_width + _lod02Dist) * _numberOfVertex) / 2) && z < (((_width + _lod02Dist) * _numberOfVertex) / 2))
                {
                    Vector3 topR = toAdd + new Vector3((1 / _numberOfVertex) * 2, 0, (1 / _numberOfVertex) * 2);
                    Vector3 topL = toAdd + new Vector3((1 / _numberOfVertex) * 2, 0, 0);
                    Vector3 botR = toAdd + new Vector3(0, 0, (1 / _numberOfVertex) * 2);
                    Vector3 botL = toAdd;

                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(botL, botR);
                    Gizmos.DrawLine(botR, topL);
                    Gizmos.DrawLine(topL, botL);

                    Gizmos.DrawLine(topL, botR);
                    Gizmos.DrawLine(botR, topR);
                    Gizmos.DrawLine(topR, topL);
                }
                else if ((x % 4 == 0 && z % 4 == 0) && x >= (((_width - _lod03Dist) * _numberOfVertex) / 2) && z >= (((_width - _lod03Dist) * _numberOfVertex) / 2) && x <= (((_width + _lod03Dist) * _numberOfVertex) / 2) && z <= (((_width + _lod03Dist) * _numberOfVertex) / 2))
                {
                    Vector3 topR = toAdd + new Vector3((1 / _numberOfVertex) * 4, 0, (1 / _numberOfVertex) * 4);
                    Vector3 topL = toAdd + new Vector3((1 / _numberOfVertex) * 4, 0, 0);
                    Vector3 botR = toAdd + new Vector3(0, 0, (1 / _numberOfVertex) * 4);
                    Vector3 botL = toAdd;

                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(botL, botR);
                    Gizmos.DrawLine(botR, topL);
                    Gizmos.DrawLine(topL, botL);

                    Gizmos.DrawLine(topL, botR);
                    Gizmos.DrawLine(botR, topR);
                    Gizmos.DrawLine(topR, topL);
                }
            }
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(PlaneLodGenerator))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlaneLodGenerator myTarget = (PlaneLodGenerator)target;

         
        if (GUILayout.Button("Generate Plane"))
        {
            myTarget.GeneratePlane();
        }
    }
}
#endif
