using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class TitleView : MonoBehaviour
{
    public GameObject canvas;
    public GameObject loadingPanel;
    public TextMeshProUGUI netWorkErrorText;
    public Button tapToStartButtton;
    public Slider progressBar;
    [SerializeField] private PlayableDirector openingTimeLine;
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// openingをポーズする。
    /// バックグラウンド状態になった時にポーズしたい。
    /// ストアレビューボタン押された時に呼び出される想定
    /// </summary>
    public void PauseOpeningMovie()
    {
        if (openingTimeLine.state == PlayState.Playing)
        {
            openingTimeLine.Pause();
            audioSource.Pause();
        }
    }

    /// <summary>
    /// Timelineを再開するメソッド
    /// バックグラウンドからフォアグラウンドになった時に呼び出す。
    /// </summary>
    public void ResumeOpeningMovie()
    {
        if (openingTimeLine.state == PlayState.Paused)
        {
            openingTimeLine.Resume();
            audioSource.UnPause();
        }
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public TextMeshProUGUI tapStart; // 点滅させるTextコンポーネント
    private double _time; // 内部時刻
    [SerializeField] private float _cycle = 1.0f; // 点滅の周期（秒）

    /// <summary>
    /// TapStartの文字を点滅させる
    /// Updateで呼び出す
    /// </summary>
    public void OnOffTapStart()
    {
        // 内部時刻を経過させる
        _time += Time.deltaTime;

        // 周期cycleで繰り返す値の取得
        // 0～cycleの範囲の値が得られる
        var repeatValue = Mathf.Repeat((float)_time, _cycle);

        // 内部時刻timeにおける明滅状態を反映
        tapStart.enabled = repeatValue >= _cycle * 0.5f;
    }
}
