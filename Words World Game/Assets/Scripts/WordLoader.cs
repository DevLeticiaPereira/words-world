using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WordLoader : MonoBehaviour
{
	[SerializeField] private string googleSheetId = "1RY9GYaMqRN-Yp4xQZqm0e-NdsvleIiSkHa5zWCK9Dn8";

	private string[] sheetData;

	private void Start()
	{
		StartCoroutine(DownloadGoogleSheet());
	}

	public IEnumerator DownloadGoogleSheet()
	{
		string url = "https://docs.google.com/spreadsheets/d/" + googleSheetId + "/export?format=csv";

		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();

		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("Failed to download Google Sheet. Error: " + request.error);
			yield break;
		}

		string text = request.downloadHandler.text;

		sheetData = text.Split('\n');

		foreach (var word in sheetData)
		{
			Debug.Log(word);
		}
	}

	public string[] GetSheetData()
	{
		return sheetData;
	}
}
