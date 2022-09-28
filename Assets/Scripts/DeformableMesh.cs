using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// https://gist.github.com/runewake2/a6ba641c9c9c51da62530d6176ee932e
// https://www.youtube.com/watch?v=l_2uGpjBMl4
// Mesh Deforming script. Modified from original to work with colliders and to collapse inward instead of down.
[RequireComponent(typeof(MeshFilter))]
public class DeformableMesh : MonoBehaviour
{

    [SerializeField] private Collider spoon;
    
    public float maximumDepression;
    public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;
    public Vector3 meshMassCenter;

    private MeshFilter matzhoBall;

    private void Awake()
    {
        MeshRegenerated();
    }

    private void Start()
    {
        AddDepression(spoon);
    }

    // Resets the mesh to the original.
    public void MeshRegenerated()
    {
        matzhoBall = GetComponent<MeshFilter>();
        matzhoBall.mesh.MarkDynamic();
        originalVertices = matzhoBall.mesh.vertices.ToList();
        modifiedVertices = matzhoBall.mesh.vertices.ToList();
        Debug.Log("Mesh Regenerated");
    }

    public bool AddDepression(Collider cutOut)
    {
        int hitCount = 0;
        for (int i = 0; i < modifiedVertices.Count; ++i)
        {
            var transformPoint = transform.TransformPoint(modifiedVertices[i]);
            if (cutOut.bounds.Contains(transformPoint))
            {
                hitCount++;
                var inwardRay = (transform.TransformPoint(matzhoBall.mesh.bounds.center) - transformPoint);
                var intersectRay = inwardRay * 10f;
                var intersectPoint = transformPoint + intersectRay; 
                //Debug.DrawRay(intersectPoint, -intersectRay, Color.red, 20f);
                RaycastHit hitInfo = new RaycastHit();
                var isHit = cutOut.Raycast(new Ray(intersectPoint, -intersectRay), out hitInfo, intersectRay.magnitude);
                if(isHit) modifiedVertices[i] = transform.InverseTransformPoint(hitInfo.point);
            }
        }
        matzhoBall.mesh.SetVertices(modifiedVertices);
        Debug.Log("Mesh Depressed");
        return hitCount >= modifiedVertices.Count;
    }
    
    public void AddDepression(Vector3 depressionPoint, float radius)
    {
        var worldPos4 = transform.worldToLocalMatrix * depressionPoint;
        var worldPos = new Vector3(worldPos4.x, worldPos4.y, worldPos4.z);
        for (int i = 0; i < modifiedVertices.Count; ++i)
        {
            var distance = (worldPos - (modifiedVertices[i] + Vector3.down * maximumDepression)).magnitude;
            if (distance < radius)
            {
                var newVert = originalVertices[i] + Vector3.down * maximumDepression;
                modifiedVertices.RemoveAt(i);
                modifiedVertices.Insert(i, newVert);
            }
        }

        matzhoBall.mesh.SetVertices(modifiedVertices);
        Debug.Log("Mesh Depressed");
    }
}
