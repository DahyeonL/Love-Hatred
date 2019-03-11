using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class CamController : MonoBehaviour
{

    public void increaseCount()
    {
        InstantPreviewTrackedPoseDriver.count = InstantPreviewTrackedPoseDriver.count + 1.0f;
    }
}
