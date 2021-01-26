Imports System.IO
Imports System.IO.Pipes

Public Class PipeServerContainer
    Const NAMEDPIPEDSERVERID = "MyNamedPipeServerSample"

    Property ClientsConnected As Boolean

    Private ReadOnly ServerInstances As New Dictionary(Of Integer, NamedPipeServerStream)
    Private ReadOnly WriterInstances As New Dictionary(Of Integer, StreamWriter)

    ''' <summary>
    ''' Know issues: if pipes are closed (e.g. client dies) no new client are accepted
    ''' </summary>
    Async Function StartServerAsync(clientMaxCount As Integer) As Task

        Try
            For key = 1 To clientMaxCount
                ServerInstances.Add(key, New NamedPipeServerStream(NAMEDPIPEDSERVERID, PipeDirection.InOut, clientMaxCount, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
                Await ServerInstances(key).WaitForConnectionAsync()
                WriterInstances.Add(key, New StreamWriter(ServerInstances(key)))
                ClientsConnected = True
            Next

        Catch ex As Exception
            Throw 'TODO
        End Try

    End Function

    Sub Write(msg As String)

        Try
            For Each ServerInstance In ServerInstances
                If ServerInstance.Value.IsConnected Then
                    WriterInstances(ServerInstance.Key).WriteLine(msg)
                    WriterInstances(ServerInstance.Key).Flush()
                End If
            Next

        Catch ex As Exception
            Throw 'TODO
        End Try

    End Sub

    Sub Close()

        Try
            For Each ServerInstance In ServerInstances
                If Not IsNothing(WriterInstances(ServerInstance.Key)) Then
                    WriterInstances(ServerInstance.Key).Close()
                End If
                If Not IsNothing(ServerInstance.Value) Then
                    If ServerInstance.Value.IsConnected Then    'TODO to we need this?
                        ServerInstance.Value.Disconnect()
                        ServerInstance.Value.Close()
                    End If
                End If
            Next

        Catch ex As Exception
            Throw 'TODO
        End Try

    End Sub
End Class
