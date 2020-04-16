Module Module1
    Public conn As New System.Data.SqlClient.SqlConnection

    Public Function OpenConnection() As Boolean
        Dim strMySQLConnectionString As String

        strMySQLConnectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True"
        Try
            conn.ConnectionString = strMySQLConnectionString
            conn.Open()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub CloseConnection()
        conn.Close()
    End Sub


    'Sub Main()
    '   Console.WriteLine("Current Date: ")
    'Dim dt As Date = Today
    '   Console.WriteLine("Today is: {0}", dt)
    '  Console.ReadKey()
    'End Sub


    'Me.FoodTableAdapter.Fill(Me.Database1DataSet.Food)
    ' Dim db As New Database1DataContext()

    ' Public Students(9) As Student
    ' Public Count As Integer = 0

    'Public Function GetNextId() As String
    'Return (Count + 1).ToString("88WAR00000")
    ' End Function
End Module
