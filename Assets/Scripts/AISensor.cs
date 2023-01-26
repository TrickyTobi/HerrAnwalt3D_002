using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class AISensor : MonoBehaviour
{

    public float _distanceFront;
    public float _distanceBack;
    public float _angle;
    float _angleBack;
    public float _heightBody;
    public float _heightEnd;
    public Color _meshColor = Color.red;
    public Color _meshBackColor = Color.blue;
    Mesh _mesh;
    Mesh _meshBack;

    public int _scanFrequency;
    public LayerMask _scanLayer;
    public LayerMask _occlusionLayers;

    Collider[] _colliders = new Collider[50];
    int _count;
    float _scanInterval;
    float _scanTimer;

    bool _inSight; public bool InSight { get => _inSight; set => _inSight = value; }



    void Start()
    {
        _angleBack = 180f - _angle;
        _scanInterval = 1f / _scanFrequency;
    }

    void Update()
    {
        _scanTimer -= Time.deltaTime;

        if (_scanTimer < 0)
        {
            _scanTimer += _scanInterval;
            ScanForPlayer();
        }
    }

    void ScanForPlayer()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position, _distanceFront, _colliders, _scanLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < _count; i++)
        {
            GameObject obj = _colliders[i].gameObject;

            if (IsInSight(obj))
            {
                _inSight = true;
                return;
            }
        }
        _inSight = false;
    }

    bool IsInSight(GameObject obj)
    {

        if (!obj.CompareTag("Player"))
            return false;

        Vector3 teacherPosition = transform.position;
        Vector3 objectPosition = obj.transform.position;
        Vector3 direction = objectPosition - teacherPosition;

        if (direction.y < -1f || direction.y > _heightBody)
        {
            return false;
        }

        direction.y = 0;



        float deltaAngleForward = Vector3.Angle(direction, transform.forward);
        float deltaAngleBackward = Vector3.Angle(direction, -transform.forward);

        float distanceToObj = (transform.position - obj.transform.position).magnitude;

        teacherPosition.y += _heightBody / 2;
        objectPosition.y = teacherPosition.y;

        if (deltaAngleForward < _angle)
        {
            return true;
        }
        if (deltaAngleBackward < _angleBack && distanceToObj < _distanceBack)
        {
            return true;
        }

        return false;

    }

    Mesh CreateWedgeMeshFront()
    {
        Mesh mesh = new Mesh();
        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0f, -_angle, 0f) * Vector3.forward * _distanceFront;
        Vector3 bottomRight = Quaternion.Euler(0f, _angle, 0f) * Vector3.forward * _distanceFront;

        Vector3 topCenter = bottomCenter + Vector3.up * _heightBody;
        Vector3 topLeft = bottomLeft + Vector3.up * _heightEnd;
        Vector3 topRight = bottomRight + Vector3.up * _heightEnd;

        int vert = 0;
        #region LeftSide
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        #endregion

        #region RightSide
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        #endregion

        float currentAngle = -_angle;
        float deltaAngle = (_angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward * _distanceFront;
            bottomRight = Quaternion.Euler(0f, currentAngle + deltaAngle, 0f) * Vector3.forward * _distanceFront;

            topLeft = bottomLeft + Vector3.up * _heightEnd;
            topRight = bottomRight + Vector3.up * _heightEnd;

            #region FarSide

            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
            #endregion

            #region TopSide
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
            #endregion

            #region BottomSide
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            #endregion

            currentAngle += deltaAngle;
        }



        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    Mesh CreateWedgeMeshBack()
    {
        Mesh mesh = new Mesh();
        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0f, -_angleBack, 0f) * -Vector3.forward * _distanceBack;
        Vector3 bottomRight = Quaternion.Euler(0f, _angleBack, 0f) * -Vector3.forward * _distanceBack;

        Vector3 topCenter = bottomCenter + Vector3.up * _heightBody;
        Vector3 topLeft = bottomLeft + Vector3.up * _heightBody;
        Vector3 topRight = bottomRight + Vector3.up * _heightBody;

        int vert = 0;
        #region LeftSide
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        #endregion

        #region RightSide
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        #endregion



        float currentAngle = -_angleBack;
        float deltaAngle = (_angleBack * 2) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0f, currentAngle, 0f) * -Vector3.forward * _distanceBack;
            bottomRight = Quaternion.Euler(0f, currentAngle + deltaAngle, 0f) * -Vector3.forward * _distanceBack;

            topLeft = bottomLeft + Vector3.up * _heightBody;
            topRight = bottomRight + Vector3.up * _heightBody;

            #region FarSide

            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
            #endregion

            #region TopSide
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
            #endregion

            #region BottomSide
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            #endregion

            currentAngle += deltaAngle;
        }



        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }


    private void OnValidate()
    {
        _angleBack = 180f - _angle;
        _scanInterval = 1f / _scanFrequency;
        _mesh = CreateWedgeMeshFront();
        _meshBack = CreateWedgeMeshBack();
    }

    private void OnDrawGizmosSelected()
    {
        if (_mesh)
        {
            Gizmos.color = _meshColor;
            Gizmos.DrawWireMesh(_mesh, transform.position, transform.rotation);
        }

        if (_meshBack)
        {
            Gizmos.color = _meshBackColor;
            Gizmos.DrawWireMesh(_meshBack, transform.position, transform.rotation);
        }
    }
}
