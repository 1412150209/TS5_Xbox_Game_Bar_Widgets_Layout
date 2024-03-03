# TS5 Layout

Use [Xbox Game Bar Widgets](https://learn.microsoft.com/en-us/gaming/game-bar/) and [TeamSpeak5 Remote Application](https://community.teamspeak.com/t/teamspeak-remote-apps/23250).

## Install

You can download the latest release,or package the source using Visual Studio by yourself.

Use 'Install.ps1' to install this UWP application.
It may ask you to install my certificate,just install it.

## Develop Environment
- Visual Studio 2022
- Windows 11 23H2

## Usage
### Settings Window
You can open it from your applicaiton list.It's like any other UWP applicaiton.<br>
It can change the IP address and port if you wish.<br>
IP address defaults to be "127.0.0.1"<br>
Port defaults to be "5899"<br>

### Layout Widget
You can open it from the Xbox Game Bar Widgets.<br>
It will notice you if it didn't connected to TS5.<br>
You will find a request on your TS5.(Notification boards on the top left)<br>

## Konwn Bugs
1. When you close the Settings Window,the layout widget will disapper.<br>
Solution:<br>
Actually,you can just use the widget.<br>

2. If you open the Settings Window first,you will not find the default IP and port.<br>
Solution:<br>
Open widget first.
