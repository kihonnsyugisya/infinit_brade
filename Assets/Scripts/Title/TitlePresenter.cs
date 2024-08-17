using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GoogleMobileAds.Sample;
using UniRx;
using GoogleMobileAds.Samples;

public class TitlePresenter : MonoBehaviour
{
    public TitleView titleView;
    public TitleModel titleModel;
    public InterstitialAdController interstitialAdController;

    // Start is called before the first frame update
    void Start()
    {

        // 言語のロードを非同期で行う
        StartCoroutine(titleModel.ShowLoadingScreen(titleModel.languageModel.ReflectionLoadLanguageAsync(), titleView.progressBar, titleView.loadingPanel));

        interstitialAdController.isReady.Subscribe(value => {
            //ネット接続ない場合（インステロードしてエラー返ってきた場合）はゲームをプレイさせない
            titleView.netWorkErrorText.gameObject.SetActive(!value);
            //titleView.tapStart.gameObject.SetActive(value);
            titleView.tapToStartButtton.interactable = value;

        }).AddTo(this);


        titleView.tapToStartButtton.onClick.AddListener(() =>
        {
            titleView.PauseOpeningMovie();
            //広告のストックがなかった場合は仕方ないのでインステを流さずにげーむ画面に遷移させる
            if (interstitialAdController.isSkipAd)
            {
                titleView.ShowLoadingPanel();
                titleModel.LoadMainSceneWithProgress(titleView.progressBar);
            }
            else
            {
                interstitialAdController.ShowAd();
            }
        });

        interstitialAdController.isAdClosed.Subscribe(value =>
        {
            if (value)
            {
                titleView.ShowLoadingPanel();
                titleModel.LoadMainSceneWithProgress(titleView.progressBar);
            }
        }).AddTo(this);


        ////////////////////////////////////////////////////////////////////////////////
        ///デバッグ中
        //titleView.tapToStartButtton.onClick.AddListener(() => {
        //    titleView.PauseOpeningMovie();
        //    titleView.ShowLoadingPanel();
        //    titleModel.LoadMainSceneWithProgress(titleView.progressBar);
        //});
        ////////////////////////////////////////////////////////////////////////////////

        GoogleMobileAdsConsentController.isConsentFormVisible.Subscribe(value => {
            // GDRPの同意画面とUIが重ならないように
            titleView.canvas.SetActive(!value);
        }).AddTo(this);
    }

    private void Update()
    {
        //if (GoogleMobileAdsController._isInitialized == true)
        //{
        //    titleView.tapToStartButtton.interactable = true;
        //}
        //else
        //{
        //    titleView.tapToStartButtton.interactable = false;
        //}
        if (interstitialAdController.isReady.Value == false) return;
        //文字を点滅させる
        titleView.OnOffTapStart();
    }

    /// <summary>
    /// バックグラウンド、フォアグラウンドの状態を監視するライフサイクルメソッド
    /// </summary>
    /// <param name="pauseStatus"></param>
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // アプリがバックグラウンドに移行した時の処理
            titleView.PauseOpeningMovie();
        }
        else
        {
            // アプリがフォアグラウンドに戻った時の処理
            titleView.ResumeOpeningMovie();
        }
    }
}
