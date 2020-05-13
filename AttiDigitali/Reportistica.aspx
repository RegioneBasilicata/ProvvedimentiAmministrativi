<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Reportistica.aspx.vb"  Inherits="AttiDigitali.Reportistica"%>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI, Version=1.9.0.0, Culture=neutral, PublicKeyToken=24d65337282035f2" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Sistema di Gestione Atti Amministrativi</title>
		<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="Determine.css" type="text/css" rel="stylesheet" />  <link href="ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon"/>
		<script language=javascript>
		    function verificaDate() {
		        //if (window.document.getElementById('CalendarPopupDataDa') != null) {
		        //var dataDa = new Date(window.document.getElementById('CalendarPopupDataDa'));
//		        var stringa = window.document.getElementById('CalendarPopupDataDa');
//		        var partsDa = stringa.split('/');
//		        var a = window.document.getElementById('CalendarPopupDataDa');
//		      
//		        var dataDa = new Date(partsDa[2], partsDa[0] - 1, partsDa[1]);
//		        var partsA = window.document.getElementById('CalendarPopupDataA').split('/');
//		        var dataA = new Date(partsA[2], partsA[0] - 1, partsA[1]); 
//		           // var dataA = new Date(window.document.getElementById('CalendarPopupDataA'));
//		            if (dataA <= dataDa) {
//		                alert("Errore nelle date selezionate. La 'Data Da' è maggiore della 'Data A'");
//		            }
//		          //  window.document.getElementById('CalendarPopupDataDa').style.visibility = 'hidden'
		        //}
		    } 
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server" onsubmit="verificaDate()">
			<table id="Table1" class="pagina" cellpadding="0" cellspacing="0">
				<tr>
					<td colspan="2" class="pagina"><asp:placeholder id="Testata" runat="server"></asp:placeholder></td>
				</tr>
				<tr>
					<td class="pagina" width="25%"><asp:placeholder id="Albero" runat="server"></asp:placeholder></td>
					<td class="pagina" width="75%"><asp:placeholder id="Contenuto" runat="server"></asp:placeholder>
					    <div  class="PrintClass">
					        <table id="tabellaContenuti">
					            <tr>
					                <td>
	                                    <asp:Label cssclass="lblSearch" ID="Filtra_i_documenti" runat="server" Text="Da data:"></asp:Label>
	                                </td>
	                                <td>
	                                    <ew:CalendarPopup id="CalendarPopupDataDa" runat="server" DisableTextboxEntry="False">
				                            <WeekdayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></WeekdayStyle>
				                            <MonthHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="#99CCFF"></MonthHeaderStyle>
				                            <OffMonthStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Gray"
					                            BackColor="Azure"></OffMonthStyle>
				                            <GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></GoToTodayStyle>
				                            <TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="Beige"></TodayDayStyle>
				                            <DayHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="Gainsboro"></DayHeaderStyle>
				                            <WeekendStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="LightGray"></WeekendStyle>
				                            <SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="#99CCFF"></SelectedDateStyle>
				                            <ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></ClearDateStyle>
				                            <HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></HolidayStyle>
			                            </ew:CalendarPopup> 
	                                </td>
					            </tr>
					            <tr>
					                <td>
	                                    <asp:Label cssclass="lblSearch" ID="Filtra_i_documenti2" runat="server" Text="A data:"></asp:Label>
	                                </td>
	                                <td>
	                                    <ew:CalendarPopup id="CalendarPopupDataA" runat="server" DisableTextboxEntry="False">
				                            <WeekdayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></WeekdayStyle>
				                            <MonthHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="#99CCFF"></MonthHeaderStyle>
				                            <OffMonthStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Gray"
					                            BackColor="Azure"></OffMonthStyle>
				                            <GoToTodayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></GoToTodayStyle>
				                            <TodayDayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="Beige"></TodayDayStyle>
				                            <DayHeaderStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="Gainsboro"></DayHeaderStyle>
				                            <WeekendStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="LightGray"></WeekendStyle>
				                            <SelectedDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="#99CCFF"></SelectedDateStyle>
				                            <ClearDateStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></ClearDateStyle>
				                            <HolidayStyle Font-Size="XX-Small" Font-Names="Verdana,Helvetica,Tahoma,Arial" ForeColor="Black"
					                            BackColor="White"></HolidayStyle>
			                            </ew:CalendarPopup> 
	                                </td>
					            </tr>
					            <tr>
					                <td colspan="2">
					                    <span style="float:right; padding-right:5px;"><asp:Button cssclass="btn" id="btnFiltraRicerca" runat="server" Text="Report"></asp:Button></span>
					                </td>
					            </tr>	
					            <tr>
					                <td colspan="2">
					                    
					                    <span style="float:right; padding-right:5px;"><asp:HyperLink  id="LinkAttiTotali" runat="server" Text="N° Atti Totali"></asp:HyperLink ></span>
					                </td>
					            </tr>	
					            <tr>
					                <td colspan="2">
					                    
					                    <span style="float:right; padding-right:5px;"></span>
					                </td>
					            </tr>						           
					        </table>
						</div>
					</td>
				</tr>
				<!-- 'modgg 10-06 9 INIZIO -->
				<tr>
					<td width="25%">
						<div align="center" class="x-panel-header"><a href="ChiudiAction.aspx">Esci dall'applicazione</a></div>
					</td>
					<td class="x-panel-header" width="75%"></td>
				</tr>
				<!-- 'modgg 10-06 9 FINE -->
			</table>
			</form>
	</body>
</html>
