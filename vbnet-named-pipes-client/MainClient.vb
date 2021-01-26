Imports System.IO
Imports System.IO.Pipes

Module MainClient
    Const NAMEDPIPEDSERVERID = "MyNamedPipeServerSample"

    Sub Main()

        Console.WriteLine("Waiting for Server message ...")
        Task.Run(Function() ReadMessagesAsync()).Wait()

    End Sub

    Async Function ReadMessagesAsync() As Task

        Try
            Dim result As String
            Using ClientInstance As New NamedPipeClientStream(NAMEDPIPEDSERVERID)
                Await ClientInstance.ConnectAsync()
                Console.WriteLine("There are currently {0} pipe server instances open.", ClientInstance.NumberOfServerInstances)

                Using reader As New StreamReader(ClientInstance)
                    Do
                        result = Await reader.ReadLineAsync()
                        If Not IsNothing(result) Then Console.WriteLine("Received: " & result)
                    Loop Until IsNothing(result)
                End Using
            End Using

        Catch ex As Exception
            Throw 'TODO

        End Try

    End Function

End Module
