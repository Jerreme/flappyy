using UnityEngine;

public class ObstacleColor : MonoBehaviour
{
    public static Color BaseColor;

    void Update()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = BaseColor;
    }
}