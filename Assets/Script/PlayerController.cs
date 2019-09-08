using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Text Resource;
    [SerializeField]
    Text TimeText;
    [SerializeField]
    Text warningText;

    public static Color[] color = { new Color(1, 0, 0), new Color(0, 0, 1) };
    private static int PlayerNum = -1;
    private static string Tribe;
    private static float ResourceAmount = 0;
    private static Text ResourceText;
    private static Text WarningText;
    private static int Startingpoint;
    public static int SanctomCount = 0;
    public static List<GameObject> TerraBuildings = new List<GameObject>();
    static float showmessageTime;
    float sec = 0;
    float min = 0;

    public void Restart()
    {
        PlayerNum = -1;
        Tribe = null;
        ResourceAmount = 0;
        Startingpoint = -10;
        SanctomCount = 0;
        TerraBuildings = new List<GameObject>();
        sec = 0;
        min = 0;
    }

    private void Awake()
    {
        WarningText = warningText;
        ResourceText = Resource;
        printResource();
    }

    private void Update()
    {
        if (PlayerNum != -1 && PhotonNetwork.playerList.Length == 2 && StartingPoint.IsStart)
        {
            sec = sec + Time.deltaTime;
            if(sec >= 60)
            {
                min = min + 1;
                sec = sec - 60;
            }
            if (sec < 10)
            {
                TimeText.text = min.ToString() + ":0" + ((int)sec).ToString();
            }
            else
            {
                TimeText.text = min.ToString() + ":" + ((int)sec).ToString();
            }
        }
        if(showmessageTime > 0)
        {
            showmessageTime = showmessageTime - Time.deltaTime;
            if(showmessageTime <= 0)
            {
                WarningText.enabled = false;
            }
        }
    }

    public static int GetPlayerNum()
    {
        return PlayerNum;
    }

    public static void SetPlayerNum(int num)
    {
        PlayerNum = num;
    }

    public static string GetTribe()
    {
        return Tribe;
    }

    public static void SetTribe(string tribe)
    {
        Tribe = tribe;
    }

    public static void printResource()
    {
        ResourceText.text = ((int)ResourceAmount).ToString();
    }

    public static void AddResource(float amount)
    {
        ResourceAmount = ResourceAmount + amount;
        printResource();
    }

    public static void RemoveResource(int amount)
    {
        ResourceAmount = ResourceAmount - amount;
        printResource();
    }

    public static float GetResource()
    {
        return ResourceAmount;
    }

    public static int GetStartingPoint()
    {
        return Startingpoint;
    }

    public static void SetStartingPoint(int i)
    {
        Startingpoint = i;
    }

    public static int GetSanctomCount()
    {
        return SanctomCount;
    }

    public static void SetSanctomCount(int Count)
    {
        SanctomCount = Count;
    }

    public static void ShowWarning(string message, float time)
    {
        WarningText.text = message;
        WarningText.enabled = true;
        showmessageTime = time;
    }
}
