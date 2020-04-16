Option Explicit On  ' Option Explicit is turned ON
Option Strict On   ' Option Strict is turned ON
Imports System.Data.SqlClient
Imports System.Text

Public Class GenerateReceipt

    ' Private ds As DataSet = New DataSet()
    'Private da As SqlDataAdapter
    ' Private currentRow As Integer = 0

    'Private Sub ShowRecord()

    'txt1.Text = CType(ds.Tables("Payment").Rows(currentRow).Item(0), String)
    'lblDate.Text = CType(ds.Tables("Payment").Rows(currentRow).Item(1), String)

    'End Sub

    ' Private Sub Display()

    'If ds.Tables("Payment").Rows.Count > 0 Then
    ' currentRow = ds.Tables("Payment").Rows.Count - 1
    ' ShowRecord()
    'End If

    'End Sub
    Private Sub ShowR()

        Dim db As New Database1DataContext()

        Dim lst = (From p In db.Payments
                   Select pay = p.paymentID).Max()


        Dim last = (From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders
                    Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID And p.paymentID = lst
                    Select p.paymentID, p.date, p.amount, p.orderID, f.name, f.price).Distinct()




        dgvReceipt.DataSource = last

    End Sub


    Private Sub GenerateReceipt_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Me.PaymentsTableAdapter.Fill(Me.Database1DataSet.Payments)

        Dim conn As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True")

        Dim command As New SqlCommand("INSERT INTO Payment(date, amount, orderID)VALUES(@param1, @param2, @param3)", conn)

        conn.Open()

        Dim MyDateTime As DateTime = Now()
        Dim MyString As String
        MyString = MyDateTime.ToString("ddMMyyyy")
        'Response.Write(MyString)

        ' Dim db As New Database1DataContext()

        'Dim query = From payment In db.Payments
        'Select Case payment.paymentID, payment.Date, payment.amount, payment.orderID

        ' dgvReceipt.DataSource = query.ToList()

        Dim db As New Database1DataContext()

        Dim order As Integer = 1000

        Dim total = (From ord In db.Orders, f In db.Foods, od In db.OrderDetails
                     Where ord.orderID = od.orderID And od.foodID = f.foodID And ord.orderID = order
                     Select od.qtyOrdered * od.Food.price).Sum()



        command.Parameters.Add("@param1", SqlDbType.Date).Value = MyDateTime
        command.Parameters.Add("@param2", SqlDbType.Int).Value = total
        command.Parameters.Add("@param3", SqlDbType.Int).Value = order

        Dim adapter As New SqlDataAdapter(command)

        Dim table As New DataTable()

        adapter.Fill(table)

        'command.ExecuteNonQuery()
        MessageBox.Show("Record Added", "Add Record")
        'db.Payments.InsertOnSubmit(Payment)
        db.SubmitChanges()
        CloseConnection()


        ShowR()



        ' Dim dt = (From p In db.Payments, order In db.Orders
        'Where p.orderID = order.orderID And order.orderID = o
        'lect Case p.date).Single


        ' lblID.Text = CStr(dt)

        'If ds.Tables.Count = 0 Then Exit Sub
        ' dt = ds.Tables(0)




        'Dim query = From payment In db.Payments
        'Select Case payment.paymentID, payment.date, payment.amount, payment.orderID

        ' dgvReceipt.DataSource = query.ToList()






        'dgvReceipt.DataSource = db.Payments

        'Dim cal As IEnumerable(Of Double) =
        'From orderdetail In db.OrderDetails,
        'Food In db.Foods
        'Where orderdetail.foodID = food.foodID
        'Select Case orderdetail.qtyOrdered, food.price

        ' Dim sum As Double = cal


        'dgv.DataSource = query.ToList()



    End Sub


End Class