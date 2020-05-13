// add in_array method to arrays
Array.prototype.in_array = function(value) {
    var found = false;
    for (var i = 0; i < this.length; i++) {
        if (this[i] == value) {
            found = i;
            break;
        } 
    }
    return found;
}
// add ISO13616Prepare method to strings
String.prototype.ISO13616Prepare = function() {
    var isostr = this.toUpperCase();
    isostr = isostr.substr(4) + isostr.substr(0, 4);
    for (var i = 0; i <= 25; i++) {
        while (isostr.search(String.fromCharCode(i + 65)) != -1) {
            isostr = isostr.replace(String.fromCharCode(i + 65), String(i + 10));
        } 
    }
    return isostr;
}
// add ISO7064Mod97_10 method to strings
String.prototype.ISO7064Mod97_10 = function() {
    var parts = Math.ceil(this.length / 7);
    var remainer = "";
    for (var i = 1; i <= parts; i++) {
        remainer = String(parseFloat(remainer + this.substr((i - 1) * 7, 7)) % 97);
    }
    return remainer;
}
// replacement of === for javascript version < 1.2
function is_ident(a, b) {
    var identical = false;
    if (typeof (a) == typeof (b)) {
        if (a == b) {
            identical = true;
        } 
    }
    return identical;
}

// country codes, fixed length for those countries, inner structure, appliance of EU REGULATION 924/2009, IBAN requirement and IBAN example
var ilbced = new Array(
            "AD", 24, "F04F04A12", "n", "n", "AD1200012030200359100100",
			"AE", 23, "F03F16", "n", "n", "AE070331234567890123456",
			"AL", 28, "F08A16", "n", "n", "AL47212110090000000235698741",
			"AT", 20, "F05F11", "y", "n", "AT611904300234573201",
			"AZ", 28, "U04A20", "n", "n", "AZ21NABZ00000000137010001944",
			"BA", 20, "F03F03F08F02", "n", "n", "BA391290079401028494",
			"BE", 16, "F03F07F02", "y", "n", "BE68539007547034",
			"BG", 22, "U04F04F02A08", "y", "n", "BG80BNBG96611020345678",
			"BH", 22, "U04A14", "n", "n", "BH67BMAG00001299123456",
			"CH", 21, "F05A12", "n", "n", "CH9300762011623852957",
			"CR", 21, "F03F14", "n", "n", "CR0515202001026284066",
			"CY", 28, "F03F05A16", "y", "n", "CY17002001280000001200527600",
			"CZ", 24, "F04F06F10", "y", "n", "CZ6508000000192000145399",
			"DE", 22, "F08F10", "y", "n", "DE89370400440532013000",
			"DK", 18, "F04F09F01", "y", "n", "DK5000400440116243",
			"DO", 28, "U04F20", "n", "n", "DO28BAGR00000001212453611324",
			"EE", 20, "F02F02F11F01", "y", "n", "EE382200221020145685",
			"ES", 24, "F04F04F01F01F10", "y", "n", "ES9121000418450200051332",
			"FI", 18, "F06F07F01", "y", "n", "FI2112345600000785",
			"FO", 18, "F04F09F01", "n", "n", "FO6264600001631634",
			"FR", 27, "F05F05A11F02", "y", "n", "FR1420041010050500013M02606",
			"GB", 22, "U04F06F08", "y", "n", "GB29NWBK60161331926819",
			"GE", 22, "U02F16", "n", "n", "GE29NB0000000101904917",
			"GI", 23, "U04A15", "y", "n", "GI75NWBK000000007099453",
			"GL", 18, "F04F09F01", "n", "n", "GL8964710001000206",
			"GR", 27, "F03F04A16", "y", "n", "GR1601101250000000012300695",
			"HR", 21, "F07F10", "n", "n", "HR1210010051863000160",
			"HU", 28, "F03F04F01F15F01", "y", "n", "HU42117730161111101800000000",
			"IE", 22, "U04F06F08", "y", "n", "IE29AIBK93115212345678",
			"IL", 23, "F03F03F13", "n", "n", "IL620108000000099999999",
			"IS", 26, "F04F02F06F10", "y", "n", "IS140159260076545510730339",
			"IT", 27, "U01F05F05A12", "y", "n", "IT60X0542811101000000123456",
			"KW", 30, "U04A22", "n", "y", "KW81CBKU0000000000001234560101",
			"KZ", 20, "F03A13", "n", "n", "KZ86125KZT5004100100",
			"LB", 28, "F04A20", "n", "n", "LB62099900000001001901229114",
			"LI", 21, "F05A12", "y", "n", "LI21088100002324013AA",
			"LT", 20, "F05F11", "y", "n", "LT121000011101001000",
			"LU", 20, "F03A13", "y", "n", "LU280019400644750000",
			"LV", 21, "U04A13", "y", "n", "LV80BANK0000435195001",
			"MC", 27, "F05F05A11F02", "y", "n", "MC5811222000010123456789030",
			"MD", 24, "U02F18", "n", "n", "MD49AG000225100013104168",
			"ME", 22, "F03F13F02", "n", "n", "ME25505000012345678951",
			"MK", 19, "F03A10F02", "n", "n", "MK07250120000058984",
			"MR", 27, "F05F05F11F02", "n", "n", "MR1300020001010000123456753",
			"MT", 31, "U04F05A18", "y", "n", "MT84MALT011000012345MTLCAST001S",
			"MU", 30, "U04F02F02F12F03U03", "n", "n", "MU17BOMM0101101030300200000MUR",
			"NL", 18, "U04F10", "y", "n", "NL91ABNA0417164300",
			"NO", 15, "F04F06F01", "y", "n", "NO9386011117947",
			"PL", 28, "F08F16", "y", "y", "PL27114020040000300201355387",
			"PT", 25, "F04F04F11F02", "y", "n", "PT50000201231234567890154",
			"RO", 24, "U04A16", "y", "n", "RO49AAAA1B31007593840000",
			"RS", 22, "F03F13F02", "n", "n", "RS35260005601001611379",
			"SA", 24, "F02A18", "n", "y", "SA0380000000608010167519",
			"SE", 24, "F03F16F01", "y", "n", "SE4550000000058398257466",
			"SI", 19, "F05F08F02", "y", "n", "SI56191000000123438",
			"SK", 24, "F04F06F10", "y", "n", "SK3112000000198742637541",
			"SM", 27, "U01F05F05A12", "n", "n", "SM86U0322509800000000270100",
			"TN", 24, "F02F03F13F02", "n", "n", "TN5914207207100707129648",
			"TR", 26, "F05A01A16", "n", "y", "TR330006100519786457841326",
			"VG", 24, "U04F16", "n", "n", "VG11VPVG0000012345678901");
			
