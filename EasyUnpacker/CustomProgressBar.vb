Public Class CustomProgressBar
    Inherits ProgressBar

    Sub New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.ForeColor = Color.AliceBlue
    End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Dim percent As Double = Me.Value / Me.Maximum
        Dim percentText As String = String.Format("{0:p0}", percent)

        ProgressBarRenderer.DrawHorizontalBar(e.Graphics, Me.ClientRectangle)

        If Me.Value > Me.Minimum Then
            Dim rect As Rectangle = Me.ClientRectangle
            rect.Inflate(-1, -1)
            rect.Width = rect.Width * percent
            ProgressBarRenderer.DrawHorizontalChunks(e.Graphics, rect)
        End If

        Using sf As New StringFormat
            sf.Alignment = StringAlignment.Center
            sf.LineAlignment = StringAlignment.Center
            e.Graphics.DrawString(percentText, Me.Font, Brushes.Black, Me.ClientRectangle, sf)
        End Using
    End Sub

End Class
