using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CashGameScript : MonoBehaviour 
{
	public void changeScene(int i)
	{
		SceneManager.LoadScene (i);
	}
}
