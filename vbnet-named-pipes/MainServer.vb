Module Main

    Sub Main()

        Dim maxClients = 2

        StartServer(maxClients)

    End Sub

    Private Sub StartServer(ByVal maxClients As Integer)

        Dim server = New PipeServerContainer()
        Task.Run(Function() server.StartServerAsync(maxClients))

        ' wait for the first client to connect
        Console.WriteLine($"Waiting for clients ... (max. {maxClients} clients are supported)")
        While Not server.ClientsConnected
            Task.Delay(100).Wait()
        End While

        Do
            Console.WriteLine("Server is sending this to client: " & Now)
            server.Write(Now.ToString())
            Console.WriteLine("Hit ESC to exit or ENTER to retry.")
        Loop Until Console.ReadKey().Key = ConsoleKey.Escape

        ' send close to clients 
        server.Write(Nothing)   'TODO refactor (do not use nothing)
        server.Close()          'TODO refactor (IDisposable)

    End Sub

End Module

