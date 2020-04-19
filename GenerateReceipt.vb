Option Explicit On  ' Option Explicit is turned ON
Option Strict On   ' Option Strict is turned ON
Imports System.Data.SqlClient
Imports System.Text

Public Class GenerateReceipt

    Private bitmap As Bitmap
    Friend disTotalReceipt As String
    Private Sub ShowReceipt()

        Dim db As New Database1DataContext()

        'Retrieve latest payment record
        Dim addedRecord = (From p In db.Payments
                           Select pay = p.paymentID).Max()

        'Display payment record
        Dim payRecord = (From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders, d In db.Deliveries
                         Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID And d.orderID = o.orderID And p.paymentID = addedRecord
                         Select p.paymentID, p.orderID, p.date, f.name, od.qtyOrdered, f.price).Distinct()

        dgvReceipt.DataSource = payRecord

        'display total amount of order
        Dim displayTotal = (From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders, d In db.Deliveries
                            Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID And d.orderID = o.orderID And p.paymentID = addedRecord
                            Select p.amount).Sum()


        lblTotalAmount.Text = "RM " + CStr(displayTotal)

        disTotalReceipt = "RM " + CStr(displayTotal)

    End Sub


    Private Sub GenerateReceipt_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim conn As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True")

        'Insert payment record to payment database
        Dim command As New SqlCommand("INSERT INTO Payment(date, amount, orderID)VALUES(@param1, @param2, @param3)", conn)

        conn.Open()

        'Generate current date
        Dim MyDateTime As DateTime = Now()
        Dim MyString As String
        MyString = MyDateTime.ToString("ddMMyyyy")


        Dim db As New Database1DataContext()

        'Hard code order ID
        Dim order As Integer = 1000

        'Calculate total price
        Dim total = (From ord In db.Orders, f In db.Foods, od In db.OrderDetails
                     Where ord.orderID = od.orderID And od.foodID = f.foodID And ord.orderID = order
                     Select od.qtyOrdered * od.Food.price).Sum()

        'add to payment database
        command.Parameters.Add("@param1", SqlDbType.Date).Value = MyDateTime
        command.Parameters.Add("@param2", SqlDbType.Int).Value = total
        command.Parameters.Add("@param3", SqlDbType.Int).Value = order

        Dim adapter As New SqlDataAdapter(command)

        Dim table As New DataTable()

        adapter.Fill(table)

        MessageBox.Show("Record Added", "Add Record")
        db.SubmitChanges()
        CloseConnection()

        'Call to display receipt
        ShowReceipt()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        'Print command
        bitmap = New Bitmap(Me.dgvReceipt.Width, Me.dgvReceipt.Height)
        dgvReceipt.DrawToBitmap(bitmap, New Rectangle(0, 0, Me.dgvReceipt.Width, Me.dgvReceipt.Height))

        PrintPreviewDialog1.PrintPreviewControl.Zoom = 1
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()

    End Sub



    Private Sub PrintDocument1_PrintPage_1(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        'Handle format for print receipt
        Dim fntPrint As New Font("Arial", 24)
        Dim fntHeading As New Font("Arial", 36, FontStyle.Bold)
        Dim sngLineHeight As Single = fntPrint.GetHeight + 1
        Dim sngXLocation As Single = e.MarginBounds.Left
        Dim sngYLocation As Single = e.MarginBounds.Top

        'Center Alignment
        Dim sf As New StringFormat
        sf.LineAlignment = StringAlignment.Center
        sf.Alignment = StringAlignment.Center

        'Content design
        e.Graphics.DrawString("                                                                        EatLah Food Ordering System", fntPrint, Brushes.Black, sngXLocation, sngYLocation, sf)
        sngYLocation += sngLineHeight + 1
        e.Graphics.DrawString("                                                                 Receipt", fntPrint, Brushes.Black, sngXLocation, sngYLocation, sf)
        sngYLocation += sngLineHeight + 10
        e.Graphics.DrawString("                            =================================================================", fntPrint, Brushes.Black, sngXLocation, sngYLocation, sf)

        Dim ratio As Single = CSng(bitmap.Width / bitmap.Height)

        'Margin
        If ratio > e.MarginBounds.Width / e.MarginBounds.Height Then

            e.Graphics.DrawImage(bitmap,
            e.MarginBounds.Left,
            CInt(e.MarginBounds.Top + (e.MarginBounds.Height / 2) - ((e.MarginBounds.Width / ratio) / 2)),
            e.MarginBounds.Width,
            CInt(e.MarginBounds.Width / ratio))


        Else


            e.Graphics.DrawImage(bitmap,
            CInt(e.MarginBounds.Left + (e.MarginBounds.Width / 2) - (e.MarginBounds.Height * ratio / 2)),
            e.MarginBounds.Top,
            CInt(e.MarginBounds.Height * ratio),
            e.MarginBounds.Height)


        End If

        sngYLocation += sngLineHeight + 560
        e.Graphics.DrawString("                                                                     Total:" + disTotalReceipt, fntPrint, Brushes.Black, sngXLocation, sngYLocation, sf)

        sngYLocation += sngLineHeight + 100
        e.Graphics.DrawString("                                                                  -----End-----", fntPrint, Brushes.Black, sngXLocation, sngYLocation, sf)

    End Sub
End Class