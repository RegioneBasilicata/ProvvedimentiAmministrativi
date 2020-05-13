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


Partial Public Class CreaProvvedimento
    
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
    '''Controllo lblEtichetta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblEtichetta As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo controlloCheck.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents controlloCheck As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo valuePub.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents valuePub As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo valueOggetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents valueOggetto As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo valueOpContabile.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents valueOpContabile As Global.System.Web.UI.WebControls.HiddenField
    
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
    
    '''<summary>
    '''Controllo flagModificato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagModificato As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagRegistra.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagRegistra As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagLivello.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagLivello As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagAbilitaOpContabili.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagAbilitaOpContabili As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagAbilitaPubblBur.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagAbilitaPubblBur As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagCodificaAltriUff.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagCodificaAltriUff As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagCodiciCupCig.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagCodiciCupCig As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagAbilitaOggettoDoc.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagAbilitaOggettoDoc As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagAbilitaSchedaLeggeTrasparenza.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagAbilitaSchedaLeggeTrasparenza As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo flagAbilitaTipologiaProvvedimento.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents flagAbilitaTipologiaProvvedimento As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo valueDataCreazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents valueDataCreazione As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo valueUffProp.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents valueUffProp As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo descrizioneUffProp.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents descrizioneUffProp As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo responsabileUffProp.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents responsabileUffProp As Global.System.Web.UI.WebControls.HiddenField
    
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
    '''Controllo schedaLeggeTrasparenzaInfo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents schedaLeggeTrasparenzaInfo As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo schedaContrattiFattureInfo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents schedaContrattiFattureInfo As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo schedaTipologiaProvvedimentoInfo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents schedaTipologiaProvvedimentoInfo As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo abilitatoCreazioneAttiSiOpCont.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents abilitatoCreazioneAttiSiOpCont As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo msgBloccoCreazioneAtti.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents msgBloccoCreazioneAtti As Global.System.Web.UI.WebControls.HiddenField
End Class
