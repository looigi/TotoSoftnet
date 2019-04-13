var myVar = [10];
var altezza = [10];
var movimento = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

function mouseOverImageSopra(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    if (movimento[Numero] == 0) {
        altezza[Numero] = -25;
        movimento[Numero] = 1;

        myVar[Numero] = setInterval(function () { SpostaLinguettaSopra(oggetto, Numero) }, 5);
    } else {
        clearInterval(myVar[Numero]);

        myVar[Numero] = setInterval(function () { SpostaLinguettaSopra(oggetto, Numero) }, 5);
    }
}

function SpostaLinguettaSopra(NomeOggetto, Numero) {
    altezza[Numero]++;
    NomeOggetto.style.top = altezza[Numero] + "px";
    if (altezza[Numero] > 0) {
        clearInterval(myVar[Numero]);
        altezza[Numero] = 0;
        movimento[Numero] = 0;
    }
}

function mouseOutImageSopra(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    clearInterval(myVar[Numero]);
    movimento[Numero] = 1;

    myVar[Numero] = setInterval(function () { SpostaLinguettaSotto(oggetto, Numero) }, 5);
}

function SpostaLinguettaSotto(NomeOggetto, Numero) {
    altezza[Numero]--;
    NomeOggetto.style.top = altezza[Numero] + "px";
    if (altezza[Numero] < -25) {
        clearInterval(myVar[Numero]);
        altezza[Numero] = -25;
        movimento[Numero] = 0;
    }
}

var myVarS = [10];
var altezzaS = [10];
var movimentoS = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

function mouseOverImageSin(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    if (movimentoS[Numero] == 0) {
        altezzaS[Numero] = -36;
        movimentoS[Numero] = 1;

        myVarS[Numero] = setInterval(function () { SpostaLinguettaSinistra(oggetto, Numero) }, 5);
    } else {
        clearInterval(myVarS[Numero]);

        myVarS[Numero] = setInterval(function () { SpostaLinguettaSinistra(oggetto, Numero) }, 5);
    }
}

function SpostaLinguettaSinistra(NomeOggetto, Numero) {
    altezzaS[Numero]++;
    NomeOggetto.style.left = altezzaS[Numero] + "px";
    if (altezzaS[Numero] > -5) {
        clearInterval(myVarS[Numero]);
        altezzaS[Numero] = -5;
        movimentoS[Numero] = 0;
    }
}

function mouseOutImageSin(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    clearInterval(myVarS[Numero]);
    movimentoS[Numero] = 1;

    myVarS[Numero] = setInterval(function () { SpostaLinguettaSinistraRitorno(oggetto, Numero) }, 5);
}

function SpostaLinguettaSinistraRitorno(NomeOggetto, Numero) {
    altezzaS[Numero]--;
    NomeOggetto.style.left = altezzaS[Numero] + "px";
    if (altezzaS[Numero] < -36) {
        clearInterval(myVarS[Numero]);
        altezzaS[Numero] = -36;
        movimentoS[Numero] = 0;
    }
}

var myVarD = [10];
var altezzaD = [10];
var movimentoD = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

function mouseOverImageDes(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    if (movimentoD[Numero] == 0) {
        altezzaD[Numero] = -10;
        movimentoD[Numero] = 1;

        myVarD[Numero] = setInterval(function () { SpostaLinguettaDestra(oggetto, Numero) }, 5);
    } else {
        clearInterval(myVarD[Numero]);

        myVarD[Numero] = setInterval(function () { SpostaLinguettaDestra(oggetto, Numero) }, 5);
    }
}

function SpostaLinguettaDestra(NomeOggetto, Numero) {
    altezzaD[Numero]--;
    NomeOggetto.style.left = altezzaD[Numero] + "px";
    if (altezzaD[Numero] < -46) {
        clearInterval(myVarD[Numero]);
        altezzaD[Numero] = -46;
        movimentoD[Numero] = 0;
    }
}

function mouseOutImageDes(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    clearInterval(myVarD[Numero]);
    movimentoD[Numero] = 1;

    myVarD[Numero] = setInterval(function () { SpostaLinguettaDestraRitorno(oggetto, Numero) }, 5);
}

function SpostaLinguettaDestraRitorno(NomeOggetto, Numero) {
    altezzaD[Numero]++;
    NomeOggetto.style.left = altezzaD[Numero] + "px";
    if (altezzaD[Numero] > -10) {
        clearInterval(myVarD[Numero]);
        altezzaD[Numero] = -10;
        movimentoD[Numero] = 0;
    }
}

