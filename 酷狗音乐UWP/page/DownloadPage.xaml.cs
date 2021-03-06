﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using KG_ClassLibrary;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using 酷狗音乐UWP.Class;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace 酷狗音乐UWP.page
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DownloadPage : Page
    {
        private BackgroundDownload.ResultData songresult;

        public ObservableCollection<DownloadManager.DownloadModel> SongDowns { get; private set; }

        public class txtdata
        {
            public string text { get; set; }
        }
        public DownloadPage()
        {
            this.InitializeComponent();
            init();
        }

        private async void init()
        {
            //await KG_ClassLibrary.BackgroundDownload.Start("123444.flac", "http://fs.pc.kugou.com/201607200123/005f3d8bbfdde9b4111164787f8a622b/G001/M07/07/1F/QQ0DAFSTAB6Aebl0AcQxqCQQQSM04.flac", KG_ClassLibrary.BackgroundDownload.DownloadType.song);
            GetSongDowning();
        }

        private async void GetSongDowning()
        {
            //await Task.Delay(3000);
            songresult= await BackgroundDownload.GetList(KG_ClassLibrary.BackgroundDownload.DownloadType.song);
            if(songresult!=null)
            {
                SongDowningList.ItemsSource = songresult.transfers;
                LocalSongList.Items.Clear();
                foreach (var file in songresult.files)
                {
                    LocalSongList.Items.Add(file);
                }
            }
        }

        private void TopBtnClicked(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
