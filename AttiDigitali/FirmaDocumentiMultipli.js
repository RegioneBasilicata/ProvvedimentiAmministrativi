debugger


function pausecomp(millis)
{
var date = new Date();
var curDate = null;

do { curDate = new Date(); }
while(curDate-date < millis);
} 

function firmaDocumentiMultipli()   
                                                
{       

try {  
	
	 var nome=window.document.getElementById("TxtTitolareNome").value;
	 var cognome=window.document.getElementById("TxtTitolareCognome").value;  
	
	 nome ="";
	 cognome="";
	
	 var  oStore = new ActiveXObject("dllFirmaDig.svrFirmaDig");                   
	 var vclient = new Array(3);                                                   
     var sclient ;                                                                 
     var vParam = new Array(8);                                                    
     var sParam ;                                                                  
     var stringaParam ;                                                            
     var stringaRit ;                                                              
     var vret;
     var numAllegati=window.document.getElementById("NumeroAllegati").value; 
     var vAllDaFirmare = new Array(numAllegati);
     var url = "";
     var stringaUrl = "";
     var inputRitorno="";
     


	var divIdName = 'WaitDiv';
	var i=0;
if (confirm('Vuoi firmare i documenti?')){

      
            
     for (i=0;i<numAllegati;i++){
		url = "link"+i;
	
		var newdiv = window.document.createElement('div');
		
		newdiv.setAttribute('id',divIdName);
		newdiv.style.position='absolute';
		newdiv.style.top= '200px';
        newdiv.style.left= '400px';
        newdiv.style.width= '200px';
		newdiv.style.height= '100px';	
        newdiv.style.backgroundColor='#99CCFF';
		newdiv.setAttribute('align','center');
		newdiv.setAttribute('valign','middle');
		newdiv.innerHTML ="<p style='margin-top:45px;color:white;'>Esaminati  " +  i  + " documenti di " + numAllegati + "</p>";
		window.document.body.appendChild(newdiv);

	//newdiv.style.verticalAlign="middle";
	//newdiv.style.textAlign="center";

        newdiv.focus();
		pausecomp(2000);
		stringaUrl=window.document.getElementById(url).href.replace("&pdf=1","").replace("&prew=1","")+'&pdf=1'+'&idx='+i;
		vclient[0]="";                                                                
		vclient[1]="";                                                                
		vclient[2]="";                                                                
		vclient[3]="";  
	     
		vParam[0]=2;                  
		vParam[1]=stringaUrl;                                                                    
		vParam[3]=0;                                                                  
		vParam[6]=1; 
		vParam[7]=nome;
		vParam[8]=cognome;                                                                 
		sclient="";                                                                   
		sParam = "";                                                                  
		stringaRit="";                                                                
	                                                                                   
		sclient = vclient.join("||");                                                 
		sParam = vParam.join("||");                                                   
	                                                                                   
		stringaParam  = "10##" + sclient + "$$" + sParam;                             
		stringaRit = oStore.Elabora_SER(stringaParam); 
	    
	    window.document.body.removeChild(newdiv)
	      
		vret =  stringaRit.split("##")
		
		if (vret[0] == 0) {
			inputRitorno="hContenutoFileFirmato" + i;                                                                                                                     
			window.document.getElementById(inputRitorno).value = vret[1];                                                         
		}                                                                             
		else {                                             
			alert(vret[1]);                                                          
			return false;  
		}      
     }
   }   
   
   		
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
   
                                                                              
     oStore = null;                                                                
     vret = null;                                                                  
    }                                                                              
catch (e)                                                                          
  {                                                                                
     alert("Errore numero " + e.number + "\n" + e.description);                    
     throw new Error( e.number, "Firma con smart card" + e.description );          
  }                                                                                
return true;                                                                               
}              
