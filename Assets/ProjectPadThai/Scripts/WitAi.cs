using UnityEngine;
using System;
using System.IO;
using System.Net;
using SimpleJSON;

public class WitAi : MonoBehaviour, IRecordingHandler
{

    public ActionHandler actionHandler;

    private string witAiToken = "";
    private string witAiUrl = "https://api.wit.ai/speech?v=20160526";

    public void handleRecordingAudio(string audioFilePath)
    {
        byte[] audioBytes = GetAudioBytes(audioFilePath);
        HttpWebRequest request = PostAudioWebRequest(audioBytes, witAiUrl);
        string response = GetWebResponseText(request);

        Debug.Log("response:" + response);
        //response: {
        //    "msg_id" : "918d0332-fd19-43cb-871e-6230a2b3d076",
        //    "_text" : "create a box",
        //    "entities" : {
        //        "rigid_body" : [ {
        //            "confidence" : 0.9746828461928303,
        //            "type" : "value",
        //            "value" : "box"
        //        } ],
        //        "intent" : [ {
        //            "confidence" : 1.0,
        //            "value" : "create"
        //        } ]
        //    }
        //}

        JSONNode responseJson = JSON.Parse(response);
        handleWitResponse(responseJson);
    }

    byte[] GetAudioBytes(string audioFilePath)
    {
        FileStream filestream = new FileStream(audioFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader filereader = new BinaryReader(filestream);
        byte[] audioBytes = filereader.ReadBytes((Int32)filestream.Length);
        filestream.Close();
        filereader.Close();
        return audioBytes;
    }

    HttpWebRequest PostAudioWebRequest(byte[] audioBytes, string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["Authorization"] = "Bearer " + witAiToken;
        request.ContentType = "audio/wav";
        request.ContentLength = audioBytes.Length;
        request.GetRequestStream().Write(audioBytes, 0, audioBytes.Length);
        return request;
    }

    string GetWebResponseText(HttpWebRequest request)
    {
        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader responseStream = new StreamReader(response.GetResponseStream());
                return responseStream.ReadToEnd();
            }
            else
            {
                return "Error: " + response.StatusCode.ToString();
            }
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }

    void handleWitResponse(JSONNode responseJson)
    {
        string text = responseJson["_text"];
        Debug.Log("text:" + text);

        string intent = "";
        string color = "";
        string rigidBody = "";
        string where = "";

        JSONNode entities = responseJson["entities"];
        if (entities["intent"] != null)
        {
            intent = entities["intent"][0]["value"];
        }
        if (entities["color"] != null)
        {
            color = entities["color"][0]["value"];
        }
        if (entities["rigid_body"] != null)
        {
            rigidBody = entities["rigid_body"][0]["value"];
        }
        if (entities["where"] != null)
        {
            where = entities["where"][0]["value"];
        }

        switch (intent)
        {
            case "create":
                switch (rigidBody)
                {
                    case "room":
                        actionHandler.createRoom();
                        break;
                    case "circle":
                    case "sphere":
                        actionHandler.createSphere(color, where);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
