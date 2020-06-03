
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="Data", menuName ="game-level", order = 1)]
public class GameRoundSetupData : ScriptableObject
{
    public int supply, numTargets, pattern;
}

