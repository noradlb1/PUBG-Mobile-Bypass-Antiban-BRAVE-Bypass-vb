Imports System
Imports System.Security.Cryptography
Imports System.Collections.Specialized
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Diagnostics
Imports System.Security.Principal
Imports System.Collections.Generic
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Threading
Imports System.Runtime.CompilerServices

Namespace KeyAuth
    Public Class api
        Public name, ownerid, secret, version As String
        Public Shared responseTime As Long
        ''' <summary>
        ''' Set up your application credentials in order to use keyauth
        ''' </summary>
        ''' <paramname="name">Application Name</param>
        ''' <paramname="ownerid">Your OwnerID, found in your account settings.</param>
        ''' <paramname="secret">Application Secret</param>
        ''' <paramname="version">Application Version, if version doesnt match it will open the download link you set up in your application settings and close the app, if empty the app will close</param>
        Public Sub New(name As String, ownerid As String, secret As String, version As String)
            If ownerid.Length <> 10 OrElse secret.Length <> 64 Then
                Process.Start("https://youtube.com/watch?v=RfDTdiBq4_o")
                Process.Start("https://keyauth.cc/app/")
                Thread.Sleep(2000)
                [error]("Application not setup correctly. Please watch the YouTube video for setup.")
                Environment.Exit(0)
            End If

            Me.name = name

            Me.ownerid = ownerid

            Me.secret = secret

            Me.version = version
        End Sub

#Region "structures"
        <DataContract>
        Private Class response_structure
            <DataMember>
            Public Property success As Boolean

            <DataMember>
            Public Property newSession As Boolean

            <DataMember>
            Public Property sessionid As String

            <DataMember>
            Public Property contents As String

            <DataMember>
            Public Property response As String

            <DataMember>
            Public Property message As String

            <DataMember>
            Public Property download As String

            <DataMember(IsRequired:=False, EmitDefaultValue:=False)>
            Public Property info As user_data_structure

            <DataMember(IsRequired:=False, EmitDefaultValue:=False)>
            Public Property appinfo As app_data_structure

            <DataMember>
            Public Property messages As List(Of msg)

            <DataMember>
            Public Property users As List(Of users)
        End Class

        Public Class msg
            Public Property message As String
            Public Property author As String
            Public Property timestamp As String
        End Class

        Public Class users
            Public Property credential As String
        End Class

        <DataContract>
        Private Class user_data_structure
            <DataMember>
            Public Property username As String

            <DataMember>
            Public Property ip As String
            <DataMember>
            Public Property hwid As String
            <DataMember>
            Public Property createdate As String
            <DataMember>
            Public Property lastlogin As String
            <DataMember>
            Public Property subscriptions As List(Of Data) ' array of subscriptions (basically multiple user ranks for user with individual expiry dates
        End Class

        <DataContract>
        Private Class app_data_structure
            <DataMember>
            Public Property numUsers As String
            <DataMember>
            Public Property numOnlineUsers As String
            <DataMember>
            Public Property numKeys As String
            <DataMember>
            Public Property version As String
            <DataMember>
            Public Property customerPanelLink As String
            <DataMember>
            Public Property downloadLink As String
        End Class
#End Region
        Private Shared sessionid, enckey As String
        Private initialized As Boolean
        ''' <summary>
        ''' Initializes the connection with keyauth in order to use any of the functions
        ''' </summary>
        Public Sub init()
            Dim sentKey As String = iv_key()
            enckey = sentKey & "-" & secret
            Dim values_to_upload = New NameValueCollection From {
    {"type", "init"},
    {"ver", version},
    {"hash", checksum(Process.GetCurrentProcess().MainModule.FileName)},
    {"enckey", sentKey},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            If Equals(response, "KeyAuth_Invalid") Then
                [error]("Application not found")
                Environment.Exit(0)
            End If

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then
                If json.newSession Then
                    Thread.Sleep(100)
                End If
                sessionid = json.sessionid
                initialized = True
            ElseIf Equals(json.message, "invalidver") Then
                app_data.downloadLink = json.download
            End If

        End Sub
        ''' <summary>
        ''' Checks if Keyauth is been Initalized
        ''' </summary>
        Public Sub CheckInit()
            If Not initialized Then
                [error]("You must run the function KeyAuthApp.init(); first")
                Environment.Exit(0)
            End If
        End Sub

        ''' <summary>
        ''' Converts Unix time to Days,Months,Hours
        ''' </summary>
        ''' <paramname="subscription">Subscription Number</param>
        ''' <paramname="Type">You can choose between Days,Hours,Months </param>
        Public Function expirydaysleft(Type As String, subscription As Integer) As String
            CheckInit()

            Dim dtDateTime As Date = New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
            dtDateTime = dtDateTime.AddSeconds(Long.Parse(user_data.subscriptions(subscription).expiry)).ToLocalTime()
            Dim difference = dtDateTime - Date.Now
            Select Case Type.ToLower()
                Case "months"
                    Return Convert.ToString(difference.Days / 30)
                Case "days"
                    Return Convert.ToString(difference.Days)
                Case "hours"
                    Return Convert.ToString(difference.Hours)
            End Select
            Return Nothing

        End Function

        ''' <summary>
        ''' Registers the user using a license and gives the user a subscription that matches their license level
        ''' </summary>
        ''' <paramname="username">Username</param>
        ''' <paramname="pass">Password</param>
        ''' <paramname="key">License key</param>
        Public Sub register(username As String, pass As String, key As String, Optional email As String = "")
            CheckInit()

            Dim hwid As String = WindowsIdentity.GetCurrent().User.Value

            Dim values_to_upload = New NameValueCollection From {
    {"type", "register"},
    {"username", username},
    {"pass", pass},
    {"key", key},
    {"email", email},
    {"hwid", hwid},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then load_user_data(json.info)
        End Sub
        ''' <summary>
        ''' Allow users to enter their account information and recieve an email to reset their password.
        ''' </summary>
        ''' <paramname="username">Username</param>
        ''' <paramname="email">Email address</param>
        Public Sub forgot(username As String, email As String)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "forgot"},
    {"username", username},
    {"email", email},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
        End Sub
        ''' <summary>
        ''' Authenticates the user using their username and password
        ''' </summary>
        ''' <paramname="username">Username</param>
        ''' <paramname="pass">Password</param>
        Public Sub login(username As String, pass As String)
            CheckInit()

            Dim hwid As String = WindowsIdentity.GetCurrent().User.Value

            Dim values_to_upload = New NameValueCollection From {
    {"type", "login"},
    {"username", username},
    {"pass", pass},
    {"hwid", hwid},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then load_user_data(json.info)
        End Sub

        Public Sub logout()
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "logout"},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
        End Sub

        Public Sub web_login()
            CheckInit()

            Dim hwid As String = WindowsIdentity.GetCurrent().User.Value

            Dim datastore, datastore2, outputten As String

start:

            Dim listener As HttpListener = New HttpListener()

            outputten = "handshake"
            outputten = "http://localhost:1337/" & outputten & "/"

            listener.Prefixes.Add(outputten)

            listener.Start()

            Dim context As HttpListenerContext = listener.GetContext()
            Dim request = context.Request
            Dim responsepp = context.Response

            responsepp.AddHeader("Access-Control-Allow-Methods", "GET, POST")
            responsepp.AddHeader("Access-Control-Allow-Origin", "*")
            responsepp.AddHeader("Via", "hugzho's big brain")
            responsepp.AddHeader("Location", "your kernel ;)")
            responsepp.AddHeader("Retry-After", "never lmao")
            responsepp.Headers.Add("Server", Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf)

            If Equals(request.HttpMethod, "OPTIONS") Then
                responsepp.StatusCode = HttpStatusCode.OK
                Thread.Sleep(1) ' without this, the response doesn't return to the website, and the web buttons can't be shown
                listener.Stop()
                GoTo start
            End If

            listener.AuthenticationSchemes = AuthenticationSchemes.Negotiate
            listener.UnsafeConnectionNtlmAuthentication = True
            listener.IgnoreWriteExceptions = True

            Dim data = request.RawUrl

            datastore2 = data.Replace("/handshake?user=", "")
            datastore2 = datastore2.Replace("&token=", " ")

            datastore = datastore2

            Dim user As String = datastore.Split()(0)
            Dim token = datastore.Split(" "c)(1)

            Dim values_to_upload = New NameValueCollection From {
    {"type", "login"},
    {"username", user},
    {"token", token},
    {"hwid", hwid},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)

            Dim success = True

            If json.success Then
                load_user_data(json.info)

                responsepp.StatusCode = 420
                responsepp.StatusDescription = "SHEESH"
            Else
                Console.WriteLine(json.message)
                responsepp.StatusCode = HttpStatusCode.OK
                responsepp.StatusDescription = json.message
                success = False
            End If

            Dim buffer = Encoding.UTF8.GetBytes("Whats up?")

            responsepp.ContentLength64 = buffer.Length
            Dim output = responsepp.OutputStream
            output.Write(buffer, 0, buffer.Length)
            Thread.Sleep(1) ' without this, the response doesn't return to the website, and the web buttons can't be shown
            listener.Stop()

            If Not success Then Environment.Exit(0)

        End Sub

        ''' <summary>
        ''' Use Buttons from KeyAuth Customer Panel
        ''' </summary>
        ''' <paramname="pButton">Button Name</param>

        Public Sub button(pButton As String)
            CheckInit()

            Dim listener As HttpListener = New HttpListener()

            Dim output As String

            output = pButton
            output = "http://localhost:1337/" & output & "/"

            listener.Prefixes.Add(output)

            listener.Start()

            Dim context As HttpListenerContext = listener.GetContext()
            Dim request = context.Request
            Dim responsepp = context.Response

            responsepp.AddHeader("Access-Control-Allow-Methods", "GET, POST")
            responsepp.AddHeader("Access-Control-Allow-Origin", "*")
            responsepp.AddHeader("Via", "hugzho's big brain")
            responsepp.AddHeader("Location", "your kernel ;)")
            responsepp.AddHeader("Retry-After", "never lmao")
            responsepp.Headers.Add("Server", Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf)

            responsepp.StatusCode = 420
            responsepp.StatusDescription = "SHEESH"

            listener.AuthenticationSchemes = AuthenticationSchemes.Negotiate
            listener.UnsafeConnectionNtlmAuthentication = True
            listener.IgnoreWriteExceptions = True

            listener.Stop()
        End Sub

        ''' <summary>
        ''' Gives the user a subscription that has the same level as the key
        ''' </summary>
        ''' <paramname="username">Username of the user thats going to get upgraded</param>
        ''' <paramname="key">License with the same level as the subscription you want to give the user</param>
        Public Sub upgrade(username As String, key As String)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "upgrade"},
    {"username", username},
    {"key", key},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            json.success = False
            load_response_struct(json)
        End Sub

        ''' <summary>
        ''' Authenticate without using usernames and passwords
        ''' </summary>
        ''' <paramname="key">Licence used to login with</param>
        Public Sub license(key As String)
            CheckInit()

            Dim hwid As String = WindowsIdentity.GetCurrent().User.Value

            Dim values_to_upload = New NameValueCollection From {
    {"type", "license"},
    {"key", key},
    {"hwid", hwid},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then load_user_data(json.info)
        End Sub
        ''' <summary>
        ''' Checks if the current session is validated or not
        ''' </summary>
        Public Sub check()
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "check"},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
        End Sub
        ''' <summary>
        ''' Change the data of an existing user variable, *User must be logged in*
        ''' </summary>
        ''' <paramname="var">User variable name</param>
        ''' <paramname="data">The content of the variable</param>
        Public Sub setvar(var As String, data As String)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "setvar"},
    {"var", var},
    {"data", data},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
        End Sub
        ''' <summary>
        ''' Gets the an existing user variable
        ''' </summary>
        ''' <paramname="var">User Variable Name</param>
        ''' <returns>The content of the user variable</returns>
        Public Function getvar(var As String) As String
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "getvar"},
    {"var", var},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then Return json.response
            Return Nothing
        End Function
        ''' <summary>
        ''' Bans the current logged in user
        ''' </summary>
        Public Sub ban(Optional reason As String = Nothing)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "ban"},
    {"reason", reason},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
        End Sub
        ''' <summary>
        ''' Gets an existing global variable
        ''' </summary>
        ''' <paramname="varid">Variable ID</param>
        ''' <returns>The content of the variable</returns>
        Public Function var(varid As String) As String
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "var"},
    {"varid", varid},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then Return json.message
            Return Nothing
        End Function
        ''' <summary>
        ''' Fetch usernames of online users
        ''' </summary>
        ''' <returns>ArrayList of usernames</returns>
        Public Function fetchOnline() As List(Of users)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "fetchOnline"},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)

            If json.success Then Return json.users
            Return Nothing
        End Function
        ''' <summary>
        ''' Fetch app statistic counts
        ''' </summary>
        Public Sub fetchStats()
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "fetchStats"},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)

            If json.success Then load_app_data(json.appinfo)
        End Sub
        ''' <summary>
        ''' Gets the last 50 sent messages of that channel
        ''' </summary>
        ''' <paramname="channelname">The channel name</param>
        ''' <returns>the last 50 sent messages of that channel</returns>
        Public Function chatget(channelname As String) As List(Of msg)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "chatget"},
    {"channel", channelname},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then
                Return json.messages
            End If
            Return Nothing
        End Function
        ''' <summary>
        ''' Sends a message to the given channel name
        ''' </summary>
        ''' <paramname="msg">Message</param>
        ''' <paramname="channelname">Channel Name</param>
        ''' <returns>If the message was sent successfully, it returns true if not false</returns>
        Public Function chatsend(msg As String, channelname As String) As Boolean
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "chatsend"},
    {"message", msg},
    {"channel", channelname},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then Return True
            Return False
        End Function
        ''' <summary>
        ''' Checks if the current ip address/hwid is blacklisted
        ''' </summary>
        ''' <returns>If found blacklisted returns true if not false</returns>
        Public Function checkblack() As Boolean
            CheckInit()
            Dim hwid As String = WindowsIdentity.GetCurrent().User.Value

            Dim values_to_upload = New NameValueCollection From {
    {"type", "checkblacklist"},
    {"hwid", hwid},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then Return True
            Return False
        End Function
        ''' <summary>
        ''' Sends a request to a webhook that you've added in the dashboard in a safe way without it being showed for example a http debugger
        ''' </summary>
        ''' <paramname="webid">Webhook ID</param>
        ''' <paramname="param">Parameters</param>
        ''' <paramname="body">Body of the request, empty by default</param>
        ''' <paramname="conttype">Content type, empty by default</param>
        ''' <returns>the webhook's response</returns>
        Public Function webhook(webid As String, param As String, Optional body As String = "", Optional conttype As String = "") As String
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "webhook"},
    {"webid", webid},
    {"params", param},
    {"body", body},
    {"conttype", conttype},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then Return json.response
            Return Nothing
        End Function
