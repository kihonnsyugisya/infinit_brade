using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public static class Util
{
   　/// <summary>
    /// シリアライズ可能なオブジェクトをJSON形式でプレイヤープリファレンスに保存する。
    /// </summary>
    /// <typeparam name="T">保存するオブジェクトの型</typeparam>
    /// <param name="key">保存する際のキー</param>
    /// <param name="saveTargetObj">保存したいオブジェクト</param>
    public static void SaveJsonToPrefs<T>(string key, T saveTargetObj)
    {
        // saveTargetObjが[System.Serializable]属性を持っているかどうかをチェック
        //TODO:リリース時にこのifのブロックはコメントアウトする。処理が重いらしいので。
        if (!IsSerializable(saveTargetObj.GetType()))
        {
            Debug.LogError("Object is not serializable.　serializableなクラスしかセーブできないよ");
            return;
        }
        var json = JsonUtility.ToJson(saveTargetObj);
        Debug.Log("Json data: " + json); // jsonの内容をログに出力
        PlayerPrefs.SetString(key, json);
    }

    /// <summary>
    /// 指定されたキーに保存されたJSON形式のオブジェクトを取得する。
    /// キーが存在しない場合、または関連付けられたデータが無効なJSON形式の場合、nullを返す。
    /// </summary>
    /// <typeparam name="T">取得するオブジェクトの型</typeparam>
    /// <param name="key">取得するオブジェクトのキー</param>
    /// <returns>保存されたオブジェクト、またはnull</returns>
    public static T GetJsonToPrefes<T>(string key)
    {
        var json = PlayerPrefs.GetString(key);
        var obj = JsonUtility.FromJson<T>(json);
        return obj;
    }

    /// <summary>
    /// 指定された型がシリアライズ可能かどうかを判定する。
    /// </summary>
    /// <param name="type">判定する型</param>
    /// <returns>シリアライズ可能かどうかを示す真偽値</returns>
    private static bool IsSerializable(Type type)
    {
        return type.GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0;
    }

    /// <summary>
    /// PlayableDirectorのTimelineを非同期で再生するメソッド
    /// </summary>
    /// <param name="playableDirector">非同期で再生したいタイムライン</param>
    /// <returns>Task</returns>
    public static async Task PlayTimeLineAsync(PlayableDirector playableDirector)
    {
        playableDirector.Play();

        while (playableDirector.state == PlayState.Playing)
        {
            await Task.Yield();
        }
    }


    public static readonly Vector3 POINT_NEMO = new(0,0,5);


}
