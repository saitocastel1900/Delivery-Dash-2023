using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Zenject;

namespace Commons.Input
{
    public class ButtonInputProvider : IInputEventProvider , IInitializable
    {
        /// <summary>
        /// 
        /// </summary>
        private Button _aheadButton;
        
        /// <summary>
        /// 
        /// </summary>
        private Button _leftButton;
        
        /// <summary>
        /// 
        /// </summary>
        private Button _rightButton;
        
        /// <summary>
        /// 
        /// </summary>
        private Button _backButton;
        
        /// <summary>
        /// 
        /// </summary>
        private Button _undoButton;

        /// <summary>
        /// 
        /// </summary>
        public ButtonInputProvider(Button aheadButton,Button leftButton,Button rightButton,Button backButton,Button undoButton)
        {
            _aheadButton = aheadButton;
            _leftButton = leftButton;
            _rightButton = rightButton;
            _backButton = backButton;
            _undoButton = undoButton;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyReactiveProperty<bool> UndoButton => _undo;
        private ReactiveProperty<bool> _undo = new ReactiveProperty<bool>();

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
           
        }
    }
}