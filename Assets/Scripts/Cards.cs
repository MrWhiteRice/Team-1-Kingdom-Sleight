using UnityEngine;

public class Cards : MonoBehaviour
{
    public Animator a;

	public void OpenHand()
    {
        a.SetBool("open", true);
    }

    public void CloseHand()
    {
        a.SetBool("open", false);
    }
}