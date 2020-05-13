<%@ Page Language="vb" AutoEventWireup="false" Codebehind="formStampa.aspx.vb" Inherits="AttiDigitali.formStampa"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<script language="JavaScript"><!--
		    function printAll() {

var printPage = Form1.idLinkToPrint.value


	num=parent.document.all.tags("FRAME").length;
   
    //document.all['slide']
   
   var act=new ActiveXObject("StampaDocumentiProvvedimenti.StampaProvvedimenti")
var vclient = new Array(); 
    
vclient[0]=""
vclient[1]=""
vclient[2]=""


var col_array=window.location.search.replace("?ids=","").split("-");

var link= window.location.href.replace("formStampa.aspx" + window.location.search , printPage +"?key=");

var part_num=0;
while (part_num < col_array.length)
 {
 
 vclient[0]=link + col_array[part_num]+'&prn=1'+'&idop='+ Form1.idOp.value
act.stampaDocumenti(vclient.join("||") )

  if (col_array[part_num]!="")
//var w=  window.open(link + col_array[part_num],'all'+part_num,'top=-10,left=-10,toolbar=no,location=no-directories=no,status=no,menubar=no,scrollbars=no,resizable=no,width=1,height=1')
 //window.open('Http://localhost/AttiDigitali/AnteprimaAllegatoAction.aspx?key='+ col_array[part_num]+','all','width=300,height=200,resizable=yes'); 
 
  part_num+=1;
  
}


 act=null;
 
 /*
   for (i=0;i<num;i++)
    {
		if ("principale" != parent.frames[i].name)
		 {
			//parent.document.frames[i].focus();
			alert(parent.document.frames[i]);
		//	window.print();     
			
         }    
    }       
*/
}
//--></script>
		<link rel="shortcut icon" href="risorse/immagini/favicon.ico" type="image/x-icon" /></head>
	<body bgColor="#3366cc" onload="printAll()">
		<div style="COLOR: #ffffff">Stampa in corso...</div>
		<script type="text/javascript" language="javascript">parent.close()</script>
		<form id="Form1" method="post" runat="server">
			<table style="DISPLAY: none"><tr><td>
			<div style="DISPLAY: none" ><asp:TextBox ID="idOp" Runat="server"></asp:TextBox>
			<asp:TextBox ID="idLinkToPrint" Runat="server"></asp:TextBox>
			</div>
			</td></tr>
			</table>
		</form>
	</body>
</html>
