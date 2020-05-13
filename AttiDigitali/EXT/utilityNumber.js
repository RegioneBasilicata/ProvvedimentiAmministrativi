
 function number_format(a, b, c, d) {
a = Math.round(a * Math.pow(10, b)) / Math.pow(10, b);
e = a + '';
f = e.split('.');
if(!f[0]) f[0] = '0';
if(!f[1]) f[1] = '';
if(f[1].length < b){
g = f[1];
for(i = f[1].length + 1; i <= b; i++) {
g += '0';
}
f[1] = g;
}
if(d != '' && f[0].length > 3) {
h = f[0];
f[0] = '';
for(j = 3; j < h.length; j += 3) {
i = h.slice(h.length - j, h.length - j + 3);
f[0] = d + i + f[0] + '';
}
j = h.substr(0, (h.length % 3 == 0) ? 3 : (h.length % 3));
f[0] = j + f[0];
}
c = (b <= 0) ? '': c;
return f[0] + c + f[1];
}
var eurRend = function(number) {

    /*  var vv = '0';
    if (v != undefined) {
    vv = ('' + v).replace(/\./, ',');
    }
    */
 
    
    if (number == undefined) {
        number = 0;
    }
    if (number == '') number = 0;
    number = ('' + number).replace(/\./, ',');
    var contenuto = ''+ number
    contenuto = contenuto.replace(/\./g, ""); //espressione regolare
    contenuto = contenuto.replace(/,/g, "."); //espressione regolare
    contenuto = parseFloat(contenuto);
        number = number_format(contenuto, 2, ",", ".");


    return ('€ ' + number);

};