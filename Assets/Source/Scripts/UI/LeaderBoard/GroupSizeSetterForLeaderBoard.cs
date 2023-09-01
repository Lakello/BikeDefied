using UnityEngine;
using UnityEngine.UI;
using YG;

public class GroupSizeSetterForLeaderBoard : GroupSizeSetter
{
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private LeaderboardYG _leaderBoardYG;

    protected override int Divisor => _leaderBoardYG.maxQuantityPlayers;

    private void Awake()
    {
        _grid.cellSize = CalculateSize();
    }
}