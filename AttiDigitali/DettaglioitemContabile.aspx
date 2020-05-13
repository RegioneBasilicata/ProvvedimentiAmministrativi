<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DettaglioitemContabile.aspx.vb" Inherits="AttiDigitali.DettaglioitemContabile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  		<title>Sistema di Gestione Atti Amministrativi</title>
  		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href=risorse/immagini/favicon.ico type="image/x-icon" /></head>
</head>

<body>
    <form id="form1" runat="server">
   <table id="Table1" class="pagina" cellspacing="0" cellpadding="0">
				<tr>
					<td colspan="2" class="pagina">
						<asp:PlaceHolder id="Testata" runat="server"></asp:PlaceHolder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%">
						<asp:PlaceHolder id="Albero" runat="server"></asp:PlaceHolder></td>
					<td class="pagina" width="75%">
						<asp:PlaceHolder id="Contenuto" runat="server">
                        
						
						</asp:PlaceHolder>  
    
        <asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" 
         Height="50px" Width="125px" AutoGenerateRows="False" 
            DataSourceID="ObjectDetail">
            <Fields>
                <asp:BoundField DataField="Dli_Id" HeaderText="Dli_Id" 
                    SortExpression="Dli_Id" />
                <asp:BoundField DataField="Dli_Documento" HeaderText="Dli_Documento" 
                    SortExpression="Dli_Documento" />
                <asp:BoundField DataField="Dli_prog" HeaderText="Dli_prog" 
                    SortExpression="Dli_prog" />
                <asp:BoundField DataField="Dli_DataRegistrazione" 
                    HeaderText="Dli_DataRegistrazione" SortExpression="Dli_DataRegistrazione" />
                <asp:BoundField DataField="Dli_Operatore" HeaderText="Dli_Operatore" 
                    SortExpression="Dli_Operatore" />
                <asp:BoundField DataField="Dli_Esercizio" HeaderText="Dli_Esercizio" 
                    SortExpression="Dli_Esercizio" />
                <asp:BoundField DataField="Dli_UPB" HeaderText="Dli_UPB" 
                    SortExpression="Dli_UPB" />
                <asp:BoundField DataField="Dli_Cap" HeaderText="Dli_Cap" 
                    SortExpression="Dli_Cap" />
                <asp:BoundField DataField="Dli_Costo" HeaderText="Dli_Costo" 
                    SortExpression="Dli_Costo" />
                <asp:BoundField DataField="Dli_NContabile" HeaderText="Dli_NContabile" 
                    SortExpression="Dli_NContabile" />
                <asp:BoundField DataField="Dli_TipoAssunzione" HeaderText="Dli_TipoAssunzione" 
                    SortExpression="Dli_TipoAssunzione" />
                <asp:BoundField DataField="Dli_Num_assunzione" HeaderText="Dli_Num_assunzione" 
                    SortExpression="Dli_Num_assunzione" />
                <asp:BoundField DataField="Dli_Data_Assunzione" 
                    HeaderText="Dli_Data_Assunzione" SortExpression="Dli_Data_Assunzione" />
            </Fields>
        </asp:DetailsView>
    
        <asp:ObjectDataSource ID="ObjectDetail" runat="server" 
            SelectMethod="FO_Get_Item_Contabile" TypeName="DllDocumentale.svrDocumenti">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="id" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
    </td>
				</tr>
			
				<tr>
					<td width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx">Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
		
			</table>
    </form>
</body>
</html>
