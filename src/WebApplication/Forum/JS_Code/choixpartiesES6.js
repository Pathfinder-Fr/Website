// Parties existantes, sous la forme d'objets {nom, noeud}
// Ex : nom = A-1, noeud = tr correspondant au forum
let cpParties = [];
// Indices des parties visibles dans cpParties
let cpVisibles = [];

// Initialisation

window.addEventListener('load', cp_init);

function cp_init () {
  cpParties = cp_listeParties();
  $("#cpChoix").empty();
  cpParties.forEach(function ({nom}, i) {
    document.getElementById("cpChoix").add(new Option (nom));
  });
  $("#cpChoix").prop("selectedIndex", 0);
  cpVisibles = cp_litCookie(cpParties);
  cp_MajAffichage();
  cp_MajVisibles();
}

function cp_listeParties () {
  let titresForums = $(".forumheading a").get();
  let titresParties = titresForums.filter(titre => titre.innerHTML.match(/^Partie [A-Z]*-.*$/i));
  return titresParties.map(titrePartie => ({
    nom : titrePartie.innerHTML.match(/^Partie (.*)$/i)[1],
    noeud : $(titrePartie).parent().parent().parent()})
  );
}

function cp_litCookie (parties) {
  let infos = document.cookie.split(";");
  let i = 0;
  let trouvé = false;
  let valeur = "";
  while (i < infos.length && !trouvé) {
    let info = infos[i];
    while (info.charAt(0) == ' ') info = info.substring(1, info.length);
    if (info.indexOf("parties-visibles=") == 0) {
      trouvé = true;
      valeur = info.substring("parties-visibles=".length, info.length);
    }
    i++;
  }
  let visibles = [];
  if (valeur == "")
    return visibles;
  valeur.split("|").forEach(function (nom) {
    let pos = 0;
    while (pos < cpParties.length && nom != cpParties[pos].nom)
      pos++;
    if (pos == cpParties.length) {
      console.log("Erreur : partie visible pas trouvée " + nom);
    } else {
      visibles.push(pos);
    }
  })
  return visibles.sort((a,b) => a-b);
}

// Màj forums visibles, liste vignettes et cookie

function cp_maj () {
  cp_MajVisibles();
  cp_MajAffichage();
  cp_écritCookie();
}

function cp_MajAffichage () {
  cpParties.forEach(function (partie, pos) {
    if (cpVisibles.indexOf(pos) == -1) {
      partie.noeud.hide();
    } else {
      partie.noeud.show();
    }
  });
}

function cp_MajVisibles () {
  $("#cpVisibles").empty();
  if (cpVisibles.length == 0) {
    $("#cpVisibles").html("Aucune !");
    return;
  }
  cpParties.forEach(function (partie, pos) {
    if (cpVisibles.indexOf(pos) != -1) {
      $("#cpVisibles").append(
        $("<div></div>")
          .html(partie.nom)
          .addClass("partie-visible")
          .click(function () { cp_enleveVis(pos); }));
    }
  })
}

function cp_écritCookie () {
  let valeur = cpVisibles.map(pos => cpParties[pos].nom).join("|");
  document.cookie = "parties-visibles=" + valeur;
}

// Gestion liste des parties visibles

function cp_ajouteVis (pos) {
  if (cpVisibles.indexOf(pos) == -1) {
    cpVisibles.push(pos);
    cpVisibles.sort((a,b) => a-b);
    cp_maj();
  }
}

function cp_enleveVis (pos) {
  let posDansVisibles = cpVisibles.indexOf(pos);
  if (posDansVisibles != -1) {
    cpVisibles.splice(posDansVisibles, 1);
    cp_maj();
  }
}

function cp_toutvoir() {
  cpVisibles = [];
  let nbParties = cpParties.length;
  for (let pos = 0 ; pos < nbParties ; pos++)
    cpVisibles.push(pos);
  cp_maj();
}

function cp_toutcacher() {
  cpVisibles = [];
  cp_maj();
}

function cp_ajouter() {
  cp_ajouteVis($("#cpChoix").prop("selectedIndex"));
}