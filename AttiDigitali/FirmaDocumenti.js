debugger

function pausecomp(millis)
{
var date = new Date();
var curDate = null;

do { curDate = new Date(); }
while(curDate-date < millis);
} 


function firmaDocumento()   
                                                
{       

try {   
	                                                             
	
   if (confirm('Vuoi firmare il documento?'))
   
   {
		var nome=window.document.getElementById("TxtTitolareNome").value;
		var cognome = window.document.getElementById("TxtTitolareCognome").value;
		nome = "";
		cognome = "";
		
		var  oStore = new ActiveXObject("dllFirmaDig.svrFirmaDig");                   
		var vclient = new Array(3);                                                   
		var sclient ;                                                                 
		var vParam = new Array(8);                                                    
		var sParam ;                                                                  
		var stringaParam ;                                                            
		var stringaRit ;                                                              
		var vret;
	                
		vclient[0]="";                                                                
		vclient[1]="";                                                                
		vclient[2]="";                                                                
		vclient[3]="";                                                                
	    //aggiunta 
	           
	           	var newdiv = window.document.createElement('div');
		var divIdName='divAttendere'
		newdiv.setAttribute('id',divIdName);
		newdiv.style.position='absolute';
		newdiv.style.top= '200px';
        newdiv.style.left= '400px';
        newdiv.style.width= '200px';
		newdiv.style.height= '100px';	
        newdiv.style.backgroundColor='#99CCFF';
		newdiv.setAttribute('align','center');
		newdiv.setAttribute('valign','middle');
		newdiv.innerHTML ="<p style='margin-top:45px;color:white;'>Sto esaminando il documento... </p>";
		window.document.body.appendChild(newdiv);

	//newdiv.style.verticalAlign="middle";
	//newdiv.style.textAlign="center";

        newdiv.focus();
		pausecomp(1000);
	                                                                                   
		
                                                                         
		
		//fine aggiunta
		//cpFD_TipoDati = 2                                                           
		//cpFD_DatiNonFirmati =                                                       
		//cpFD_DatiFirmati =                                                          
		//cpFD_ENCTYPE = 0                                                            
		//cpFD_CodiceFiscale = CRLRCC75M04A743X                                       
		//cpFD_Azione =                                                               
		//cpFD_CA = 1                                                                 
		vParam[0]=2;                  
	   
		//  alert(window.document.getElementById("lnkAnteprima").href+'&pdf=1'); 
		//vParam[1]=window.document.getElementById("lnkAnteprima").href+'&pdf=1';  
		
		vParam[1]=window.document.getElementById("lnkAnteprima").href.replace("&pdf=1","").replace("&prew=1","")+'&pdf=1';               
	
		vParam[3]=0;
	                                                                    
		vParam[6]=1;     
		vParam[7]= nome;
		vParam[8]= cognome;                                                              
		sclient="";                                                                   
		sParam = "";                                                                  
		stringaRit="";                                                                
	     
	   //  alert(vParam[7]);
	    // alert(vParam[8]);
	     
		//alert(nome);
		//alert(cognome);
     
     
                                                                                   
		sclient = vclient.join("||");                                                 
		sParam = vParam.join("||");                                                   
	                                                                                   
		stringaParam  = "10##" + sclient + "$$" + sParam;                             
		stringaRit = oStore.Elabora_SER(stringaParam); 
	   window.document.body.removeChild(newdiv)
       vret =  stringaRit.split("##")
       if (vret[0] == 0) {                                                           
	    window.document.getElementById("hContenutoFileFirmato").value = vret[1];
	    var newdiv = window.document.createElement('div');
		
		newdiv.setAttribute('id',divIdName);
		newdiv.style.position='absolute';
		newdiv.style.top= '140px';
        newdiv.style.left= '250px';
        newdiv.style.width= '200px';
		newdiv.style.height= '100px';	
        newdiv.style.backgroundColor='#99CCFF';
		newdiv.setAttribute('align','center');
		newdiv.setAttribute('valign','middle');
		newdiv.innerHTML ="<p style='margin-top:45px;color:white;'>Attendere ...</p>";
		window.document.body.appendChild(newdiv);
		pausecomp(1000);
		
        // alert( window.document.getElementById("hContenutoFileFirmato").value ); 
        return true;                                                               
       }                                                                             
        else {                                             
         alert(vret[1]);                                                          
         return false;  
        }                                         
  }else {
  
       
		oStore = null;                                                                
		vret = null;                                                                  
       } 
}                                                                             
catch (e)                                                                          
  {                                                                                
     alert("Errore numero " + e.number + "\n" + e.description);                    
     throw new Error( e.number, "Firma con smart card" + e.description );          
  }                                                                                
return true;                                                                               
}              
