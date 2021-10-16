using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        SavePlayerData();
    }

    public void Load()
    {
        LoadPlayerData();
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetFloat("PositionX",player.transform.position.x);
        PlayerPrefs.SetFloat("PositionY",player.transform.position.y);
        PlayerPrefs.SetFloat("PositionZ",player.transform.position.z);
    }

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("PositionX") && PlayerPrefs.HasKey("PositionY") && PlayerPrefs.HasKey("PositionZ"))
        {
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PositionX"),
                PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));
        }
    }
}
