using UnityEngine;
using System.Collections;

public class ActionHandler : MonoBehaviour {

    public GameObject floorPrefab;
    public GameObject spherePrefab;

    public void createRoom()
    {
        GameObject newFloor = (GameObject)Instantiate(floorPrefab, new Vector3(-0.5f, -1.9f, 2.75f), Quaternion.identity);
    }

    public void createSphere(string color, string where)
    {
        float x = 0;
        float y = -1;
        float z = 0;
        switch (where)
        {
            case "in blue":
            case "blue":
            case "in blue section":
                x = -1;
                y = -1;
                z = 1.75f;
                break;
            default:
                break;
        }
        GameObject newSphere = (GameObject)Instantiate(spherePrefab, new Vector3(x, y, z), Quaternion.identity);
        switch (color)
        {
            case "goal":
            case "gold":
                Color goldColor = new Color(255, 215, 0);
                newSphere.GetComponent<Renderer>().material.color = goldColor;
                break;
            default:
                break;
        }
    }
}
