using System.Collections.Generic;
using Commons.Audio;
using Commons.Const;
using Commons.Save;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Widget.Result
{
    public class ResultWidgetController : MonoBehaviour
    {
        /// <summary>
        /// アニメーションが終了したら呼ばれる
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsAnimationFinished => _isAnimationFinished;
        private BoolReactiveProperty _isAnimationFinished = new BoolReactiveProperty(false);

        /// <summary>
        /// Text
        /// </summary>
        [SerializeField] private Text _text;

        /// <summary>
        /// String
        /// </summary>
        private List<string> _message = new List<string>()
        {
            "Completed!",
            "Next.. "
        };
        
        /// <summary>
        /// AudioManager
        /// </summary>
        [Inject] private AudioManager _audioManager;

        /// <summary>
        /// SaveManager
        /// </summary>
        [Inject] private SaveManager _saveManager;

        /// <summary>
        /// リザルトアニメーション
        /// </summary>
        public void Animation()
        {
            DOTween.Sequence()
                .AppendCallback(()=>_audioManager.PlaySoundEffect(SoundEffect.Result1))
                .Append(_text.DOText(_message[0], 0.5f, scrambleMode: ScrambleMode.None).SetEase(Ease.Linear))
                .Append(_text.DOFade(0f, 0.1f).SetEase(Ease.Linear))
                .Append(_text.DOFade(1f, 0.1f).SetEase(Ease.Linear))
                .AppendCallback(()=>_audioManager.PlaySoundEffect(SoundEffect.Result2))
                .AppendInterval(0.5f)
                .Append(_text.transform.DORotate(new Vector3(90, 0, 0), 0.2f).SetEase(Ease.Linear))
                .AppendCallback(()=>_audioManager.PlaySoundEffect(SoundEffect.Result3))
                .Append(_text
                    .DOText(_message[1] + _saveManager.Data.CurrentStageNumber + "/" + Const.StagesNumber, 0.0f,
                        scrambleMode: ScrambleMode.None).SetEase(Ease.Linear)).SetLink(this.gameObject)
                .Append(_text.transform.DORotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.Linear))
                .OnComplete(() => _isAnimationFinished.Value = true);
        }

        /// <summary>
        /// 表示を設定
        /// </summary>
        public void SetView(bool isView)
        {
            _text.gameObject.SetActive(isView);
        }
    }
}