Public Class ItemRiduzioneInfo
    Inherits ItemContabileInfo


    Private _Div_TipoAssunzione As Integer
    Private _Div_Num_assunzione As String
    Private _Div_Data_Assunzione As DateTime
    Private _Div_IsEconomia As Integer = 0
    Private _IsPreImp As Boolean = False

	
    Property IsPreImp() As Boolean
        Get
            Return _IsPreImp
        End Get
        Set(ByVal value As Boolean)
            _IsPreImp = value
        End Set
    End Property

    Private _Dbi_Anno As String
    Property DBi_Anno() As String
        Get
            Return _Dbi_Anno
        End Get
        Set(ByVal value As String)
            _Dbi_Anno = value
        End Set
    End Property

    Private _Div_NumeroReg As String

    Property Div_NumeroReg() As String
        Get
            Return _Div_NumeroReg
        End Get
        Set(ByVal value As String)
            _Div_NumeroReg = value
        End Set
    End Property

    Property Div_TipoAssunzione() As Integer
        Get
            Return _Div_TipoAssunzione
        End Get
        Set(ByVal value As Integer)
            _Div_TipoAssunzione = value
        End Set
    End Property
    Property Div_Num_assunzione() As String
        Get
            Return _Div_Num_assunzione
        End Get
        Set(ByVal value As String)
            _Div_Num_assunzione = value
        End Set
    End Property
    Property Div_Data_Assunzione() As DateTime
        Get
            Return _Div_Data_Assunzione
        End Get
        Set(ByVal value As DateTime)
            _Div_Data_Assunzione = value
        End Set
    End Property

    Property Div_IsEconomia() As Integer
        Get
            Return _Div_IsEconomia
        End Get
        Set(ByVal value As Integer)
            _Div_IsEconomia = value
        End Set
    End Property


End Class
