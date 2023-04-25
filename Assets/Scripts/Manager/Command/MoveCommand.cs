using UnityEngine;

public class MoveCommand : ICommand
{
    private IReceiver _receiver;
    private Vector3 _pos;

    public MoveCommand(IReceiver receiver,Vector3 pos)
    {
        _receiver = receiver;
        _pos = pos;
    }

    public void Execute()
    {
        _receiver.Move(_pos);
    }

    public void Undo()
    {
        _receiver.Move(-_pos);
    }
}
