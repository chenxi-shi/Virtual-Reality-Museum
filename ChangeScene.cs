using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public void ChengeToScene (string sceneToChengeTo) {
        Application.LoadLevel(sceneToChengeTo);
	}	
}
