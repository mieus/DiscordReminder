﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DiscordReminder {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ここにトークンを手入力すること")]
        public string token {
            get {
                return ((string)(this["token"]));
            }
            set {
                this["token"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"## Us-Reminder ヘルプ ##
決まった時間にリマインドします

[コマンド]
!radd キー文字列 時間(hh:mm) コメント everyoneするかどうか(y,n) 曜日（指定なしだと毎日）

リマインド設定を登録します

例：20:00の通知 19:55 もうすぐ20:00です！ y 月,火,水 
→月,火,水曜日の19:55に「もうすぐ20:00です！」とeveryoneを付けて投稿されます。

例：20:00の通知 19:55 もうすぐ20:00です！ y
→毎日19:55に「もうすぐ20:00です！」とeveryoneを付けて投稿されます。

!rdelete キー文字列

リマインド設定を削除します

!rlist

リマインド設定の一覧を表示します

!rstart

リマインドを開始します

!rend

Remindを終了します
")]
        public string heip {
            get {
                return ((string)(this["heip"]));
            }
            set {
                this["heip"] = value;
            }
        }
    }
}
