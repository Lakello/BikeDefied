using UnityEngine;
using UnityEngine.UI;
using YG;

public class GridCellSizeSetterForLeaderBoard : GridCellSizeSetter
{
    [SerializeField] private LeaderboardYG _leaderBoardYG;

    protected override int Divisor => _leaderBoardYG.maxQuantityPlayers;
}