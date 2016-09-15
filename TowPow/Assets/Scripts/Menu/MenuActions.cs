using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MenuActions : MonoBehaviour {
	public class statics {
		public static int port = 7777;
		public static string serverIP;	
	}

	private string mainSceneName = "Main";

	public bool isClient = false;
	public bool isVive = false;
	public bool isPixelSense = false;

	public GameObject SelectIPAddress;
	public GameObject StartMenu;
	public InputField IPInputField;

	void Update(){
		if(Input.GetKeyDown("return") && (isVive || isPixelSense)){
			StartClient ();
		}
	}

	public void StartServer() 
	{
		Debug.Log ("Start server at "+Network.player.ipAddress+" port "+statics.port);

		statics.serverIP = Network.player.ipAddress;

		NetworkServer.Listen (statics.port);

		SceneManager.LoadScene (mainSceneName);
	}

	public void SelectClient(string type) 
	{
		Debug.Log ("Start client of type: " + type);
		if (type == "Vive") {
			isVive = true;

		} else if (type == "PixelSense"){
			isPixelSense = true;

		} else {
			Debug.LogError("Unknown Client Type"+type);

		}

		// Hide the first menu
		StartMenu.SetActive(false);

		// Show the IP address input field menu
		SelectIPAddress.SetActive(true);

		IPInputField.Select ();
	}

	public void StartClient(){
		statics.serverIP = IPInputField.text;

		NetworkClient client = new NetworkClient ();

		client.Connect (statics.serverIP, statics.port);

		SceneManager.LoadScene (mainSceneName);
	}
}