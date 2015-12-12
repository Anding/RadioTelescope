'SSRT Robot.
'This software is released under the GNU GPL


Imports System.io
Imports ZedGraph

Public Class SSRT_Robot
    Inherits System.Windows.Forms.Form

    Const BuildDate As Date = #4/2/2011#     'M/D/Y
    Const SecondsPerDay As Double = 86400
    Const StartDate As DateTime = #12/30/1899#
    'The ZedGraph charting library uses a double 
    'for date times, representing seconds after 1899/12/30 00:00
    'StartDate constant is used in conversions between the VisualBasic
    'DateTime type and the ZedGraph date time type

    Private workingFolder As String
    'Folder for saving the generated graphs

    Private graphDate, UTDate As DateTime
    Private graphDay, graphDayL As String
    Private graphHour As Integer
    Private UToffsetDbl As Double
    Private UToffsetStr As String
    Private xMin As Double
    'Variables for specifying the time range to be charted

    Private uploadFTPClient, downloadFTPclient As Utilities.FTP.FTPclient

    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ftbOutput As FileTextBox.FileTextBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tbStationName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents tbSSRTusername As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbSSRTpassword As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents tbStationIndex As System.Windows.Forms.TextBox
    Friend WithEvents cbActivateFTP As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tbName1 As System.Windows.Forms.TextBox
    Friend WithEvents cb1 As System.Windows.Forms.CheckBox
    Friend WithEvents tbName6 As System.Windows.Forms.TextBox
    Friend WithEvents cb6 As System.Windows.Forms.CheckBox
    Friend WithEvents tbName5 As System.Windows.Forms.TextBox
    Friend WithEvents cb5 As System.Windows.Forms.CheckBox
    Friend WithEvents tbName4 As System.Windows.Forms.TextBox
    Friend WithEvents cb4 As System.Windows.Forms.CheckBox
    Friend WithEvents tbName3 As System.Windows.Forms.TextBox
    Friend WithEvents cb3 As System.Windows.Forms.CheckBox
    Friend WithEvents tbName2 As System.Windows.Forms.TextBox
    Friend WithEvents cb2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Extras As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnBackProcess As System.Windows.Forms.Button
    Friend WithEvents cbActivateGOES As System.Windows.Forms.CheckBox
    Friend WithEvents GOESDay As System.Windows.Forms.TabPage
    Friend WithEvents GOESHour As System.Windows.Forms.TabPage
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents zedSIDDay As ZedGraph.ZedGraphControl
    Friend WithEvents zedSIDHour As ZedGraph.ZedGraphControl
    Friend WithEvents zedGOESDay As ZedGraph.ZedGraphControl
    Friend WithEvents zedGOESHour As ZedGraph.ZedGraphControl
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents tbFloor As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents btn1 As System.Windows.Forms.Button
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents btnBottomColor As System.Windows.Forms.Button
    Friend WithEvents btnTopColor As System.Windows.Forms.Button
    Friend WithEvents lblBottomColor As System.Windows.Forms.Label
    Friend WithEvents lblTopColor As System.Windows.Forms.Label
    Friend WithEvents Btn6 As System.Windows.Forms.Button
    Friend WithEvents Btn5 As System.Windows.Forms.Button
    Friend WithEvents Btn4 As System.Windows.Forms.Button
    Friend WithEvents btn3 As System.Windows.Forms.Button
    Friend WithEvents btn2 As System.Windows.Forms.Button
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents tbOffset1 As System.Windows.Forms.TextBox
    Friend WithEvents tbOffset6 As System.Windows.Forms.TextBox
    Friend WithEvents tbOffset5 As System.Windows.Forms.TextBox
    Friend WithEvents tbOffset4 As System.Windows.Forms.TextBox
    Friend WithEvents tbOffset3 As System.Windows.Forms.TextBox
    Friend WithEvents tbOffset2 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbTitle As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents btnLogUpload As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ftbLog As FileTextBox.FileTextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tbStationKey As System.Windows.Forms.TextBox
    Friend WithEvents ofdLog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Log As System.Windows.Forms.TabPage
    Friend WithEvents tbOutput As System.Windows.Forms.TextBox
    Friend WithEvents tbCeiling As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ftbInput As FileTextBox.FileTextBox


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Start the graph charting
        Application.EnableVisualStyles()

        initialize()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents tabBucket As System.Windows.Forms.TabControl
    Friend WithEvents Settings As System.Windows.Forms.TabPage
    'Friend WithEvents ftbInput As FileTextBox.FileTextBox
    Friend WithEvents btnInput As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnOutput As System.Windows.Forms.Button
    'Friend WithEvents ftbOutput As FileTextBox.FileTextBox
    Friend WithEvents fbdInput As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fbdOutput As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents SIDHour As System.Windows.Forms.TabPage
    Friend WithEvents SIDDay As System.Windows.Forms.TabPage
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SSRT_Robot))
        Me.tabBucket = New System.Windows.Forms.TabControl()
        Me.SIDDay = New System.Windows.Forms.TabPage()
        Me.zedSIDDay = New ZedGraph.ZedGraphControl()
        Me.SIDHour = New System.Windows.Forms.TabPage()
        Me.zedSIDHour = New ZedGraph.ZedGraphControl()
        Me.GOESDay = New System.Windows.Forms.TabPage()
        Me.zedGOESDay = New ZedGraph.ZedGraphControl()
        Me.GOESHour = New System.Windows.Forms.TabPage()
        Me.zedGOESHour = New ZedGraph.ZedGraphControl()
        Me.Settings = New System.Windows.Forms.TabPage()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbTitle = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.tbFloor = New System.Windows.Forms.TextBox()
        Me.lblTopColor = New System.Windows.Forms.Label()
        Me.lblBottomColor = New System.Windows.Forms.Label()
        Me.btnTopColor = New System.Windows.Forms.Button()
        Me.btnBottomColor = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.cbActivateGOES = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tbStationKey = New System.Windows.Forms.TextBox()
        Me.cbActivateFTP = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbStationIndex = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbStationName = New System.Windows.Forms.TextBox()
        Me.tbSSRTpassword = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbSSRTusername = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.tbOffset6 = New System.Windows.Forms.TextBox()
        Me.tbOffset5 = New System.Windows.Forms.TextBox()
        Me.tbOffset4 = New System.Windows.Forms.TextBox()
        Me.tbOffset3 = New System.Windows.Forms.TextBox()
        Me.tbOffset2 = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.tbOffset1 = New System.Windows.Forms.TextBox()
        Me.Btn6 = New System.Windows.Forms.Button()
        Me.Btn5 = New System.Windows.Forms.Button()
        Me.Btn4 = New System.Windows.Forms.Button()
        Me.btn3 = New System.Windows.Forms.Button()
        Me.btn2 = New System.Windows.Forms.Button()
        Me.btn1 = New System.Windows.Forms.Button()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.tbName6 = New System.Windows.Forms.TextBox()
        Me.cb6 = New System.Windows.Forms.CheckBox()
        Me.tbName5 = New System.Windows.Forms.TextBox()
        Me.cb5 = New System.Windows.Forms.CheckBox()
        Me.tbName4 = New System.Windows.Forms.TextBox()
        Me.cb4 = New System.Windows.Forms.CheckBox()
        Me.tbName3 = New System.Windows.Forms.TextBox()
        Me.cb3 = New System.Windows.Forms.CheckBox()
        Me.tbName2 = New System.Windows.Forms.TextBox()
        Me.cb2 = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tbName1 = New System.Windows.Forms.TextBox()
        Me.cb1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ftbOutput = New FileTextBox.FileTextBox()
        Me.ftbInput = New FileTextBox.FileTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnInput = New System.Windows.Forms.Button()
        Me.btnOutput = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Extras = New System.Windows.Forms.TabPage()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.btnLogUpload = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ftbLog = New FileTextBox.FileTextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.btnBackProcess = New System.Windows.Forms.Button()
        Me.Log = New System.Windows.Forms.TabPage()
        Me.tbOutput = New System.Windows.Forms.TextBox()
        Me.fbdInput = New System.Windows.Forms.FolderBrowserDialog()
        Me.fbdOutput = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnStart = New System.Windows.Forms.Button()
        Me.ofdLog = New System.Windows.Forms.OpenFileDialog()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.tbCeiling = New System.Windows.Forms.TextBox()
        Me.tabBucket.SuspendLayout()
        Me.SIDDay.SuspendLayout()
        Me.SIDHour.SuspendLayout()
        Me.GOESDay.SuspendLayout()
        Me.GOESHour.SuspendLayout()
        Me.Settings.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Extras.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.Log.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabBucket
        '
        Me.tabBucket.Controls.Add(Me.SIDDay)
        Me.tabBucket.Controls.Add(Me.SIDHour)
        Me.tabBucket.Controls.Add(Me.GOESDay)
        Me.tabBucket.Controls.Add(Me.GOESHour)
        Me.tabBucket.Controls.Add(Me.Settings)
        Me.tabBucket.Controls.Add(Me.Extras)
        Me.tabBucket.Controls.Add(Me.Log)
        Me.tabBucket.Location = New System.Drawing.Point(8, 35)
        Me.tabBucket.Name = "tabBucket"
        Me.tabBucket.SelectedIndex = 0
        Me.tabBucket.Size = New System.Drawing.Size(744, 544)
        Me.tabBucket.TabIndex = 0
        '
        'SIDDay
        '
        Me.SIDDay.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SIDDay.Controls.Add(Me.zedSIDDay)
        Me.SIDDay.Location = New System.Drawing.Point(4, 22)
        Me.SIDDay.Name = "SIDDay"
        Me.SIDDay.Size = New System.Drawing.Size(736, 518)
        Me.SIDDay.TabIndex = 3
        Me.SIDDay.Text = "SID Day"
        Me.SIDDay.UseVisualStyleBackColor = True
        '
        'zedSIDDay
        '
        Me.zedSIDDay.Location = New System.Drawing.Point(4, 3)
        Me.zedSIDDay.Name = "zedSIDDay"
        Me.zedSIDDay.ScrollGrace = 0.0R
        Me.zedSIDDay.ScrollMaxX = 0.0R
        Me.zedSIDDay.ScrollMaxY = 0.0R
        Me.zedSIDDay.ScrollMaxY2 = 0.0R
        Me.zedSIDDay.ScrollMinX = 0.0R
        Me.zedSIDDay.ScrollMinY = 0.0R
        Me.zedSIDDay.ScrollMinY2 = 0.0R
        Me.zedSIDDay.Size = New System.Drawing.Size(700, 500)
        Me.zedSIDDay.TabIndex = 1
        '
        'SIDHour
        '
        Me.SIDHour.BackColor = System.Drawing.Color.Transparent
        Me.SIDHour.Controls.Add(Me.zedSIDHour)
        Me.SIDHour.Location = New System.Drawing.Point(4, 22)
        Me.SIDHour.Name = "SIDHour"
        Me.SIDHour.Size = New System.Drawing.Size(736, 518)
        Me.SIDHour.TabIndex = 1
        Me.SIDHour.Text = "SID Hour"
        Me.SIDHour.UseVisualStyleBackColor = True
        '
        'zedSIDHour
        '
        Me.zedSIDHour.Location = New System.Drawing.Point(4, 3)
        Me.zedSIDHour.Name = "zedSIDHour"
        Me.zedSIDHour.ScrollGrace = 0.0R
        Me.zedSIDHour.ScrollMaxX = 0.0R
        Me.zedSIDHour.ScrollMaxY = 0.0R
        Me.zedSIDHour.ScrollMaxY2 = 0.0R
        Me.zedSIDHour.ScrollMinX = 0.0R
        Me.zedSIDHour.ScrollMinY = 0.0R
        Me.zedSIDHour.ScrollMinY2 = 0.0R
        Me.zedSIDHour.Size = New System.Drawing.Size(700, 500)
        Me.zedSIDHour.TabIndex = 1
        '
        'GOESDay
        '
        Me.GOESDay.Controls.Add(Me.zedGOESDay)
        Me.GOESDay.Location = New System.Drawing.Point(4, 22)
        Me.GOESDay.Name = "GOESDay"
        Me.GOESDay.Size = New System.Drawing.Size(736, 518)
        Me.GOESDay.TabIndex = 5
        Me.GOESDay.Text = "GOES Day"
        Me.GOESDay.UseVisualStyleBackColor = True
        '
        'zedGOESDay
        '
        Me.zedGOESDay.Location = New System.Drawing.Point(4, 3)
        Me.zedGOESDay.Name = "zedGOESDay"
        Me.zedGOESDay.ScrollGrace = 0.0R
        Me.zedGOESDay.ScrollMaxX = 0.0R
        Me.zedGOESDay.ScrollMaxY = 0.0R
        Me.zedGOESDay.ScrollMaxY2 = 0.0R
        Me.zedGOESDay.ScrollMinX = 0.0R
        Me.zedGOESDay.ScrollMinY = 0.0R
        Me.zedGOESDay.ScrollMinY2 = 0.0R
        Me.zedGOESDay.Size = New System.Drawing.Size(700, 500)
        Me.zedGOESDay.TabIndex = 1
        '
        'GOESHour
        '
        Me.GOESHour.Controls.Add(Me.zedGOESHour)
        Me.GOESHour.Location = New System.Drawing.Point(4, 22)
        Me.GOESHour.Name = "GOESHour"
        Me.GOESHour.Size = New System.Drawing.Size(736, 518)
        Me.GOESHour.TabIndex = 6
        Me.GOESHour.Text = "GOES Hour"
        Me.GOESHour.UseVisualStyleBackColor = True
        '
        'zedGOESHour
        '
        Me.zedGOESHour.Location = New System.Drawing.Point(4, 3)
        Me.zedGOESHour.Name = "zedGOESHour"
        Me.zedGOESHour.ScrollGrace = 0.0R
        Me.zedGOESHour.ScrollMaxX = 0.0R
        Me.zedGOESHour.ScrollMaxY = 0.0R
        Me.zedGOESHour.ScrollMaxY2 = 0.0R
        Me.zedGOESHour.ScrollMinX = 0.0R
        Me.zedGOESHour.ScrollMinY = 0.0R
        Me.zedGOESHour.ScrollMinY2 = 0.0R
        Me.zedGOESHour.Size = New System.Drawing.Size(700, 500)
        Me.zedGOESHour.TabIndex = 2
        '
        'Settings
        '
        Me.Settings.BackColor = System.Drawing.Color.Transparent
        Me.Settings.Controls.Add(Me.GroupBox6)
        Me.Settings.Controls.Add(Me.GroupBox4)
        Me.Settings.Controls.Add(Me.GroupBox3)
        Me.Settings.Controls.Add(Me.GroupBox1)
        Me.Settings.Controls.Add(Me.GroupBox2)
        Me.Settings.Location = New System.Drawing.Point(4, 22)
        Me.Settings.Name = "Settings"
        Me.Settings.Size = New System.Drawing.Size(736, 518)
        Me.Settings.TabIndex = 0
        Me.Settings.Text = "Settings"
        Me.Settings.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.tbCeiling)
        Me.GroupBox6.Controls.Add(Me.Label14)
        Me.GroupBox6.Controls.Add(Me.Label4)
        Me.GroupBox6.Controls.Add(Me.tbTitle)
        Me.GroupBox6.Controls.Add(Me.Label26)
        Me.GroupBox6.Controls.Add(Me.Label25)
        Me.GroupBox6.Controls.Add(Me.Label21)
        Me.GroupBox6.Controls.Add(Me.tbFloor)
        Me.GroupBox6.Controls.Add(Me.lblTopColor)
        Me.GroupBox6.Controls.Add(Me.lblBottomColor)
        Me.GroupBox6.Controls.Add(Me.btnTopColor)
        Me.GroupBox6.Controls.Add(Me.btnBottomColor)
        Me.GroupBox6.Location = New System.Drawing.Point(388, 103)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(331, 219)
        Me.GroupBox6.TabIndex = 28
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Chart settings"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 24)
        Me.Label4.TabIndex = 46
        Me.Label4.Text = "Station title"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTitle
        '
        Me.tbTitle.Location = New System.Drawing.Point(124, 17)
        Me.tbTitle.Name = "tbTitle"
        Me.tbTitle.Size = New System.Drawing.Size(189, 20)
        Me.tbTitle.TabIndex = 45
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(6, 137)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(100, 24)
        Me.Label26.TabIndex = 44
        Me.Label26.Text = "Chart color bottom"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(6, 107)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(100, 24)
        Me.Label25.TabIndex = 43
        Me.Label25.Text = "Chart color top"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(6, 40)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(100, 24)
        Me.Label21.TabIndex = 32
        Me.Label21.Text = "Signal floor"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbFloor
        '
        Me.tbFloor.Location = New System.Drawing.Point(124, 43)
        Me.tbFloor.Name = "tbFloor"
        Me.tbFloor.Size = New System.Drawing.Size(189, 20)
        Me.tbFloor.TabIndex = 33
        '
        'lblTopColor
        '
        Me.lblTopColor.BackColor = System.Drawing.Color.DarkBlue
        Me.lblTopColor.ForeColor = System.Drawing.Color.White
        Me.lblTopColor.Location = New System.Drawing.Point(124, 105)
        Me.lblTopColor.Name = "lblTopColor"
        Me.lblTopColor.Size = New System.Drawing.Size(155, 24)
        Me.lblTopColor.TabIndex = 39
        Me.lblTopColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBottomColor
        '
        Me.lblBottomColor.BackColor = System.Drawing.Color.DarkBlue
        Me.lblBottomColor.ForeColor = System.Drawing.Color.White
        Me.lblBottomColor.Location = New System.Drawing.Point(124, 135)
        Me.lblBottomColor.Name = "lblBottomColor"
        Me.lblBottomColor.Size = New System.Drawing.Size(155, 24)
        Me.lblBottomColor.TabIndex = 40
        Me.lblBottomColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnTopColor
        '
        Me.btnTopColor.Location = New System.Drawing.Point(293, 109)
        Me.btnTopColor.Name = "btnTopColor"
        Me.btnTopColor.Size = New System.Drawing.Size(20, 20)
        Me.btnTopColor.TabIndex = 41
        Me.btnTopColor.Text = "<"
        '
        'btnBottomColor
        '
        Me.btnBottomColor.Location = New System.Drawing.Point(293, 139)
        Me.btnBottomColor.Name = "btnBottomColor"
        Me.btnBottomColor.Size = New System.Drawing.Size(20, 20)
        Me.btnBottomColor.TabIndex = 42
        Me.btnBottomColor.Text = "<"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.cbActivateGOES)
        Me.GroupBox4.Location = New System.Drawing.Point(388, 328)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(331, 177)
        Me.GroupBox4.TabIndex = 27
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "NOAA GOES settings"
        '
        'cbActivateGOES
        '
        Me.cbActivateGOES.AutoSize = True
        Me.cbActivateGOES.Location = New System.Drawing.Point(6, 19)
        Me.cbActivateGOES.Name = "cbActivateGOES"
        Me.cbActivateGOES.Size = New System.Drawing.Size(164, 17)
        Me.cbActivateGOES.TabIndex = 32
        Me.cbActivateGOES.Text = "Activate GOES x-ray charting"
        Me.cbActivateGOES.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.tbStationKey)
        Me.GroupBox3.Controls.Add(Me.cbActivateFTP)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.tbStationIndex)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.tbStationName)
        Me.GroupBox3.Controls.Add(Me.tbSSRTpassword)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.tbSSRTusername)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 328)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(363, 177)
        Me.GroupBox3.TabIndex = 26
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "SSRT network settings"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(8, 91)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(159, 24)
        Me.Label12.TabIndex = 12
        Me.Label12.Text = "SSRT station key"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbStationKey
        '
        Me.tbStationKey.Location = New System.Drawing.Point(173, 94)
        Me.tbStationKey.Name = "tbStationKey"
        Me.tbStationKey.Size = New System.Drawing.Size(174, 20)
        Me.tbStationKey.TabIndex = 13
        '
        'cbActivateFTP
        '
        Me.cbActivateFTP.AutoSize = True
        Me.cbActivateFTP.Location = New System.Drawing.Point(11, 19)
        Me.cbActivateFTP.Name = "cbActivateFTP"
        Me.cbActivateFTP.Size = New System.Drawing.Size(172, 17)
        Me.cbActivateFTP.TabIndex = 11
        Me.cbActivateFTP.Text = "Activate SSRT online reporting"
        Me.cbActivateFTP.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 65)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(159, 24)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "SSRT station index"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbStationIndex
        '
        Me.tbStationIndex.Location = New System.Drawing.Point(173, 68)
        Me.tbStationIndex.Name = "tbStationIndex"
        Me.tbStationIndex.Size = New System.Drawing.Size(174, 20)
        Me.tbStationIndex.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(159, 24)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "SSRT station name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbStationName
        '
        Me.tbStationName.Location = New System.Drawing.Point(173, 42)
        Me.tbStationName.Name = "tbStationName"
        Me.tbStationName.Size = New System.Drawing.Size(174, 20)
        Me.tbStationName.TabIndex = 4
        '
        'tbSSRTpassword
        '
        Me.tbSSRTpassword.Location = New System.Drawing.Point(173, 146)
        Me.tbSSRTpassword.Name = "tbSSRTpassword"
        Me.tbSSRTpassword.Size = New System.Drawing.Size(174, 20)
        Me.tbSSRTpassword.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 143)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(159, 24)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "SSRT network password"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSSRTusername
        '
        Me.tbSSRTusername.Location = New System.Drawing.Point(173, 120)
        Me.tbSSRTusername.Name = "tbSSRTusername"
        Me.tbSSRTusername.Size = New System.Drawing.Size(174, 20)
        Me.tbSSRTusername.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(159, 24)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "SSRT network username"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tbOffset6)
        Me.GroupBox1.Controls.Add(Me.tbOffset5)
        Me.GroupBox1.Controls.Add(Me.tbOffset4)
        Me.GroupBox1.Controls.Add(Me.tbOffset3)
        Me.GroupBox1.Controls.Add(Me.tbOffset2)
        Me.GroupBox1.Controls.Add(Me.Label24)
        Me.GroupBox1.Controls.Add(Me.tbOffset1)
        Me.GroupBox1.Controls.Add(Me.Btn6)
        Me.GroupBox1.Controls.Add(Me.Btn5)
        Me.GroupBox1.Controls.Add(Me.Btn4)
        Me.GroupBox1.Controls.Add(Me.btn3)
        Me.GroupBox1.Controls.Add(Me.btn2)
        Me.GroupBox1.Controls.Add(Me.btn1)
        Me.GroupBox1.Controls.Add(Me.Label22)
        Me.GroupBox1.Controls.Add(Me.tbName6)
        Me.GroupBox1.Controls.Add(Me.cb6)
        Me.GroupBox1.Controls.Add(Me.tbName5)
        Me.GroupBox1.Controls.Add(Me.cb5)
        Me.GroupBox1.Controls.Add(Me.tbName4)
        Me.GroupBox1.Controls.Add(Me.cb4)
        Me.GroupBox1.Controls.Add(Me.tbName3)
        Me.GroupBox1.Controls.Add(Me.cb3)
        Me.GroupBox1.Controls.Add(Me.tbName2)
        Me.GroupBox1.Controls.Add(Me.cb2)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.tbName1)
        Me.GroupBox1.Controls.Add(Me.cb1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 103)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(363, 219)
        Me.GroupBox1.TabIndex = 25
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Signal settings"
        '
        'tbOffset6
        '
        Me.tbOffset6.Location = New System.Drawing.Point(215, 173)
        Me.tbOffset6.Name = "tbOffset6"
        Me.tbOffset6.Size = New System.Drawing.Size(60, 20)
        Me.tbOffset6.TabIndex = 54
        '
        'tbOffset5
        '
        Me.tbOffset5.Location = New System.Drawing.Point(215, 148)
        Me.tbOffset5.Name = "tbOffset5"
        Me.tbOffset5.Size = New System.Drawing.Size(60, 20)
        Me.tbOffset5.TabIndex = 53
        '
        'tbOffset4
        '
        Me.tbOffset4.Location = New System.Drawing.Point(215, 121)
        Me.tbOffset4.Name = "tbOffset4"
        Me.tbOffset4.Size = New System.Drawing.Size(60, 20)
        Me.tbOffset4.TabIndex = 52
        '
        'tbOffset3
        '
        Me.tbOffset3.Location = New System.Drawing.Point(215, 96)
        Me.tbOffset3.Name = "tbOffset3"
        Me.tbOffset3.Size = New System.Drawing.Size(60, 20)
        Me.tbOffset3.TabIndex = 51
        '
        'tbOffset2
        '
        Me.tbOffset2.Location = New System.Drawing.Point(215, 69)
        Me.tbOffset2.Name = "tbOffset2"
        Me.tbOffset2.Size = New System.Drawing.Size(60, 20)
        Me.tbOffset2.TabIndex = 50
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(215, 16)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(57, 24)
        Me.Label24.TabIndex = 49
        Me.Label24.Text = "Offset"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbOffset1
        '
        Me.tbOffset1.Location = New System.Drawing.Point(215, 43)
        Me.tbOffset1.Name = "tbOffset1"
        Me.tbOffset1.Size = New System.Drawing.Size(60, 20)
        Me.tbOffset1.TabIndex = 48
        '
        'Btn6
        '
        Me.Btn6.Location = New System.Drawing.Point(289, 174)
        Me.Btn6.Name = "Btn6"
        Me.Btn6.Size = New System.Drawing.Size(20, 20)
        Me.Btn6.TabIndex = 47
        Me.Btn6.Text = "<"
        '
        'Btn5
        '
        Me.Btn5.Location = New System.Drawing.Point(289, 148)
        Me.Btn5.Name = "Btn5"
        Me.Btn5.Size = New System.Drawing.Size(20, 20)
        Me.Btn5.TabIndex = 46
        Me.Btn5.Text = "<"
        '
        'Btn4
        '
        Me.Btn4.Location = New System.Drawing.Point(289, 121)
        Me.Btn4.Name = "Btn4"
        Me.Btn4.Size = New System.Drawing.Size(20, 20)
        Me.Btn4.TabIndex = 45
        Me.Btn4.Text = "<"
        '
        'btn3
        '
        Me.btn3.Location = New System.Drawing.Point(289, 96)
        Me.btn3.Name = "btn3"
        Me.btn3.Size = New System.Drawing.Size(20, 20)
        Me.btn3.TabIndex = 44
        Me.btn3.Text = "<"
        '
        'btn2
        '
        Me.btn2.Location = New System.Drawing.Point(289, 70)
        Me.btn2.Name = "btn2"
        Me.btn2.Size = New System.Drawing.Size(20, 20)
        Me.btn2.TabIndex = 43
        Me.btn2.Text = "<"
        '
        'btn1
        '
        Me.btn1.Location = New System.Drawing.Point(289, 43)
        Me.btn1.Name = "btn1"
        Me.btn1.Size = New System.Drawing.Size(20, 20)
        Me.btn1.TabIndex = 35
        Me.btn1.Text = "<"
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(287, 16)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(65, 24)
        Me.Label22.TabIndex = 34
        Me.Label22.Text = "Pen color"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbName6
        '
        Me.tbName6.Location = New System.Drawing.Point(67, 174)
        Me.tbName6.Name = "tbName6"
        Me.tbName6.Size = New System.Drawing.Size(137, 20)
        Me.tbName6.TabIndex = 17
        '
        'cb6
        '
        Me.cb6.AutoSize = True
        Me.cb6.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cb6.Location = New System.Drawing.Point(11, 176)
        Me.cb6.Name = "cb6"
        Me.cb6.Size = New System.Drawing.Size(32, 17)
        Me.cb6.TabIndex = 16
        Me.cb6.Text = "6"
        Me.cb6.UseVisualStyleBackColor = True
        '
        'tbName5
        '
        Me.tbName5.Location = New System.Drawing.Point(67, 148)
        Me.tbName5.Name = "tbName5"
        Me.tbName5.Size = New System.Drawing.Size(137, 20)
        Me.tbName5.TabIndex = 15
        '
        'cb5
        '
        Me.cb5.AutoSize = True
        Me.cb5.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cb5.Location = New System.Drawing.Point(11, 150)
        Me.cb5.Name = "cb5"
        Me.cb5.Size = New System.Drawing.Size(32, 17)
        Me.cb5.TabIndex = 14
        Me.cb5.Text = "5"
        Me.cb5.UseVisualStyleBackColor = True
        '
        'tbName4
        '
        Me.tbName4.Location = New System.Drawing.Point(67, 122)
        Me.tbName4.Name = "tbName4"
        Me.tbName4.Size = New System.Drawing.Size(137, 20)
        Me.tbName4.TabIndex = 13
        '
        'cb4
        '
        Me.cb4.AutoSize = True
        Me.cb4.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cb4.Location = New System.Drawing.Point(11, 123)
        Me.cb4.Name = "cb4"
        Me.cb4.Size = New System.Drawing.Size(32, 17)
        Me.cb4.TabIndex = 12
        Me.cb4.Text = "4"
        Me.cb4.UseVisualStyleBackColor = True
        '
        'tbName3
        '
        Me.tbName3.Location = New System.Drawing.Point(67, 96)
        Me.tbName3.Name = "tbName3"
        Me.tbName3.Size = New System.Drawing.Size(137, 20)
        Me.tbName3.TabIndex = 11
        '
        'cb3
        '
        Me.cb3.AutoSize = True
        Me.cb3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cb3.Location = New System.Drawing.Point(11, 98)
        Me.cb3.Name = "cb3"
        Me.cb3.Size = New System.Drawing.Size(32, 17)
        Me.cb3.TabIndex = 10
        Me.cb3.Text = "3"
        Me.cb3.UseVisualStyleBackColor = True
        '
        'tbName2
        '
        Me.tbName2.Location = New System.Drawing.Point(67, 70)
        Me.tbName2.Name = "tbName2"
        Me.tbName2.Size = New System.Drawing.Size(137, 20)
        Me.tbName2.TabIndex = 9
        '
        'cb2
        '
        Me.cb2.AutoSize = True
        Me.cb2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cb2.Location = New System.Drawing.Point(11, 72)
        Me.cb2.Name = "cb2"
        Me.cb2.Size = New System.Drawing.Size(32, 17)
        Me.cb2.TabIndex = 8
        Me.cb2.Text = "2"
        Me.cb2.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(67, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(137, 24)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Label"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 24)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Column"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbName1
        '
        Me.tbName1.Location = New System.Drawing.Point(67, 43)
        Me.tbName1.Name = "tbName1"
        Me.tbName1.Size = New System.Drawing.Size(137, 20)
        Me.tbName1.TabIndex = 5
        '
        'cb1
        '
        Me.cb1.AutoSize = True
        Me.cb1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cb1.Checked = True
        Me.cb1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cb1.Location = New System.Drawing.Point(11, 45)
        Me.cb1.Name = "cb1"
        Me.cb1.Size = New System.Drawing.Size(32, 17)
        Me.cb1.TabIndex = 0
        Me.cb1.Text = "1"
        Me.cb1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ftbOutput)
        Me.GroupBox2.Controls.Add(Me.ftbInput)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.btnInput)
        Me.GroupBox2.Controls.Add(Me.btnOutput)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(711, 85)
        Me.GroupBox2.TabIndex = 24
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Local disk settings"
        '
        'ftbOutput
        '
        Me.ftbOutput.FileText = ""
        Me.ftbOutput.Format = FileTextBox.FileTextBox.FileTextFormat.FullPath
        Me.ftbOutput.Location = New System.Drawing.Point(173, 47)
        Me.ftbOutput.Name = "ftbOutput"
        Me.ftbOutput.ReadOnly = True
        Me.ftbOutput.Size = New System.Drawing.Size(486, 20)
        Me.ftbOutput.TabIndex = 11
        '
        'ftbInput
        '
        Me.ftbInput.FileText = ""
        Me.ftbInput.Format = FileTextBox.FileTextBox.FileTextFormat.FullPath
        Me.ftbInput.Location = New System.Drawing.Point(173, 20)
        Me.ftbInput.Name = "ftbInput"
        Me.ftbInput.ReadOnly = True
        Me.ftbInput.Size = New System.Drawing.Size(486, 20)
        Me.ftbInput.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(159, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Spectrum Lab log folder"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnInput
        '
        Me.btnInput.Location = New System.Drawing.Point(673, 19)
        Me.btnInput.Name = "btnInput"
        Me.btnInput.Size = New System.Drawing.Size(20, 22)
        Me.btnInput.TabIndex = 1
        Me.btnInput.Text = "<"
        '
        'btnOutput
        '
        Me.btnOutput.Location = New System.Drawing.Point(673, 47)
        Me.btnOutput.Name = "btnOutput"
        Me.btnOutput.Size = New System.Drawing.Size(20, 20)
        Me.btnOutput.TabIndex = 8
        Me.btnOutput.Text = "<"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(159, 24)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Chart output folder"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Extras
        '
        Me.Extras.Controls.Add(Me.GroupBox7)
        Me.Extras.Controls.Add(Me.GroupBox5)
        Me.Extras.Location = New System.Drawing.Point(4, 22)
        Me.Extras.Name = "Extras"
        Me.Extras.Size = New System.Drawing.Size(736, 518)
        Me.Extras.TabIndex = 4
        Me.Extras.Text = "Extras"
        Me.Extras.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.btnLogUpload)
        Me.GroupBox7.Controls.Add(Me.Label10)
        Me.GroupBox7.Controls.Add(Me.Button1)
        Me.GroupBox7.Controls.Add(Me.ftbLog)
        Me.GroupBox7.Location = New System.Drawing.Point(12, 114)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(711, 85)
        Me.GroupBox7.TabIndex = 14
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "SSRT network station log file"
        '
        'btnLogUpload
        '
        Me.btnLogUpload.Location = New System.Drawing.Point(590, 27)
        Me.btnLogUpload.Name = "btnLogUpload"
        Me.btnLogUpload.Size = New System.Drawing.Size(115, 24)
        Me.btnLogUpload.TabIndex = 9
        Me.btnLogUpload.Text = "Upload Log File"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(6, 27)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(128, 24)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "Station log text file"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(566, 27)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(18, 22)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "<"
        '
        'ftbLog
        '
        Me.ftbLog.FileText = ""
        Me.ftbLog.Format = FileTextBox.FileTextBox.FileTextFormat.FullPath
        Me.ftbLog.Location = New System.Drawing.Point(140, 30)
        Me.ftbLog.Name = "ftbLog"
        Me.ftbLog.ReadOnly = True
        Me.ftbLog.Size = New System.Drawing.Size(420, 20)
        Me.ftbLog.TabIndex = 13
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.Label11)
        Me.GroupBox5.Controls.Add(Me.dtpTo)
        Me.GroupBox5.Controls.Add(Me.dtpFrom)
        Me.GroupBox5.Controls.Add(Me.btnBackProcess)
        Me.GroupBox5.Location = New System.Drawing.Point(12, 14)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(711, 94)
        Me.GroupBox5.TabIndex = 6
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "SID file back processing"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(137, 25)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(20, 13)
        Me.Label13.TabIndex = 8
        Me.Label13.Text = "To"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 25)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(30, 13)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "From"
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTo.Location = New System.Drawing.Point(140, 46)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(128, 20)
        Me.dtpTo.TabIndex = 6
        Me.dtpTo.Value = New Date(2010, 6, 6, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFrom.Location = New System.Drawing.Point(6, 46)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(128, 20)
        Me.dtpFrom.TabIndex = 0
        Me.dtpFrom.Value = New Date(2010, 6, 6, 0, 0, 0, 0)
        '
        'btnBackProcess
        '
        Me.btnBackProcess.Location = New System.Drawing.Point(274, 44)
        Me.btnBackProcess.Name = "btnBackProcess"
        Me.btnBackProcess.Size = New System.Drawing.Size(115, 24)
        Me.btnBackProcess.TabIndex = 1
        Me.btnBackProcess.Text = "Process SID files"
        '
        'Log
        '
        Me.Log.BackColor = System.Drawing.Color.Transparent
        Me.Log.Controls.Add(Me.tbOutput)
        Me.Log.Location = New System.Drawing.Point(4, 22)
        Me.Log.Name = "Log"
        Me.Log.Size = New System.Drawing.Size(736, 518)
        Me.Log.TabIndex = 2
        Me.Log.Text = "Log"
        Me.Log.UseVisualStyleBackColor = True
        '
        'tbOutput
        '
        Me.tbOutput.Location = New System.Drawing.Point(8, 15)
        Me.tbOutput.MaxLength = 65535
        Me.tbOutput.Multiline = True
        Me.tbOutput.Name = "tbOutput"
        Me.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbOutput.Size = New System.Drawing.Size(720, 497)
        Me.tbOutput.TabIndex = 0
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(170, 6)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 1
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Enabled = False
        Me.btnStop.Location = New System.Drawing.Point(89, 6)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(75, 23)
        Me.btnStop.TabIndex = 2
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(676, 11)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(68, 13)
        Me.Label18.TabIndex = 4
        Me.Label18.Text = "22 Sep 2010"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(440, 11)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(219, 13)
        Me.Label20.TabIndex = 6
        Me.Label20.Text = "Design and code copyright Percival Andrews"
        '
        'Timer1
        '
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(8, 6)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 7
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'ofdLog
        '
        Me.ofdLog.DefaultExt = "txt"
        Me.ofdLog.ReadOnlyChecked = True
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(6, 67)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(100, 24)
        Me.Label14.TabIndex = 47
        Me.Label14.Text = "Signal ceiling"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbCeiling
        '
        Me.tbCeiling.Location = New System.Drawing.Point(124, 69)
        Me.tbCeiling.Name = "tbCeiling"
        Me.tbCeiling.Size = New System.Drawing.Size(189, 20)
        Me.tbCeiling.TabIndex = 48
        '
        'SSRT_Robot
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(756, 584)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.tabBucket)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SSRT_Robot"
        Me.Text = "SSRT Robot 2"
        Me.tabBucket.ResumeLayout(False)
        Me.SIDDay.ResumeLayout(False)
        Me.SIDHour.ResumeLayout(False)
        Me.GOESDay.ResumeLayout(False)
        Me.GOESHour.ResumeLayout(False)
        Me.Settings.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Extras.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.Log.ResumeLayout(False)
        Me.Log.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub initialize()
        loadSettings()
        Label18.Text = BuildDate.ToString("dd MMM yyyy")
        dtpFrom.Value = Now
        dtpTo.Value = Now
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        hourlyCycle()
    End Sub


    Private Sub hourlyCycle()
        Timer1.Enabled = False        'hold the timer
        createGraphs()
        restartTimer()
    End Sub


    Private Sub createGraphs()
        Dim n As DateTime = Now()
        Dim localZone As TimeZone = TimeZone.CurrentTimeZone

        Try
            'Prepare to graph the previous hour's data
            graphDate = n.AddHours(-1).Date
            graphHour = n.AddHours(-1).Hour
            graphDay = graphDate.ToString("yyyy-MM-dd")
            graphDayL = graphDate.ToString("dd MMM yyyy")
            xMin = Int(graphDate.Subtract(StartDate).TotalDays)

            'Calculate UTC offset
            UToffsetDbl = localZone.GetUtcOffset(graphDate).TotalHours
            UToffsetStr = UToffsetDbl.ToString(" + #0.0; - #0.0")

            'Prepare the folder to accept the charts
            createWorkingFolder()

            'Process the SSRT data
            processSIDdata()
            MakeSIDDayGraph()
            MakeSIDHourGraph()

            'ProcessGOESdata()
            If My.Settings.ActivateGOESCharting = True Then
                downloadFTPclient = New Utilities.FTP.FTPclient("ftp.sec.noaa.gov", "ftp", "SSRT@monkeyboo.net")
                UTDate = DateTime.UtcNow
                'GOES data is usually delayed by 2 hours, so look back to the hour before last
                graphDate = n.AddHours(-2).Date
                graphHour = n.AddHours(-2).Hour
                graphDay = graphDate.ToString("yyyy-MM-dd")
                graphDayL = graphDate.ToString("dd MMM yyyy")
                xMin = Int(graphDate.Subtract(StartDate).TotalDays)
                processGOESdata()
                MakeGOESDayGraph()
                MakeGOESHourGraph()
            End If
        Catch ex As Exception
            output("Sub createGraphics()")
            output(ex.Source + " : " + ex.Message)
        End Try
    End Sub

    'Will rerun the graphing just after the next hour
    Private Sub restartTimer()
        Try
            Dim n As DateTime = Now()
            Dim x As Integer = 1
            Dim nextTime As DateTime

            'set at exactly 'n' mins past next hour (to avoid server congestion, offset by individual station code 'x')
            If My.Settings.ActivateSSRTonline Then
                x = Integer.Parse(My.Settings.VLFStationIndex) + 1
            End If

            If n.Hour < 23 Then
                nextTime = New DateTime(n.Year, n.Month, n.Day, n.Hour + 1, x, 0)
            Else
                nextTime = Now.AddHours(1)  'too hard to calculate the exact time as day/month/year rollover, so just add 1 hour
            End If


            output("Waiting for " + nextTime.ToShortTimeString)
            Timer1.Interval = nextTime.Subtract(Now).TotalMilliseconds
            Timer1.Enabled = True
            Application.DoEvents()
        Catch ex As Exception
            output("Sub restartTimer()")
            output(ex.Source + " : " + ex.Message)
        End Try
    End Sub




    Private Sub createWorkingFolder()
        'Creates a folder to hold the charts
        'A new folder is used for each month.  Folder names have the format 2007-05, 2007-06, etc.
        Try
            workingFolder = My.Settings.ChartOutputFolderRoot + "\" + graphDay.Substring(0, 4) + "\" + graphDay.Substring(0, 7) + "\"
            IO.Directory.CreateDirectory(workingFolder)
        Catch ex As Exception
            output("Sub createGraphics()")
            output(ex.Source + " : " + ex.Message)
        End Try
    End Sub


    Private Function processSIDdata() As Boolean

        Dim list1 As New ZedGraph.PointPairList
        Dim list2 As New ZedGraph.PointPairList
        Dim list3 As New ZedGraph.PointPairList
        Dim list4 As New ZedGraph.PointPairList
        Dim list5 As New ZedGraph.PointPairList
        Dim list6 As New ZedGraph.PointPairList
        Dim filenameInput, s As String
        Dim ss(), sss() As String
        Dim infile As IO.FileStream
        Dim instream As IO.StreamReader
        Dim x As Double
        Dim floor, ceiling As Double
        Dim y1, y2, y3, y4, y5, y6 As Double
        Dim o1, o2, o3, o4, o5, o6 As Double
        Dim hour, minute, second As Integer
        Dim newDay As Boolean = False
        Dim d As XDate
        Dim title As String

        Try
            floor = Double.Parse(tbFloor.Text)
            ceiling = Double.Parse(tbCeiling.Text)
            o1 = Double.Parse(My.Settings.Column1Offset)
            o2 = Double.Parse(My.Settings.Column2Offset)
            o3 = Double.Parse(My.Settings.Column3Offset)
            o4 = Double.Parse(My.Settings.Column4Offset)
            o5 = Double.Parse(My.Settings.Column5Offset)
            o6 = Double.Parse(My.Settings.Column6Offset)

            filenameInput = My.Settings.SpectrumLabLogFolder + "\" + graphDay + ".txt"
            output("Opening file : " + filenameInput)
            infile = New System.IO.FileStream(filenameInput, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            instream = New System.IO.StreamReader(infile)

            'skip the title
            s = instream.ReadLine()

            Do While True
                Application.DoEvents()
                s = instream.ReadLine()
                If s Is Nothing Then Exit Do
                ss = s.Split(Microsoft.VisualBasic.vbTab)
                sss = ss(0).Split(":")
                hour = Integer.Parse(sss(0))
                minute = Integer.Parse(sss(1))
                second = Integer.Parse(sss(2))
                If Not (hour = 23 And minute > 50) Then newDay = True
                If newDay Then
                    d = New XDate(graphDate.Year, graphDate.Month, graphDate.Day, hour, minute, second)
                    x = d
                    If cb1.Checked And ss.Length > 1 Then
                        y1 = Math.Min(Math.Max(CDbl(ss(1)) + o1, floor), ceiling)
                        list1.Add(x, y1)
                    End If
                    If cb2.Checked And ss.Length > 2 Then
                        y2 = Math.Min(Math.Max(CDbl(ss(2)) + o2, floor), ceiling)
                        list2.Add(x, y2)
                    End If
                    If cb3.Checked And ss.Length > 3 Then
                        y3 = Math.Min(Math.Max(CDbl(ss(3)) + o3, floor), ceiling)
                        list3.Add(x, y3)
                    End If
                    If cb4.Checked And ss.Length > 4 Then
                        y4 = Math.Min(Math.Max(CDbl(ss(4)) + o4, floor), ceiling)
                        list4.Add(x, y4)
                    End If
                    If cb5.Checked And ss.Length > 5 Then
                        y5 = Math.Min(Math.Max(CDbl(ss(5)) + o5, floor), ceiling)
                        list5.Add(x, y5)
                    End If
                    If cb6.Checked And ss.Length > 6 Then
                        y6 = Math.Min(Math.Max(CDbl(ss(6)) + o6, floor), ceiling)
                        list6.Add(x, y6)
                    End If
                End If
            Loop
            instream.Close()
            infile.Close()

            If My.Settings.ActivateSSRTonline = False Then
                title = "VLF radio at " + My.Settings.StationTitle
            Else
                If My.Settings.StationTitle = "" Then
                    title = "VLF radio at station '" + My.Settings.VLFStationName + "'"
                Else
                    title = "VLF radio at " + My.Settings.StationTitle + " ('" + My.Settings.VLFStationName + "')"
                End If
            End If

            'Day Graph
            zedSIDDay.GraphPane.CurveList.Clear()
            zedSIDDay.GraphPane.AxisChange(zedSIDDay.CreateGraphics)
            If cb1.Checked Then
                zedSIDDay.GraphPane.AddCurve(My.Settings.Column1Name, list1, My.Settings.Column1Color, ZedGraph.SymbolType.None)
            End If
            If cb2.Checked Then
                zedSIDDay.GraphPane.AddCurve(My.Settings.Column2Name, list2, My.Settings.Column2Color, ZedGraph.SymbolType.None)
            End If
            If cb3.Checked Then
                zedSIDDay.GraphPane.AddCurve(My.Settings.Column3Name, list3, My.Settings.Column3Color, ZedGraph.SymbolType.None)
            End If
            If cb4.Checked Then
                zedSIDDay.GraphPane.AddCurve(My.Settings.Column4Name, list4, My.Settings.Column4Color, ZedGraph.SymbolType.None)
            End If
            If cb5.Checked Then
                zedSIDDay.GraphPane.AddCurve(My.Settings.Column5Name, list5, My.Settings.Column5Color, ZedGraph.SymbolType.None)
            End If
            If cb6.Checked Then
                zedSIDDay.GraphPane.AddCurve(My.Settings.Column6Name, list6, My.Settings.Column6Color, ZedGraph.SymbolType.None)
            End If
            zedSIDDay.GraphPane.XAxis.Type = ZedGraph.AxisType.Date
            zedSIDDay.GraphPane.XAxis.Title.Text = "Time on " + graphDayL + " (UT" + UToffsetStr + " hours)"
            zedSIDDay.GraphPane.YAxis.Title.Text = "Power (dB)"
            zedSIDDay.BackColor = Drawing.Color.WhiteSmoke
            zedSIDDay.GraphPane.Chart.Fill = New ZedGraph.Fill(My.Settings.SIDChartTopColor, My.Settings.SIDChartBottomColor, 90)
            zedSIDDay.GraphPane.Legend.Fill = New ZedGraph.Fill(My.Settings.SIDChartBottomColor)
            zedSIDDay.GraphPane.Legend.FontSpec.FontColor = Color.WhiteSmoke
            zedSIDDay.GraphPane.Legend.Position = LegendPos.BottomCenter
            zedSIDDay.GraphPane.YAxis.Scale.MinGrace = 0
            zedSIDDay.GraphPane.YAxis.Scale.MaxGrace = 0
            zedSIDDay.GraphPane.Title.Text = title
            Application.DoEvents()

            'Hour Graph
            zedSIDHour.GraphPane.CurveList.Clear()
            zedSIDHour.GraphPane.AxisChange(zedSIDHour.CreateGraphics)
            If cb1.Checked Then
                zedSIDHour.GraphPane.AddCurve(My.Settings.Column1Name, list1, My.Settings.Column1Color, ZedGraph.SymbolType.None)
            End If
            If cb2.Checked Then
                zedSIDHour.GraphPane.AddCurve(My.Settings.Column2Name, list2, My.Settings.Column2Color, ZedGraph.SymbolType.None)
            End If
            If cb3.Checked Then
                zedSIDHour.GraphPane.AddCurve(My.Settings.Column3Name, list3, My.Settings.Column3Color, ZedGraph.SymbolType.None)
            End If
            If cb4.Checked Then
                zedSIDHour.GraphPane.AddCurve(My.Settings.Column4Name, list4, My.Settings.Column4Color, ZedGraph.SymbolType.None)
            End If
            If cb5.Checked Then
                zedSIDHour.GraphPane.AddCurve(My.Settings.Column5Name, list5, My.Settings.Column5Color, ZedGraph.SymbolType.None)
            End If
            If cb6.Checked Then
                zedSIDHour.GraphPane.AddCurve(My.Settings.Column6Name, list6, My.Settings.Column6Color, ZedGraph.SymbolType.None)
            End If
            zedSIDHour.GraphPane.XAxis.Type = ZedGraph.AxisType.Date
            zedSIDHour.GraphPane.XAxis.Title.Text = "Time on " + graphDayL + " (UT" + UToffsetStr + " hours)"
            zedSIDHour.GraphPane.YAxis.Title.Text = "Power (dB)"
            zedSIDHour.BackColor = Drawing.Color.WhiteSmoke
            zedSIDHour.GraphPane.Chart.Fill = New ZedGraph.Fill(My.Settings.SIDChartTopColor, My.Settings.SIDChartBottomColor, 90)
            zedSIDHour.GraphPane.Legend.Fill = New ZedGraph.Fill(My.Settings.SIDChartBottomColor)
            zedSIDHour.GraphPane.Legend.FontSpec.FontColor = Color.WhiteSmoke
            zedSIDHour.GraphPane.Legend.Position = LegendPos.BottomCenter
            zedSIDHour.GraphPane.YAxis.Scale.MinGrace = 0
            zedSIDHour.GraphPane.YAxis.Scale.MaxGrace = 0
            zedSIDHour.GraphPane.Title.Text = title
            Application.DoEvents()

            Return True
        Catch ex As Exception
            output("Sub processSIDdata()")
            output(ex.Source + " : " + ex.Message)
            Return False
        End Try
    End Function


    'Display the series data on the Day Graph and save it
    Private Sub MakeSIDDayGraph()

        Dim filenameLocalOutput, filenameRemote, filenameRemoteDated As String

        Try
            output("Preparing SID day graph " + graphDay)
            Application.DoEvents()
            zedSIDDay.GraphPane.XAxis.Scale.Min = xMin
            zedSIDDay.GraphPane.XAxis.Scale.Max = xMin + 1
            zedSIDDay.GraphPane.AxisChange(zedSIDDay.CreateGraphics)
            zedSIDDay.GraphPane.Draw(zedSIDDay.CreateGraphics)
            Application.DoEvents()
            filenameLocalOutput = workingFolder + graphDay + "-SIDday.png"
            filenameRemote = My.Settings.VLFStationName + My.Settings.VLFStationIndex + "-SIDday.png"
            filenameRemoteDated = My.Settings.VLFStationName + My.Settings.VLFStationIndex + "-" + graphDay + "-SIDday.png"
            zedSIDDay.GetImage().Save(filenameLocalOutput, Drawing.Imaging.ImageFormat.Png)
            Application.DoEvents()
            If My.Settings.ActivateSSRTonline Then
                output("Upload SID day graph to SSRT Network")
                uploadFTPClient = New Utilities.FTP.FTPclient("ftp.monkeyboo.net", My.Settings.SSRT_FTPusername, My.Settings.SSRT_FTPpassword)
                uploadFTPClient.CurrentDirectory = "/Current"
                uploadFTPClient.Upload(filenameLocalOutput, filenameRemote)
                uploadFTPClient.CurrentDirectory = "/" + graphDay.Substring(0, 4) + "/" + graphDay.Substring(0, 7)
                uploadFTPClient.Upload(filenameLocalOutput, filenameRemoteDated)
            End If
        Catch ex As Exception
            output("Sub MakeSIDDayGraph()")
            output(ex.Source + " : " + ex.Message)
        End Try
    End Sub

    Private Sub MakeSIDHourGraph()
        Dim filenameOutput As String

        Try
            output("Preparing SID hour graph " + graphHour.ToString("00"))
            Application.DoEvents()
            zedSIDHour.GraphPane.XAxis.Scale.Min = xMin + graphHour / 24.0 - 1.0 / 2880.0
            zedSIDHour.GraphPane.XAxis.Scale.Max = xMin + (graphHour + 1) / 24.0 - 1.0 / 2880.0
            zedSIDHour.GraphPane.AxisChange(zedSIDHour.CreateGraphics)
            zedSIDHour.GraphPane.Draw(zedSIDHour.CreateGraphics)

            Application.DoEvents()
            filenameOutput = workingFolder + graphDay + "-SIDhour" + graphHour.ToString("00") + ".png"
            zedSIDHour.GetImage.Save(filenameOutput, Drawing.Imaging.ImageFormat.Png)

        Catch ex As Exception
            output("Sub MakeSIDHourGraph()")
            output(ex.Source + " : " + ex.Message)
        End Try
    End Sub

    Private Sub processGOESdata()
        Const ModifiedJulianDateZero As Double = 15018.0
        Dim list(2) As ZedGraph.PointPairList
        Dim s, syS, syL As String
        Dim infile As IO.FileStream
        Dim instream As IO.StreamReader
        Dim filenames(2), localFilenames(2) As String
        Dim i As Integer
        Dim x, x1, yS, yL As Double

        Try

            'Download datafiles
            localFilenames(0) = "Yesterday.txt"
            localFilenames(1) = "Today.txt"

            filenames(0) = UTDate.AddDays(-1).ToString("yyyyMMdd") + "_Gp_xr_1m.txt"
            filenames(1) = UTDate.ToString("yyyyMMdd") + "_Gp_xr_1m.txt"

            downloadFTPclient.CurrentDirectory = "/pub/lists/xray/"
            For i = 0 To 1
                Application.DoEvents()
                Try
                    output("Downloading GOES data file " + filenames(i))
                    downloadFTPclient.Download(filenames(i), My.Settings.ChartOutputFolderRoot + "\" + localFilenames(i), True)
                Catch ex As Exception
                    output("Sub processGOESData() download routine")
                    output(ex.Source + " : " + ex.Message)
                End Try
            Next

            'Initialize pointpair lists
            For i = 0 To 1
                list(i) = New ZedGraph.PointPairList
            Next

            'Read datafiles into pointpair lists
            For i = 0 To 1
                Application.DoEvents()
                Try
                    infile = New System.IO.FileStream(ftbOutput.FileText + "\" + localFilenames(i), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    'output("Opening file : " + localFilenames(i))
                    instream = New System.IO.StreamReader(infile)
                    Do While True
                        Application.DoEvents()
                        s = instream.ReadLine()
                        If s Is Nothing Then Exit Do
                        If s.Substring(0, 1) = ":" Or s.Substring(0, 1) = "#" Then GoTo bottom 'Ignore comments
                        'Read the date and time stamp from the file
                        'Modified JD               Seconds of the day
                        x = CDbl(s.Substring(19, 5)) + CDbl(s.Substring(25, 6)) / SecondsPerDay + UToffsetDbl / 24.0 - ModifiedJulianDateZero

                        'Quit reading if the file contains old data as sometimes happens
                        If x < x1 Then Exit Do
                        x1 = x

                        syS = s.Substring(36, 8)    'Shortwave
                        If syS <> "1.00e+05" Then   'missing data
                            yS = Math.Log(CDbl(syS), 10) 'data is raw values but chart is log format
                            list(1).Add(x, yS)
                        End If
                        syL = s.Substring(48, 8)    'Longwave
                        If syL <> "1.00e+05" Then
                            yL = Math.Log(CDbl(syL), 10)
                            list(0).Add(x, yL)
                        End If
bottom:
                    Loop
                    instream.Close()
                    infile.Close()
                Catch ex As Exception
                    output(ex.Source + " : " + ex.Message)
                End Try
            Next

            'Reset the graph
            zedGOESDay.GraphPane.CurveList.Clear()
            zedGOESDay.GraphPane.AxisChange(zedGOESDay.CreateGraphics)
            zedGOESDay.GraphPane.AddCurve("GOES Longwave", list(0), Drawing.Color.White, ZedGraph.SymbolType.None)
            zedGOESDay.GraphPane.AddCurve("GOES Shortwave", list(1), Drawing.Color.Yellow, ZedGraph.SymbolType.None)
            zedGOESDay.GraphPane.CurveList(0).IsVisible = True
            zedGOESDay.GraphPane.CurveList(1).IsVisible = True
            zedGOESDay.GraphPane.XAxis.Type = ZedGraph.AxisType.Date
            zedGOESDay.GraphPane.XAxis.Title.Text = "Time on " + graphDayL + " (UT" + UToffsetStr + " hours)"
            zedGOESDay.GraphPane.YAxis.Title.Text = "Log Watts / sq. m"
            zedGOESDay.GraphPane.YAxis.Scale.Format = "0.0"
            zedGOESDay.GraphPane.YAxis.Scale.Min = -9
            zedGOESDay.GraphPane.YAxis.Scale.Max = -2
            'zedGOESDay.GraphPane.YAxis.IsShowGrid = True
            zedGOESDay.BackColor = Drawing.Color.WhiteSmoke
            zedGOESDay.GraphPane.Chart.Fill = New ZedGraph.Fill(Drawing.Color.Red, Drawing.Color.DarkRed, 90)
            zedGOESDay.GraphPane.Legend.Fill = New ZedGraph.Fill(Drawing.Color.DarkRed)
            zedGOESDay.GraphPane.Legend.FontSpec.FontColor = Color.WhiteSmoke
            zedGOESDay.GraphPane.Legend.Position = LegendPos.BottomCenter
            zedGOESDay.GraphPane.Title.Text = "Solar X-rays"
            Application.DoEvents()

            zedGOESHour.GraphPane.CurveList.Clear()
            zedGOESHour.GraphPane.AxisChange(zedGOESHour.CreateGraphics)
            zedGOESHour.GraphPane.AddCurve("GOES Longwave", list(0), Drawing.Color.White, ZedGraph.SymbolType.None)
            zedGOESHour.GraphPane.AddCurve("GOES Shortwave", list(1), Drawing.Color.Yellow, ZedGraph.SymbolType.None)
            zedGOESHour.GraphPane.CurveList(0).IsVisible = True
            zedGOESHour.GraphPane.CurveList(1).IsVisible = True
            zedGOESHour.GraphPane.XAxis.Type = ZedGraph.AxisType.Date
            zedGOESHour.GraphPane.XAxis.Title.Text = "Time on " + graphDayL + " (UT" + UToffsetStr + " hours)"
            zedGOESHour.GraphPane.YAxis.Title.Text = "Log Watts / sq. m"
            zedGOESHour.GraphPane.YAxis.Scale.Format = "0.0"
            zedGOESHour.GraphPane.YAxis.Scale.Min = -9
            zedGOESHour.GraphPane.YAxis.Scale.Max = -2
            'zedGOESHour.GraphPane.YAxis.IsShowGrid = True
            zedGOESHour.GraphPane.Chart.Fill = New ZedGraph.Fill(Drawing.Color.Red, Drawing.Color.DarkRed, 90)
            zedGOESHour.GraphPane.Legend.Fill = New ZedGraph.Fill(Drawing.Color.DarkRed)
            zedGOESHour.GraphPane.Legend.FontSpec.FontColor = Color.WhiteSmoke
            zedGOESHour.GraphPane.Legend.Position = LegendPos.BottomCenter
            zedGOESHour.GraphPane.Title.Text = "Solar X-rays"
            Application.DoEvents()
        Catch ex As Exception
            output("Sub processGOESData()")
            output(ex.Source + " : " + ex.Message)
        End Try

    End Sub


    Private Sub MakeGOESDayGraph()
        Dim filenameOutput, filenameRemoteDated As String

        Try
            output("Preparing GOES day graph")
            Application.DoEvents()
            zedGOESDay.GraphPane.XAxis.Scale.Min = xMin
            zedGOESDay.GraphPane.XAxis.Scale.Max = xMin + 1
            zedGOESDay.GraphPane.AxisChange(zedGOESDay.CreateGraphics)
            zedGOESDay.GraphPane.Draw(zedGOESDay.CreateGraphics)
            filenameOutput = workingFolder + graphDay + "-GOESday.png"
            zedGOESDay.GetImage().Save(filenameOutput, Drawing.Imaging.ImageFormat.Png)

            If My.Settings.ActivateSSRTonline Then
                output("Upload GOES day graph to SSRT Network")
                uploadFTPClient.CurrentDirectory = "/" + graphDay.Substring(0, 4) + "/" + graphDay.Substring(0, 7)
                filenameRemoteDated = My.Settings.VLFStationName + My.Settings.VLFStationIndex + "-" + graphDay + "-GOESday.png"
                uploadFTPClient.Upload(filenameOutput, filenameRemoteDated)
            End If

        Catch ex As Exception
            output("Sub Make GOESDayGraph()")
            output(ex.Source + " : " + ex.Message)
        End Try
    End Sub


    Private Sub MakeGOESHourGraph()
        Dim filenameOutput As String

        Try
            output("Preparing GOES hour graph " + graphHour.ToString("00"))
            Application.DoEvents()
            zedGOESHour.GraphPane.XAxis.Scale.Min = xMin + graphHour / 24.0 - 1.0 / 2880.0
            zedGOESHour.GraphPane.XAxis.Scale.Max = xMin + (graphHour + 1) / 24.0 - 1.0 / 2880.0
            zedGOESHour.GraphPane.AxisChange(zedGOESHour.CreateGraphics)
            zedGOESHour.GraphPane.Draw(zedGOESHour.CreateGraphics)

            Application.DoEvents()
            filenameOutput = workingFolder + graphDay + "-GOEShour" + graphHour.ToString("00") + ".png"
            zedGOESHour.GetImage.Save(filenameOutput, Drawing.Imaging.ImageFormat.Png)

        Catch ex As Exception
            output("Sub MakeGOESHourGraph()")
            output(ex.Source + " : " + ex.Message)
        End Try
    End Sub


    'Log window records activity and errors
    Private Sub output(ByVal s As String)
        Try
            If tbOutput.Text.Length > 65000 Then tbOutput.Clear()
            tbOutput.AppendText(Now.ToString("ddd HH-mm-ss ") + s + Microsoft.VisualBasic.vbCrLf)
            Application.DoEvents()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub loadSettings()
        ftbInput.FileText = My.Settings.SpectrumLabLogFolder
        ftbOutput.FileText = My.Settings.ChartOutputFolderRoot
        ftbLog.FileText = My.Settings.LogFileFolder
        tbTitle.Text = My.Settings.StationTitle
        cb1.Checked = My.Settings.Column1Check
        cb2.Checked = My.Settings.Column2Check
        cb3.Checked = My.Settings.Column3Check
        cb4.Checked = My.Settings.Column4Check
        cb5.Checked = My.Settings.Column5Check
        cb6.Checked = My.Settings.Column6Check
        tbName1.Text = My.Settings.Column1Name
        tbName1.ForeColor = My.Settings.Column1Color
        tbName2.Text = My.Settings.Column2Name
        tbName2.ForeColor = My.Settings.Column2Color
        tbName3.Text = My.Settings.Column3Name
        tbName3.ForeColor = My.Settings.Column3Color
        tbName4.Text = My.Settings.Column4Name
        tbName4.ForeColor = My.Settings.Column4Color
        tbName5.Text = My.Settings.Column5Name
        tbName5.ForeColor = My.Settings.Column5Color
        tbName6.Text = My.Settings.Column6Name
        tbName6.ForeColor = My.Settings.Column6Color
        lblTopColor.BackColor = My.Settings.SIDChartTopColor
        tbName1.BackColor = My.Settings.SIDChartTopColor
        tbName2.BackColor = My.Settings.SIDChartTopColor
        tbName3.BackColor = My.Settings.SIDChartTopColor
        tbName4.BackColor = My.Settings.SIDChartTopColor
        tbName5.BackColor = My.Settings.SIDChartTopColor
        tbName6.BackColor = My.Settings.SIDChartTopColor
        cbActivateFTP.Checked = My.Settings.ActivateSSRTonline
        tbStationName.Text = My.Settings.VLFStationName
        tbStationIndex.Text = My.Settings.VLFStationIndex
        tbStationKey.Text = My.Settings.VLFStationKey
        tbSSRTusername.Text = My.Settings.SSRT_FTPusername
        tbSSRTpassword.Text = My.Settings.SSRT_FTPpassword
        cbActivateGOES.Checked = My.Settings.ActivateGOESCharting
        tbFloor.Text = My.Settings.Floor
        tbCeiling.Text = My.Settings.Ceiling
        tbOffset1.Text = My.Settings.Column1Offset
        tbOffset2.Text = My.Settings.Column2Offset
        tbOffset3.Text = My.Settings.Column3Offset
        tbOffset4.Text = My.Settings.Column4Offset
        tbOffset5.Text = My.Settings.Column5Offset
        tbOffset6.Text = My.Settings.Column6Offset
    End Sub

    Private Sub saveSettings()
        My.Settings.SpectrumLabLogFolder = ftbInput.FileText
        My.Settings.ChartOutputFolderRoot = ftbOutput.FileText
        My.Settings.LogFileFolder = ftbLog.FileText
        My.Settings.StationTitle = tbTitle.Text
        My.Settings.Column1Check = cb1.Checked
        My.Settings.Column2Check = cb2.Checked
        My.Settings.Column3Check = cb3.Checked
        My.Settings.Column4Check = cb4.Checked
        My.Settings.Column5Check = cb5.Checked
        My.Settings.Column6Check = cb6.Checked
        My.Settings.Column1Name = tbName1.Text
        My.Settings.Column2Name = tbName2.Text
        My.Settings.Column3Name = tbName3.Text
        My.Settings.Column4Name = tbName4.Text
        My.Settings.Column5Name = tbName5.Text
        My.Settings.Column6Name = tbName6.Text
        My.Settings.ActivateSSRTonline = cbActivateFTP.Checked
        My.Settings.VLFStationName = tbStationName.Text
        My.Settings.VLFStationIndex = tbStationIndex.Text
        My.Settings.VLFStationKey = tbStationKey.Text
        My.Settings.SSRT_FTPusername = tbSSRTusername.Text
        My.Settings.SSRT_FTPpassword = tbSSRTpassword.Text
        My.Settings.ActivateGOESCharting = cbActivateGOES.Checked
        My.Settings.Floor = tbFloor.Text
        My.Settings.Ceiling = tbCeiling.Text
        My.Settings.Column1Offset = tbOffset1.Text
        My.Settings.Column2Offset = tbOffset2.Text
        My.Settings.Column3Offset = tbOffset3.Text
        My.Settings.Column4Offset = tbOffset4.Text
        My.Settings.Column5Offset = tbOffset5.Text
        My.Settings.Column6Offset = tbOffset6.Text

        My.Settings.Save()
    End Sub

    Private Function validateSettings() As Boolean
        Dim pass As Boolean = True

        If My.Settings.ChartOutputFolderRoot = "" Then
            MsgBox("Please set the chart output folder")
            pass = False
        End If

        If My.Settings.SpectrumLabLogFolder = "" Then
            MsgBox("Please set Spectrum Lab log folder")
            pass = False
        End If

        'If My.Settings.UToffset = "" Then
        '    MsgBox("Please set the UT offset for your timezone")
        '    pass = False
        'End If

        If My.Settings.ActivateSSRTonline = False And My.Settings.StationTitle = "" Then
            MsgBox("Please set station title")
            pass = False
        End If

        If My.Settings.ActivateSSRTonline And My.Settings.VLFStationName = "" Then
            MsgBox("Please set SSRT station name")
            pass = False
        End If

        If My.Settings.ActivateSSRTonline And My.Settings.VLFStationIndex = "" Then
            MsgBox("Please set SSRT station code")
            pass = False
        End If

        If My.Settings.ActivateSSRTonline And My.Settings.SSRT_FTPpassword = "" Then
            MsgBox("Please set the SSRT Net FTP password as per your registration e-mail")
            pass = False
        End If

        If My.Settings.ActivateSSRTonline Then
            Dim s As String
            s = processSecurity("JODRELLBANKBERNARDLOWELL", My.Settings.VLFStationName)
            If s <> My.Settings.VLFStationKey Then
                MsgBox("Invalid SSRT station name/key pair")
                pass = False
            End If
        End If
        Return (pass)
    End Function

    Private Function processSecurity(ByVal m As String, ByVal n As String) As String
        Dim k As String = ""
        Dim x, y, z As Integer
        Dim a As Integer = Asc("A")
        Try
            For i As Integer = 0 To n.Length - 1
                x = Asc(n.Substring(i, 1))
                y = Asc(m.Substring(i, 1))
                z = (x Xor y) Mod 26
                k = k + Chr(z + a)
            Next
            Return k
        Catch ex As Exception
            output("Sub processSecurity()")
            output(ex.Source + " : " + ex.Message)
            Return ""
        End Try

    End Function

    Private Sub btnInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInput.Click
        fbdInput.SelectedPath = ftbInput.FileText
        If fbdInput.ShowDialog() = Windows.Forms.DialogResult.OK Then ftbInput.FileText = fbdInput.SelectedPath
    End Sub

    Private Sub btnOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutput.Click
        fbdOutput.SelectedPath = ftbOutput.FileText
        If fbdOutput.ShowDialog() = Windows.Forms.DialogResult.OK Then ftbOutput.FileText = fbdOutput.SelectedPath
    End Sub

    Private Sub SSRT_Robot_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        saveSettings()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        saveSettings()
        If validateSettings() Then
            btnRefresh.Enabled = False
            saveSettings()
            createGraphs()
            btnRefresh.Enabled = True
        End If
    End Sub

    Private Sub btnBackProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackProcess.Click
        Dim localZone As TimeZone = TimeZone.CurrentTimeZone
        saveSettings()
        Try
            If validateSettings() Then
                btnBackProcess.Enabled = False
                Dim d As DateTime = dtpFrom.Value
                While (DateTime.Compare(d, dtpTo.Value) <= 0)

                    'Calculate UTC offset
                    UToffsetDbl = localZone.GetUtcOffset(graphDate).TotalHours
                    UToffsetStr = UToffsetDbl.ToString(" + #0.0; - #0.0")

                    graphDate = d.Date
                    graphDay = graphDate.ToString("yyyy-MM-dd")
                    graphDayL = graphDate.ToString("dd MMM yyyy")
                    xMin = Int(graphDate.Subtract(StartDate).TotalDays)

                    'Prepare the folder to accept the charts
                    createWorkingFolder()

                    'Process the SSRT data
                    processSIDdata()
                    MakeSIDDayGraph()

                    d = d.AddDays(1)

                End While
                btnBackProcess.Enabled = True
            End If
        Catch ex As Exception
            output("Sub btnBackProcess_Click()")
            output(ex.Source + " : " + ex.Message)
        End Try

    End Sub


    Private Sub btnTopColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTopColor.Click
        ColorDialog1.Color = My.Settings.SIDChartTopColor
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.SIDChartTopColor = ColorDialog1.Color
            lblTopColor.BackColor = My.Settings.SIDChartTopColor
            tbName1.BackColor = My.Settings.SIDChartTopColor
            tbName2.BackColor = My.Settings.SIDChartTopColor
            tbName3.BackColor = My.Settings.SIDChartTopColor
            tbName4.BackColor = My.Settings.SIDChartTopColor
            tbName5.BackColor = My.Settings.SIDChartTopColor
            tbName6.BackColor = My.Settings.SIDChartTopColor
        End If
    End Sub

    Private Sub btnBottomColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomColor.Click
        ColorDialog1.Color = My.Settings.SIDChartBottomColor
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.SIDChartBottomColor = ColorDialog1.Color
            lblBottomColor.BackColor = My.Settings.SIDChartBottomColor
        End If
    End Sub

    Private Sub btn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn1.Click
        ColorDialog1.Color = My.Settings.Column1Color
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.Column1Color = ColorDialog1.Color
            tbName1.ForeColor = My.Settings.Column1Color
        End If
    End Sub

    Private Sub btn2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn2.Click
        ColorDialog1.Color = My.Settings.Column2Color
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.Column2Color = ColorDialog1.Color
            tbName2.ForeColor = My.Settings.Column2Color
        End If
    End Sub


    Private Sub btn3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn3.Click
        ColorDialog1.Color = My.Settings.Column3Color
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.Column3Color = ColorDialog1.Color
            tbName3.ForeColor = My.Settings.Column3Color
        End If
    End Sub

    Private Sub Btn4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn4.Click
        ColorDialog1.Color = My.Settings.Column4Color
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.Column4Color = ColorDialog1.Color
            tbName4.ForeColor = My.Settings.Column4Color
        End If
    End Sub

    Private Sub Btn5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn5.Click
        ColorDialog1.Color = My.Settings.Column5Color
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.Column5Color = ColorDialog1.Color
            tbName5.ForeColor = My.Settings.Column5Color
        End If
    End Sub

    Private Sub Btn6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn6.Click
        ColorDialog1.Color = My.Settings.Column6Color
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.Column6Color = ColorDialog1.Color
            tbName6.ForeColor = My.Settings.Column6Color
        End If
    End Sub


    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        saveSettings()
        If validateSettings() Then
            btnStart.Enabled = False
            btnStop.Enabled = True
            output("Switching on hourly cycle")
            hourlyCycle()
        End If
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        btnStart.Enabled = True
        btnStop.Enabled = False
        output("Switching off hourly cycle")
        Timer1.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ofdLog.FileName = ftbLog.FileText
        If ofdLog.ShowDialog() = Windows.Forms.DialogResult.OK Then ftbLog.FileText = ofdLog.FileName
    End Sub

    Private Sub btnLogUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogUpload.Click
        Dim filenameRemote As String
        btnLogUpload.Enabled = False
        saveSettings()
        If validateSettings() Then
            Try
                uploadFTPClient = New Utilities.FTP.FTPclient("ftp.monkeyboo.net", My.Settings.SSRT_FTPusername, My.Settings.SSRT_FTPpassword)
                uploadFTPClient.CurrentDirectory = "/StationLogs"
                filenameRemote = My.Settings.VLFStationName + My.Settings.VLFStationIndex + "-log.txt"
                uploadFTPClient.Upload(ftbLog.FileText, filenameRemote)
                output("Station log file uploaded")
                btnLogUpload.Enabled = True
            Catch ex As Exception
                output("Sub btnLogUpload_Click()")
                output(ex.Source + " : " + ex.Message)
            End Try
        End If
        btnLogUpload.Enabled = True
    End Sub
End Class
