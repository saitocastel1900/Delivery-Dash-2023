using UnityEngine;
using UnityEngine.UI;

public class MoveCommandPresenter : MonoBehaviour
{
    [SerializeField] private Button _leftMoveButton;
    [SerializeField] private Button _rightMoveButton;
    [SerializeField] private Button _aheadMoveButton;
    [SerializeField] private Button _backMoveButton;

    [SerializeField] private Button _undoButton;
   
    [SerializeField] private PlayerReceiver _receiver;

    public void Initialize()
    {
        _currentPlayerPos = _player.transform.position;
        _currentBlockPos = _block.transform.position;
        
        SetEvent();
    }

    private void SetEvent()
    {
        _leftMoveButton.onClick.AddListener(OnLeftMove);
        _rightMoveButton.onClick.AddListener(OnRightMove);
        _aheadMoveButton.onClick.AddListener(OnAheadMove);
        _backMoveButton.onClick.AddListener(OnBackMove);
        
        _undoButton.onClick.AddListener(OnUndo);
    }

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _block;
  
    private Vector3 _currentPlayerPos;
    private Vector3 _currentBlockPos;
    
    private void OnLeftMove()
    {
        if (_currentPlayerPos + Vector3.left == _block.transform.position)
        {
            _block.gameObject.transform.position += Vector3.left;
            _currentBlockPos = _block.gameObject.transform.position;
        }

        ICommand command = new MoveCommand(_receiver,Vector3.left);
        MoveCommandInvoker.Execute(command);
        
        _currentPlayerPos = _player.gameObject.transform.position;
    }

    private void OnRightMove()
    {
        if (_currentPlayerPos + Vector3.right == _block.transform.position)
        {
            _block.gameObject.transform.position += Vector3.right;
            _currentBlockPos = _block.gameObject.transform.position;
        }

        ICommand command = new MoveCommand(_receiver, Vector3.right);
        MoveCommandInvoker.Execute(command);

        _currentPlayerPos = _player.gameObject.transform.position;
    }
    
    private void OnAheadMove()
    {
        if (_currentPlayerPos+Vector3.forward == _block.transform.position)
        {
           _block.gameObject.transform.position +=Vector3.forward;
           _currentBlockPos = _block.gameObject.transform.position;
        }
        ICommand command = new MoveCommand(_receiver,Vector3.forward);
        MoveCommandInvoker.Execute(command);
        
        _currentPlayerPos = _player.gameObject.transform.position;
    }

    private void OnBackMove()
    {
        if (_currentPlayerPos + Vector3.back == _block.transform.position)
        {
            _block.gameObject.transform.position += Vector3.back;
            _currentBlockPos = _block.gameObject.transform.position;
        }

        ICommand command = new MoveCommand(_receiver, Vector3.back);
        MoveCommandInvoker.Execute(command);

        _currentPlayerPos = _player.gameObject.transform.position;
    }
    
    private void OnUndo()
    {
        MoveCommandInvoker.Undo();
    }
}
