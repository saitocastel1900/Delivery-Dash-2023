using UnityEngine;

public class BlockMover : MonoBehaviour,IReceiver
{
    public void Move(Vector3 pos)
    {
        transform.position += pos;
    }
}