#Disable Warning BC42304 ' XML documentation parse error
        ''' <summary>
        ''' KeyAuth acts as proxy and downlods the file in a secure way
        ''' </summary>
        ''' <paramname="fileid">File ID</param>
        ''' <returns>The bytes of the download file</returns>
        Public Function download(fileid As String) As Byte()
#Enable Warning BC42304 ' XML documentation parse error
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "file"},
    {"fileid", fileid},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
            If json.success Then Return str_to_byte_arr(json.contents)
            Return Nothing
        End Function
        ''' <summary>
        ''' Logs the IP address,PC Name with a message, if a discord webhook is set up in the app settings, the log will get sent there and the dashboard if not set up it will only be in the dashboard
        ''' </summary>
        ''' <paramname="message">Message</param>
        Public Sub log(message As String)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "log"},
    {"pcuser", Environment.UserName},
    {"message", message},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            req(values_to_upload)
        End Sub
        ''' <summary>
        ''' Change the username of a user, *User must be logged in*
        ''' </summary>
        ''' <paramusername="username">New username.</param>
        Public Sub changeUsername(username As String)
            CheckInit()

            Dim values_to_upload = New NameValueCollection From {
    {"type", "changeUsername"},
    {"newUsername", username},
    {"sessionid", sessionid},
    {"name", name},
    {"ownerid", ownerid}
}

            Dim response = req(values_to_upload)

            Dim json = response_decoder.string_to_generic(Of response_structure)(response)
            load_response_struct(json)
        End Sub

        Public Shared Function checksum(filename As String) As String
            Dim result As String
            Using md As MD5 = MD5.Create()
                Using fileStream = File.OpenRead(filename)
                    Dim value = md.ComputeHash(fileStream)
                    result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant()
                End Using
            End Using
            Return result
        End Function
        Public Shared Sub [error](message As String)
            Dim folder = "Logs", file = Path.Combine(folder, "ErrorLogs.txt")

            If Not Directory.Exists(folder) Then
                Directory.CreateDirectory(folder)
            End If

            If Not IO.File.Exists(file) Then
                Using stream = IO.File.Create(file)
                    Call IO.File.AppendAllText(file, Date.Now.ToString() & " > This is the start of your error logs file")
                End Using
            End If

            Call IO.File.AppendAllText(file, Date.Now.ToString() & $" > {message}" & Environment.NewLine)

            Call Process.Start(New ProcessStartInfo("cmd.exe", $"/c start cmd /C ""color b && title Error && echo {message} && timeout /t 5""") With {
    .CreateNoWindow = True,
    .RedirectStandardOutput = True,
    .RedirectStandardError = True,
    .UseShellExecute = False
})
            Environment.Exit(0)
        End Sub

        Private Shared Function req(post_data As NameValueCollection) As String
            Try
                Using client As WebClient = New WebClient()
                    client.Proxy = Nothing

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf KeyAuth.api.assertSSL

                    Dim stopwatch As Stopwatch = New Stopwatch()
                    stopwatch.Start()

                    Dim raw_response = client.UploadValues("https://keyauth.win/api/1.2/", post_data)

                    stopwatch.Stop()
                    responseTime = stopwatch.ElapsedMilliseconds

                    ServicePointManager.ServerCertificateValidationCallback = Function() True

                    sigCheck(Encoding.Default.GetString(raw_response), client.ResponseHeaders("signature"), post_data.Get(0))

                    Return Encoding.Default.GetString(raw_response)
                End Using
            Catch webex As WebException
                Dim response = CType(webex.Response, HttpWebResponse)
                Select Case response.StatusCode
                    Case 429 ' client hit our rate limit
                        [error]("You're connecting too fast to loader, slow down.")
                        Environment.Exit(0)
                        Return "" ' site won't resolve. you should use keyauth.uk domain since it's not blocked by any ISPs
                    Case Else
                        [error]("Connection failure. Please try again, or contact us for help.")
                        Environment.Exit(0)
                        Return ""
                End Select
            End Try
        End Function

        Private Shared Function assertSSL(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
            If Not certificate.Issuer.Contains("Cloudflare Inc") AndAlso Not certificate.Issuer.Contains("Google Trust Services") AndAlso Not certificate.Issuer.Contains("Let's Encrypt") OrElse sslPolicyErrors <> SslPolicyErrors.None Then
                [error]("SSL assertion fail, make sure you're not debugging Network. Disable internet firewall on router if possible. & echo: & echo If not, ask the developer of the program to use custom domains to fix this.")
                Return False
            End If
            Return True
        End Function

        Private Shared Sub sigCheck(resp As String, signature As String, type As String)
            If Equals(type, "log") OrElse Equals(type, "file") Then ' log doesn't return a response.
                Return
            End If

            Try
                Dim clientComputed = HashHMAC(If(Equals(type, "init"), enckey.Substring(17, 64), enckey), resp)
                If Not CheckStringsFixedTime(clientComputed, signature) Then
                    [error]("Signature checksum failed. Request was tampered with or session ended most likely. & echo: & echo Response: " & resp)
                    Environment.Exit(0)
                End If

            Catch
                [error]("Signature checksum failed. Request was tampered with or session ended most likely. & echo: & echo Response: " & resp)
                Environment.Exit(0)
            End Try
        End Sub

#Region "app_data"
        Public app_data As app_data_class = New app_data_class()

        Public Class app_data_class
            Public Property numUsers As String
            Public Property numOnlineUsers As String
            Public Property numKeys As String
            Public Property version As String
            Public Property customerPanelLink As String
            Public Property downloadLink As String
        End Class

        Private Sub load_app_data(data As app_data_structure)
            app_data.numUsers = data.numUsers
            app_data.numOnlineUsers = data.numOnlineUsers
            app_data.numKeys = data.numKeys
            app_data.version = data.version
            app_data.customerPanelLink = data.customerPanelLink
        End Sub
#End Region

#Region "user_data"
        Public user_data As user_data_class = New user_data_class()

        Public Class user_data_class
            Public Property username As String
            Public Property ip As String
            Public Property hwid As String
            Public Property createdate As String
            Public Property lastlogin As String
            Public Property subscriptions As List(Of Data) ' array of subscriptions (basically multiple user ranks for user with individual expiry dates
        End Class
        Public Class Data
            Public Property subscription As String
            Public Property expiry As String
            Public Property timeleft As String
        End Class

        Private Sub load_user_data(data As user_data_structure)
            user_data.username = data.username
            user_data.ip = data.ip
            user_data.hwid = data.hwid
            user_data.createdate = data.createdate
            user_data.lastlogin = data.lastlogin
            user_data.subscriptions = data.subscriptions ' array of subscriptions (basically multiple user ranks for user with individual expiry dates 
        End Sub
#End Region

#Region "response_struct"
        Public response As response_class = New response_class()

        Public Class response_class
            Public Property success As Boolean
            Public Property message As String
        End Class

        Private Sub load_response_struct(data As response_structure)
            response.success = data.success
            response.message = data.message
        End Sub
#End Region

        Private response_decoder As json_wrapper = New json_wrapper(New response_structure())
    End Class

    Public Module encryption
        Public Function HashHMAC(enckey As String, resp As String) As String
            Dim key = Encoding.ASCII.GetBytes(enckey)
            Dim message = Encoding.ASCII.GetBytes(resp)
            Dim hash = New HMACSHA256(key)
            Return byte_arr_to_str(hash.ComputeHash(message))
        End Function

        Public Function byte_arr_to_str(ba As Byte()) As String
            Dim hex As StringBuilder = New StringBuilder(ba.Length * 2)
            For Each b In ba
                hex.AppendFormat("{0:x2}", b)
            Next
            Return hex.ToString()
        End Function

        Public Function str_to_byte_arr(hex As String) As Byte()
            Try
                Dim NumberChars = hex.Length
                Dim bytes = New Byte(NumberChars / 2 - 1) {}
                For i = 0 To NumberChars - 1 Step 2
                    bytes(i / 2) = Convert.ToByte(hex.Substring(i, 2), 16)
                Next
                Return bytes
            Catch
                api.error("The session has ended, open program again.")
                Environment.Exit(0)
                Return Nothing
            End Try
        End Function

        <MethodImpl(MethodImplOptions.NoInlining Or MethodImplOptions.NoOptimization)>
        Public Function CheckStringsFixedTime(str1 As String, str2 As String) As Boolean
            If str1.Length <> str2.Length Then
                Return False
            End If
            Dim result = 0
            For i = 0 To str1.Length - 1
                'result = (result Or str1(i)) Xor str2(i)
            Next
            Return result = 0
        End Function

        Public Function iv_key() As String
            Return Guid.NewGuid().ToString().Substring(0, 16)
        End Function
    End Module

    Public Class json_wrapper
        Public Shared Function is_serializable(to_check As Type) As Boolean
            Return to_check.IsSerializable OrElse to_check.IsDefined(GetType(DataContractAttribute), True)
        End Function

        Public Sub New(obj_to_work_with As Object)
            current_object = obj_to_work_with

            Dim object_type = current_object.GetType()

            serializer = New DataContractJsonSerializer(object_type)

            If Not is_serializable(object_type) Then Throw New Exception($"the object {current_object} isn't a serializable")
        End Sub

        Public Function string_to_object(json As String) As Object
            Dim buffer = Encoding.Default.GetBytes(json)

            'SerializationException = session expired

            Using mem_stream = New MemoryStream(buffer)
                Return serializer.ReadObject(mem_stream)
            End Using
        End Function

        Public Function string_to_generic(Of T)(json As String) As T
            Return string_to_object(json)
        End Function

        Private serializer As DataContractJsonSerializer

        Private current_object As Object
    End Class
End Namespace
