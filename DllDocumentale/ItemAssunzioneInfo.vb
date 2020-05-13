Public Class ItemAssunzioneContabileInfo

    Private _Da_Documento As String
    Private _Da_prog As Long
    Private _Da_Costo As Decimal
    Private _Da_DataRegistrazione As DateTime
    Private _Da_Operatore As String
    Private _Da_Stato As Integer = 1

    Property Da_Documento() As String
        Get
            Return _Da_Documento
        End Get
        Set(ByVal value As String)
            _Da_Documento = value
        End Set
    End Property
    Property Da_prog() As Long
        Get
            Return _Da_prog
        End Get
        Set(ByVal value As Long)
            _Da_prog = value
        End Set
    End Property
 
    Property Da_Operatore() As String
        Get
            Return _Da_Operatore
        End Get
        Set(ByVal value As String)
            _Da_Operatore = value
        End Set
    End Property
    Property Da_Costo() As Decimal
        Get
            Return _Da_Costo
        End Get
        Set(ByVal value As Decimal)
            _Da_Costo = value
        End Set
    End Property
    Property Da_DataRegistrazione() As DateTime
        Get
            Return _Da_DataRegistrazione
        End Get
        Set(ByVal value As DateTime)
            _Da_DataRegistrazione = value
        End Set
    End Property
    Property Da_Stato() As Integer
        Get
            Return _Da_Stato
        End Get
        Set(ByVal value As Integer)
            _Da_Stato = value
        End Set
    End Property


End Class


