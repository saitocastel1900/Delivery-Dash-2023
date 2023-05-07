using System.Collections.Generic;
using System.Linq;
using Commons.Audio;
using Commons.Const;
using Commons.Extensions;
using Commons.Input;
using Commons.Save;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Widget.Select
{
    public class StageSelectWidgetController : MonoBehaviour
    {
        /// <summary>
        /// StageSelectPanelList
        /// </summary>
        [SerializeField] List<RectTransform> _stageSelectPanelList;

        /// <summary>
        /// AudioManager
        /// </summary>
        [Inject] private AudioManager _audioManager;

        /// <summary>
        /// SaveManager
        /// </summary>
        [Inject] private SaveManager _saveManager;

        /// <summary>
        /// Tween
        /// </summary>
        private Tween _tween;

        /// <summary>
        /// 現在いる要素の番号
        /// </summary>
        private int _currentElementIndex = 0;
        
        private void Start()
        {
            _saveManager.Load();
            SetInteractable(_saveManager.Data.MaxStageClearNumber);

            this.SetStageSelectPanelListScale();
        }

        /// <summary>
        /// 大きさを設定
        /// </summary>
        private void SetStageSelectPanelListScale()
        {
            foreach (var stageSelectPane in _stageSelectPanelList)
            {
                float distance = Mathf.Abs(stageSelectPane.GetAnchoredPosX());
                float scale = Mathf.Clamp(1.0f - distance / (170.0f * 4.0f), 0.65f, 1.0f);
                stageSelectPane.SetLocalScaleXY(scale);
            }
        }

        /// <summary>
        /// 進行先のRectTransformを取得
        /// </summary>
        private RectTransform GetNextRectTransform()
        {
            RectTransform nearestRect = _stageSelectPanelList[_currentElementIndex];
            return nearestRect;
        }

        /// <summary>
        /// シーン遷移
        /// </summary>
        public void SceneTransition()
        {
            if (_saveManager.Data.MaxStageClearNumber >= _currentElementIndex)
            {
                _saveManager.Data.CurrentStageNumber = _currentElementIndex + 1;
                _saveManager.Save();
                _audioManager.PlaySoundEffect(SoundEffect.SelectEnter1);
                SceneManager.LoadScene(Const.StageTemplateName + (_currentElementIndex + 1).ToString());
            }
            else
            {
                _audioManager.PlaySoundEffect(SoundEffect.SelectEnter1);
            }
        }

        /// <summary>
        /// インタラクションを設定する
        /// </summary>
        private void SetInteractable(int clearStageNo)
        {
            foreach ((RectTransform stageSelectPane, int index) in _stageSelectPanelList.Select((x, i) => (x, i)))
            {
                bool isClear = clearStageNo >= index;
                stageSelectPane.GetComponent<StageSelectWidget>().SetInteractable(isClear);
            }
        }

        /// <summary>
        /// 左に移動
        /// </summary>
        public void TurnLeft()
        {
            if (_currentElementIndex <= 0)
            {
                return;
            }

            _tween.Kill();
            _currentElementIndex--;

            RectTransform itemRectTransform = this.GetNextRectTransform();
            float nextPositionX = -itemRectTransform.GetAnchoredPosX();

            foreach (var stageSelectPane in _stageSelectPanelList)
            {
                var seq = DOTween.Sequence();
                seq.Append(stageSelectPane.DOAnchorPosX(stageSelectPane.GetAnchoredPosX() + nextPositionX, 0.075f));
                seq.AppendCallback(SetStageSelectPanelListScale).SetLink(this.gameObject);
                _tween = seq;
            }
        }

        /// <summary>
        /// 右に移動
        /// </summary>
        public void TurnRight()
        {
            if (_currentElementIndex >= _stageSelectPanelList.Count - 1)
            {
                return;
            }

            _tween.Kill();
            _currentElementIndex++;

            RectTransform itemRectTransform = this.GetNextRectTransform();
            float nextPositionX = -itemRectTransform.GetAnchoredPosX();

            foreach (var stageSelectPane in _stageSelectPanelList)
            {
                var seq = DOTween.Sequence();
                seq.Append(stageSelectPane.DOAnchorPosX(stageSelectPane.GetAnchoredPosX() + nextPositionX, 0.075f));
                seq.AppendCallback(SetStageSelectPanelListScale).SetLink(this.gameObject);
                _tween = seq;
            }
        }
    }
}