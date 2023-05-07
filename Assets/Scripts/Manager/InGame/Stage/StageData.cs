using UnityEditor;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData")]
    public class StageData : ScriptableObject
    {
        /// <summary>
        /// ステージ生成のファイル
        /// </summary>
        public TextAsset StageFile;

        /// <summary>
        /// 床
        /// </summary>
        public GameObject GroundObject;

        /// <summary>
        /// 目的地
        /// </summary>
        public GameObject TargetObject;

        /// <summary>
        /// プレイヤー
        /// </summary>
        public GameObject PlayerObject;

        /// <summary>
        /// ブロック
        /// </summary>
        public GameObject BlockObject;


#if UNITY_EDITOR
        [CustomEditor(typeof(StageData))]
        public class StageDataEditor : Editor
        {
            /// <summary>
            /// StageFileのSerializedProperty
            /// </summary>
            private SerializedProperty _stageFileProperty;
            
            /// <summary>
            /// StageFile
            /// </summary>
            private TextAsset _stageFile;

            public void OnEnable()
            {
                _stageFileProperty = serializedObject.FindProperty("StageFile");
                _stageFile = (target as StageData)?.StageFile;
            }

            //TODO:将来的にインスペクタ上でステージを編集出来るようにする
            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                SerializedProperty stageProperty = serializedObject.FindProperty("StageFile");
                
                EditorGUILayout.Separator();
                
                SerializedProperty groundObjectProperty = serializedObject.FindProperty("GroundObject");
                SerializedProperty targetObjectProperty = serializedObject.FindProperty("TargetObject");
                SerializedProperty playerObjectProperty = serializedObject.FindProperty("PlayerObject");
                SerializedProperty blockObjectProperty = serializedObject.FindProperty("BlockObject");

                EditorGUILayout.PropertyField(stageProperty);
                EditorGUILayout.PropertyField(groundObjectProperty);
                EditorGUILayout.PropertyField(targetObjectProperty);
                EditorGUILayout.PropertyField(playerObjectProperty);
                EditorGUILayout.PropertyField(blockObjectProperty);

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}