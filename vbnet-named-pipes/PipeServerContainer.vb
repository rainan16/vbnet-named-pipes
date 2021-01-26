Imports System.IO
Imports System.IO.Pipes

Public Class PipeServerContainer
    Const NAMEDPIPEDSERVERID = "MyNamedPipeServerSample"

    Property ClientsConnected As Integer

    Private ServerInstances As New Dictionary(Of Integer, NamedPipeServerStream)
    Private WriterInstances As New Dictionary(Of Integer, StreamWriter)


    Async Function StartServerAsync(clientCount As Integer) As Task

        For key = 1 To clientCount
            ServerInstances.Add(key, New NamedPipeServerStream(NAMEDPIPEDSERVERID, PipeDirection.InOut, clientCount, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
            Await ServerInstances(key).WaitForConnectionAsync()
            WriterInstances.Add(key, New StreamWriter(ServerInstances(key)))
            ClientsConnected = key
        Next

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
                    If ServerInstance.Value.IsConnected Then
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
