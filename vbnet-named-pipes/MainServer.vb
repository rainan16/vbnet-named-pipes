Module Main

    Sub Main()

        Dim maxClients = 2

        StartServer(maxClients)

    End Sub

    Private Sub StartServer(ByVal maxClients As Integer)

        Dim server = New PipeServerContainer()

        Console.WriteLine($"Waiting for clients ... (max. {maxClients} clients are supported)")
        Task.Run(Function() server.StartServerAsync(maxClients))

        While server.ClientsConnected = 0
            Task.Delay(100).Wait()
        End While

        Do
            Console.WriteLine("Server sending to client: " & Now)
            server.Write(Now.ToString())
            Console.WriteLine("Hit ESC to exit or ENTER to retry.")
        Loop Until Console.ReadKey().Key = ConsoleKey.Escape

        server.Write(Nothing)
        server.Close()

    End Sub

End Module

