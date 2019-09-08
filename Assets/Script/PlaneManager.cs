using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlaneManager : MonoBehaviour
{
    [SerializeField]
    GameObject Guide1;

    [SerializeField]
    GameObject Guide2;

    [SerializeField]
    GameObject Guide3;

    public GameObject cube;
    public GameObject ready;
    public static bool IsCreated = false;
    public static bool stop = false;
    GameObject readyCube;
    public static GameObject Map;
    public static Vector3 Pos;
    public static bool plane = false;
    bool test = false;
    bool cancel = false;
    int waitcount = 0;
    float time = 0;

    public void Restart()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Guide1.SetActive(false);
        Guide2.SetActive(false);
        Guide3.SetActive(false);
        if (stop == false)
        {
            TrackableHit hit;
            TrackableHitFlags flags = TrackableHitFlags.PlaneWithinPolygon
                                     | TrackableHitFlags.FeaturePointWithSurfaceNormal;
            if (IsCreated)
            {
                ChangeSize();
            }
            if (Frame.Raycast(Screen.width / 2, Screen.height / 2, flags, out hit))
            {
                if (IsCreated == false)
                {
                    readyCube = Instantiate(ready, hit.Pose.position, hit.Pose.rotation);
                    IsCreated = true;
                }
                else if(!test) 
                {
                    readyCube.transform.position = hit.Pose.position + new Vector3(0, 0.001f, 0);
                    Guide2.SetActive(true);
                }
                else if (test)
                {
                    Guide3.SetActive(true);
                }
                Pos = hit.Pose.position;
            }
            else if(!test)
            {
                Guide1.SetActive(true);
            }
            else if (IsCreated && test)
            {
                Guide3.SetActive(true);
            }
            if(IsCreated && Input.GetMouseButtonDown(0))
            {
                time = 0;
                cancel = false;
            }
            else if (IsCreated == true && Input.GetMouseButtonUp(0))
            {
                if (test)
                {
                    Map = Instantiate(cube, readyCube.transform.position, new Quaternion(0, 0, 0, 0));

                    Destroy(readyCube);
                    stop = true;
                    Map.GetComponent<NavmeshBaker>().BakeNavmesh();
                    InstantPreviewTrackedPoseDriver.count = 15f;
                    plane = true;
                }
                else if(!cancel)
                {
                    test = true;
                }
            }
            else if(IsCreated && Input.GetMouseButton(0))
            {
                time = time + Time.deltaTime;
                if(time > 1)
                {
                    test = false;
                    cancel = true;
                }
            }
        }
    }

    void ChangeSize()
    {
        if (readyCube.transform.localScale.x < 1)
        {
            readyCube.transform.localScale = readyCube.transform.localScale + new Vector3(0.01f, 0.01f, 0.01f);
        }
        else if(readyCube.transform.localScale.x >= 1)
        {
            waitcount = waitcount + 1;
            if (waitcount > 15)
            {
                readyCube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                waitcount = 0;
            }
        }
    }

}
