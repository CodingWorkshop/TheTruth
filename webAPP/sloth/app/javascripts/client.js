!function(e){var t={};function o(n){if(t[n])return t[n].exports;var i=t[n]={i:n,l:!1,exports:{}};return e[n].call(i.exports,i,i.exports,o),i.l=!0,i.exports}o.m=e,o.c=t,o.d=function(e,t,n){o.o(e,t)||Object.defineProperty(e,t,{configurable:!1,enumerable:!0,get:n})},o.r=function(e){Object.defineProperty(e,"__esModule",{value:!0})},o.n=function(e){var t=e&&e.__esModule?function(){return e.default}:function(){return e};return o.d(t,"a",t),t},o.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},o.p="",o(o.s=0)}([function(e,t,o){"use strict";o.r(t);var n={Play:"播放",Pause:"暫停","Current Time":"目前時間","Duration Time":"總共時間","Remaining Time":"剩餘時間","Stream Type":"串流類型",LIVE:"直播",Loaded:"載入完畢",Progress:"進度",Fullscreen:"全螢幕","Non-Fullscreen":"退出全螢幕",Mute:"靜音",Unmute:"取消靜音","Playback Rate":" 播放速率",Subtitles:"字幕","subtitles off":"關閉字幕",Captions:"內嵌字幕","captions off":"關閉內嵌字幕",Chapters:"章節","Close Modal Dialog":"關閉對話框",Descriptions:"描述","descriptions off":"關閉描述","Audio Track":"音軌","You aborted the media playback":"影片播放已終止","A network error caused the media download to fail part-way.":"網路錯誤導致影片下載失敗。","The media could not be loaded, either because the server or network failed or because the format is not supported.":"影片因格式不支援或者伺服器或網路的問題無法載入。","The media playback was aborted due to a corruption problem or because the media used features your browser did not support.":"由於影片檔案損毀或是該影片使用了您的瀏覽器不支援的功能，播放終止。","No compatible source was found for this media.":"無法找到相容此影片的來源。","The media is encrypted and we do not have the keys to decrypt it.":"影片已加密，無法解密。","Play Video":"播放影片",Close:"關閉","Modal Window":"對話框","This is a modal window":"這是一個對話框","This modal can be closed by pressing the Escape key or activating the close button.":"可以按ESC按鍵或啟用關閉按鈕來關閉此對話框。",", opens captions settings dialog":", 開啟標題設定對話框",", opens subtitles settings dialog":", 開啟字幕設定對話框",", opens descriptions settings dialog":", 開啟描述設定對話框",", selected":", 選擇","captions settings":"字幕設定","Audio Player":"音頻播放器","Video Player":"視頻播放器",Replay:"重播","Progress Bar":"進度小節","Volume Level":"音量","subtitles settings":"字幕設定","descriptions settings":"描述設定",Text:"文字",White:"白",Black:"黑",Red:"紅",Green:"綠",Blue:"藍",Yellow:"黃",Magenta:"紫紅",Cyan:"青",Background:"背景",Window:"視窗",Transparent:"透明","Semi-Transparent":"半透明",Opaque:"不透明","Font Size":"字型尺寸","Text Edge Style":"字型邊緣樣式",None:"無",Raised:"浮雕",Depressed:"壓低",Uniform:"均勻",Dropshadow:"下陰影","Font Family":"字型庫","Proportional Sans-Serif":"比例無細體","Monospace Sans-Serif":"單間隔無細體","Proportional Serif":"比例細體","Monospace Serif":"單間隔細體",Casual:"輕便的",Script:"手寫體","Small Caps":"小型大寫字體",Reset:"重置","restore all settings to the default values":"恢復全部設定至預設值",Done:"完成","Caption Settings Dialog":"字幕設定視窗","Beginning of dialog window. Escape will cancel and close the window.":"開始對話視窗。離開會取消及關閉視窗","End of dialog window.":"結束對話視窗","Now Playing":"播放中","Untitled Video":"無標題影片","Up Next":"接著播放","Video Name":"影片名稱","Video Description":"影片描述"};var i={getAppConfig:function(e){return new Promise((t,o)=>{if(window.location.search){const o=window.location.search.substring(1).split("&"),n=Object.assign({},e);o.forEach(e=>{let t=e.split("=");n[t[0]]=t[1]}),t(n)}else t(e)})},getVideoList:function(e){return fetch(e,{method:"get"}).then(function(e){return e.json()}).catch(function(e){console.log(e);var t=[];for(let e=0;e<10;e++)t[e]={category:"video-"+e,date:new Date};return t})}},a=(()=>({showLoading:()=>{const e=document.getElementsByClassName("loading-mask-disable");e.length>0&&(e[0].className="loading-mask")},hideLoading:()=>{const e=document.getElementsByClassName("loading-mask");e.length>0&&(e[0].className="loading-mask-disable")}}))();o.d(t,"config",function(){return r}),o.d(t,"player",function(){return d});const s=function(e){const t=document.createElement("canvas");t.width=640,t.height=480;const o=t.getContext("2d");return o.fillStyle="#FFF",o.fillRect(0,0,t.width,t.height),o.fillStyle="#000",o.font="48px serif",o.fillText(e||"Video",10,50),t.toDataURL()}();a.showLoading();var r={};const l={webApiRoot:"http://127.0.0.1:5000",webApiGetVideoList:"/api/Video/GetVideoList",webApiPlayVideo:"/api/Video/PlayVideo",signalrApi:"/videohub",signalrChannelPlay:"play",defaultPoster:s,defaultType:"video/mp4",defaultName:n["Video Name"],defaultDescription:n["Video Description"]};var d={};videojs.addLanguage("zh-tw",n),d=videojs("video-learning-player",{language:"zh-tw"}),i.getAppConfig(l).then(function(e){var t,o;t=r=e,(o=new signalR.HubConnection(t.webApiRoot+t.signalrApi)).start().then(()=>{o.invoke("requestVideo")}),o.on("playVideo",function(e){console.log(e)}),a.hideLoading()})}]);