var myVarSA = [10];
var altezzaSA = [10];
var movimentoSA = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

function mouseOverImageSopraSA(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_cphNoAjax_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    if (movimentoSA[Numero] == 0) {
        altezzaSA[Numero] = -25;
        movimentoSA[Numero] = 1;

        myVarSA[Numero] = setInterval(function () { SpostaLinguettaSopraSA(oggetto, Numero) }, 5);
    } else {
        clearInterval(myVarSA[Numero]);

        myVarSA[Numero] = setInterval(function () { SpostaLinguettaSopraSA(oggetto, Numero) }, 5);
    }
}

function SpostaLinguettaSopraSA(NomeOggetto, Numero) {
    altezzaSA[Numero]++;
    NomeOggetto.style.top = altezzaSA[Numero] + "px";
    if (altezzaSA[Numero] > 0) {
        clearInterval(myVarSA[Numero]);
        altezzaSA[Numero] = 0;
        movimentoSA[Numero] = 0;
    }
}

function mouseOutImageSopraSA(NomeOggetto, Numero) {
    NomeOggetto = "ctl00_cphNoAjax_" + NomeOggetto;
    var oggetto = document.getElementById(NomeOggetto);

    clearInterval(myVarSA[Numero]);
    movimentoSA[Numero] = 1;

    myVarSA[Numero] = setInterval(function () { SpostaLinguettaSottoSA(oggetto, Numero) }, 5);
}

function SpostaLinguettaSottoSA(NomeOggetto, Numero) {
    altezzaSA[Numero]--;
    NomeOggetto.style.top = altezzaSA[Numero] + "px";
    if (altezzaSA[Numero] < -25) {
        clearInterval(myVarSA[Numero]);
        altezzaSA[Numero] = -25;
        movimentoSA[Numero] = 0;
    }
}

// Master
var myVarM = [10];
var altezzaM = [10];
var movimentoM = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

function mouseOverImageSopraM(NomeOggetto, Numero) {
    var aprechiude = document.getElementById("ctl00_hdnApreChiude");

    if (aprechiude.value == "SI") {
        var NomeOggettoL = "ctl00_divLogo";
        var oggettoLogo = document.getElementById(NomeOggettoL);

        if (oggettoLogo == null) {
            NomeOggetto = "ctl00_" + NomeOggetto;
            var oggetto = document.getElementById(NomeOggetto);

            if (movimentoM[Numero] == 0) {
                altezzaM[Numero] = -110;
                movimentoM[Numero] = 1;

                myVarM[Numero] = setInterval(function () { SpostaLinguettaSopraM(oggetto, Numero) }, 5);
            } else {
                clearInterval(myVarM[Numero]);

                myVarM[Numero] = setInterval(function () { SpostaLinguettaSopraM(oggetto, Numero) }, 5);
            }
        }
    }
}

function SpostaLinguettaSopraM(NomeOggetto, Numero) {
    altezzaM[Numero]++;
    NomeOggetto.style.top = altezzaM[Numero] + "px";
    if (altezzaM[Numero] > 0) {
        clearInterval(myVarM[Numero]);
        altezzaM[Numero] = 0;
        movimentoM[Numero] = 0;
    }

    PrendeDimensioni();
}

function mouseOutImageSopraM(NomeOggetto, Numero) {
    var aprechiude = document.getElementById("ctl00_hdnApreChiude");

    if (aprechiude.value == "SI") {
        var NomeOggettoL = "ctl00_divLogo";
        var oggettoLogo = document.getElementById(NomeOggettoL);

        if (oggettoLogo == null) {
            NomeOggetto = "ctl00_" + NomeOggetto;
            var oggetto = document.getElementById(NomeOggetto);

            clearInterval(myVarM[Numero]);
            movimentoM[Numero] = 1;

            myVarM[Numero] = setInterval(function () { SpostaLinguettaSottoM(oggetto, Numero) }, 5);
        }
    }
}

function SpostaLinguettaSottoM(NomeOggetto, Numero) {
    altezzaM[Numero]--;
    NomeOggetto.style.top = altezzaM[Numero] + "px";
    if (altezzaM[Numero] < -110) {
        clearInterval(myVarM[Numero]);
        altezzaM[Numero] = -110;
        movimentoM[Numero] = 0;
    }

    PrendeDimensioni();
}