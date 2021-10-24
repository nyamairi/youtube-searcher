# youtube-searcher

- コマンドラインで1つの引数を指定して起動する
- 引数で指定されたキーワードを使ってYouTubeの動画を検索し、リストで出力する

## 実行方法

Windows, MacOS, Linux それぞれの環境用にバイナリが用意されています。

### Windows

```sh
publish/win-x64/YouTubeSearcher "keyword"
```

### MacOS

```sh
publish/osx-x64/YouTubeSearcher "keyword"
```

### Linux

```sh
publish/linux-x64/YouTubeSearcher "keyword"
```

### Docker

Dockerfile もあります。

```sh
docker build -f YouTubeSearcher/Dockerfile -t yts .
docker run --rm yts "keyword"
```
