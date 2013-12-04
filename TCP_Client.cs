using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using SimpleJSON;

public class TCP_Client : MonoBehaviour {

	public GUIText[] IDS;
	private NetworkStream stm = null;
	private StreamReader read = null;
	private StreamWriter write = null;
	private GUIText g = null;

	// Use this for initialization
	void Start () {
		try {
			TcpClient tcpclnt = new TcpClient();
			Console.WriteLine("Connecting.....");
			
			tcpclnt.Connect("talmeeno.com",6969);
			
			Console.WriteLine("Connected");

			this.stm = tcpclnt.GetStream();
			this.stm.ReadTimeout = 5;

			this.read = new StreamReader(this.stm);
			this.write = new StreamWriter(this.stm);

			ASCIIEncoding asen= new ASCIIEncoding();
			Console.WriteLine("Transmitting.....");

			foreach (GUIText id in IDS) {
				byte[] ba=asen.GetBytes(id.name);
				this.stm.Write(ba,0,ba.Length);
				Console.WriteLine (id.name);
				while (!stm.DataAvailable);
				try {
					byte[] br = new byte[128];
					this.stm.Read (br, 0, 127);
					string r = System.Text.Encoding.Default.GetString (br);
					if (r != "") {
						var N = JSON.Parse (r);
						Console.WriteLine(N["id"]);
						if (id.name.Equals (N["id"], StringComparison.Ordinal)) {
							id.guiText.text = N["value"];
						}
					}
				} catch (Exception e) {
					Console.WriteLine ( "exception: " + e);
				}
			}
		}
		catch (Exception e) {
			Console.WriteLine("Error..... " + e.StackTrace);
		}
		this.g = GetComponent<GUIText> ();
		this.g.guiText.text = "YEAH I CHANGED";
	}
	
	// Update is called once per frame
	void Update () {
		updateTemperatures ();
	}

	void updateTemperatures() {
		String r = readTcpStream ();
		if (r != "") {
			var N = JSON.Parse (r);
			Console.WriteLine(N["id"]);
			foreach (GUIText id in IDS) {
				if (id.name.Equals (N["id"], StringComparison.Ordinal)) {
					id.guiText.text = N["value"];
				}
			}
		}
	}

	String readTcpStream() {
		if (this.stm.DataAvailable) {
			try {
				byte[] br = new byte[128];
				this.stm.Read (br, 0, 127);
				return System.Text.Encoding.Default.GetString (br);
			} catch (Exception e) {
				return "exception: " + e;
			}
		} else {
			return "";
		}
	}
}
