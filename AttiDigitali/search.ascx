<%@ Control Language="vb" AutoEventWireup="false" Codebehind="search.ascx.vb" Inherits="AttiDigitali.search" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI, Version=1.9.0.0, Culture=neutral, PublicKeyToken=24d65337282035f2" %>
<div style="width: 100%;" >
	<asp:panel id="pnlRicerca"  HorizontalAlign="Center" runat="server" >
	<fieldset style="margin-top:15px;border:1px solid green">
	    <%--<legend style="font-weight:bold;font-size:13px;">Ricerca per Documento: </legend>--%>
	    <legend style="padding: 0.2em 0.5em;
          border:1px solid green;
          color:green;
          font-size:90%;
          text-align:right;">Dati documento: </legend>
	
	<table style="width: 98%; line-height:2em; margin: auto; margin-top: 10px; margin-bottom: 10px;">
	<!--tr>
	    <th colspan="2">
	        <asp:Label cssclass="lblInfo" ID="Titolo_Ricerca" runat="server" Text="RICERCA DEI DOCUMENTI"></asp:Label>
	    </th>
	</tr-->
	<tr>
	    <td>
	        <asp:Label cssclass="lblSearch" ID="labelTipoData" runat="server" Text="Tipo data: "></asp:Label>
	    </td>
        <td>
	        <asp:RadioButtonList RepeatDirection="Horizontal" RepeatColumns="1"  id="chkDataSeduta" Runat="server" Width="40%"  cssclass="radio" >
                   <asp:ListItem Text="Data Creazione" Value="1" ></asp:ListItem>
                   <asp:ListItem Text="Data Seduta" Value="2"></asp:ListItem>
            </asp:RadioButtonList>
	    </td>
	</tr>
	<tr>
	    <td>
	        <asp:Label cssclass="lblSearch" ID="Ricerca_i_documenti" runat="server" Text="Da data:"></asp:Label>
	    </td>
	    <td>
	        <ew:CalendarPopup id="CalendarPopup1" runat="server" DisableTextboxEntry="False" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);">
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
	        <asp:Label cssclass="lblSearch" ID="alla_data" runat="server" Text="A data:"></asp:Label>
	    </td>
	    <td>
			<ew:CalendarPopup id="CalendarPopup2" runat="server" DisableTextboxEntry="False" Culture="Italian (Italy)"
				VisibleDate="2008-05-06" AllowArbitraryText="False" UpperBoundDate="12/31/9999 23:59:59" CellPadding="2px"
				CellSpacing="0px" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);">
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
	        <asp:Label cssclass="lblSearch" ID="Numero" runat="server" Text="Numero:"></asp:Label> 
	    </td>
	    <td>
	        <asp:TextBox id="txtNumero" runat="server" Width="100%" CssClass="sololinea" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox>
	    </td>
	</tr>
	<tr>
	    <td>
	        <asp:Label cssclass="lblSearch" ID="con_oggetto" runat="server" Text="Oggetto:"></asp:Label> 
	    </td>
	    <td>
	        <asp:TextBox id="txtOggettoRicerca" runat="server" Width="100%" CssClass="sololinea" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox>
	    </td>
	</tr>
	
	<tr>
	    <td>
	        <asp:Label cssclass="lblSearch" ID="con_codiceCUP" runat="server" Text="Codice CUP:"></asp:Label> 
	    </td>
	    <td>
	        <asp:TextBox id="txtCodiceCUP" runat="server" Width="40%" CssClass="sololinea" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox>
	    </td>
	</tr>
    <tr>
	    <td>
	        <asp:Label cssclass="lblSearch" ID="con_codiceCIG" runat="server" Text="Codice CIG:"></asp:Label> 
	    </td>
	    <td>
	        <asp:TextBox id="txtCodiceCIG" runat="server" Width="40%" CssClass="sololinea" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox>
	    </td>
	</tr>
			
	<tr>
	    <td>
		    <asp:Label cssclass="lblSearch" ID="del_Dipartimento" runat="server" Text="Dipartimento:"></asp:Label>
		</td>
		<td>
		    <asp:DropDownList id="dllDipartimenti" runat="server" Width="100%" AutoPostBack="True"></asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td>
	        <asp:Label cssclass="lblSearch" ID="dell_ufficio" runat="server" Text="Ufficio:"/>
	    </td>
	    <td>
		    <asp:DropDownList id="ddlUffici" runat="server" Width="100%"></asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td>
	        <asp:Label cssclass="lblSearch" ID="lblTipo" runat="server" Text="Filtra se:"></asp:Label> 
	    </td>
	    <td>
            <asp:DropDownList id="dllTipoRigetto" runat="server" Width="100%"><asp:ListItem  Value=""></asp:ListItem><asp:ListItem Value="UDD" >Rigettata da UDG</asp:ListItem><asp:ListItem Value="UCA">Rigettata da UCA</asp:ListItem><asp:ListItem Value="UR">Rigettata da UR</asp:ListItem></asp:DropDownList>	
	    </td>
	</tr>
	<tr>
	    <td style="width:20%">
	        <asp:Label cssclass="lblSearch" ID="lblBeneficiario" runat="server" Text="Beneficiario:"></asp:Label> 
	    </td>
	    <td style="width:80%">
	        <asp:DropDownList id="TipologiaRicercaBeneficiario" runat="server" Width="30%">
                <asp:ListItem Value="DenominazioneBeneficiario">Denominazione</asp:ListItem>
                <asp:ListItem Value="CFBeneficiario" >Codice Fiscale</asp:ListItem>
                <asp:ListItem Value="PIVABeneficiario">Partita Iva</asp:ListItem>
                <asp:ListItem Value="CodiceSICBeneficiario">Codice SIC</asp:ListItem>
	        </asp:DropDownList>	
	    <%--</td>
	    <td style="width:60%">--%>
	        <asp:TextBox id="BeneficiarioTxt" runat="server" Width="68%" CssClass="sololinea"  onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox>
	    </td>
	</tr>
	</table>
	</fieldset>
	<fieldset style="margin-top:8px;margin-bottom:8px;border:1px solid green">
	    <legend style="padding: 0.2em 0.5em;
          border:1px solid green;
          color:green;
          font-size:90%;
          text-align:right;">Dati trasparenza: </legend>
	    <table style="width: 98%; line-height:2em; margin: auto; margin-top: 10px; margin-bottom: 10px;">
	        <tr>
	            <td style="width:20%">
	                <asp:Label cssclass="lblSearch" ID="lblTipologiaDocumento" runat="server" Text="Tipologia atto:"></asp:Label> 
	            </td>
	            <td style="width:80%">
	                <asp:DropDownList id="ddlTipologiaDocumento" runat="server" Width="100%"></asp:DropDownList>
	            </td>
            </tr>
            <tr>
	            <td style="width:20%">
	                <asp:Label cssclass="lblSearch" ID="lblIsPubblicato" runat="server" Text="Autorizzazione alla pubblicazione:"></asp:Label> 
	            </td>
	            <td style="width:80%">
	                <asp:DropDownList id="ddlAutorizPubblicazione" runat="server" Width="100%">
	                    <asp:ListItem Value="Tutti">Tutti</asp:ListItem>
                        <asp:ListItem Value="NonSpecificato">Non specificato</asp:ListItem>
                        <asp:ListItem Value="True">Si</asp:ListItem>
                        <asp:ListItem Value="False">No</asp:ListItem>
	                </asp:DropDownList>
	            </td>
            </tr>
            <tr>
	    <td style="width:20%">
	        <asp:Label cssclass="lblSearch" ID="lblDestinatario" runat="server" Text="Destinatario:"></asp:Label> 
	    </td>
	    <td style="width:80%">
	        <asp:DropDownList id="TipologiaRicercaDestinatario" runat="server" Width="30%">
                <asp:ListItem Value="DenominazioneDestinatario">Denominazione</asp:ListItem>
                <asp:ListItem Value="CFDestinatario" >Codice Fiscale</asp:ListItem>
                <asp:ListItem Value="PIVADestinatario">Partita Iva</asp:ListItem>
                <asp:ListItem Value="CodiceSICDestinatario">Codice SIC</asp:ListItem>
	        </asp:DropDownList>	
	    <%--</td>
	    <td style="width:60%">--%>
	        <asp:TextBox id="DestinatarioTxt" runat="server" Width="68%" CssClass="sololinea"  onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox>
	    </td>
	</tr>
        </table>
    </fieldset>
    
	<!--tr>
	    <td colspan="2" style="padding-top:5px">			
	        <asp:Button cssclass="btn" id="btnAvviaRicercaOld" runat="server" Text="Avvia Ricerca"></asp:Button>
        </td>
    </tr-->
    
	
	
	<!--fieldset style="margin:10px 5px 10px 5px;">
	    <legend>Ricerca per Beneficiario</legend>
	    
	    <table style="width: 98%; line-height:1.5em; margin: auto; margin-top: 10px; margin-bottom: 10px;">
	        <tr>
	            <td style="width:20%;">
	                <asp:DropDownList id="TipologiaRicercaBeneficiario2" runat="server" Width="100%">
	                    <asp:ListItem  Value="Seleziona..."></asp:ListItem>
	                    <asp:ListItem Value="CFBeneficiario" >Codice Fiscale</asp:ListItem>
	                    <asp:ListItem Value="PIVABeneficiario">Partita Iva</asp:ListItem>
	                    <asp:ListItem Value="DenominazioneBeneficiario">Denominazione</asp:ListItem>
	                    <asp:ListItem Value="CodiceSICBeneficiario">Codice SIC</asp:ListItem>
	                </asp:DropDownList>	
	            </td>
	            <td style="width:80%">
	                <asp:TextBox id="BeneficiarioTxt2" runat="server" Width="100%" CssClass="sololinea" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox>
	            </td>
	        </tr>
	    </table>
	    
	</fieldset-->
	
	<span style="float:right; padding-right:5px;"><asp:Button cssclass="btn" id="btnAvviaRicerca" runat="server" Text="Avvia Ricerca"></asp:Button></span>
	</asp:panel></div>
	
	<script type="text/javascript">
	    <!--
	    function KeyDownHandler(event, btn) {
	        // process only the Enter key
	        if (event.keyCode == 13) {
	            // cancel the default submit
	            event.returnValue = false;
	            event.cancel = true;
	            // submit the form by programmatically clicking the specified button
	            btn.click();
	        }
	    }
	    // -->
	</script>
