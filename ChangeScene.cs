using UnityEngine;
using System.Collections;

// change scene by load level
public class ChangeScene : MonoBehaviour {

	public void ChengeToScene (string sceneToChengeTo) {
        Application.LoadLevel(sceneToChengeTo);
	}	
}
