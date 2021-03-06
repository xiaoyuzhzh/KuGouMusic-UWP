﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Playback;
using Windows.Storage;

namespace 酷狗音乐UWP.Class
{
    public class Model
    {
        public class SearchResultModel
        {
            public class Song
            {
                public string filename { get; set; }
                public string mvview { get; set; }
                public string songname { get; set; }
                public string hash320 { get; set; }
                public string mvhash { get; set; }
                public string album_name { get; set; }
                public string topic { get; set; }
                public string album_id { get; set; }
                public string singername { get; set; }
                public string extname { get; set; }
                public string hash { get; set; }
                public string sqhash { get; set; }
                public int bitrate { get; set; }
                public int duration { get; set; }
            }
            public class MV
            {
                public string filename { get; set; }
                public string imgurl { get; set; }
                public string album_id { get; set; }
                public string hash { get; set; }
                public string singername { get; set; }
                public string intro { get; set; }
                public string topic { get; set; }
            }
            public class Album
            {
                public int albumid { get; set; }
                public int singerid { get; set; }
                public string albumname { get; set; }
                public string publishtime { get; set; }
                public string singername { get; set; }
                public string imgurl { get; set; }
                public string intro { get; set; }
            }
            public class SongList
            {
                public string specialname { get; set; }
                public string singername { get; set; }
                public string intro { get; set; }
                public string imgurl { get; set; }
                public string publishtime { get; set; }
                public string count { get; set; }
                public int suid { get; set; }
                public int specialid { get; set; }
                public int playcount { get; set; }
                public int collectcount { get; set; }
                public int slid { get; set; }
            }
            public class Lrc
            {
                public string filename { get; set; }
                public string singername { get; set; }
                public string hash { get; set; }
                public string extname { get; set; }
                public string mvhash { get; set; }
                public string hash320 { get; set; }
                public string sqhash { get; set; }
                public string lyric { get; set; }
                public int bitrate { get; set; }
                public int duration { get; set; }
            }
            public class Singer
            {
                public int singerid { get; set; }
                public int songcount { get; set; }
                public int albumcount { get; set; }
                public int mvcount { get; set; }
                public string singername { get; set; }
                public string intro { get; set; }
                public string imgurl { get; set; }
                public List<string> pics { get; set; }
            }
            public static async Task<ObservableCollection<Album>> GetAlbumResult(string keyword, int page)
            {
                try
                {
                    var httpclient = new Noear.UWP.Http.AsyncHttpClient();
                    httpclient.Url("http://mobilecdn.kugou.com/api/v3/search/album?pagesize=20&sver=2&page=" + page + "&version=8150&keyword=" + keyword);
                    var httpresult = await httpclient.Get();
                    var jsondata = httpresult.GetString();
                    jsondata = jsondata.Replace("{size}", "150");
                    jsondata = jsondata.Replace("00:00:00", "");
                    var obj = Windows.Data.Json.JsonObject.Parse(jsondata);
                    var arry = obj.GetNamedObject("data").GetNamedArray("info");
                    var resultdata = new ObservableCollection<Class.Model.SearchResultModel.Album>();
                    foreach (var item in arry)
                    {
                        resultdata.Add(Class.data.DataContractJsonDeSerialize<Class.Model.SearchResultModel.Album>(item.ToString()));
                    }
                    return resultdata;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static async Task<ObservableCollection<SongList>> GetSonglistResult(string keyword, int page)
            {
                try
                {
                    var httpclient = new Noear.UWP.Http.AsyncHttpClient();
                    httpclient.Url("http://mobilecdn.kugou.com/api/v3/search/special?pagesize=20&sver=2&page=" + page + "&version=8150&keyword=" + keyword);
                    var httpresult = await httpclient.Get();
                    var jsondata = httpresult.GetString();
                    jsondata = jsondata.Replace("{size}", "150");
                    var obj = Windows.Data.Json.JsonObject.Parse(jsondata);
                    var arry = obj.GetNamedObject("data").GetNamedArray("info");
                    var resultdata = new ObservableCollection<Class.Model.SearchResultModel.SongList>();
                    foreach (var item in arry)
                    {
                        var tempdata = Class.data.DataContractJsonDeSerialize<Class.Model.SearchResultModel.SongList>(item.ToString());
                        if (tempdata.playcount > 1000)
                        {
                            tempdata.count = (tempdata.playcount / 10000).ToString() + "万";
                        }
                        else
                        {
                            tempdata.count = tempdata.playcount.ToString();
                        }
                        resultdata.Add(tempdata);
                    }
                    return resultdata;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static async Task<ObservableCollection<Lrc>> GetLrcResult(string keyword, int page)
            {
                try
                {
                    var httpclient = new Noear.UWP.Http.AsyncHttpClient();
                    httpclient.Url("http://mobilecdn.kugou.com/api/v3/lyric/search?pagesize=20&plat=0&page=" + page + "&version=8150&keyword=" + keyword);
                    var httpresult = await httpclient.Get();
                    var jsondata = httpresult.GetString();
                    jsondata = jsondata.Replace("320hash", "hash320");
                    var obj = Windows.Data.Json.JsonObject.Parse(jsondata);
                    var arry = obj.GetNamedObject("data").GetNamedArray("info");
                    var resultdata = new ObservableCollection<Class.Model.SearchResultModel.Lrc>();
                    foreach (var item in arry)
                    {
                        resultdata.Add(Class.data.DataContractJsonDeSerialize<Class.Model.SearchResultModel.Lrc>(item.ToString()));
                    }
                    return resultdata;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static async Task<Singer> GetSingerResult(string singername)
            {
                try
                {
                    var httpclient = new Noear.UWP.Http.AsyncHttpClient();
                    httpclient.Url("http://mobilecdn.kugou.com/api/v3/singer/info?singername=" + singername);
                    var httpresult = await httpclient.Get();
                    var jsondata = httpresult.GetString();
                    jsondata = jsondata.Replace("{size}", "150");
                    var obj = Windows.Data.Json.JsonObject.Parse(jsondata).GetNamedObject("data").ToString();
                    var resultdata = Class.data.DataContractJsonDeSerialize<Model.SearchResultModel.Singer>(obj);
                    var pics= await GetSingerPics(resultdata.singerid);
                    if (pics != null)
                    {
                        resultdata.pics = pics;
                    }
                    return resultdata;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            private static async Task<List<string>> GetSingerPics(int singerid)
            {
                try
                {
                    var httpclient = new Windows.Web.Http.HttpClient();
                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    var time = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                    var key = Class.MD5.GetMd5String("1005Ilwieks28dk2k092lksi2UIkp8150" + time);
                    var postobj = new JsonObject();
                    var array = new JsonArray();
                    var videodata = new JsonObject();
                    videodata.Add("author_name", JsonValue.CreateStringValue(""));
                    videodata.Add("author_id", JsonValue.CreateNumberValue(singerid));
                    array.Add(videodata);
                    postobj.Add("data", array);
                    postobj.Add("appid", JsonValue.CreateStringValue("1005"));
                    postobj.Add("mid", JsonValue.CreateStringValue(""));
                    postobj.Add("type", JsonValue.CreateNumberValue(5));
                    postobj.Add("clienttime", JsonValue.CreateStringValue("1469035332000"));
                    postobj.Add("key", JsonValue.CreateStringValue("27b498a7d890373fadb673baa1dabf7e"));
                    postobj.Add("clientver", JsonValue.CreateStringValue("8150"));
                    var postdata = new Windows.Web.Http.HttpStringContent(postobj.ToString());
                    var result = await httpclient.PostAsync(new Uri("http://kmr.service.kugou.com/v1/author_image/author"), postdata);
                    var json = await result.Content.ReadAsStringAsync();
                    var obj=JObject.Parse(json);
                    var piclist = new List<string>();
                    for (int i = 0; i < obj["data"][0][0]["imgs"]["5"].Count(); i++)
                    {
                        piclist.Add(obj["data"][0][0]["imgs"]["5"][i]["filename"].ToString());
                    }
                    for (int i = 0; i < piclist.Count; i++)
                    {
                        var head = "http://singerimg.kugou.com/uploadpic/mobilehead/{size}/";
                        for (int j = 0; j < 8; j++)
                        {
                            head = head + (piclist[i])[j].ToString();
                        }
                        piclist[i] = head + "/" + piclist[i];
                    }
                    return piclist;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public class Player
        {
            public class NowPlay
            {
                public string title { get; set; }
                public string singername { get; set; }
                public string url { get; set; }
                public string imgurl { get; set; }
                public string albumid { get; set; }
                public string hash { get; set; }
                public List<string> pics { get; set; }
            }
            public static async Task<NowPlay> GetNowPlay()
            {
                try
                {
                    var datafolder= await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var nowplayfile = await datafolder.GetFileAsync("nowplay.json");
                    var json = await FileIO.ReadTextAsync(nowplayfile);
                    var nowplay = Class.data.DataContractJsonDeSerialize<NowPlay>(json);
                    return nowplay;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static async Task SetNowPlay(NowPlay nowplay)
            {
                try
                {
                    var json = Class.data.ToJsonData(nowplay);
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var nowplayfile = await datafolder.GetFileAsync("nowplay.json");
                    await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, json);
                }
                catch (Exception)
                {
                    
                }
            }
        }
        public class PlayList
        {
            public int nowplay { get; set; }
            public List<Player.NowPlay> SongList { get; set; }
            public cycling cyc { get; set; }
            public enum cycling
            {
                单曲循环,列表循环,随机播放
            }
            public static async Task<PlayList> GetPlayList()
            {
                try
                {
                    var list = new PlayList();
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    var json = await Windows.Storage.FileIO.ReadTextAsync(listfile);
                    list = Class.data.DataContractJsonDeSerialize<PlayList>(json);
                    try
                    {
                        if(list.nowplay>=list.SongList.Count)
                        {
                            list.nowplay = 0;
                            await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                    return list;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public static async Task<Player.NowPlay> GetNowPlay()
            {
                try
                {
                    var list = new PlayList();
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    var json = await Windows.Storage.FileIO.ReadTextAsync(listfile);
                    list = Class.data.DataContractJsonDeSerialize<PlayList>(json);
                    return list.SongList[list.nowplay];
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public static async Task Add(Player.NowPlay nowplay,bool isplay)
            {
                try
                {
                    var datafolder= await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    var list = new PlayList();
                    var json = await Windows.Storage.FileIO.ReadTextAsync(listfile);
                    list = Class.data.DataContractJsonDeSerialize<PlayList>(json);
                    list.SongList.Add(nowplay);
                    await Windows.Storage.FileIO.WriteTextAsync(listfile,Class.data.ToJsonData(list));
                    if (isplay)
                    {
                        PlayAt(list.SongList.Count-1);
                    }
                }
                catch (Exception)
                {
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    var list = new PlayList();
                    list.nowplay = new int();
                    list.nowplay = 0;
                    list.SongList = new List<Player.NowPlay>();
                    list.SongList.Add(nowplay);
                    list.cyc = new cycling();
                    list.cyc = cycling.列表循环;
                    await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                    Play();
                }
            }

            public static async Task PlayAt(int nowplay)
            {
                try
                {
                    var list = await GetPlayList();
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    list.nowplay = nowplay;
                    await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                    Play();
                }
                catch (Exception)
                {
                    
                }
            }

            public static async Task SetCycling(cycling cyc)
            {
                try
                {
                    var list = await GetPlayList();
                    list.cyc = cyc;
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                }
                catch (Exception)
                {
                    
                }
            }

            public static async Task Del(int nowplay)
            {
                var list = await GetPlayList();
                var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                var listfile = await datafolder.GetFileAsync("playlist.json");
                var json = await Windows.Storage.FileIO.ReadTextAsync(listfile);
                list = Class.data.DataContractJsonDeSerialize<PlayList>(json);
                if(list.SongList!=null)
                {
                    if (list.SongList.Count < 1)
                    {
                        await Clear();
                        return;
                    }
                    else
                    {
                        if(await IsNowPlay(list.SongList[nowplay]))
                        {
                            await Next();
                            list.SongList.RemoveAt(nowplay);
                        }
                        else
                        {
                            list.SongList.RemoveAt(nowplay);
                        }
                    }
                }
                await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                return;
            }

            public static async Task Clear()
            {
                try
                {
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    await Windows.Storage.FileIO.WriteTextAsync(listfile, "");
                    BackgroundMediaPlayer.Current.Pause();
                }
                catch (Exception)
                {
                    
                }
            }

            public static async Task Play()
            {
                try
                {
                    var stmp = SystemMediaTransportControls.GetForCurrentView();
                    var mediaplayer = BackgroundMediaPlayer.Current;
                    var list = await GetPlayList();
                    var now = list.nowplay;
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var nowplayfile = await datafolder.GetFileAsync("nowplay.json");
                    await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, Class.data.ToJsonData(list.SongList[now]));
                    stmp.DisplayUpdater.ClearAll();
                    stmp.DisplayUpdater.Type = MediaPlaybackType.Music;
                    stmp.DisplayUpdater.MusicProperties.Title = list.SongList[now].title;
                    stmp.DisplayUpdater.MusicProperties.Artist = list.SongList[now].singername;
                    stmp.DisplayUpdater.Update();
                    LoadUrl(list.SongList[now].url);
                }
                catch (Exception)
                {
                    
                }
            }
            public static async Task Next()
            {
                try
                {
                    var stmp = SystemMediaTransportControls.GetForCurrentView();
                    var mediaplayer = BackgroundMediaPlayer.Current;
                    var list = await PlayList.GetPlayList();
                    var now = list.nowplay;
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var nowplayfile = await datafolder.GetFileAsync("nowplay.json");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    switch (list.cyc)
                    {
                        case PlayList.cycling.单曲循环:
                            await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, Class.data.ToJsonData(list.SongList[now]));
                            stmp.DisplayUpdater.ClearAll();
                            stmp.DisplayUpdater.Type = MediaPlaybackType.Music;
                            stmp.DisplayUpdater.MusicProperties.Title = list.SongList[now].title;
                            stmp.DisplayUpdater.MusicProperties.Artist = list.SongList[now].singername;
                            stmp.DisplayUpdater.Update();
                            LoadUrl(list.SongList[now].url);
                            break;
                        case PlayList.cycling.列表循环:
                            if (now == list.SongList.Count - 1)
                            {
                                now = 0;
                            }
                            else
                            {
                                now = now + 1;
                            }
                            list.nowplay = now;
                            await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, Class.data.ToJsonData(list.SongList[now]));
                            stmp.DisplayUpdater.ClearAll();
                            stmp.DisplayUpdater.Type = MediaPlaybackType.Music;
                            stmp.DisplayUpdater.MusicProperties.Title = list.SongList[now].title;
                            stmp.DisplayUpdater.MusicProperties.Artist = list.SongList[now].singername;
                            stmp.DisplayUpdater.Update();
                            LoadUrl(list.SongList[now].url);
                            await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                            break;
                        case PlayList.cycling.随机播放:
                                now = new Random().Next(0, list.SongList.Count);
                                list.nowplay = now;
                                await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, Class.data.ToJsonData(list.SongList[now]));
                                stmp.DisplayUpdater.ClearAll();
                                stmp.DisplayUpdater.Type = MediaPlaybackType.Music;
                                stmp.DisplayUpdater.MusicProperties.Title = list.SongList[now].title;
                                stmp.DisplayUpdater.MusicProperties.Artist = list.SongList[now].singername;
                                stmp.DisplayUpdater.Update();
                                LoadUrl(list.SongList[now].url);
                                await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {

                }
            }
            public static async Task Previous()
            {
                try
                {
                    var stmp = SystemMediaTransportControls.GetForCurrentView();
                    var mediaplayer = BackgroundMediaPlayer.Current;
                    var list = await PlayList.GetPlayList();
                    var now = list.nowplay;
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var nowplayfile = await datafolder.GetFileAsync("nowplay.json");
                    var listfile = await datafolder.GetFileAsync("playlist.json");
                    switch (list.cyc)
                    {
                        case PlayList.cycling.单曲循环:
                            await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, Class.data.ToJsonData(list.SongList[now]));
                            stmp.DisplayUpdater.ClearAll();
                            stmp.DisplayUpdater.Type = MediaPlaybackType.Music;
                            stmp.DisplayUpdater.MusicProperties.Title = list.SongList[now].title;
                            stmp.DisplayUpdater.MusicProperties.Artist = list.SongList[now].singername;
                            stmp.DisplayUpdater.Update();
                            LoadUrl(list.SongList[now].url);
                            break;
                        case PlayList.cycling.列表循环:
                            if (now == 0)
                            {
                                now = list.SongList.Count - 1;
                            }
                            else
                            {
                                now = now - 1;
                            }
                            list.nowplay = now;
                            await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, Class.data.ToJsonData(list.SongList[now]));
                            stmp.DisplayUpdater.ClearAll();
                            stmp.DisplayUpdater.Type = MediaPlaybackType.Music;
                            stmp.DisplayUpdater.MusicProperties.Title = list.SongList[now].title;
                            stmp.DisplayUpdater.MusicProperties.Artist = list.SongList[now].singername;
                            stmp.DisplayUpdater.Update();
                            LoadUrl(list.SongList[now].url);
                            await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                            break;
                        case PlayList.cycling.随机播放:
                            now = new Random().Next(0,list.SongList.Count);
                            list.nowplay = now;
                            await Windows.Storage.FileIO.WriteTextAsync(nowplayfile, Class.data.ToJsonData(list.SongList[now]));
                            stmp.DisplayUpdater.ClearAll();
                            stmp.DisplayUpdater.Type = MediaPlaybackType.Music;
                            stmp.DisplayUpdater.MusicProperties.Title = list.SongList[now].title;
                            stmp.DisplayUpdater.MusicProperties.Artist = list.SongList[now].singername;
                            stmp.DisplayUpdater.Update();
                            LoadUrl(list.SongList[now].url);
                            await Windows.Storage.FileIO.WriteTextAsync(listfile, Class.data.ToJsonData(list));
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {

                }
            }
            private static void LoadUrl(string url)
            {
                try
                {
                    var data = new ValueSet();
                    data["path"] = url;
                    BackgroundMediaPlayer.SendMessageToBackground(data);
                }
                catch (Exception)
                {
                    
                }
            }
            private static async Task<bool> IsNowPlay(Player.NowPlay nowplay)
            {
                try
                {
                    var list = await GetPlayList();
                    if (list.SongList[list.nowplay].title == nowplay.title && list.SongList[list.nowplay].singername == nowplay.singername)
                    {
                        return true ;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public class LocalList
        {
            public class MusicInGroup
            {
                public string key { get; set; }
                public List<Music> MusicContent { get; set; }
            }
            public class Music
            {
                public string Title { get; set; }
                public string songer { get; set; }
                public string isplay { get; set; }
                public string Content { get; set; }
                public string path { get; set; }
            }
            public static async Task<List<MusicInGroup>> GetList()
            {
                try
                {
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var locallistfile = await datafolder.GetFileAsync("locallist.json");
                    var json = await FileIO.ReadTextAsync(locallistfile);
                    var list = Class.data.DataContractJsonDeSerialize<List<MusicInGroup>>(json);
                    return list;
                }
                catch (Exception)
                {
                    await UpdateList();
                    return await GetList();
                }
            }
            public static async Task UpdateList()
            {
                var files = new List<StorageFile>();
                var mainItem = new List<Music>();
                var folders = await GetFolder();
                var musics= (await KnownFolders.MusicLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList();
                files.AddRange(musics);
                if(folders!=null&&folders.Count>0)
                {
                    foreach (var folder in folders)
                    {
                        var folder1 = await StorageFolder.GetFolderFromPathAsync(folder);
                        var files1 = (await folder1.GetFilesAsync()).ToList();
                        foreach (var file in files1)
                        {
                            if (file.ContentType.Contains("audio"))
                            {
                                files.Add(file);
                            }
                        }
                    }
                }
                if(files.Count>0)
                {
                    foreach (var item in files)
                    {
                        if(item.ContentType.Contains("audio"))
                        {
                            string firstword = "";
                            if ((await item.Properties.GetMusicPropertiesAsync()).Title != "")
                            {
                                try
                                {
                                    firstword = PinYIn.SpellCodeHelper.GetFirstPYLetter((await item.Properties.GetMusicPropertiesAsync()).Title[0].ToString());
                                }
                                catch (Exception)
                                {
                                    firstword = (await item.Properties.GetMusicPropertiesAsync()).Title[0].ToString();
                                }
                            }
                            else
                            {
                                try
                                {
                                    firstword = PinYIn.SpellCodeHelper.GetFirstPYLetter(item.DisplayName[0].ToString());
                                }
                                catch (Exception)
                                {
                                    firstword = item.DisplayName[0].ToString();
                                }
                            }
                            if ((await item.Properties.GetMusicPropertiesAsync()).AlbumArtist != "")
                            {
                                if ((await item.Properties.GetMusicPropertiesAsync()).Title == "")
                                {
                                    mainItem.Add(new Music() { Content = firstword, Title = item.DisplayName, path = item.Path, songer = (await item.Properties.GetMusicPropertiesAsync()).AlbumArtist, isplay = "Collapsed" });
                                }
                                else
                                {
                                    mainItem.Add(new Music() { Content = firstword, path = item.Path, Title = (await item.Properties.GetMusicPropertiesAsync()).Title, songer = (await item.Properties.GetMusicPropertiesAsync()).AlbumArtist, isplay = "Collapsed" });
                                }

                            }
                            else
                            {
                                if ((await item.Properties.GetMusicPropertiesAsync()).Title == "")
                                {
                                    mainItem.Add(new Music() { Content = firstword, path = item.Path, Title = item.DisplayName, songer = "未知歌手", isplay = "Collapsed" });
                                }
                                else
                                {
                                    mainItem.Add(new Music() { Content = firstword, path = item.Path, Title = (await item.Properties.GetMusicPropertiesAsync()).Title, songer = "未知歌手", isplay = "Collapsed" });
                                }
                            }
                        }
                    }
                }
                List<MusicInGroup> Items = (from item in mainItem group item by item.Content into newItems select new MusicInGroup { key = newItems.Key, MusicContent = newItems.ToList() }).ToList();
                var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                var locallistfile = await datafolder.GetFileAsync("locallist.json");
                await FileIO.WriteTextAsync(locallistfile, Class.data.ToJsonData(Items));
            }
            public static async Task AddFolder()
            {
                var picker = new Windows.Storage.Pickers.FolderPicker();
                var folder = await picker.PickSingleFolderAsync();
                if (folder != null)
                {
                    var list = await GetFolder();
                    if (list != null)
                    {
                        list.Add(folder.Path);
                    }
                    else
                    {
                        list = new List<string>();
                        list.Add(folder.Path);
                    }
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var localfoldersfile = await datafolder.GetFileAsync("localfolders.json");
                    await FileIO.WriteTextAsync(localfoldersfile, Class.data.ToJsonData(list));
                }
            }
            public static async Task<List<StorageFile>> GetFiles()
            {
                var files = new List<StorageFile>();
                var mainItem = new List<Music>();
                var folders = await GetFolder();
                var musics = (await KnownFolders.MusicLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList();
                files.AddRange(musics);
                if (folders != null && folders.Count > 0)
                {
                    foreach (var folder in folders)
                    {
                        var files1 = (await(await StorageFolder.GetFolderFromPathAsync(folder)).GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList();
                        foreach (var file in files1)
                        {
                            if (file.ContentType.Contains("audio"))
                            {
                                files.Add(file);
                            }
                        }
                    }
                }
                return files;
            }
            public static async Task<List<string>> GetFolder()
            {
                var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                var localfoldersfile = await datafolder.GetFileAsync("localfolders.json");
                var json = await FileIO.ReadTextAsync(localfoldersfile);
                try
                {
                    var folders = Class.data.DataContractJsonDeSerialize<List<string>>(json);
                    return folders;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static async Task DelFolder(int num)
            {
                try
                {
                    var list = await GetFolder();
                    list.RemoveAt(num);
                    var datafolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                    var localfoldersfile = await datafolder.GetFileAsync("localfolders.json");
                    await FileIO.WriteTextAsync(localfoldersfile, Class.data.ToJsonData(list));
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
