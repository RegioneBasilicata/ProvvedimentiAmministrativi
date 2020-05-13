'------------------------------------------------------------------------------
' <generato automaticamente>
'     Codice generato da uno strumento.
'
'     Le modifiche a questo file possono causare un comportamento non corretto e verranno perse se
'     il codice viene rigenerato. 
' </generato automaticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class Contabile
    
    '''<summary>
    '''Controllo Head1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Head1 As Global.System.Web.UI.HtmlControls.HtmlHead
    
    '''<summary>
    '''Controllo Testata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Testata As Global.System.Web.UI.WebControls.PlaceHolder
    
    '''<summary>
    '''Controllo Albero.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Albero As Global.System.Web.UI.WebControls.PlaceHolder
    
    '''<summary>
    '''Controllo Contenuto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Contenuto As Global.System.Web.UI.WebControls.PlaceHolder
    
    '''<summary>
    '''Controllo chkPreimpegni.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkPreimpegni As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo chkImpegno.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkImpegno As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo chkImpegnoSuPerenti.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkImpegnoSuPerenti As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo chkAccertamento.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkAccertamento As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo chkRiduzione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkRiduzione As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo chkLiquidazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkLiquidazione As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo chkRiduzionePreImp.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkRiduzionePreImp As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo chkRiduzioneLiq.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkRiduzioneLiq As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo TipoRettifiche.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TipoRettifiche As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo PrenotazioniDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents PrenotazioniDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo ImpegniDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ImpegniDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo ImpegniPerentiDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ImpegniPerentiDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo RiduzioniDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents RiduzioniDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo LiquidazioniDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LiquidazioniDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo LiquidazioniContestualiDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LiquidazioniContestualiDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo RiduzioniPreImpDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents RiduzioniPreImpDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo RiduzioniLiqDaConfermare.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents RiduzioniLiqDaConfermare As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo NFattureDocumento.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents NFattureDocumento As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo NPreimpReg.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents NPreimpReg As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo NimpReg.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents NimpReg As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo NliqReg.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents NliqReg As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo NMandati.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents NMandati As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo RidLiq.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents RidLiq As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo isUffProp.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents isUffProp As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo TestoCaricato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TestoCaricato As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo Cod_uff_Prop.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Cod_uff_Prop As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo codFiscOperatore.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents codFiscOperatore As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo uffPubblicoOperatore.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents uffPubblicoOperatore As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo tipo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents tipo As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo codDocumento.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents codDocumento As Global.System.Web.UI.WebControls.HiddenField
End Class
