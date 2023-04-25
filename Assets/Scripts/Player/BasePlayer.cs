using UnityEngine;
using Zenject;
using UniRx;

public abstract class BasePlayer : MonoBehaviour
{
    //共通して使う物
        
    /// <summary>
    /// MoleCore
    /// </summary>
    protected PlayerCore _moleCore;
       
    /// <summary>
    /// Input
    /// </summary>
    [Inject] protected IInputEventProvider _input;
        
    private void Start()
    {
        _moleCore = this.gameObject.GetComponent<PlayerCore>();
        _moleCore.OnInitializeAsync.Subscribe(_=>OnInitialize()).AddTo(this);

        OnStart();
    }

    protected virtual void OnStart() { }

    protected abstract void OnInitialize();
}
