using UnityEngine;

public class CardPoint : MonoBehaviour
{
    public enum Lane
    {
        left,
        middle,
        right
    };
    public Lane lane;

	public bool isPlayer;
	public int buildID;

    private void Start()
    {
        
    }
}