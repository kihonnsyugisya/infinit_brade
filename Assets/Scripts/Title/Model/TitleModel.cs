using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class TitleModel : MonoBehaviour
{
    public LanguageModel languageModel;

    /// <summary>
    /// メインシーンをプログレスバー付きでロードするメソッド
    /// </summary>
    /// <param name="slider">プログレスバー</param>
    public void LoadMainSceneWithProgress(Slider slider)
    {
        StartCoroutine(LoadSceneAsyncWithProgress("MainScene", slider));
    }

    /// <summary>
    /// プログレスバーを更新しながらシーンを非同期でロードするコルーチン
    /// </summary>
    /// <param name="sceneName">ロードするシーンの名前</param>
    /// <param name="slider">プログレスバー</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsyncWithProgress(string sceneName, Slider slider)
    {
        // シーンを非同期でロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // ロードが完了するまでループ
        while (!asyncLoad.isDone)
        {
            // プログレスバーの値を更新
            slider.value = asyncLoad.progress;
            yield return null;
        }
    }

    /// <summary>
    /// タスクの進捗に応じてローディング画面を表示および非表示にするメソッドです。
    /// </summary>
    /// <param name="task">進捗を監視するタスク。</param>
    /// <param name="slider">ローディング画面に表示する進捗バーのスライダー。</param>
    /// <param name="loadingPanel">ローディング画面のゲームオブジェクト。</param>
    /// <returns>コルーチン。</returns>
    public IEnumerator ShowLoadingScreen(Task task, Slider slider, GameObject loadingPanel)
    {
        // ローディング画面を表示
        loadingPanel.SetActive(true);

        // ローディング画面の進捗バーを更新するループ
        while (!task.IsCompleted)
        {
            // タスクの進捗を計算し、スライダーに反映する
            float progress = CalculateProgress(task);
            slider.value = progress;

            // 1フレーム待機
            yield return null;
        }

        // ローディングが完了したらローディング画面を非表示にする
        loadingPanel.SetActive(false);

        Debug.Log("dsf");
    }

    /// <summary>
    /// タスクの進捗を計算するメソッドです。
    /// </summary>
    /// <param name="task">進捗を計算するタスク。</param>
    /// <returns>計算された進捗率（0.0から1.0の間の値）。</returns>
    private float CalculateProgress(Task task)
    {
        // 仮の進捗計算例として、taskの完了状態と実行時間から進捗を計算する
        if (task.IsCompleted)
        {
            return 1.0f; // 完了時の進捗は1.0とする
        }
        else
        {
            Debug.Log(Time.time / 10.0f);
            // 実際の進捗計算ロジックをここに追加する
            return Time.time / 10.0f; // 例として、実行時間の10分の1を進捗とする
        }
    }
}
