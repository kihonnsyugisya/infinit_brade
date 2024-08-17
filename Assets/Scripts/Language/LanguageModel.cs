using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageModel : MonoBehaviour
{
    [HideInInspector] public static Language currentLanguage;

    private const string SAVE_LANGUAGE_KEY = "SAVE_LANGUAGE_KEY";

    // UnityのSystemLanguageと言語のマッピングを定義する辞書
    private static Dictionary<SystemLanguage, Language> systemLanguageMapping = new()
    {
        { SystemLanguage.Japanese, Language.ja }, // 日本語
        { SystemLanguage.English, Language.en }, // 英語
        { SystemLanguage.Chinese, Language.zh }, // 中国
        { SystemLanguage.ChineseSimplified, Language.zh }, // 中国
        { SystemLanguage.ChineseTraditional, Language.zh }, // 中国
        { SystemLanguage.Hindi, Language.hi }, // インド
        { SystemLanguage.Indonesian, Language.id }, // インドネシア
        { SystemLanguage.Belarusian, Language.bz }, // ブラジル
        { SystemLanguage.Russian, Language.ru }, // ロシア語

        //{ SystemLanguage.Spanish, Language.es }, // スペイン語
        //{ SystemLanguage.French, Language.fr }, // フランス語
        //{ SystemLanguage.German, Language.de }, // ドイツ語
        //{ SystemLanguage.Portuguese, Language.pt }, // ポルトガル語
    };


    /// <summary>
    /// 選択された言語を保存するメソッド。
    /// </summary>
    private void SaveLanguage(Language language)
    {
        PlayerPrefs.SetString(SAVE_LANGUAGE_KEY, language.ToString());
    }

    /// <summary>
    /// プレイヤーが選択した言語をロードするメソッド。
    /// </summary>
    /// <returns>プレイヤーが選択した言語を表す列挙型 Language。保存された言語が存在しない場合はデバイスの言語を返す。</returns>
    private static Language LoadLanguage()
    {
        // PlayerPrefsから保存された言語をロードする
        string loadString = PlayerPrefs.GetString(SAVE_LANGUAGE_KEY);
        Language language;

        // ロードした文字列をEnumとしてパースし、言語を取得する
        if (Enum.TryParse<Language>(loadString, out language))
        {
            return language;
        }
        else
        {
            // 保存された言語が存在しない場合はデバイスの言語を返す
            Debug.LogWarning("保存された言語が存在しないため、デバイスの言語を使用します。");
            return GetDeviceLanguage();
        }
    }


    /// <summary>
    /// 指定したキーに対応するロケールに言語を変更する非同期メソッド。
    /// </summary>
    private async Task ChangeLocale(string key)
    {
        Debug.Log($"変更しようとしているキー: {key}");

        // 初期化が完了するのを待つ
        await LocalizationSettings.InitializationOperation.Task;

        //// 利用可能なロケールとそのコードをすべてログに出力
        //foreach (var loc in LocalizationSettings.AvailableLocales.Locales)
        //{
        //    Debug.Log($"利用可能なロケール: {loc.Identifier.Code}");
        //}

        // 指定されたキーに対応するロケールを検索
        var locale = LocalizationSettings.AvailableLocales.Locales.Find((x) => x.Identifier.Code == key);
        if (locale != null)
        {
            Debug.Log($"一致するロケールが見つかりました: {locale.Identifier.Code}");
        }
        else
        {
            Debug.LogWarning($"一致するロケールが見つかりませんでした: {key}");
        }

        // ロケールを設定し、初期化の完了を待つ
        LocalizationSettings.SelectedLocale = locale;
        await LocalizationSettings.InitializationOperation.Task;
        Debug.Log($"選択されたロケール: {LocalizationSettings.SelectedLocale.Identifier.Code}");
    }

    /// <summary>
    /// ロードされた言語を反映し、ロケールを変更する非同期メソッド。これをとりあえず最初に読んでおけばなんとかなる。
    /// ロードされた言語はプレイヤーの選択により保存された言語を取得し、現在の言語として設定します。
    /// ロケールの変更はUnityのローカライズシステムを使用し、言語に応じたテキストやリソースの表示を切り替えます。
    /// ロードに失敗した場合はデバイスの言語を使用してロケールを変更します。
    /// </summary>
    public async Task ReflectionLoadLanguageAsync()
    {
        // ロードされた言語を取得する
        Language loadLanguage = LoadLanguage();

        if (loadLanguage == Language.en)
        {
            // ロードに失敗した場合はデバイスの言語を使用する
            Debug.Log("保存された言語が存在しないため、デバイスの言語を使用します。");
            loadLanguage = GetDeviceLanguage();
        }

        // カレント言語をロードされた言語に設定する
        currentLanguage = loadLanguage;

        // ロケールを変更して反映する
        Debug.Log(currentLanguage + "をカレント言語に設定しました。");
        await ChangeLocale(loadLanguage.ToString());
    }


    /// <summary>
    /// 保存された言語を反映する非同期メソッド。
    /// </summary>
    public async Task ReflectionSaveLanguageAsync(Language language)
    {
        SaveLanguage(language);
        currentLanguage = language;
        await ChangeLocale(language.ToString());
    }

    /// <summary>
    /// デバイスの言語を取得するメソッド。
    /// </summary>
    /// <returns>デバイスの言語を表す列挙型 Language。マッピングが存在しない場合はデフォルトで英語を返す。</returns>
    private static Language GetDeviceLanguage()
    {
        // Application.systemLanguageがマッピングされている場合はその言語を返す
        if (systemLanguageMapping.TryGetValue(Application.systemLanguage, out Language language))
        {
            Debug.Log(language + "があなたのデバイス言語でした。");
            return language;
        }
        else
        {
            // デバイス言語がマッピングされていない場合はデフォルトで英語を返す
            Debug.LogWarning("デバイスの言語がマッピングされていません。デフォルトで英語を使用します。");

            // デフォルトの言語として英語を返す
            return Language.en;
        }
    }


}
