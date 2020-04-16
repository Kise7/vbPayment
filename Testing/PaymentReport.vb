Option Explicit On  ' Option Explicit is turned ON
Option Strict On   ' Option Strict is turned ON
Imports System.Data.SqlClient
Imports System.Text
Public Class PaymentReport
    Private Sub BindData()
        'Dim oid As Double = Double.Parse(txtDate.Text)


        Dim db As New Database1DataContext()

        ' LINQ query that selects Customer records from the database
        ' Also apply filters that specified via the Where clause
        Dim rs = From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders
                 Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID
                 Select c.custID, o.orderID, f.name, f.price, od.qtyOrdered, p.amount

        'dgvHistory.DataSource = rs ' Bind the LINQ query result to the data grid view

        ' lblCount.Text = rs.Count().ToString("0 record(s)") ' Get the count of the query result
    End Sub

    Private Sub PaymentReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindData()
    End Sub
End Class