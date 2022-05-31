using UnityEngine;

public class Mover : MonoBehaviour
{
    public static float Speed { set; get; }

    void Awake()
    {
        Speed = 10f;
    }

    void Update()
    {
        transform.position += Speed * Time.deltaTime * -Vector3.right;
    }
}
