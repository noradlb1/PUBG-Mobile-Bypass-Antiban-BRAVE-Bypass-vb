Imports System

Namespace BraveBypass
    Partial Class bypass1
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bypass1))
            Me.button1 = New System.Windows.Forms.Button()
            Me.button2 = New System.Windows.Forms.Button()
            Me.button3 = New System.Windows.Forms.Button()
            Me.radioButton1 = New System.Windows.Forms.RadioButton()
            Me.radioButton2 = New System.Windows.Forms.RadioButton()
            Me.radioButton3 = New System.Windows.Forms.RadioButton()
            Me.radioButton4 = New System.Windows.Forms.RadioButton()
            Me.status = New System.Windows.Forms.Label()
            Me.label1 = New System.Windows.Forms.Label()
            Me.radioButton5 = New System.Windows.Forms.RadioButton()
            Me.button4 = New System.Windows.Forms.Button()
            Me.statusv = New System.Windows.Forms.Label()
            Me.label2 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'button1
            '
            Me.button1.BackColor = System.Drawing.Color.Transparent
            Me.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green
            Me.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.button1.ForeColor = System.Drawing.Color.Aqua
            Me.button1.Location = New System.Drawing.Point(60, 203)
            Me.button1.Name = "button1"
            Me.button1.Size = New System.Drawing.Size(179, 28)
            Me.button1.TabIndex = 0
            Me.button1.Text = "Start Emulator"
            Me.button1.UseVisualStyleBackColor = False
            '
            'button2
            '
            Me.button2.BackColor = System.Drawing.Color.Transparent
            Me.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green
            Me.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.button2.ForeColor = System.Drawing.Color.Aqua
            Me.button2.Location = New System.Drawing.Point(60, 237)
            Me.button2.Name = "button2"
            Me.button2.Size = New System.Drawing.Size(179, 29)
            Me.button2.TabIndex = 1
            Me.button2.Text = "Start Bypass"
            Me.button2.UseVisualStyleBackColor = False
            '
            'button3
            '
            Me.button3.BackColor = System.Drawing.Color.Transparent
            Me.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
            Me.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.button3.ForeColor = System.Drawing.Color.Aqua
            Me.button3.Location = New System.Drawing.Point(60, 272)
            Me.button3.Name = "button3"
            Me.button3.Size = New System.Drawing.Size(179, 28)
            Me.button3.TabIndex = 2
            Me.button3.Text = "Safe Exit"
            Me.button3.UseVisualStyleBackColor = False
            '
            'radioButton1
            '
            Me.radioButton1.AutoSize = True
            Me.radioButton1.BackColor = System.Drawing.Color.Transparent
            Me.radioButton1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radioButton1.ForeColor = System.Drawing.Color.Aquamarine
            Me.radioButton1.Location = New System.Drawing.Point(27, 121)
            Me.radioButton1.Name = "radioButton1"
            Me.radioButton1.Size = New System.Drawing.Size(45, 20)
            Me.radioButton1.TabIndex = 3
            Me.radioButton1.Text = "GL"
            Me.radioButton1.UseVisualStyleBackColor = False
            '
            'radioButton2
            '
            Me.radioButton2.AutoSize = True
            Me.radioButton2.BackColor = System.Drawing.Color.Black
            Me.radioButton2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radioButton2.ForeColor = System.Drawing.Color.Aquamarine
            Me.radioButton2.Location = New System.Drawing.Point(72, 121)
            Me.radioButton2.Name = "radioButton2"
            Me.radioButton2.Size = New System.Drawing.Size(61, 20)
            Me.radioButton2.TabIndex = 4
            Me.radioButton2.TabStop = True
            Me.radioButton2.Text = "TWN"
            Me.radioButton2.UseVisualStyleBackColor = False
            '
            'radioButton3
            '
            Me.radioButton3.AutoSize = True
            Me.radioButton3.BackColor = System.Drawing.Color.Transparent
            Me.radioButton3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radioButton3.ForeColor = System.Drawing.Color.Aquamarine
            Me.radioButton3.Location = New System.Drawing.Point(128, 121)
            Me.radioButton3.Name = "radioButton3"
            Me.radioButton3.Size = New System.Drawing.Size(63, 20)
            Me.radioButton3.TabIndex = 5
            Me.radioButton3.TabStop = True
            Me.radioButton3.Text = "BGMI"
            Me.radioButton3.UseVisualStyleBackColor = False
            '
            'radioButton4
            '
            Me.radioButton4.AutoSize = True
            Me.radioButton4.BackColor = System.Drawing.Color.Transparent
            Me.radioButton4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radioButton4.ForeColor = System.Drawing.Color.Aquamarine
            Me.radioButton4.Location = New System.Drawing.Point(190, 121)
            Me.radioButton4.Name = "radioButton4"
            Me.radioButton4.Size = New System.Drawing.Size(58, 20)
            Me.radioButton4.TabIndex = 6
            Me.radioButton4.TabStop = True
            Me.radioButton4.Text = "VNG"
            Me.radioButton4.UseVisualStyleBackColor = False
            '
            'status
            '
            Me.status.AutoSize = True
            Me.status.BackColor = System.Drawing.Color.Transparent
            Me.status.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
            Me.status.Location = New System.Drawing.Point(65, 90)
            Me.status.Name = "status"
            Me.status.Size = New System.Drawing.Size(167, 18)
            Me.status.TabIndex = 7
            Me.status.Text = "Select Game Version"
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.BackColor = System.Drawing.Color.Transparent
            Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.label1.ForeColor = System.Drawing.Color.Aquamarine
            Me.label1.Location = New System.Drawing.Point(282, 0)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(26, 25)
            Me.label1.TabIndex = 8
            Me.label1.Text = "X"
            '
            'radioButton5
            '
            Me.radioButton5.AutoSize = True
            Me.radioButton5.BackColor = System.Drawing.Color.Transparent
            Me.radioButton5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radioButton5.ForeColor = System.Drawing.Color.Aquamarine
            Me.radioButton5.Location = New System.Drawing.Point(245, 121)
            Me.radioButton5.Name = "radioButton5"
            Me.radioButton5.Size = New System.Drawing.Size(46, 20)
            Me.radioButton5.TabIndex = 11
            Me.radioButton5.TabStop = True
            Me.radioButton5.Text = "KR"
            Me.radioButton5.UseVisualStyleBackColor = False
            '
            'button4
            '
            Me.button4.BackColor = System.Drawing.Color.Black
            Me.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green
            Me.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.button4.ForeColor = System.Drawing.Color.Aqua
            Me.button4.Location = New System.Drawing.Point(60, 163)
            Me.button4.Name = "button4"
            Me.button4.Size = New System.Drawing.Size(179, 25)
            Me.button4.TabIndex = 12
            Me.button4.Text = "Guest Reset"
            Me.button4.UseVisualStyleBackColor = False
            '
            'statusv
            '
            Me.statusv.AutoSize = True
            Me.statusv.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
            Me.statusv.ForeColor = System.Drawing.Color.Lime
            Me.statusv.Location = New System.Drawing.Point(12, 316)
            Me.statusv.Name = "statusv"
            Me.statusv.Size = New System.Drawing.Size(0, 20)
            Me.statusv.TabIndex = 13
            '
            'label2
            '
            Me.label2.AutoSize = True
            Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
            Me.label2.Location = New System.Drawing.Point(68, 35)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(164, 24)
            Me.label2.TabIndex = 14
            Me.label2.Text = "BRAVE BYPASS"
            '
            'bypass1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.Black
            Me.ClientSize = New System.Drawing.Size(309, 345)
            Me.Controls.Add(Me.label2)
            Me.Controls.Add(Me.statusv)
            Me.Controls.Add(Me.button4)
            Me.Controls.Add(Me.radioButton5)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.status)
            Me.Controls.Add(Me.radioButton4)
            Me.Controls.Add(Me.radioButton3)
            Me.Controls.Add(Me.radioButton2)
            Me.Controls.Add(Me.radioButton1)
            Me.Controls.Add(Me.button3)
            Me.Controls.Add(Me.button2)
            Me.Controls.Add(Me.button1)
            Me.ForeColor = System.Drawing.Color.White
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "bypass1"
            Me.Text = "bypass1"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private button1 As Windows.Forms.Button
        Private button2 As Windows.Forms.Button
        Private button3 As Windows.Forms.Button
        Private radioButton1 As Windows.Forms.RadioButton
        Private radioButton2 As Windows.Forms.RadioButton
        Private radioButton3 As Windows.Forms.RadioButton
        Private radioButton4 As Windows.Forms.RadioButton
        Private status As Windows.Forms.Label
        Private label1 As Windows.Forms.Label
        Private radioButton5 As Windows.Forms.RadioButton
        Private button4 As Windows.Forms.Button
        Private statusv As Windows.Forms.Label
        Private label2 As Windows.Forms.Label
    End Class
End Namespace
