using UnityEngine;

public class Cards : MonoBehaviour
{
	Animator a;

	private void Start()
	{
		a = GetComponent<Animator>();
	}

	public void OpenHand()
    {
        a.SetBool("open", true);
    }

    public void CloseHand()
    {
        a.SetBool("open", false);
    }
}