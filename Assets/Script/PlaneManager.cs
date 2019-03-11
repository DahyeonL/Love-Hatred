using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlaneManager : MonoBehaviour
{
    public GameObject cube;
    public GameObject ready;
    public static bool IsCreated = false;
    public static bool stop = false;
    GameObject readyCube;
    GameObject Map;
    public static Vector3 Pos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TrackableHit hit;
        TrackableHitFlags flags = TrackableHitFlags.PlaneWithinPolygon
                                 | TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(Screen.width / 2, Screen.height / 2, flags, out hit) && stop == false)
        {
            if (IsCreated == false)
            {
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                readyCube = Instantiate(ready, hit.Pose.position, hit.Pose.rotation);
                IsCreated = true;
            }
            else
            {
                readyCube.transform.position = hit.Pose.position;
            }
            Pos = hit.Pose.position;
        }

        if(IsCreated == true && Input.GetMouseButtonDown(0) && stop == false)
        {

            Map = Instantiate(cube, readyCube.transform.position, new Quaternion(0, 0, 0, 0));
            
            Destroy(readyCube);
            stop = true;
            Map.GetComponent<NavmeshBaker>().BakeNavmesh();
            InstantPreviewTrackedPoseDriver.count = 15f;
        }
        
        //cube.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
    }
}
