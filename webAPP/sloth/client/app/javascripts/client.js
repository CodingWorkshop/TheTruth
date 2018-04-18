!function(t){var e={};function o(n){if(e[n])return e[n].exports;var s=e[n]={i:n,l:!1,exports:{}};return t[n].call(s.exports,s,s.exports,o),s.l=!0,s.exports}o.m=t,o.c=e,o.d=function(t,e,n){o.o(t,e)||Object.defineProperty(t,e,{configurable:!1,enumerable:!0,get:n})},o.r=function(t){Object.defineProperty(t,"__esModule",{value:!0})},o.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return o.d(e,"a",e),e},o.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},o.p="",o(o.s=10)}([function(t,e,o){"use strict";var n;o.d(e,"a",function(){return n}),function(t){t[t.Trace=0]="Trace",t[t.Information=1]="Information",t[t.Warning=2]="Warning",t[t.Error=3]="Error",t[t.None=4]="None"}(n||(n={}))},function(t,e,o){"use strict";var n=o(2),s=o(0);class r{constructor(){this.isAborted=!1}abort(){this.isAborted||(this.isAborted=!0,this.onabort&&this.onabort())}get signal(){return this}get aborted(){return this.isAborted}}o.d(e,"c",function(){return i}),o.d(e,"d",function(){return l}),o.d(e,"b",function(){return h}),o.d(e,"a",function(){return u});var i,a,c=function(t,e,o,n){return new(o||(o=Promise))(function(s,r){function i(t){try{c(n.next(t))}catch(t){r(t)}}function a(t){try{c(n.throw(t))}catch(t){r(t)}}function c(t){t.done?s(t.value):new o(function(e){e(t.value)}).then(i,a)}c((n=n.apply(t,e||[])).next())})};(a=i||(i={}))[a.WebSockets=0]="WebSockets",a[a.ServerSentEvents=1]="ServerSentEvents",a[a.LongPolling=2]="LongPolling";class l{constructor(t,e){this.logger=e,this.accessTokenFactory=t||(()=>null)}connect(t,e,o){return new Promise((o,n)=>{t=t.replace(/^http/,"ws");let r=this.accessTokenFactory();r&&(t+=(t.indexOf("?")<0?"?":"&")+`access_token=${encodeURIComponent(r)}`);let i=new WebSocket(t);2==e&&(i.binaryType="arraybuffer"),i.onopen=(n=>{this.logger.log(s.a.Information,`WebSocket connected to ${t}`),this.webSocket=i,o(e)}),i.onerror=(t=>{n()}),i.onmessage=(t=>{this.logger.log(s.a.Trace,`(WebSockets transport) data received: ${t.data}`),this.onreceive&&this.onreceive(t.data)}),i.onclose=(t=>{this.onclose&&this.webSocket&&(!1===t.wasClean||1e3!==t.code?this.onclose(new Error(`Websocket closed with status code: ${t.code} (${t.reason})`)):this.onclose())})})}send(t){return this.webSocket&&this.webSocket.readyState===WebSocket.OPEN?(this.webSocket.send(t),Promise.resolve()):Promise.reject("WebSocket is not in the OPEN state")}stop(){return this.webSocket&&(this.webSocket.close(),this.webSocket=null),Promise.resolve()}}class h{constructor(t,e,o){this.httpClient=t,this.accessTokenFactory=e||(()=>null),this.logger=o}connect(t,e,o){return"undefined"==typeof EventSource&&Promise.reject("EventSource not supported by the browser."),this.url=t,new Promise((e,o)=>{let n=this.accessTokenFactory();n&&(t+=(t.indexOf("?")<0?"?":"&")+`access_token=${encodeURIComponent(n)}`);let r=new EventSource(t);try{r.onmessage=(t=>{if(this.onreceive)try{this.logger.log(s.a.Trace,`(SSE transport) data received: ${t.data}`),this.onreceive(t.data)}catch(t){return void(this.onclose&&this.onclose(t))}}),r.onerror=(t=>{o(),this.eventSource&&this.onclose&&this.onclose(new Error(t.message||"Error occurred"))}),r.onopen=(()=>{this.logger.log(s.a.Information,`SSE connected to ${this.url}`),this.eventSource=r,e(1)})}catch(t){return Promise.reject(t)}})}send(t){return c(this,void 0,void 0,function*(){return d(this.httpClient,this.url,this.accessTokenFactory,t)})}stop(){return this.eventSource&&(this.eventSource.close(),this.eventSource=null),Promise.resolve()}}class u{constructor(t,e,o){this.httpClient=t,this.accessTokenFactory=e||(()=>null),this.logger=o,this.pollAbort=new r}connect(t,e,o){if(this.url=t,o.features.inherentKeepAlive=!0,2===e&&"string"!=typeof(new XMLHttpRequest).responseType)throw new Error("Binary protocols over XmlHttpRequest not implementing advanced features are not supported.");return this.poll(this.url,e),Promise.resolve(e)}poll(t,e){return c(this,void 0,void 0,function*(){let o={timeout:12e4,abortSignal:this.pollAbort.signal,headers:new Map};2===e&&(o.responseType="arraybuffer");let r=this.accessTokenFactory();for(r&&o.headers.set("Authorization",`Bearer ${r}`);!this.pollAbort.signal.aborted;)try{let e=`${t}&_=${Date.now()}`;this.logger.log(s.a.Trace,`(LongPolling transport) polling: ${e}`);let r=yield this.httpClient.get(e,o);204===r.statusCode?(this.logger.log(s.a.Information,"(LongPolling transport) Poll terminated by server"),this.onclose&&this.onclose(),this.pollAbort.abort()):200!==r.statusCode?(this.logger.log(s.a.Error,`(LongPolling transport) Unexpected response code: ${r.statusCode}`),this.onclose&&this.onclose(new n.a(r.statusText,r.statusCode)),this.pollAbort.abort()):r.content?(this.logger.log(s.a.Trace,`(LongPolling transport) data received: ${r.content}`),this.onreceive&&this.onreceive(r.content)):this.logger.log(s.a.Trace,"(LongPolling transport) Poll timed out, reissuing.")}catch(t){t instanceof n.b?this.logger.log(s.a.Trace,"(LongPolling transport) Poll timed out, reissuing."):(this.onclose&&this.onclose(t),this.pollAbort.abort())}})}send(t){return c(this,void 0,void 0,function*(){return d(this.httpClient,this.url,this.accessTokenFactory,t)})}stop(){return this.pollAbort.abort(),Promise.resolve()}}function d(t,e,o,n){return c(this,void 0,void 0,function*(){let s;o()&&(s=new Map).set("Authorization",`Bearer ${o()}`),yield t.post(e,{content:n,headers:s})})}},function(t,e,o){"use strict";o.d(e,"a",function(){return n}),o.d(e,"b",function(){return s});class n extends Error{constructor(t,e){super(t),this.statusCode=e}}class s extends Error{constructor(t="A timeout occurred."){super(t)}}},function(t,e,o){"use strict";o.d(e,"a",function(){return n});var n,s=o(0);class r{log(t,e){}}class i{constructor(t){this.minimumLogLevel=t}log(t,e){if(t>=this.minimumLogLevel)switch(t){case s.a.Error:console.error(`${s.a[t]}: ${e}`);break;case s.a.Warning:console.warn(`${s.a[t]}: ${e}`);break;case s.a.Information:console.info(`${s.a[t]}: ${e}`);break;default:console.log(`${s.a[t]}: ${e}`)}}}!function(t){t.createLogger=function(t){return void 0===t?new i(s.a.Information):null===t?new r:t.log?t:new i(t)}}(n||(n={}))},function(t,e,o){"use strict";o.d(e,"a",function(){return s});class n{constructor(t,e){this.subject=t,this.observer=e}dispose(){let t=this.subject.observers.indexOf(this.observer);t>-1&&this.subject.observers.splice(t,1),0===this.subject.observers.length&&this.subject.cancelCallback().catch(t=>{})}}class s{constructor(t){this.observers=[],this.cancelCallback=t}next(t){for(let e of this.observers)e.next(t)}error(t){for(let e of this.observers)e.error&&e.error(t)}complete(){for(let t of this.observers)t.complete&&t.complete()}subscribe(t){return this.observers.push(t),new n(this,t)}}},function(t,e,o){"use strict";o.d(e,"a",function(){return c});var n=o(1),s=o(6),r=o(0),i=o(3),a=function(t,e,o,n){return new(o||(o=Promise))(function(s,r){function i(t){try{c(n.next(t))}catch(t){r(t)}}function a(t){try{c(n.throw(t))}catch(t){r(t)}}function c(t){t.done?s(t.value):new o(function(e){e(t.value)}).then(i,a)}c((n=n.apply(t,e||[])).next())})};class c{constructor(t,e={}){this.features={},this.logger=i.a.createLogger(e.logger),this.baseUrl=this.resolveUrl(t),(e=e||{}).accessTokenFactory=e.accessTokenFactory||(()=>null),this.httpClient=e.httpClient||new s.a,this.connectionState=2,this.options=e}start(){return a(this,void 0,void 0,function*(){return 2!==this.connectionState?Promise.reject(new Error("Cannot start a connection that is not in the 'Disconnected' state.")):(this.connectionState=0,this.startPromise=this.startInternal(),this.startPromise)})}startInternal(){return a(this,void 0,void 0,function*(){try{if(this.options.transport===n.c.WebSockets)this.url=this.baseUrl,this.transport=this.createTransport(this.options.transport,[n.c[n.c.WebSockets]]);else{let t,e=this.options.accessTokenFactory();e&&(t=new Map).set("Authorization",`Bearer ${e}`);let o=yield this.httpClient.post(this.resolveNegotiateUrl(this.baseUrl),{content:"",headers:t}),n=JSON.parse(o.content);if(this.connectionId=n.connectionId,2==this.connectionState)return;this.connectionId&&(this.url=this.baseUrl+(-1===this.baseUrl.indexOf("?")?"?":"&")+`id=${this.connectionId}`,this.transport=this.createTransport(this.options.transport,n.availableTransports))}this.transport.onreceive=this.onreceive,this.transport.onclose=(t=>this.stopConnection(!0,t));let t=2===this.features.transferMode?2:1;this.features.transferMode=yield this.transport.connect(this.url,t,this),this.changeState(0,1)}catch(t){throw this.logger.log(r.a.Error,"Failed to start the connection. "+t),this.connectionState=2,this.transport=null,t}})}createTransport(t,e){if((null===t||void 0===t)&&e.length>0&&(t=n.c[e[0]]),t===n.c.WebSockets&&e.indexOf(n.c[t])>=0)return new n.d(this.options.accessTokenFactory,this.logger);if(t===n.c.ServerSentEvents&&e.indexOf(n.c[t])>=0)return new n.b(this.httpClient,this.options.accessTokenFactory,this.logger);if(t===n.c.LongPolling&&e.indexOf(n.c[t])>=0)return new n.a(this.httpClient,this.options.accessTokenFactory,this.logger);if(this.isITransport(t))return t;throw new Error("No available transports found.")}isITransport(t){return"object"==typeof t&&"connect"in t}changeState(t,e){return this.connectionState==t&&(this.connectionState=e,!0)}send(t){if(1!=this.connectionState)throw new Error("Cannot send data if the connection is not in the 'Connected' State");return this.transport.send(t)}stop(t){return a(this,void 0,void 0,function*(){let e=this.connectionState;this.connectionState=2;try{yield this.startPromise}catch(t){}this.stopConnection(1==e,t)})}stopConnection(t,e){this.transport&&(this.transport.stop(),this.transport=null),e?this.logger.log(r.a.Error,`Connection disconnected with error '${e}'.`):this.logger.log(r.a.Information,"Connection disconnected."),this.connectionState=2,t&&this.onclose&&this.onclose(e)}resolveUrl(t){if(0===t.lastIndexOf("https://",0)||0===t.lastIndexOf("http://",0))return t;if("undefined"==typeof window||!window||!window.document)throw new Error(`Cannot resolve '${t}'.`);let e=window.document.createElement("a");e.href=t;let o=e.protocol&&":"!==e.protocol?`${e.protocol}//${e.host}`:`${window.document.location.protocol}//${e.host||window.document.location.host}`;t&&"/"==t[0]||(t="/"+t);let n=o+t;return this.logger.log(r.a.Information,`Normalizing '${t}' to '${n}'`),n}resolveNegotiateUrl(t){let e=t.indexOf("?"),o=t.substring(0,-1===e?t.length:e);return"/"!==o[o.length-1]&&(o+="/"),o+="negotiate",o+=-1===e?"":t.substring(e)}}},function(t,e,o){"use strict";o.d(e,"a",function(){return i});var n=o(2);class s{constructor(t,e,o){this.statusCode=t,this.statusText=e,this.content=o}}class r{get(t,e){return this.send(Object.assign({},e,{method:"GET",url:t}))}post(t,e){return this.send(Object.assign({},e,{method:"POST",url:t}))}}class i extends r{send(t){return new Promise((e,o)=>{let r=new XMLHttpRequest;r.open(t.method,t.url,!0),r.setRequestHeader("X-Requested-With","XMLHttpRequest"),t.headers&&t.headers.forEach((t,e)=>r.setRequestHeader(e,t)),t.responseType&&(r.responseType=t.responseType),t.abortSignal&&(t.abortSignal.onabort=(()=>{r.abort()})),t.timeout&&(r.timeout=t.timeout),r.onload=(()=>{t.abortSignal&&(t.abortSignal.onabort=null),r.status>=200&&r.status<300?e(new s(r.status,r.statusText,r.response||r.responseText)):o(new n.a(r.statusText,r.status))}),r.onerror=(()=>{o(new n.a(r.statusText,r.status))}),r.ontimeout=(()=>{o(new n.b)}),r.send(t.content||"")})}}},function(t,e,o){"use strict";var n=o(5),s=o(4);class r{static write(t){return`${t}${r.RecordSeparator}`}static parse(t){if(t[t.length-1]!=r.RecordSeparator)throw new Error("Message is incomplete.");let e=t.split(r.RecordSeparator);return e.pop(),e}}r.RecordSeparator=String.fromCharCode(30);const i="json";class a{constructor(){this.name=i,this.type=1}parseMessages(t){if(!t)return[];let e=r.parse(t),o=[];for(var n=0;n<e.length;++n)o.push(JSON.parse(e[n]));return o}writeMessage(t){return r.write(JSON.stringify(t))}}class c{constructor(t){this.wrappedProtocol=t,this.name=this.wrappedProtocol.name,this.type=1}parseMessages(t){let e=t.indexOf(":");if(-1==e||";"!=t[t.length-1])throw new Error("Invalid payload.");let o=t.substring(0,e);if(!/^[0-9]+$/.test(o))throw new Error(`Invalid length: '${o}'`);if(parseInt(o,10)!=t.length-e-2)throw new Error("Invalid message size.");let n=t.substring(e+1,t.length-1),s=atob(n),r=new Uint8Array(s.length);for(let t=0;t<r.length;t++)r[t]=s.charCodeAt(t);return this.wrappedProtocol.parseMessages(r.buffer)}writeMessage(t){let e=new Uint8Array(this.wrappedProtocol.writeMessage(t)),o="";for(let t=0;t<e.byteLength;t++)o+=String.fromCharCode(e[t]);let n=btoa(o);return`${n.length.toString()}:${n};`}}var l=o(0),h=o(3);o.d(e,"a",function(){return p}),o.d(e,!1,function(){return a});var u=function(t,e,o,n){return new(o||(o=Promise))(function(s,r){function i(t){try{c(n.next(t))}catch(t){r(t)}}function a(t){try{c(n.throw(t))}catch(t){r(t)}}function c(t){t.done?s(t.value):new o(function(e){e(t.value)}).then(i,a)}c((n=n.apply(t,e||[])).next())})};const d=3e4;class p{constructor(t,e={}){e=e||{},this.timeoutInMilliseconds=e.timeoutInMilliseconds||d,this.connection="string"==typeof t?new n.a(t,e):t,this.logger=h.a.createLogger(e.logger),this.protocol=e.protocol||new a,this.connection.onreceive=(t=>this.processIncomingData(t)),this.connection.onclose=(t=>this.connectionClosed(t)),this.callbacks=new Map,this.methods=new Map,this.closedCallbacks=[],this.id=0}processIncomingData(t){void 0!==this.timeoutHandle&&clearTimeout(this.timeoutHandle);let e=this.protocol.parseMessages(t);for(var o=0;o<e.length;++o){var n=e[o];switch(n.type){case 1:this.invokeClientMethod(n);break;case 2:case 3:let e=this.callbacks.get(n.invocationId);null!=e&&(3===n.type&&this.callbacks.delete(n.invocationId),e(n));break;case 6:break;default:this.logger.log(l.a.Warning,"Invalid message type: "+t)}}this.configureTimeout()}configureTimeout(){this.connection.features&&this.connection.features.inherentKeepAlive||(this.timeoutHandle=setTimeout(()=>this.serverTimeout(),this.timeoutInMilliseconds))}serverTimeout(){this.connection.stop(new Error("Server timeout elapsed without receiving a message from the server."))}invokeClientMethod(t){let e=this.methods.get(t.target.toLowerCase());if(e){if(e.forEach(e=>e.apply(this,t.arguments)),t.invocationId){let t="Server requested a response, which is not supported in this version of the client.";this.logger.log(l.a.Error,t),this.connection.stop(new Error(t))}}else this.logger.log(l.a.Warning,`No client method with the name '${t.target}' found.`)}connectionClosed(t){this.callbacks.forEach(e=>{e(void 0,t||new Error("Invocation canceled due to connection being closed."))}),this.callbacks.clear(),this.closedCallbacks.forEach(e=>e.apply(this,[t])),this.cleanupTimeout()}start(){return u(this,void 0,void 0,function*(){let t=2===this.protocol.type?2:1;this.connection.features.transferMode=t,yield this.connection.start();var e=this.connection.features.transferMode;yield this.connection.send(r.write(JSON.stringify({protocol:this.protocol.name}))),this.logger.log(l.a.Information,`Using HubProtocol '${this.protocol.name}'.`),2===t&&1===e&&(this.protocol=new c(this.protocol)),this.configureTimeout()})}stop(){return this.cleanupTimeout(),this.connection.stop()}stream(t,...e){let o=this.createStreamInvocation(t,e),n=new s.a(()=>{let t=this.createCancelInvocation(o.invocationId),e=this.protocol.writeMessage(t);return this.callbacks.delete(o.invocationId),this.connection.send(e)});this.callbacks.set(o.invocationId,(t,e)=>{if(e)n.error(e);else if(3===t.type){let e=t;e.error?n.error(new Error(e.error)):n.complete()}else n.next(t.item)});let r=this.protocol.writeMessage(o);return this.connection.send(r).catch(t=>{n.error(t),this.callbacks.delete(o.invocationId)}),n}send(t,...e){let o=this.createInvocation(t,e,!0),n=this.protocol.writeMessage(o);return this.connection.send(n)}invoke(t,...e){let o=this.createInvocation(t,e,!1);return new Promise((t,e)=>{this.callbacks.set(o.invocationId,(o,n)=>{if(n)e(n);else if(3===o.type){let n=o;n.error?e(new Error(n.error)):t(n.result)}else e(new Error(`Unexpected message type: ${o.type}`))});let n=this.protocol.writeMessage(o);this.connection.send(n).catch(t=>{e(t),this.callbacks.delete(o.invocationId)})})}on(t,e){t&&e&&(t=t.toLowerCase(),this.methods.has(t)||this.methods.set(t,[]),this.methods.get(t).push(e))}off(t,e){if(!t||!e)return;t=t.toLowerCase();let o=this.methods.get(t);if(o){var n=o.indexOf(e);-1!=n&&o.splice(n,1)}}onclose(t){t&&this.closedCallbacks.push(t)}cleanupTimeout(){this.timeoutHandle&&clearTimeout(this.timeoutHandle)}createInvocation(t,e,o){if(o)return{type:1,target:t,arguments:e};{let o=this.id;return this.id++,{type:1,invocationId:o.toString(),target:t,arguments:e}}}createStreamInvocation(t,e){let o=this.id;return this.id++,{type:4,invocationId:o.toString(),target:t,arguments:e}}createCancelInvocation(t){return{type:5,invocationId:t}}}},function(t,e){},function(t,e,o){"use strict";o(2),o(6),o(5);var n=o(7);o.d(e,"HubConnection",function(){return n.a});o(8),o(0),o(3),o(1),o(4)},function(t,e,o){"use strict";o.r(e);var n={Play:"播放",Pause:"暫停","Current Time":"目前時間","Duration Time":"總共時間","Remaining Time":"剩餘時間","Stream Type":"串流類型",LIVE:"直播",Loaded:"載入完畢",Progress:"進度",Fullscreen:"全螢幕","Non-Fullscreen":"退出全螢幕",Mute:"靜音",Unmute:"取消靜音","Playback Rate":" 播放速率",Subtitles:"字幕","subtitles off":"關閉字幕",Captions:"內嵌字幕","captions off":"關閉內嵌字幕",Chapters:"章節","Close Modal Dialog":"關閉對話框",Descriptions:"描述","descriptions off":"關閉描述","Audio Track":"音軌","You aborted the media playback":"影片播放已終止","A network error caused the media download to fail part-way.":"網路錯誤導致影片下載失敗。","The media could not be loaded, either because the server or network failed or because the format is not supported.":"影片因格式不支援或者伺服器或網路的問題無法載入。","The media playback was aborted due to a corruption problem or because the media used features your browser did not support.":"由於影片檔案損毀或是該影片使用了您的瀏覽器不支援的功能，播放終止。","No compatible source was found for this media.":"無法找到相容此影片的來源。","The media is encrypted and we do not have the keys to decrypt it.":"影片已加密，無法解密。","Play Video":"播放影片",Close:"關閉","Modal Window":"對話框","This is a modal window":"這是一個對話框","This modal can be closed by pressing the Escape key or activating the close button.":"可以按ESC按鍵或啟用關閉按鈕來關閉此對話框。",", opens captions settings dialog":", 開啟標題設定對話框",", opens subtitles settings dialog":", 開啟字幕設定對話框",", opens descriptions settings dialog":", 開啟描述設定對話框",", selected":", 選擇","captions settings":"字幕設定","Audio Player":"音頻播放器","Video Player":"視頻播放器",Replay:"重播","Progress Bar":"進度小節","Volume Level":"音量","subtitles settings":"字幕設定","descriptions settings":"描述設定",Text:"文字",White:"白",Black:"黑",Red:"紅",Green:"綠",Blue:"藍",Yellow:"黃",Magenta:"紫紅",Cyan:"青",Background:"背景",Window:"視窗",Transparent:"透明","Semi-Transparent":"半透明",Opaque:"不透明","Font Size":"字型尺寸","Text Edge Style":"字型邊緣樣式",None:"無",Raised:"浮雕",Depressed:"壓低",Uniform:"均勻",Dropshadow:"下陰影","Font Family":"字型庫","Proportional Sans-Serif":"比例無細體","Monospace Sans-Serif":"單間隔無細體","Proportional Serif":"比例細體","Monospace Serif":"單間隔細體",Casual:"輕便的",Script:"手寫體","Small Caps":"小型大寫字體",Reset:"重置","restore all settings to the default values":"恢復全部設定至預設值",Done:"完成","Caption Settings Dialog":"字幕設定視窗","Beginning of dialog window. Escape will cancel and close the window.":"開始對話視窗。離開會取消及關閉視窗","End of dialog window.":"結束對話視窗","Now Playing":"播放中","Untitled Video":"無標題影片","Up Next":"接著播放","Video Name":"影片名稱","Video Description":"影片描述"};function s(t){const e=document.createElement("canvas");e.width=640,e.height=480;const o=e.getContext("2d");return o.fillStyle="#FFF",o.fillRect(0,0,e.width,e.height),o.fillStyle="#000",o.font="48px serif",o.fillText(t||"Video",10,50),e.toDataURL()}var r={webApiRoot:"http://127.0.0.1:5000",webApiGetVideoList:"/api/Video/GetVideoList",webApiPlayVideo:"/api/Video/PlayVideo",signalrApi:"/videohub",signalrChannelPlay:"play",defaultPoster:s(),defaultType:"video/mp4",defaultName:n["Video Name"],defaultDescription:n["Video Description"]},i=(()=>({showLoading:()=>{const t=document.getElementsByClassName("loading-mask-disable");t.length>0&&(t[0].className="loading-mask")},hideLoading:()=>{const t=document.getElementsByClassName("loading-mask");t.length>0&&(t[0].className="loading-mask-disable")},showLogo:()=>{const t=document.getElementsByClassName("logo-mask-disable");t.length>0&&(t[0].className="logo-mask")},hideLogo:()=>{const t=document.getElementsByClassName("logo-mask");t.length>0&&(t[0].className="logo-mask-disable")}}))();var a={getAppConfig:function(t){return new Promise((e,o)=>{if(console.log(window.location.search),window.location.search){const o=window.location.search.substring(1).split("&"),n=Object.assign({},t);o.forEach(t=>{let e=t.split("=");n[e[0]]=e[1]}),e(n)}else e(t)})},getVideoList:function(t){return fetch(t,{method:"get"}).then(t=>t.json())}},c=o(9),l=t=>new Promise((e,o)=>{const n=new c.HubConnection(t.webApiRoot+t.signalrApi);n.start().then(t=>{console.log("initial"),e(n)}).catch(t=>{console.log("noneApi"),i.hideLoading(),i.showLogo(),o(null)})});s();function h(t,e){return e.webApiRoot+e.webApiPlayVideo+"?code="+t.code}var u={covertToPlayList:function(t,e){let o=[];for(let n=0;n<t.length;n++)o.push({name:t[n].categoryName||e.defaultName,description:t[n].description||e.defaultDescription,sources:[{src:h(t[n],e),type:e.defaultType}],poster:t[n].poster||e.defaultPoster,thumbnail:[{src:e.defaultPoster}]});return o},generateVideoParams:h},d=t=>{if(g.playlist([]),g.playlist.autoadvance(),0===t.length)return i.hideLoading(),void i.showLogo();const e=u.covertToPlayList(t,p.config);g.playlist(e),g.playlist.autoadvance(0),g.playlist.first(),i.hideLoading(),i.hideLogo()};o.d(e,"sloth",function(){return p}),o.d(e,"player",function(){return g}),i.showLoading(),i.hideLogo(),videojs.addLanguage("zh-tw",n);var p={},g=videojs("video-learning-player",{language:"zh-tw"});g.playlist([]),g.playlistUi(),g.playlist.autoadvance(0),function t(e){a.getAppConfig(r).then(o=>{i.showLoading(),e.config=o,l(e.config).then(o=>{o&&(a.getVideoList(e.config.webApiRoot+e.config.webApiGetVideoList).then(t=>{d(t)}),o.on("playVideo",function(t){d(t)}),o.on("loginOk",function(t){console.log(t)}),o.onclose(()=>{setTimeout(t(e),1e4)}))},()=>{setTimeout(t(e),1e4)})})}(p)}]);