Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports KeyAuth

Namespace BraveBypass
    Public Partial Class bypass
        Inherits Form
        Private Const MutexIdentifier As String = "BRAVE"
        Private ReadOnly reversingToolProcessNames As String() = {"OllyDbg", "ida_32bit", "ida_64bit", "cheatengine-x86_64-SSE4-AVX2.exe", "Wireshark"}
        Private Shared singleInstanceMutex As Mutex


        Public Shared KeyAuthApp As api = New api(name:="BGMI BYPASS", ownerid:="5J0eFrdWq9", secret:="00caa6078fd967bea73eb81c3cc2bae408b800956e8d23e3b0b2921bc9aa754b", version:="1.0")


        Private dragging As Boolean = False
        Private startPoint As Point = New Point(0, 0)
        Private Main As bypass1 = New bypass1()

        Public Sub New()
            InitializeComponent()
            SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
            UpdateStyles()
            Dim isFirstInstance As Boolean = Nothing
            singleInstanceMutex = New Mutex(True, MutexIdentifier, isFirstInstance)

            If Not isFirstInstance Then
                MessageBox.Show("Another instance of the application is already running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Environment.Exit(0)
            End If


            If DetectReversingTools() Then
                MessageBox.Show("Reversing tools detected. Madarchod ! Baap Hu tera Randi ka Baccha", "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Environment.Exit(0)
            End If
        End Sub

        Private Function DetectReversingTools() As Boolean
            For Each processName In reversingToolProcessNames
                If IsProcessRunning(processName) Then
                    Return True ' Reversing tool detected !
                End If
            Next
            Return False ' No reversing tool detected !
        End Function

        Private Function IsProcessRunning(processName As String) As Boolean
            Dim processes = Process.GetProcessesByName(processName)
            Return processes.Length > 0
        End Function

        Private Sub label1_Click(sender As Object, e As EventArgs)
            Call Application.Exit()
        End Sub

        Private Async Sub button1_Click(sender As Object, e As EventArgs)
            button1.Enabled = False
            statusv.Text = "Checking Verification..."
            DeleteFileIfExists("Brave.txt")

            SaveTextToFile(textBox1.Text, "Brave.txt")

            If Equals(textBox1.Text, "BraveBypass") Then
                Await AnimateTextChange("Verification Done...", statusv)
                Await Task.Delay(1000)
                Await AnimateTextChange("Logged in Successfully...", statusv)
                Await Task.Delay(3000)
                Hide()
                Main.Show()
            Else
                ' statusv.Text = "Wrong Key...";
                Await AnimateTextChange("Wrong Key...", statusv)

            End If
            button1.Enabled = True
        End Sub

        Private Async Function AnimateTextChange(newText As String, label As Label) As Task
            For i = 0 To newText.Length - 1
                label.Text = newText.Substring(0, i + 1)
                Await Task.Delay(100)
            Next
        End Function


        Private Sub SaveTextToFile(text As String, fileName As String)
            Try
                Dim filePath As String = Path.Combine(Directory.GetCurrentDirectory(), fileName)

                '   statusv.Text = $"Key saved to {fileName} successfully!";
                File.WriteAllText(filePath, text)
            Catch
                '   statusv.Text = $"Error saving {fileName}: {ex.Message}";
            End Try
        End Sub


        Private Sub DeleteFileIfExists(fileName As String)
            Try
                Dim filePath As String = Path.Combine(Directory.GetCurrentDirectory(), fileName)

                If File.Exists(filePath) Then
                    File.Delete(filePath)
                    statusv.Text = $"{fileName} deleted successfully!"
                End If
            Catch ex As Exception
                statusv.Text = $"Error deleting {fileName}: {ex.Message}"
            End Try
        End Sub


        Private Function UnixTimeToDateTime(unixTime As Long) As Date
            Return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime
        End Function

        Private Sub bypass_Load(sender As Object, e As EventArgs)
            Call KeyAuthApp.init()
            AddHandler MouseDown, AddressOf FormMouseDown
            AddHandler MouseMove, AddressOf FormMouseMove
            AddHandler MouseUp, AddressOf FormMouseUp
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

        Private Sub textBox1_TextChanged(sender As Object, e As EventArgs)
        End Sub

        Private Sub button2_Click(sender As Object, e As EventArgs) ' Get Key
            Task.Run(Function() Process.Start("https://t.me/")) ' add your link
        End Sub

        Private Sub status_Click(sender As Object, e As EventArgs)

        End Sub
    End Class
End Namespace
