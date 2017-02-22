using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour {

	public void changeScene(int i)
	{
		SceneManager.LoadScene (i);
	}

}
