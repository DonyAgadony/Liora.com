import { send } from "./_utils"

let submitButton = document.getElementById("submitButton");

submitButton.onclick = function () {
    let XEq1 = parseFloat(document.getElementById("NumOfXEq1").value);
    let YEq1 = parseFloat(document.getElementById("NumOfYEq1").value);
    let ZEq1 = parseFloat(document.getElementById("NumOfZEq1").value);
    let WEq1 = parseFloat(document.getElementById("NumOfWEq1").value);
    let NEq1 = parseFloat(document.getElementById("NumOfNEq1").value);

    let XEq2 = parseFloat(document.getElementById("NumOfXEq2").value);
    let YEq2 = parseFloat(document.getElementById("NumOfYEq2").value);
    let ZEq2 = parseFloat(document.getElementById("NumOfZEq2").value);
    let WEq2 = parseFloat(document.getElementById("NumOfWEq2").value);
    let NEq2 = parseFloat(document.getElementById("NumOfNEq2").value);

    let XEq3 = parseFloat(document.getElementById("NumOfXEq3").value);
    let YEq3 = parseFloat(document.getElementById("NumOfYEq3").value);
    let ZEq3 = parseFloat(document.getElementById("NumOfZEq3").value);
    let WEq3 = parseFloat(document.getElementById("NumOfWEq3").value);
    let NEq3 = parseFloat(document.getElementById("NumOfNEq3").value);

    let XEq4 = parseFloat(document.getElementById("NumOfXEq4").value);
    let YEq4 = parseFloat(document.getElementById("NumOfYEq4").value);
    let ZEq4 = parseFloat(document.getElementById("NumOfZEq4").value);
    let WEq4 = parseFloat(document.getElementById("NumOfWEq4").value);
    let NEq4 = parseFloat(document.getElementById("NumOfNEq4").value);

    if (isNaN(XEq1)) { XEq1 = null }
    if (isNaN(YEq1)) { YEq1 = null }
    if (isNaN(ZEq1)) { ZEq1 = null }
    if (isNaN(WEq1)) { WEq1 = null }
    if (isNaN(NEq1)) { NEq1 = null }

    if (isNaN(XEq2)) { XEq2 = null }
    if (isNaN(YEq2)) { YEq2 = null }
    if (isNaN(ZEq2)) { ZEq2 = null }
    if (isNaN(WEq2)) { WEq2 = null }
    if (isNaN(NEq2)) { NEq2 = null }


    if (isNaN(XEq3)) { XEq3 = null }
    if (isNaN(YEq3)) { YEq3 = null }
    if (isNaN(ZEq3)) { ZEq3 = null }
    if (isNaN(WEq3)) { WEq3 = null }
    if (isNaN(NEq3)) { NEq3 = null; }

    if (isNaN(XEq4)) { XEq4 = null }
    if (isNaN(YEq4)) { YEq4 = null }
    if (isNaN(ZEq4)) { ZEq4 = null }
    if (isNaN(WEq4)) { WEq4 = null }
    if (isNaN(NEq4)) { NEq4 = null; }

    // 2x - 8y + 1z - 9w = 43
    let eq1 = FormatEquation(XEq1, YEq1, ZEq1, WEq1, NEq1);
    var eq2 = FormatEquation(XEq2, YEq2, ZEq2, WEq2, NEq2);
    var eq3 = FormatEquation(XEq3, YEq3, ZEq3, WEq3, NEq3);
    var eq4 = FormatEquation(XEq4, YEq4, ZEq4, WEq4, NEq4);
    console.log(eq1);
    console.log(eq2);
    console.log(eq3);
    console.log(eq4);
    addEquations(eq1, eq2, eq3, eq4);
}

function FormatEquation(X, Y, Z, W, N) {
    let FormattedEq = "";
    console.log(X)
    if (X == null || X == 0) { }
    else if (X >= 0) { FormattedEq += X + "x"; }
    else { FormattedEq += " - " + Math.abs(X) + "x"; }

    console.log(Y)
    if (Y == null || Y == 0) { }
    else if (Y >= 0) { FormattedEq += " + " + Math.abs(Y) + "y"; }
    else { FormattedEq += " - " + Math.abs(Y) + "y"; }

    console.log(Z)
    if (Z == null || Z == 0) { }
    else if (Z >= 0) { FormattedEq += " + " + Math.abs(Z) + "z"; }
    else { FormattedEq += " - " + Math.abs(Z) + "z"; }

    console.log(W)
    if (W == null || W == 0) { }
    else if (W >= 0) { FormattedEq += " + " + Math.abs(W) + "w"; }
    else { FormattedEq += " - " + Math.abs(W) + "w"; }

    if (N >= 0) { FormattedEq += " = " + Math.abs(N); }
    else { FormattedEq += " = - " + Math.abs(N); }

    if ((X == null || 0) && (Y == null || Y == 0) && (Z == null || Z == 0) && (Y == null || Y == 0)) {
        FormattedEq = null;
    }
    return FormattedEq;
}

async function addEquations(eq1, eq2, eq3, eq4) {
    let equations = {
        Eq1: eq1,
        Eq2: eq2,
        Eq3: eq3,
        Eq4: eq4
    };
    console.log(equations);
    send("/addEquations", equations);
}