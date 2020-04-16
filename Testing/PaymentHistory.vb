﻿Option Explicit On  ' Option Explicit is turned ON
Option Strict On   ' Option Strict is turned ON
Imports System.Data.SqlClient
Imports System.Text
Public Class PaymentHistory
    Private Sub PaymentHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindData()
    End Sub

    Private Sub BindData()
        ' Dim oid As String = CDate(txtDate.Text)


        Dim db As New Database1DataContext()

        Dim cust As Integer = 1001

        ' Let shortexpiryDate = p.date.ToShortDateString()


        'Dim rs = From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders
        ' Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID 'And
        'Select c.custID, o.orderID, f.name, f.price, od.qtyOrdered, p.amount

        'Dim rs = (From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders
        'Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID And c.custID = cust 'And shortexpiryDate >= txtDate.Text
        'Select Case c.custID, o.orderID, f.name, f.price, od.qtyOrdered, p.amount).Distinct()

        Dim rs = From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders
                 Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID And c.custID = cust 'And shortexpiryDate >= txtDate.Text
                 Order By p.date
                 Select c.custID, o.orderID, f.name, f.price, od.qtyOrdered, p.date, p.amount Distinct

        dgvHistory.DataSource = rs.ToList() ' Bind the LINQ query result to the data grid view

        ' lblCount.Text = rs.Count().ToString("0 record(s)") ' Get the count of the query result
    End Sub

    Private Sub txtDate_TextChanged(sender As Object, e As EventArgs)

    End Sub
End Class