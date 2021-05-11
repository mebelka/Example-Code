using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class JsonTranslationFailure : Exception {
    public JsonTranslationFailure(string message) : base(message) { }
}

/// Class that takes an object or json and translates it into json or an object.
public class JsonTranslator {

    public string TranslateToJson(object objectData) {
        if (objectData == null) {
            throw new JsonTranslationFailure("Object attemping to translate into Json string is null");
        }

        string json = JsonUtility.ToJson(objectData);
        return json;
    }

    public T TranslateToObject<T>(string json) {
        if (json == null) {
            throw new JsonTranslationFailure("Json string attemping to translate into an object is null");
        }

        return JsonUtility.FromJson<T>(json);
    }
}

public class NoPathExeption : Exception {
    public NoPathExeption(string message) : base(message) { }
}

/// Interface that defines objects that can save and load.
public interface ISaveLoad {
    void Save(ISaveable data);
    void Load<T>(String path) where T: ISaveable;
}

/// Saves and loads JSON data from the serlized system. This system is designed for mobile devices but also may work for desktop.
public class SaveLoadMobile : ISaveLoad {

    private JsonTranslator jsonTraslator = new JsonTranslator();
    private IGameState gameState;

    public SaveLoadMobile(IGameState gameState) {
        this.gameState = gameState;
    }

    public void Save(ISaveable data) {
        try {
            string json = jsonTraslator.TranslateToJson(data);

            string destination = Application.persistentDataPath + String.Format("/{0}.dat", data.PathName());
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, json);
            file.Close();
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }

    public void Load<T>(String path) where T: ISaveable {
        T loadedObject = default;
        try {
            string destination = Application.persistentDataPath + String.Format("/{0}.dat", path);
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else {
                throw (new NoPathExeption(String.Format("Path {0} does not exisit, but load attempted.", path)));
            }

            BinaryFormatter bf = new BinaryFormatter();
            string json = (String)bf.Deserialize(file);
            file.Close();

            loadedObject = jsonTraslator.TranslateToObject<T>(json);
            loadedObject.RecoverIfNeeded();
        } catch (Exception e) {
            Debug.Log($"error on path {path}: {e.Message}");
        }

        gameState.LoadData(loadedObject);
    }
}

/// Saves and loads JSON data from the serlized system. This system is designed for mobile devices but also may work for desktop.
public class SaveLoadWebGL : ISaveLoad {

    private JsonTranslator jsonTraslator = new JsonTranslator();
    private IGameState gameState;

    public SaveLoadWebGL(IGameState gameState) {
        this.gameState = gameState;
    }

    public void Save(ISaveable data) {
        try {
            string json = jsonTraslator.TranslateToJson(data);

            string destination = Application.persistentDataPath + String.Format("/{0}.dat", data.PathName());
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, json);
            file.Close();

            #if UNITY_WEBGL
                WebUtil.SyncFs();
            #endif
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }

    public void Load<T>(String path) where T : ISaveable {
        T loadedObject = default;
        try {
            string destination = Application.persistentDataPath + String.Format("/{0}.dat", path);
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else {
                throw (new NoPathExeption(String.Format("Path {0} does not exisit, but load attempted.", path)));
            }

            BinaryFormatter bf = new BinaryFormatter();
            string json = (String)bf.Deserialize(file);
            file.Close();

            loadedObject = jsonTraslator.TranslateToObject<T>(json);
            loadedObject.RecoverIfNeeded();
        } catch (Exception e) {
            Debug.Log($"error on path {path}: {e.Message}");
        }

        gameState.LoadData(loadedObject);
    }
}
