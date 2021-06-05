using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class SaveSystem
{
    private static readonly string _path = "/Highscore.dat";

    public static void Save(List<Highscore> highscores)
    {
        FileStream file = new FileStream(Application.persistentDataPath + _path, FileMode.OpenOrCreate);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(file, highscores);
        }
        catch (SerializationException e)
        {
            Debug.LogError("There was an issue serializing this data: " + e.Message);
        }
        finally
        {
            file.Close();
        }
    }

    public static void Load()
    {
        if(File.Exists(Application.persistentDataPath + _path))
        {
            FileStream file = new FileStream(Application.persistentDataPath + _path, FileMode.Open);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                List<Highscore> highscores = (List<Highscore>)formatter.Deserialize(file);
                GameManager.Instance.LoadHighscores(highscores);
            }
            catch (SerializationException e)
            {
                Debug.LogError("Error Deserializing Data: " + e.Message);
            }
            finally
            {
                file.Close();
            }
        }
    }
}
