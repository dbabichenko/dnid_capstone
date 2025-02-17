using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTRequest : MonoBehaviour
{
    private string apiKey = "YOUR_API_KEY";
    
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    void Start()
    {
        string prompt = "What is the capital of France?";
        StartCoroutine(SendRequest(prompt));
    }

    IEnumerator SendRequest(string prompt)
    {
        // Create the request JSON
        string jsonData = "{\"model\": \"gpt-4o\", \"messages\": [{\"role\": \"system\", \"content\": \"You are a helpful assistant.\"},{\"role\": \"user\", \"content\": \"" + prompt + "\"}], \"temperature\": 0.7}";

        // Create the web request
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Response: " + jsonResponse);

                // Parse the response (basic parsing for the assistant's message)
                ChatGPTResponse response = JsonUtility.FromJson<ChatGPTResponse>(jsonResponse);
                if (response != null && response.choices.Length > 0)
                {
                    Debug.Log("ChatGPT: " + response.choices[0].message.content);
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    [Serializable]
    public class ChatGPTResponse
    {
        public Choice[] choices;
    }

    [Serializable]
    public class Choice
    {
        public Message message;
    }

    [Serializable]
    public class Message
    {
        public string content;
    }
}
