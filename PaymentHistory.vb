Option Explicit On  ' Option Explicit is turned ON
Option Strict On   ' Option Strict is turned ON
Imports System.Data.SqlClient
Imports System.Text
Public Class PaymentHistory
    Private Sub PaymentHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindData()
    End Sub

    Private Sub BindData()

        Dim db As New Database1DataContext()

        'Hard code customer ID
        Dim cust As Integer = 1001


        Dim history = From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders
                      Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID And c.custID = cust 'And shortexpiryDate >= txtDate.Text
                      Order By p.date
                      Select p.paymentID, o.orderID, f.name, od.qtyOrdered, f.price, p.date, p.amount Distinct

        dgvHistory.DataSource = history.ToList() ' Bind the LINQ query result to the data grid view

        lblCount.Text = history.Count().ToString("0 record(s)") ' Get the count of the query result
    End Sub



End Class