using System.Collections.Generic;
using Manager.Command;
using UnityEditor;
using UnityEngine;

namespace Manager.Stage
{
    [CreateAssetMenu(fileName = "StageAnswerData", menuName = "ScriptableObject/StageAnswerData")]
    public class StageAnswerData : ScriptableObject
    {
        /// <summary>
        /// ステージクリアのコマンド群
        /// </summary>
        public List<DirectionType> CommandAnswerList = new List<DirectionType>();

#if UNITY_EDITOR
        /// <summary>
        /// コマンドを録音中か
        /// </summary>
        public bool IsRecoding = false;

        [CustomEditor(typeof(StageAnswerData))]
        public class StageAnswerDataEditor : Editor
        {
            /// <summary>
            /// CommandAnswerListのSerializedProperty
            /// </summary>
            private SerializedProperty _commandAnswerListProperty;

            /// <summary>
            /// CommandAnswerList
            /// </summary>
            private List<DirectionType> _commandAnswerList;

            /// <summary>
            /// Commandの要素に表示するラベル
            /// </summary>
            private GUIContent _commandLabel = new GUIContent("移動");

            /// <summary>
            /// 録音中か
            /// </summary>
            private bool _isRecoding = false;

            public void OnEnable()
            {
                _commandAnswerListProperty = serializedObject.FindProperty("CommandAnswerList");
                _commandAnswerList = (target as StageAnswerData)?.CommandAnswerList;
                _isRecoding = ((StageAnswerData) target).IsRecoding;
            }

            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                GUI.backgroundColor = _isRecoding ? Color.red : Color.white;
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(_isRecoding ? "停止" : "録音"))
                    {
                        _isRecoding = !_isRecoding;
                        ((StageAnswerData) target).IsRecoding = _isRecoding;
                        Repaint();
                        return;
                    }
                }

                EditorGUILayout.Separator();
                GUI.backgroundColor = Color.white;

                var count = _commandAnswerListProperty.arraySize;
                for (int i = 0; i < count; ++i)
                {
                    var element = _commandAnswerListProperty.GetArrayElementAtIndex(i);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField($"{i.ToString()}.", GUILayout.Width(20f));
                        EditorGUILayout.PropertyField(element, _commandLabel);

                        var buttonWidth = GUILayout.Width(20f);
                        if (i != 0)
                        {
                            if (GUILayout.Button("↑", buttonWidth))
                            {
                                (_commandAnswerList[i], _commandAnswerList[i - 1]) =
                                    (_commandAnswerList[i - 1], _commandAnswerList[i]);
                                Repaint();
                                return;
                            }
                        }
                        else
                        {
                            GUILayout.Label("", buttonWidth);
                        }

                        if (i != count - 1)
                        {
                            if (GUILayout.Button("↓", buttonWidth))
                            {
                                (_commandAnswerList[i], _commandAnswerList[i + 1]) =
                                    (_commandAnswerList[i + 1], _commandAnswerList[i]);
                                Repaint();
                                return;
                            }
                        }
                        else
                        {
                            GUILayout.Label("", buttonWidth);
                        }

                        if (GUILayout.Button("×", buttonWidth))
                        {
                            for (int j = i + 1; j < count; ++j)
                            {
                                _commandAnswerList[j - 1] = _commandAnswerList[j];
                            }

                            _commandAnswerList.RemoveAt(count - 1);
                            Repaint();
                            return;
                        }
                    }
                }

                EditorGUILayout.Separator();

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("コマンド追加"))
                    {
                        _commandAnswerList.Add(DirectionType.AheadMove);
                        Repaint();
                        return;
                    }

                    if (GUILayout.Button("すべて削除"))
                    {
                        _commandAnswerList.Clear();
                        Repaint();
                        return;
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}