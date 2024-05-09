function GetInfo() {
    var eq1 = document.getElementById("eq1");
    var eq2 = document.getElementById("eq2");
    var eq3 = document.getElementById("eq3");
    var eq4 = document.getElementById("eq4");
    eq1 = eq1.value;
    eq2 = eq2.value;
    eq3 = eq3.value;
    eq4 = eq4.value;

    async function addEquations(name) {
        let equations = {
            Eq1: eq1,
            Eq2: eq2,
            Eq3: eq3,
            Eq4: eq4
        };
        console.log(equations);

        let equationsString = JSON.stringify(equations);

        console.log("adds equations");
        await fetch("/addEquations", {
            method: "POST",
            body: equationsString,
        });
    }
}

