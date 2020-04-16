Option Explicit On  ' Option Explicit is turned ON
Option Strict On   ' Option Strict is turned ON
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports System.Linq
Imports System.Configuration


Public Class Payment

    Private Sub radCard_CheckedChanged(sender As Object, e As EventArgs) Handles radCard.CheckedChanged


        mskCard.Enabled = True
        cboMonth.Enabled = True
        cboYear.Enabled = True
        txtHolder.Enabled = True
        mskCVV.Enabled = True
        cboBank.Enabled = True

        mskCard.Text = ""
        mskCVV.Text = ""
        txtHolder.Text = ""


        mskCard.Focus()
        mskCVV.Focus()
        txtHolder.Focus()
        cboMonth.Focus()
        cboYear.Focus()
        cboBank.Focus()

    End Sub

    Private Sub BindData()
        Dim db As New Database1DataContext()

        Dim query = From o In db.Orders
                    Select o.orderID, o.status

        ' dgvOrder.DataSource = query.ToList()
    End Sub

    Private Sub Calculate()
        Dim oid As Integer = 1001

        ' Dim dt As DataTable = Me.BindData()

        Dim db As New Database1DataContext()

        'Dim customers = dt.AsEnumerable().Select(Function(customer) New With {
        '.CustomerId = customer.Field(Of Integer)("CustomerId"),
        '.Name = customer.Field(Of String)("Name"),
        '.Country = customer.Field(Of String)("Country")
        '}).Where(Function(customer) customer.Country = country)



        'Dim query = From p In db.Payments, od In db.OrderDetails, f In db.Foods, c In db.Customers, o In db.Orders
        ' Where p.orderID = od.orderID And od.foodID = f.foodID And c.custID = o.custID And od.orderID = o.orderID And o.orderID = oid
        ' Select o.orderID, od.qtyOrdered

        'Dim qty As Integer = query.First



        Dim rs = (From o In db.Orders, f In db.Foods, od In db.OrderDetails
                  Where o.orderID = od.orderID And od.foodID = f.foodID And o.orderID = oid
                  Select od.qtyOrdered * od.Food.price).Sum()

        lblTotal.Text = CStr(rs)

        Dim dgv = From o In db.Orders, f In db.Foods, od In db.OrderDetails
                  Where o.orderID = od.orderID And od.foodID = f.foodID And o.orderID = oid
                  Select o.orderID, f.name, od.qtyOrdered, od.Food.price

        dgvOrder.DataSource = dgv

    End Sub

    Private Sub Payment_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'BindData()
        Calculate()

        txtHolder.Enabled = False
        cboBank.Enabled = False
        mskCard.Enabled = False
        mskCVV.Enabled = False
        cboMonth.Enabled = False
        cboYear.Enabled = False

        ' Next

        radCard.Select()

        cboBank.Items.Add("Maybank")
        cboBank.Items.Add("Public Bank")
        cboBank.Items.Add("OCBC Bank")
        cboBank.Items.Add("Standard Chartered Bank")
        cboBank.Items.Add("CIMB Bank")
        cboBank.Items.Add("Hong Leong Bank")

        For i = 1 To 12
            cboMonth.Items.Add(i)
        Next

        For i = 2021 To 2060
            cboYear.Items.Add(i)
        Next

        cboBank.SelectedIndex = 0
        cboMonth.SelectedIndex = 0
        cboYear.SelectedIndex = 0




    End Sub

    Private Sub radCash_CheckedChanged(sender As Object, e As EventArgs) Handles radCash.CheckedChanged
        txtHolder.Enabled = False
        cboBank.Enabled = False
        mskCard.Enabled = False
        mskCVV.Enabled = False
        cboMonth.Enabled = False
        cboYear.Enabled = False
    End Sub

    Private Sub radBanking_CheckedChanged(sender As Object, e As EventArgs) Handles radBanking.CheckedChanged
        txtHolder.Enabled = False
        cboBank.Enabled = False
        mskCard.Enabled = False
        mskCVV.Enabled = False
        cboMonth.Enabled = False
        cboYear.Enabled = False
    End Sub

    Private Sub txtCardNo_TextChanged(sender As Object, e As EventArgs)

        'Try
        ' Dim cardNo As Integer = txtCardNo.Text
        'cardNo = Integer.Parse(txtCardNo.Text)

        ' Catch ex As Exception

        'MessageBox.Show("Only numbers are accepted", "Error")


        'End Try

    End Sub

    Private Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        'Dim cardNum As String
        'Dim name As String = txtName.Text.Trim()
        'Dim program As String = CStr(cboProgram.SelectedItem) ' If no selection = Nothing
        'Dim year As Integer = CInt(cboYear.SelectedItem) ' If no selection = 0
        'Dim cgpa As Decimal = 0D
        'Dim cardNo As Integer = Integer.Parse(txtCardNo.Text)
        'Dim expiry As Integer = Integer.Parse(txtExpiry.Text)
        'Dim holder As String = txtHolder.Text.Trim()
        'Dim cvv As Integer = Integer.Parse(txtCVV.Text)

        ' (1) For validation purpose
        Dim err As New StringBuilder() ' For holding the error messages
        Dim ctr As Control = Nothing ' For remember the first control with input error

        ' (2) Read inputs
        ' Dim id As String = If(mskID.MaskCompleted, mskID.Text, "")
        'Dim name As String = txtName.Text.Trim()

        'Try

        Dim cardNo As String = If(mskCard.MaskCompleted, mskCard.Text, "")
        Dim cvv As String = If(mskCVV.MaskCompleted, mskCVV.Text, "")
        Dim holder As String = txtHolder.Text.Trim()

        If radCard.Checked = True Then

            If cardNo = "" Then
                err.AppendLine("- Invalid Card Number")
                ctr = If(ctr, mskCard) ' Remember the control, if none has been remember before
            End If


            If cvv = "" Then
                err.AppendLine("- Invalid CVV")
                ctr = If(ctr, mskCVV) ' Remember the control, if none has been remember before

            End If

            '  Validate Holder Name
            If holder = "" Then
                err.AppendLine("- Holder name empty")
                ctr = If(ctr, txtHolder)
            End If




            'If cgpa = -1D Then
            'err.AppendLine("- Invalid [CGPA]")
            'ctr = If(ctr, mskCGPA)
            'ElseIf cgpa < 0D Or cgpa > 4D Then
            'err.AppendLine("- [CGPA] must between 0.0000 to 4.0000")
            ' If(ctr, mskCGPA)
            'End If




            '  Check if there is input error
            If err.Length > 0 Then
                MessageBox.Show(err.ToString(), "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ctr.Focus() ' Focus on the first control with input error
                Return
            Else
                MessageBox.Show("Payment successfully", "Payment Completed", MessageBoxButtons.OK)
                GenerateReceipt.ShowDialog(Me)
            End If

        ElseIf radCash.Checked = True OrElse radBanking.Checked = True Then
            MessageBox.Show("Checkout successfully", "Checkout Completed", MessageBoxButtons.OK)
            ' MessageBox.Show(DateTime.Now.ToString("dd-MM-yyyy"))
            'GenerateReceipt.ShowDialog(Me)
            PaymentHistory.ShowDialog(Me)

            'Try




            'Catch ex As Exception


            'End Try
        End If


        'Try

        'cardNo = Integer.Parse(txtCardNo.Text)
        ' holder = txtHolder.Text


        ' Catch ex As Exception

        ' MessageBox.Show("Only numbers are accepted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        'txtCardNo.Focus()

        ' End Try


    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboBank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBank.SelectedIndexChanged
        Dim n As String = CStr(cboBank.SelectedItem)
        'n = CStr(cboBank.SelectedItem)

    End Sub

    Private Sub cboMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMonth.SelectedIndexChanged
        Dim month As Integer = CInt(cboMonth.SelectedItem)
    End Sub

    Private Sub cboYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboYear.SelectedIndexChanged
        Dim year As Integer = CInt(cboYear.SelectedItem)

    End Sub


End Class
