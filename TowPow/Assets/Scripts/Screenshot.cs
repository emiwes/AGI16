using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour {

    void Update() {

        if(Input.GetKeyUp(KeyCode.Space)) {
            Debug.Log("Screenshot");
            Application.CaptureScreenshot("screenshot.png", 4);
        }
    }
}
