using UnityEngine;

public class PlayerReceiver : MonoBehaviour,IReceiver
{
    public void Move(Vector3 pos)
    {
        transform.position += pos;
    }
}
