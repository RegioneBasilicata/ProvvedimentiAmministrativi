function SelezionaTutti(idcontrol) {
    var valueCheck = document.getElementById('ALL_' + idcontrol).checked
    var lists = document.getElementsByName(idcontrol);

    for (var i = 0; i < lists.length; i++) {
        //lists[i].checked=true
        lists[i].checked = valueCheck

    }
}