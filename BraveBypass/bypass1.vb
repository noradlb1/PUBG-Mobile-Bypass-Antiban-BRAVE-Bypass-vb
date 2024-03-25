Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports KeyAuth
Imports Microsoft.Win32

Namespace BraveBypass
    Public Partial Class bypass1
        Inherits Form
        Private Const MutexIdentifier As String = "YourApplicationMutex"
        Private ReadOnly reversingToolProcessNames As String() = {"OllyDbg", "ida_32bit", "ida_64bit", "cheatengine-x86_64-SSE4-AVX2.exe", "Wireshark"}
        Private dragging As Boolean = False
        Private startPoint As Point = New Point(0, 0)

        Public Shared KeyAuthApp As api = New api(name:="BGMI BYPASS", ownerid:="5J0eFrdWq9", secret:="00caa6078fd967bea73eb81c3cc2bae408b800956e8d23e3b0b2921bc9aa754b", version:="1.0")

        Private reversingToolCheckTimer As Timer

        Public Sub New()
            InitializeComponent()

            reversingToolCheckTimer = New Timer()
            reversingToolCheckTimer.Interval = 5000 ' Check every 5 seconds 
            AddHandler reversingToolCheckTimer.Tick, AddressOf ReversingToolCheckTimer_Tick
            reversingToolCheckTimer.Start()

            If DetectReversingTools() Then
                Environment.Exit(0)
            End If
        End Sub

        Private Sub ReversingToolCheckTimer_Tick(sender As Object, e As EventArgs)
            If DetectReversingTools() Then
                Environment.Exit(0)
            End If
        End Sub

        Private Function DetectReversingTools() As Boolean
            For Each processName In reversingToolProcessNames
                If Process.GetProcessesByName(processName).Length > 0 Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub bypass1_Load(sender As Object, e As EventArgs)
            AddHandler MouseDown, AddressOf FormMouseDown
            AddHandler MouseMove, AddressOf FormMouseMove
            AddHandler MouseUp, AddressOf FormMouseUp
            Call KeyAuthApp.init()
            CheckBraveKey()
        End Sub

        Private Sub CheckBraveKey()
            Dim filePath As String = Path.Combine(Directory.GetCurrentDirectory(), "Brave.txt")

            Try
                If File.Exists(filePath) Then
                    Dim savedKey = File.ReadAllText(filePath)

                    Task.Run(Sub() VerifyKey(savedKey))
                Else
                    Call Application.Exit()
                End If

            Catch
                Call Application.Exit()
            End Try
        End Sub

        Private Sub VerifyKey(key As String)
            Try
                Task.Run(Sub() KeyAuthApp.license(key))

                If KeyAuthApp.response.success Then
                    ' statusv.Text = "Welcome to Brave Bypass";
                    Invoke(New Action(Sub() Show()))
                Else
                    Call Application.Exit()
                End If

            Catch
                Call Application.Exit()
            End Try
        End Sub

        Private Sub FormMouseDown(sender As Object, e As MouseEventArgs)
            dragging = True
            startPoint = New Point(e.X, e.Y)
        End Sub

        Private Sub FormMouseMove(sender As Object, e As MouseEventArgs)
            If dragging Then
                Dim p = PointToScreen(e.Location)
                Location = New Point(p.X - startPoint.X, p.Y - startPoint.Y)
            End If
        End Sub

        Private Sub FormMouseUp(sender As Object, e As MouseEventArgs)
            dragging = False
        End Sub

        Private Sub radioButton1_CheckedChanged(sender As Object, e As EventArgs)
            GameLibFunction("Global", "libmarsxlog.so")
        End Sub

        Private Sub SaveFile(filePath As String, content As Byte())
            Try
                'Console.WriteLine($"File saved to: {filePath}");
                File.WriteAllBytes(filePath, content)
            Catch
                statusv.Text = "Error"
                Throw
            End Try
        End Sub

        Private Sub radioButton2_CheckedChanged(sender As Object, e As EventArgs)
            GameLibFunction("TWN", "libmarsxlog.so")
        End Sub

        Private Sub radioButton3_CheckedChanged(sender As Object, e As EventArgs)
            GameLibFunction("BGMI", "libBrave++.so") ' your lib 
            GameLibFunction("BGMI", "libhdmpve.so") ' loader lib
        End Sub

        Private Sub radioButton4_CheckedChanged(sender As Object, e As EventArgs)
            GameLibFunction("VNG", "libmarsxlog.so")
        End Sub

        Private Async Sub button1_Click(sender As Object, e As EventArgs)
            Await AnimateTextChange("Starting Emulator...", statusv)
            Await Task.Delay(1000)

            Await Task.Run(Sub() StartAndroidEmulator())
        End Sub

        Private Async Function AnimateTextChange(newText As String, label As Label) As Task
            For i = 0 To newText.Length - 1
                label.Text = newText.Substring(0, i + 1)
                Await Task.Delay(100)
            Next
        End Function

        Private Sub StartAndroidEmulator()
            Dim registryKey = "HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Tencent\MobileGamePC\UI"
            Dim valueName = "InstallPath"
            Dim defaultPath = ""

            Dim start As String = Registry.GetValue(registryKey, valueName, defaultPath)?.ToString()

            If Not String.IsNullOrEmpty(start) Then
                Dim executablePath = Path.Combine(start, "AndroidEmulatorEx.exe")

                If Not IsProcessRunning("AndroidEmulatorEx") Then
                    Process.Start(executablePath, "-vm 100")
                Else
                    Invoke(CType(Sub() statusv.Text = "Emulator is Already Running...", MethodInvoker))
                    statusv.Refresh()
                End If
            Else
                Console.WriteLine("Error Contact Seller!")
            End If
        End Sub

        Private Shared Function IsProcessRunning(processName As String) As Boolean
            Dim processes = Process.GetProcessesByName(processName)
            Return processes.Length > 0
        End Function

        Private Sub button3_Click(sender As Object, e As EventArgs)
            Try
                Task.Run(Sub() CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/Brave.lic"))
                Task.Run(Sub() CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/libmarsxlog.so"))
                '   Task.Run(() => CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/libmarsxlog.so")); // add for bgmi
                '   Task.Run(() => CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/libmarsxlog.so")); // add for bgmi
                Dim filePath = Path.Combine("C:\", "libmarsxlog.so")
                '    string filePath1 = Path.Combine("C:\\", "libmarsxlog.so"); // add for bgmi too

                If File.Exists(filePath) Then
                    File.Delete(filePath)
                End If
                KillProcessesByName("AppMarket")
                KillProcessesByName("AppMarket.exe")
                KillProcessesByName("AndroidEmulatorEx.exe")
                KillProcessesByName("AndroidEmulatorEx")
                KillProcessesByName("AndroidEmulatorEn.exe")
                KillProcessesByName("AndroidEmulatorEn")
                KillProcessesByName("appmarket.exe")
                KillProcessesByName("appmarket")
                KillProcessesByName("androidemulator")
                KillProcessesByName("androidemulator.exe")
                KillProcessesByName("aow_exe.exe")
                KillProcessesByName("QMEmulatorService.exe")
                KillProcessesByName("RuntimeBroker.exe")
                KillProcessesByName("adb.exe")
                KillProcessesByName("GameLoader.exe")
                KillProcessesByName("TSettingCenter.exe")
                KillProcessesByName("syzs_dl_svr.exe")

                statusv.Text = "SAFE EXIT DONE!"
            Catch ex As Exception
                statusv.Text = $"SAFE EXIT FAILED: {ex.Message}"
            End Try
        End Sub

        Private Sub KillProcessesByName(processName As String)
            For Each proc In Process.GetProcessesByName(processName)
                Try
                    proc.Kill()
                    proc.WaitForExit()
                Catch
                    '   Console.WriteLine($"Failed to kill process {processName}: {ex.Message}");
                End Try
            Next
        End Sub

        Private Sub label1_Click(sender As Object, e As EventArgs)

            Call Task.Delay(100).Wait()
            Dim filePath = Path.Combine("C:\", "libmarsxlog.so")


            If File.Exists(filePath) Then
                File.Delete(filePath)
            End If
            Call Application.Exit()
        End Sub

        Private Async Sub button2_Click(sender As Object, e As EventArgs)

            button2.Enabled = False


            If radioButton1.Checked Then  ' GL

                Await AnimateTextChange("Starting Global!...", statusv)
                Await Task.Run(Sub() CommandLine("adb kill-server"))
                Await Task.Run(Sub() CommandLine("adb start-server"))
                Await Task.Run(Sub() CommandLine("adb shell am kill com.tencent.ig"))
                Await Task.Run(Sub() CommandLine("adb shell am force-stop com.tencent.ig"))


                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/.backups"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/BGMI"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/MidasOversea"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.json"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/LobbyBubble"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Lobby"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Login"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.sav"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashrecord"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashSight"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_databases"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_flutter"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_geolocation"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_textures"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_webview"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/code_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/files/*"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/no_backup"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/Brave.lic"))


                Await PushFileToEmulatorSystem("C:\libmarsxlog.so", "data/app/com.tencent.ig-1/lib/arm/")

                Await Task.Run(Sub() CommandLine("adb shell am start -n com.tencent.ig/com.epicgames.ue4.SplashActivity filter"))

                Call Task.Delay(2000).Wait()


                Dim filePath = Path.Combine("C:\", "libmarsxlog.so")


                If File.Exists(filePath) Then
                    File.Delete(filePath)
                    Await AnimateTextChange("Bypass Done! Enjoy Global...", statusv)

                End If
                button2.Enabled = True
            ElseIf radioButton2.Checked Then ' TWN
                Await AnimateTextChange("Starting Taiwan!...", statusv)

                Await Task.Run(Sub() CommandLine("adb kill-server"))
                Await Task.Run(Sub() CommandLine("adb start-server"))
                Await Task.Run(Sub() CommandLine("adb shell am kill com.tencent.ig"))
                Await Task.Run(Sub() CommandLine("adb shell am force-stop com.tencent.ig"))


                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/.backups"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/BGMI"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/MidasOversea"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.json"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/LobbyBubble"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Lobby"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Login"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.sav"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashrecord"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashSight"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_databases"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_flutter"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_geolocation"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_textures"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_webview"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/code_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/files/*"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/no_backup"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/Brave.lic"))


                Await PushFileToEmulatorSystem("C:\libmarsxlog.so", "data/app/com.tencent.ig-1/lib/arm/")

                Await Task.Run(Sub() CommandLine("adb shell am start -n com.tencent.ig/com.epicgames.ue4.SplashActivity filter"))

                Call Task.Delay(2000).Wait()


                Dim filePath = Path.Combine("C:\", "libmarsxlog.so")


                If File.Exists(filePath) Then
                    File.Delete(filePath)
                    Await AnimateTextChange("Bypass Done! Enjoy Taiwan...", statusv)

                End If

                button2.Enabled = True
            ElseIf radioButton3.Checked Then ' BGMI

                Await AnimateTextChange("Starting BGMI!...", statusv)

                Await Task.Run(Sub() CommandLine("adb kill-server"))
                Await Task.Run(Sub() CommandLine("adb start-server"))
                Await Task.Run(Sub() CommandLine("adb shell am kill com.pubg.imobile"))
                Await Task.Run(Sub() CommandLine("adb shell am force-stop com.pubg.imobile"))

                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/.backups"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/BGMI"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/MidasOversea"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.pubg.imobile/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.json"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.pubg.imobile/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/LobbyBubble"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.pubg.imobile/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Lobby"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.pubg.imobile/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Login"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.pubg.imobile/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.sav"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/app_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/datacom.pubg.imobile/app_crashrecord"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/app_crashSight"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/app_databases"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/app_flutter"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/app_geolocation"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/app_textures"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/app_webview"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/code_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/files/*"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.pubg.imobile/no_backup"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf data/app/com.pubg.imobile-1/lib/arm/Brave.lic"))


                Await PushFileToEmulatorSystem("C:\libmarsxlog.so", "data/app/com.pubg.imobile-1/lib/arm/")

                Await Task.Run(Sub() CommandLine("adb shell am start -n com.pubg.imobile/com.epicgames.ue4.SplashActivity filter"))

                Call Task.Delay(2000).Wait()


                Dim filePath = Path.Combine("C:\", "libmarsxlog.so") ' add bgmi lib


                If File.Exists(filePath) Then
                    File.Delete(filePath)
                    Await AnimateTextChange("Bypass Done! Enjoy BGMI...", statusv)

                End If
                button2.Enabled = True
            ElseIf radioButton4.Checked Then ' VNG

                Await AnimateTextChange("Starting VNG!...", statusv)

                Await Task.Run(Sub() CommandLine("adb kill-server"))
                Await Task.Run(Sub() CommandLine("adb start-server"))
                Await Task.Run(Sub() CommandLine("adb shell am kill com.tencent.ig"))
                Await Task.Run(Sub() CommandLine("adb shell am force-stop com.tencent.ig"))

                ' package name will be different 

                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/.backups"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/BGMI"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/MidasOversea"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.json"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/LobbyBubble"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Lobby"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Login"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.sav"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashrecord"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashSight"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_databases"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_flutter"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_geolocation"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_textures"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_webview"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/code_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/files/*"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/no_backup"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/Brave.lic"))


                Await PushFileToEmulatorSystem("C:\libmarsxlog.so", "data/app/com.tencent.ig-1/lib/arm/")

                Await Task.Run(Sub() CommandLine("adb shell am start -n com.tencent.ig/com.epicgames.ue4.SplashActivity filter"))

                Call Task.Delay(2000).Wait()


                Dim filePath = Path.Combine("C:\", "libmarsxlog.so")


                If File.Exists(filePath) Then
                    File.Delete(filePath)
                    Await AnimateTextChange("Bypass Done! Enjoy VNG...", statusv)

                End If

                button2.Enabled = True

            ElseIf radioButton5.Checked Then ' kr
                Await AnimateTextChange("Starting KR!...", statusv)

                ' package name will be different 

                Await Task.Run(Sub() CommandLine("adb kill-server"))
                Await Task.Run(Sub() CommandLine("adb start-server"))
                Await Task.Run(Sub() CommandLine("adb shell am kill com.tencent.ig"))
                Await Task.Run(Sub() CommandLine("adb shell am force-stop com.tencent.ig"))


                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/.backups"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/BGMI"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/media/0/MidasOversea"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.json"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/LobbyBubble"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Lobby"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/Login"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /storage/emulated/0/Android/data/com.tencent.ig/files/UE4Game/ShadowTrackerExtra/ShadowTrackerExtra/Saved/SaveGames/*.sav"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashrecord"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_crashSight"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_databases"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_flutter"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_geolocation"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_textures"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/app_webview"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/code_cache"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/files/*"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf /data/data/com.tencent.ig/no_backup"))
                Await Task.Run(Sub() CommandLine("adb shell rm -rf data/app/com.tencent.ig-1/lib/arm/Brave.lic"))


                Await PushFileToEmulatorSystem("C:\libmarsxlog.so", "data/app/com.tencent.ig-1/lib/arm/")

                Await Task.Run(Sub() CommandLine("adb shell am start -n com.tencent.ig/com.epicgames.ue4.SplashActivity filter"))

                Call Task.Delay(2000).Wait()


                Dim filePath = Path.Combine("C:\", "libmarsxlog.so")


                If File.Exists(filePath) Then
                    File.Delete(filePath)
                    Await AnimateTextChange("Bypass Done! Enjoy KR...", statusv)

                End If

                button2.Enabled = True

            End If

        End Sub

        Private Async Function PushFileToEmulatorSystem(localFilePath As String, targetDirectory As String) As Task
            Await Task.Run(Sub()
                               Dim filePath As String = Path.Combine(Directory.GetCurrentDirectory(), "Brave.lic")
                               If File.Exists(localFilePath) Then
                                   CommandLine($"adb push ""{localFilePath}"" ""{targetDirectory}""")
                                   CommandLine($"adb push ""{filePath}"" ""{targetDirectory}""")
                               Else
                                   statusv.Text = "Error Contact Seller"
                               End If
                           End Sub)
        End Function

        Private Sub CommandLine(arg As String)
            Dim process As Process = New Process()
            Dim startInfo As ProcessStartInfo = New ProcessStartInfo With {
    .WindowStyle = ProcessWindowStyle.Hidden,
    .CreateNoWindow = True,
    .UseShellExecute = False,
    .RedirectStandardOutput = True,
    .FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe"),
    .Arguments = $"/c {arg}"
}

            process.StartInfo = startInfo
            process.Start()
            process.WaitForExit()
        End Sub

        <Obsolete>
        Private Async Sub button4_Click(sender As Object, e As EventArgs) ' reset guest
            Dim filePath = Path.Combine("C:\", "greset.bat")

            Try
                If File.Exists(filePath) Then
                    File.Delete(filePath)
                    Console.WriteLine($"Existing file deleted: {filePath}")
                End If

                Dim result1 = Await Task.Run(Function() KeyAuthApp.download("153682")) ' greset.bat

                Await Task.Run(Sub() SaveFile(filePath, result1))

                Await Task.Run(Function() CSharpImpl.__Assign(statusv.Text, "Resetting Guest..."))

                Await Task.Run(Sub() RunFile(filePath))


                Await Task.Run(Function() CSharpImpl.__Assign(statusv.Text, "Guest reset done!"))
            Catch ex As Exception
                Console.WriteLine($"An error occurred: {ex.Message}")

                'Await Task.Run(Function() CSharpImpl.__Assign(statusv.Text, $"Error: {ex.Message}"))
            End Try
        End Sub


        Private Sub RunFile(filePath As String)
            Try
                ' Start the process to run the saved file
                Process.Start(filePath)
            Catch ex As Exception
                Console.WriteLine($"Error running file: {ex.Message}")
            End Try
        End Sub

        Private Sub radioButton5_CheckedChanged(sender As Object, e As EventArgs)
            GameLibFunction("KR", "libmarsxlog.so")
        End Sub

        Private Async Sub GameLibFunction(selection As String, libraryName As String)
            If radioButton1.Checked OrElse radioButton2.Checked OrElse radioButton3.Checked OrElse radioButton4.Checked OrElse radioButton5.Checked Then
                radioButton1.Enabled = False
                radioButton2.Enabled = False
                radioButton3.Enabled = False
                radioButton4.Enabled = False
                radioButton5.Enabled = False
                button1.Enabled = False
                button2.Enabled = False

                Try
                    Await AnimateTextChange($"You Selected {selection}...", statusv)


                    Dim filePath = Path.Combine("C:\", libraryName)

                    If File.Exists(filePath) Then
                        File.Delete(filePath)
                    End If

                    If Equals(selection, "BGMI") Then
                        Dim result1 = Await Task.Run(Function() KeyAuthApp.download("685552")) ' for bgmi
                        Dim result2 = Await Task.Run(Function() KeyAuthApp.download("685552")) ' for bgmi
                        Await Task.Run(Sub() SaveFile(filePath, result1))
                        Await Task.Run(Sub() SaveFile(filePath, result2))
                        Await AnimateTextChange("Start Game Now...", statusv) ' Non-BGMI versions ! ....
                    Else
                        Dim result = Await Task.Run(Function() KeyAuthApp.download("685552"))
                        Await AnimateTextChange("Start Game Now...", statusv)
                    End If
                Catch ex As Exception
                    statusv.Text = $"Error Contact Seller: {ex.Message}"
                Finally
                    radioButton1.Enabled = True
                    radioButton2.Enabled = True
                    radioButton3.Enabled = True
                    radioButton4.Enabled = True
                    radioButton5.Enabled = True
                    button1.Enabled = True
                    button2.Enabled = True
                End Try
            End If
        End Sub

        Private Sub button5_Click(sender As Object, e As EventArgs)

        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
