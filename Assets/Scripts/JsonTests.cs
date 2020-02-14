using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JsonTests : MonoBehaviour
{
    private const string httpServer = "http://localhost:57487/";

    public InputField jsonInputField1;
    public Text debugConsoleText;

    public void GetJson()
    {
        UnityWebRequest httpClient = new UnityWebRequest(httpServer + "api/Values/GetString", "GET");
        httpClient.downloadHandler = new DownloadHandlerBuffer();
        //httpClient.SetRequestHeader("Content-Type","application/json");
        httpClient.SetRequestHeader("Accept", "application/json");
        httpClient.SendWebRequest();

        while (!httpClient.isDone)
        {
            Task.Delay(1);
        }

        if(httpClient.isHttpError || httpClient.isNetworkError)
        {
            throw new Exception("JsonTest > ReceiveJsonString: " + httpClient.error);
        }

        string jsonResponse = httpClient.downloadHandler.text;

        //string response = JsonUtility.FromJson<string>(jsonResponse);
        //
        string response = jsonResponse.Replace("\"", "");
        debugConsoleText.text = "\nJsonTests > ReceiveJsonString: " + response;
    }

    public void GetString()
    {
        UnityWebRequest httpClient = new UnityWebRequest(httpServer + "api/Values/PostString", "POST");
        //httpClient.downloadHandler = new DownloadHandlerBuffer();
        string stringToSend = jsonInputField1.text;
        string jsonString = JsonUtility.ToJson(stringToSend);
        //string data = JsonUtility.ToJson(jsonString);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonString);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);
        httpClient.SetRequestHeader("Content-Type","application/json");
        //httpClient.SetRequestHeader("Accept", "application/json");
        httpClient.SendWebRequest();

        while (!httpClient.isDone)
        {
            Task.Delay(1);
        }

        if (httpClient.isHttpError || httpClient.isNetworkError)
        {
            throw new Exception("JsonTest > SendJsonString: " + httpClient.error);
        }

        //string jsonResponse = httpClient.downloadHandler.text;

        //string response = JsonUtility.FromJson<string>(jsonResponse);
        //
        
        
        debugConsoleText.text = "\nJsonTests > SendJsonString: " + httpClient.responseCode;
    }

}
