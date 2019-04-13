<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblStatoConcorso = New System.Windows.Forms.Label()
        Me.lblCampionato = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblScadenza = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTermine = New System.Windows.Forms.Label()
        Me.lblGiornata = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblSpeciale = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.lblTest = New System.Windows.Forms.Label()
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.cmdInvia = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 10000
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 98)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Stato concorso"
        '
        'lblStatoConcorso
        '
        Me.lblStatoConcorso.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatoConcorso.Location = New System.Drawing.Point(139, 100)
        Me.lblStatoConcorso.Name = "lblStatoConcorso"
        Me.lblStatoConcorso.Size = New System.Drawing.Size(153, 13)
        Me.lblStatoConcorso.TabIndex = 1
        Me.lblStatoConcorso.Text = "Label2"
        Me.lblStatoConcorso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCampionato
        '
        Me.lblCampionato.Font = New System.Drawing.Font("Arial", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCampionato.Location = New System.Drawing.Point(12, 9)
        Me.lblCampionato.Name = "lblCampionato"
        Me.lblCampionato.Size = New System.Drawing.Size(280, 20)
        Me.lblCampionato.TabIndex = 2
        Me.lblCampionato.Text = "Label2"
        Me.lblCampionato.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 132)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Scadenza concorso"
        '
        'lblScadenza
        '
        Me.lblScadenza.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScadenza.Location = New System.Drawing.Point(139, 132)
        Me.lblScadenza.Name = "lblScadenza"
        Me.lblScadenza.Size = New System.Drawing.Size(153, 13)
        Me.lblScadenza.TabIndex = 4
        Me.lblScadenza.Text = "Label2"
        Me.lblScadenza.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 164)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 15)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Al termine"
        '
        'lblTermine
        '
        Me.lblTermine.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTermine.ForeColor = System.Drawing.Color.Blue
        Me.lblTermine.Location = New System.Drawing.Point(142, 166)
        Me.lblTermine.Name = "lblTermine"
        Me.lblTermine.Size = New System.Drawing.Size(150, 13)
        Me.lblTermine.TabIndex = 6
        Me.lblTermine.Text = "Label2"
        Me.lblTermine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblGiornata
        '
        Me.lblGiornata.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGiornata.Location = New System.Drawing.Point(139, 41)
        Me.lblGiornata.Name = "lblGiornata"
        Me.lblGiornata.Size = New System.Drawing.Size(153, 13)
        Me.lblGiornata.TabIndex = 8
        Me.lblGiornata.Text = "Label2"
        Me.lblGiornata.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 15)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Giornata"
        '
        'lblSpeciale
        '
        Me.lblSpeciale.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpeciale.Location = New System.Drawing.Point(139, 70)
        Me.lblSpeciale.Name = "lblSpeciale"
        Me.lblSpeciale.Size = New System.Drawing.Size(153, 13)
        Me.lblSpeciale.TabIndex = 10
        Me.lblSpeciale.Text = "Label2"
        Me.lblSpeciale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 70)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(55, 15)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Speciale"
        '
        'Timer2
        '
        Me.Timer2.Interval = 60000
        '
        'lblTest
        '
        Me.lblTest.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTest.ForeColor = System.Drawing.Color.Green
        Me.lblTest.Location = New System.Drawing.Point(12, 190)
        Me.lblTest.Name = "lblTest"
        Me.lblTest.Size = New System.Drawing.Size(174, 24)
        Me.lblTest.TabIndex = 11
        Me.lblTest.Text = "Label2"
        Me.lblTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer3
        '
        '
        'cmdInvia
        '
        Me.cmdInvia.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInvia.Location = New System.Drawing.Point(193, 190)
        Me.cmdInvia.Name = "cmdInvia"
        Me.cmdInvia.Size = New System.Drawing.Size(99, 23)
        Me.cmdInvia.TabIndex = 12
        Me.cmdInvia.Text = "Invia ora"
        Me.cmdInvia.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 218)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdInvia)
        Me.Controls.Add(Me.lblTest)
        Me.Controls.Add(Me.lblSpeciale)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblGiornata)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblTermine)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblScadenza)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblCampionato)
        Me.Controls.Add(Me.lblStatoConcorso)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Invia messaggi TotoMIO II"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblStatoConcorso As System.Windows.Forms.Label
    Friend WithEvents lblCampionato As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblScadenza As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTermine As System.Windows.Forms.Label
    Friend WithEvents lblGiornata As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblSpeciale As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents lblTest As System.Windows.Forms.Label
    Friend WithEvents Timer3 As System.Windows.Forms.Timer
    Friend WithEvents cmdInvia As System.Windows.Forms.Button

End Class
