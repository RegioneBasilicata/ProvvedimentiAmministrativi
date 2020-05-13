<%@ Control Language="vb" AutoEventWireup="false" Codebehind="searchCompetenza.ascx.vb" Inherits="AttiDigitali.searchCompetenza" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI, Version=1.9.0.0, Culture=neutral, PublicKeyToken=24d65337282035f2" %>
<DIV style="WIDTH: 100px; HEIGHT: 100px" ms_positioning="FlowLayout">
	<asp:panel id="pnlRicerca" Height="120px" Width="580px" HorizontalAlign="Center" runat="server" >
		<P align="center">RICERCA DEI DOCUMENTI</p>
		<p>Ricerca i documenti dalla data
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
			</ew:CalendarPopup><br/>
			alla data
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
			</ew:CalendarPopup></p>
		<p>Numero
			<asp:TextBox id="txtNumero" runat="server" Width="126px" CssClass="sololinea" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox></p>
		<p>con oggetto
			<asp:TextBox id="txtOggettoRicerca" runat="server" Width="441px" CssClass="sololinea" onKeyDown="KeyDownHandler(event, ricerca_btnAvviaRicerca);"></asp:TextBox></p>
		<p>del Dipartimento:&nbsp;
			<asp:DropDownList id="dllDipartimenti" runat="server" AutoPostBack="True"></asp:DropDownList></p>
		<p>dell'ufficio:&nbsp;
			<asp:DropDownList id="ddlUffici" runat="server"></asp:DropDownList></p>
<p><asp:Label ID="lblTipo" runat="server" Text="Filtra se: "></asp:Label> <asp:DropDownList id="dllTipoRigetto" runat="server" ><asp:ListItem  Value=""></asp:ListItem><asp:ListItem Value="UDD" >Rigettata da UDG</asp:ListItem><asp:ListItem Value="UCA">Rigettata da UCA</asp:ListItem><asp:ListItem Value="UR">Rigettata da UR</asp:ListItem></asp:DropDownList></p>			
		<p>
			<asp:Button cssclass="btn" id="btnAvviaRicerca" runat="server" Text="Avvia Ricerca"></asp:Button></p>
			
	</asp:panel></DIV>
	
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
