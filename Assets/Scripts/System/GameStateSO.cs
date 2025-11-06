using UnityEngine;

[CreateAssetMenu(menuName = "SO/GameState")]
public class GameStateSO : ScriptableObject
{
	public string modeName;
	public string modeDecs;
	public int cols;
	public int rows;
	public float mineRatio;
	public float mineHigh;
	public float itemRatio;
	public float itemLow;
	public short floorMax;

	public SGameState GetGameState()
	{
		SGameState state = new SGameState();

		state.modeName = modeName;
		state.modeDecs = modeDecs;
		state.cols = cols;
		state.rows = rows;
		state.mineRatio = mineRatio;
		state.mineHigh = mineHigh;
		state.itemRatio = itemRatio;
		state.itemLow = itemLow;
		state.floorNow = 0;
		state.floorMax = floorMax;
		return (state);
	}
}