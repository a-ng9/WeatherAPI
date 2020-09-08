using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.IO;
using System.Threading.Tasks;
//using Assets;

[Serializable]
public class Weather
{
    public int id;
    public string main;
}
[Serializable]
public class WeatherInfo
{
    public int id;
    public string name;
    public List<Weather> weather;
}

public class WeatherController : MonoBehaviour
{
    private const string API_KEY = "81797bcc47d79864d98cc069376c3e75";
    private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes
    public GameObject SnowSystem;
    private const string CityId = "6077243";
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    void Start()
    {
        CheckSnowStatus();
    }
    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CheckSnowStatus();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }
    }
    public async void CheckSnowStatus()
    {
        bool raining = (await GetWeather()).weather[0].main.Equals("Clouds");
        if (raining)
            SnowSystem.SetActive(true);
        else
            SnowSystem.SetActive(false);
    }
    private async Task<WeatherInfo> GetWeather()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY));
        HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        return info;
    }
}