using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshBaker : MonoBehaviour
{
    public NavMeshSurface[] surface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BakeNavmesh()
    {
        for(int i = 0; i < surface.Length; i++)
        {
            surface[i].BuildNavMesh();
        }
    }
}
