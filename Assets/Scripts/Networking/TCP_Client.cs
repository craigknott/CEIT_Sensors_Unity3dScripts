using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class TCP_Client : MonoBehaviour {

	
	//Server config
	private String server = "winter.ceit.uq.edu.au";
	private int port = 3001;

	public TextMesh gumball;
	public TextMesh southdoor;
	private String[] tags = {"Temperature", "Mic", "Light", "Motion", "Voltage", "Gumball", "SouthDoor", "Plant"};

	private GameObject[] endPoints;

	private NetworkStream stm;

	//Horizontal Slider vars
	private float hSliderValue = 0.0f;
	private float phSliderValue = 0.0f;

	//Selection grid vars
	private int selectionGridint = 0;
	private string[] selectionStrings = {"Normal", "Fast", "SUPER"};

	//Message communication throttling vars
	private bool recieved = false;
	private Int32 lastmsg = 0;

	// Use this for initialization
	void Start () {
		//try {
			List<GameObject> temp = new List<GameObject>();
			foreach (String tag in tags) {
				temp.AddRange (GameObject.FindGameObjectsWithTag(tag));
			}
			endPoints = temp.ToArray ();
			Debug.Log (endPoints);
			TcpClient tcpclnt = new TcpClient();
			Debug.Log("Connecting.....");
			tcpclnt.Connect(server,port);

			Debug.Log("Connected");

			this.stm = tcpclnt.GetStream();
			this.stm.ReadTimeout = 5;

			Debug.Log("Transmitting.....");

			/*
			 * Convert to JSON and send JSON string
			 * {"data":[
			 * 	{"id":" ", "value": " "},
			 *  {"id":" ", "value": " "},
			 *  {"id":" ", "value": " "},
			 *  {"id":" ", "value": " "},
			 * ]}
			 * 
			 **/
			String jsonStringIDS = buildJSONString (unixTimestamp(), "init");
			Debug.Log (jsonStringIDS);
			
			sendString (jsonStringIDS);
		//}
		//catch (Exception e) {
		//	Debug.Log ("Error..... " + e.StackTrace);
		//}
	}

	// Update is called once per frame
	void Update () {
		if (hSliderValue != phSliderValue) {
			if (recieved == true) {
				String jsonStringIDS = "";
				lastmsg = unixTimestamp ();
				if (hSliderValue == 0.0f) {
					jsonStringIDS = buildJSONString (lastmsg, "init");
				} else {
					jsonStringIDS = buildJSONString (lastmsg - (Int32)(hSliderValue * 3600), "1"); 
				} 
				sendString (jsonStringIDS);
				phSliderValue = hSliderValue;
				recieved = false;
			}
		} else if (hSliderValue != 0.0f) {
			Int32 diff = unixTimestamp () - lastmsg;
			if ((recieved == true) && (diff >= 1)) {
				lastmsg = unixTimestamp ();
				hSliderValue -= diff * selectionGridint * selectionGridint * selectionGridint * selectionGridint / 60.0f;
				if (hSliderValue < 0.0f) {
					hSliderValue = 0.0f;
				}
				String jsonStringIDS = buildJSONString (lastmsg - (Int32)(hSliderValue * 3600), "1"); 
				sendString (jsonStringIDS);
				recieved = false;
			}
		}
		recieveData ();
	}

	/*sendString
	 * takes a string and sends it to the network stream as a stream of bytes.
	 **/
	void sendString(String s) {
		ASCIIEncoding asen = new ASCIIEncoding ();
		byte[] ba = asen.GetBytes (s);
		Debug.Log ("number of bytes to write: " + ba.Length);
		this.stm.Write (ba, 0, ba.Length);
	}

	/* OnGUI
	 * Initialises and runs on GUI actions.
	 **/
	void OnGUI() {
		hSliderValue = GUI.HorizontalSlider (new Rect (10, 10, 240, 20), hSliderValue, 24.0f, 0.0f);
		selectionGridint = GUI.SelectionGrid (new Rect (10, 25, 240, 30), selectionGridint, selectionStrings, 3);
		if (GUI.Button (new Rect (10, 60, 80, 30), "Heatmap")) {
			//toggleHeatmap();
		}
	}

	//void toggleHeatmap() {
	//	foreach (GameObject id in Spheres) {
	//		id.GetComponent<Light>().enabled = !id.GetComponent<Light>().enabled;
	//	}
	//}

	Int32 unixTimestamp() {
		return (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
	}

	/* buildJSONString
	 * creates a JSON string array out of all the sensors stored in the pub vars
	 **/
	String buildJSONString(Int32 timestamp, String type) {
		String jsonStringIDS = "{\"data\":\""+type+"\", \"timestamp\":"+timestamp+", \"ids\":[";
		foreach (GameObject endpt in endPoints) {
			jsonStringIDS += "{\"id\":\""+endpt.transform.name+"\",\"value\":"+endpt.transform.GetComponent<valueOf>().value+"},";
		}
		jsonStringIDS = jsonStringIDS.Remove (jsonStringIDS.Length - 1);
		jsonStringIDS += "]}\0";
		return jsonStringIDS;
	}

	/* recieveData
	 * attempts to read the data from tcpstream and then parses into JSON
	 * and updates sensor values and displays.
	 **/
	void recieveData() {
		String r = readTcpStream ();
		if (r != "") {
			var NA = JSON.Parse (r);
			Debug.Log(NA);
			foreach(JSONNode N in NA.AsArray) {
				Debug.Log (N);
				GameObject.Find (N["id"]).GetComponent<valueOf>().value = float.Parse ( N["value"] );
				/*if ("1.1.0".Equals (N["id"], StringComparison.Ordinal)) {
					gumball.renderer.enabled = true;
				} else if ("130.102.86.133".Equals (N["id"], StringComparison.Ordinal)) {
					southdoor.renderer.enabled = true;
				} else if (isTemperature(N)) {
					//Do nothing
				} else if (isBattery(N)) {
					//Do nothing
				} else if (isLight(N)) {
					//Do nothing
				} else if (isMic(N)) {
					//Do nothing
				} else if (isMotion(N)) {
					//Do nothing
				}*/
			}
			recieved = true;
		}
	}

	/*isMic(JSONNode N)
	 * takes a JSON string N and checks if it represents a microphone object
	 * and then manipulates the corresponding object.
	 **
	bool isMic(JSONNode N) {
		foreach (AudioSource A in Mics) {
			try {
				if (A.name.Equals (N["id"], StringComparison.Ordinal)) {
					A.audio.volume = float.Parse(N["value"]) / 250;
					return true;
				}
			} catch (Exception e) {
				Debug.Log ("Exception: " + e);
			}
		}
		return false;
	}

	/*isLight(JSONNode N)
	 * takes a JSON string N and checks if it represents a light sensor object
	 * and then manipulates the corresponding object.
	 **
	bool isLight(JSONNode N) {
		foreach (Light L in Lights) {
			try {
				if (L.name.Equals (N["id"], StringComparison.Ordinal)) {
					L.light.intensity = float.Parse(N["value"]) / 100;
					return true;
				}
			} catch (Exception e) {
				Debug.Log ("Exception: " + e);
			}
		}
		return false;
	}

	/*isTemperature(JSONNode N)
	 * takes a JSON string N and checks if it represents a temperature sensor object
	 * and then manipulates the corresponding object.
	 **
	bool isTemperature(JSONNode N) {
		foreach (GUIText id in Temps) {
			try {
				if (id.name.Equals (N["id"], StringComparison.Ordinal)) {
					id.guiText.text = N["value"];
					return true;
				}
			} catch (Exception e) {
				Debug.Log ("Exception: " + e);
			}
		}
		return false;
	}

	/*isBattery(JSONNode N)
	 * takes a JSON string N and checks if it represents a voltage/battery object
	 * and then manipulates the corresponding object.
	 **
	bool isBattery(JSONNode N) {
		foreach (GUIText id in Voltages) {
			try {
				if (id.name.Equals (N["id"], StringComparison.Ordinal)) {
					if (float.Parse (N["value"]) < 1.0) {
						id.guiText.text = "Replace Batteries";
						return true;
					}
				}
			} catch (Exception e) {
				Debug.Log ("Exception: " + e);
			}
		}
		return false;
	}

	/*isBattery(JSONNode N)
	 * takes a JSON string N and checks if it represents a voltage/battery object
	 **
	bool isMotion(JSONNode N) {
		foreach (ParticleSystem id in Motion) {
			try {
				if (id.name.Equals (N["id"], StringComparison.Ordinal)) {
					id.maxParticles = int.Parse(N["value"]);
				}
			} catch (Exception e) {
				Debug.Log ("Exception: " + e);
			}
		}
		return false;
	}

	/*readTcpStream()
	 * attempts to read all data in the network stream up until the null terminator
	 * returns the data read, an empty string.
	 **/
	String readTcpStream() {
		String data = "";
		try {	
			if (this.stm.DataAvailable) {
				int b;
				while ((b = this.stm.ReadByte()) != 0) {
					data += (char)b;
				}
			}
		} catch (Exception e) {
			Debug.Log ("exception: " + e);
		}
		return data;
	}
}
