using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TitleDataDataBase", menuName = "ScriptableObject/TitleDataDataBase")]
public class TitleDataBase : ScriptableObject
{
    public List<TitleData> TileObjectList = new List<TitleData>();
}
