using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	public void ChangeScene(string Name)
	{
		Application.LoadLevel (Name);
	}
}