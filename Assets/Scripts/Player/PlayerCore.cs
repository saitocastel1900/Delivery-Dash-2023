using System;
using UI.DebugMessage;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerCore : MonoBehaviour , IDamagable
{
    /// <summary>
    /// 初期化したか
    /// </summary>
    public IObservable<Unit> OnInitializeAsync => _onInitializeAsyncSubject;
    private readonly AsyncSubject<Unit> _onInitializeAsyncSubject = new AsyncSubject<Unit>();
        
    /// <summary>
    /// ダメージを受けたか
    /// </summary>
    public IObservable<Unit> OnDamagedCallBack => _damageSubject;
    private Subject<Unit> _damageSubject = new Subject<Unit>();

    private void Awake()
    {
        InitializeMole();
        
    }
   
    /// <summary>
    /// 初期化
    /// </summary>
    private void InitializeMole()
    {
        _onInitializeAsyncSubject.OnNext(Unit.Default);
        _onInitializeAsyncSubject.OnCompleted();
    }

    /// <summary>
    /// Damage
    /// </summary>
    public void Damage()
    {
        _damageSubject.OnNext(Unit.Default);
    }
}