// we have currently # countries
var ctcnt = ilbced.length / 6;

// rearange country codes and related info
var ilbc = new Array();

for (j = 0; j < 6; j++) {
    for (i = 0; i < ctcnt; i++) {
        ilbc[ilbc.length] = ilbced[j + i * 6];
    }
}

// the magic core routine
function checkibancore(iban) {
    var standard = -1;
    illegal = /\W|_/; // contains chars other than (a-zA-Z0-9) 
    if (illegal.test(iban)) { // yes, exit        
        return "0";
    }
    else { // no, continue
        illegal = /^\D\D\d\d.+/; // first chars are letter letter digit digit
        if (illegal.test(iban) == false) { // no, exit
            return "0";
        }
        else { // yes, continue
            illegal = /^\D\D00.+|^\D\D01.+|^\D\D99.+/; // check digit are 00 or 01 or 99
            if (illegal.test(iban)) { // yes, exit
                return "0";
            }
            else { // no, continue
                lofi = ilbc.slice(0, ctcnt).in_array(iban.substr(0, 2).toUpperCase()); // test if country respected
                if (is_ident(false, lofi)) { ctck = -1; lofi = 6; }  // country not respected
                else { ctck = lofi; lofi = ilbc[lofi + ctcnt * 1]; } // country respected
                if (lofi == 6) { // not respected, alert
                    //alert("Non è possibile verificare l'IBAN perchè '" + iban.substr(0, 2).toUpperCase() + "' non è attualmente inventariato.");
                    lofi = iban.length;
                }  // but continue
                // fits length to country
                if ((iban.length - lofi) != 0) { // no, exit
                    return "0";
                } // yes, continue
                if (ctck >= 0) { illegal = buildtest("B04" + ilbc[ctck + ctcnt * 2], standard); } // fetch sub structure of respected country
                else { illegal = /.+/; } // or take care of not respected country
                // fits sub structure to country
                if (illegal.test(iban) == false) { // no, exit
                    return "0";
                }
                else { // yes, continue
                    return iban.ISO13616Prepare().ISO7064Mod97_10();
                } 
            } 
        } 
    }
} // calculate and return the remainer

// perform the check
function checkiban(iban) {
    return (checkibancore(iban) == "1");
}

function buildtest(structure, kind) {
    var result = "";
    var testpattern = structure.match(/([ABCFLUW]\d{2})/g);
    var patterncount = testpattern.length;
    for (var i = 0; i < patterncount; ++i) {
        if (((kind >= 0) && (i != kind)) || (kind == -2)) {
            result += testpart(testpattern[i], "any");
        }
        else {
            result += testpart(testpattern[i], "standard");
        } 
    }
    return new RegExp(result);
}

function testpart(pattern, kind) {
    var testpattern = "(";
    if (kind == "any") {
        testpattern += ".";
    }
    else {
        testpattern += "[";
        if (kind == "reverse") {
            testpattern += "^";
        }
        switch (pattern.substr(0, 1)) {
            case "A": testpattern += "0-9A-Za-z"; break;
            case "B": testpattern += "0-9A-Z"; break;
            case "C": testpattern += "A-Za-z"; break;
            case "F": testpattern += "0-9"; break;
            case "L": testpattern += "a-z"; break;
            case "U": testpattern += "A-Z"; break;
            case "W": testpattern += "0-9a-z"; break;
        }
        testpattern += "]";
    }
    if (((pattern.substr(1, 2) * 1) > 1) && (kind != "reverse")) {
        testpattern += "{" + String(pattern.substr(1, 2) * 1) + "}";
    }
    testpattern += ")";
    return testpattern;
}

