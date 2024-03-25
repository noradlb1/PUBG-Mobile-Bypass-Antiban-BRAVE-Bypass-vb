Imports System

Namespace BraveBypass
    Partial Class bypass
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <paramname="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(bypass))
            label1 = New Windows.Forms.Label()
            status = New Windows.Forms.Label()
            button1 = New Windows.Forms.Button()
            textBox1 = New Windows.Forms.TextBox()
            button2 = New Windows.Forms.Button()
            label2 = New Windows.Forms.Label()
            label4 = New Windows.Forms.Label()
            statusv = New Windows.Forms.Label()
            SuspendLayout()
            ' 
            ' label1
            ' 
            label1.AutoSize = True
            label1.BackColor = Drawing.Color.Transparent
            label1.Font = New Drawing.Font("Microsoft Sans Serif", 20.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 0)
            label1.ForeColor = Drawing.Color.FromArgb(128, 255, 255)
            label1.Location = New Drawing.Point(410, -1)
            label1.Name = "label1"
            label1.Size = New Drawing.Size(33, 31)
            label1.TabIndex = 1
            label1.Text = "X"
            AddHandler label1.Click, New EventHandler(AddressOf label1_Click)
            ' 
            ' status
            ' 
            status.AutoSize = True
            status.BackColor = Drawing.Color.Transparent
            status.Font = New Drawing.Font("Microsoft Sans Serif", 12.0F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 0)
            status.Location = New Drawing.Point(87, 36)
            status.Name = "status"
            status.Size = New Drawing.Size(268, 20)
            status.TabIndex = 2
            status.Text = "WELCOME TO BRAVE BYPASS"
            AddHandler status.Click, New EventHandler(AddressOf status_Click)
            ' 
            ' button1
            ' 
            button1.BackColor = Drawing.Color.Aquamarine
            button1.FlatStyle = Windows.Forms.FlatStyle.Flat
            button1.Font = New Drawing.Font("Microsoft Sans Serif", 12.0F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 0)
            button1.ForeColor = Drawing.SystemColors.ControlText
            button1.Location = New Drawing.Point(241, 188)
            button1.Name = "button1"
            button1.Size = New Drawing.Size(163, 38)
            button1.TabIndex = 3
            button1.Text = "LOGIN"
            button1.UseVisualStyleBackColor = False
            AddHandler button1.Click, New EventHandler(AddressOf button1_Click)
            ' 
            ' textBox1
            ' 
            textBox1.Font = New Drawing.Font("Microsoft Sans Serif", 9.749999F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 0)
            textBox1.Location = New Drawing.Point(97, 135)
            textBox1.Name = "textBox1"
            textBox1.Size = New Drawing.Size(247, 22)
            textBox1.TabIndex = 4
            AddHandler textBox1.TextChanged, New EventHandler(AddressOf textBox1_TextChanged)
            ' 
            ' button2
            ' 
            button2.BackColor = Drawing.Color.Aquamarine
            button2.FlatStyle = Windows.Forms.FlatStyle.Popup
            button2.Font = New Drawing.Font("Microsoft Sans Serif", 12.0F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 0)
            button2.ForeColor = Drawing.Color.Black
            button2.Location = New Drawing.Point(31, 188)
            button2.Name = "button2"
            button2.Size = New Drawing.Size(163, 38)
            button2.TabIndex = 5
            button2.Text = "GET KEY"
            button2.UseVisualStyleBackColor = False
            AddHandler button2.Click, New EventHandler(AddressOf button2_Click)
            ' 
            ' label2
            ' 
            label2.AutoSize = True
            label2.Location = New Drawing.Point(156, 309)
            label2.Name = "label2"
            label2.Size = New Drawing.Size(0, 13)
            label2.TabIndex = 6
            ' 
            ' label4
            ' 
            label4.AutoSize = True
            label4.BackColor = Drawing.Color.Transparent
            label4.Font = New Drawing.Font("Microsoft Sans Serif", 14.25F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 162)
            label4.Location = New Drawing.Point(159, 99)
            label4.Name = "label4"
            label4.Size = New Drawing.Size(126, 24)
            label4.TabIndex = 8
            label4.Text = "ENTER KEY"
            ' 
            ' statusv
            ' 
            statusv.AutoSize = True
            statusv.Font = New Drawing.Font("Microsoft Sans Serif", 12.0F, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, 162)
            statusv.Location = New Drawing.Point(12, 250)
            statusv.Name = "statusv"
            statusv.Size = New Drawing.Size(0, 20)
            statusv.TabIndex = 9
            ' 
            ' bypass
            ' 
            AutoScaleDimensions = New Drawing.SizeF(6.0F, 13.0F)
            AutoScaleMode = Windows.Forms.AutoScaleMode.Font
            BackColor = Drawing.Color.Black
            ClientSize = New Drawing.Size(445, 279)
            Controls.Add(statusv)
            Controls.Add(label4)
            Controls.Add(label2)
            Controls.Add(button2)
            Controls.Add(textBox1)
            Controls.Add(button1)
            Controls.Add(status)
            Controls.Add(label1)
            ForeColor = Drawing.Color.White
            FormBorderStyle = Windows.Forms.FormBorderStyle.None
            Icon = CType(resources.GetObject("$this.Icon"), Drawing.Icon)
            Name = "bypass"
            Text = "Form1"
            AddHandler Load, New EventHandler(AddressOf bypass_Load)
            ResumeLayout(False)
            PerformLayout()

        End Sub

#End Region
        Private label1 As Windows.Forms.Label
        Private status As Windows.Forms.Label
        Private button1 As Windows.Forms.Button
        Private textBox1 As Windows.Forms.TextBox
        Private button2 As Windows.Forms.Button
        Private label2 As Windows.Forms.Label
        Private label4 As Windows.Forms.Label
        Private statusv As Windows.Forms.Label
    End Class
End Namespace
