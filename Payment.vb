Option Explicit On  ' Option Explicit is turned ON
Option Strict On   ' Option Strict is turned ON
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports System.Linq
Imports System.Configuration

Public Class Payment

    Private Sub radCard_CheckedChanged(sender As Object, e As EventArgs) Handles radCard.CheckedChanged 'Debit/Credit card

        'enable these input when this radio button is selected
        cboBank.Enabled = True
        mskCard.Enabled = True
        cboMonth.Enabled = True
        cboYear.Enabled = True
        txtHolder.Enabled = True
        mskCVV.Enabled = True

        mskCard.Text = ""
        txtHolder.Text = ""
        mskCVV.Text = ""

        cboBank.Focus()
        mskCard.Focus()
        cboMonth.Focus()
        cboYear.Focus()
        txtHolder.Focus()
        mskCVV.Focus()

    End Sub

    Private Sub Calculate()
        'Hard code order id
        Dim oid As Integer = 1001

        Dim db As New Database1DataContext()

        'display total amount of order
        Dim displayTotal = (From o In db.Orders, f In db.Foods, od In db.OrderDetails
                            Where o.orderID = od.orderID And od.foodID = f.foodID And o.orderID = oid
                            Select od.qtyOrdered * od.Food.price).Sum()

        lblTotal.Text = "RM " + CStr(displayTotal)

        'display order details
        Dim displayOrder = From o In db.Orders, f In db.Foods, od In db.OrderDetails
                           Where o.orderID = od.orderID And od.foodID = f.foodID And o.orderID = oid
                           Select o.orderID, f.name, od.qtyOrdered, od.Food.price

        dgvOrder.DataSource = displayOrder

    End Sub

    Private Sub Payment_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Function to retrieve order
        Calculate()

        'disable input at default
        cboBank.Enabled = False
        mskCard.Enabled = False
        cboMonth.Enabled = False
        cboYear.Enabled = False
        txtHolder.Enabled = False
        mskCVV.Enabled = False

        'default selected radio button
        radCard.Select()

        cboBank.Items.Add("Maybank")
        cboBank.Items.Add("Public Bank")
        cboBank.Items.Add("OCBC Bank")
        cboBank.Items.Add("Standard Chartered Bank")
        cboBank.Items.Add("CIMB Bank")
        cboBank.Items.Add("Hong Leong Bank")

        'add month 
        For i = 1 To 12
            cboMonth.Items.Add(i)
        Next

        'add year
        For i = 2021 To 2100
            cboYear.Items.Add(i)
        Next

        cboBank.SelectedIndex = 0
        cboMonth.SelectedIndex = 0
        cboYear.SelectedIndex = 0

    End Sub

    Private Sub radCash_CheckedChanged(sender As Object, e As EventArgs) Handles radCash.CheckedChanged
        'disbale input 
        cboBank.Enabled = False
        mskCard.Enabled = False
        cboMonth.Enabled = False
        cboYear.Enabled = False
        txtHolder.Enabled = False
        mskCVV.Enabled = False
    End Sub

    Private Sub radBanking_CheckedChanged(sender As Object, e As EventArgs) Handles radBanking.CheckedChanged
        'disable input
        cboBank.Enabled = False
        mskCard.Enabled = False
        cboMonth.Enabled = False
        cboYear.Enabled = False
        txtHolder.Enabled = False
        mskCVV.Enabled = False
    End Sub

    Private Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click

        ' For validation purpose
        Dim err As New StringBuilder() ' For holding the error messages
        Dim ctr As Control = Nothing ' For remember the first control with input error

        ' Read inputs
        Dim cardNo As String = If(mskCard.MaskCompleted, mskCard.Text, "")
        Dim cvv As String = If(mskCVV.MaskCompleted, mskCVV.Text, "")
        Dim holder As String = txtHolder.Text.Trim()

        ' Validation check
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
            'GenerateReceipt.ShowDialog(Me)

            '***Payment history not called yet, use main menu button to call
            PaymentHistory.ShowDialog(Me)
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboBank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBank.SelectedIndexChanged
        Dim selectedBank As String = CStr(cboBank.SelectedItem)

    End Sub

    Private Sub cboMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMonth.SelectedIndexChanged
        Dim month As Integer = CInt(cboMonth.SelectedItem)
    End Sub

    Private Sub cboYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboYear.SelectedIndexChanged
        Dim year As Integer = CInt(cboYear.SelectedItem)

    End Sub

End Class
