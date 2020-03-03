"use strict";

// Parties existantes, sous la forme d'objets {nom, noeud}
// Ex : nom = A-1, noeud = tr correspondant au forum
var cpParties = [];
// Indices des parties visibles dans cpParties
var cpVisibles = [];

// Initialisation

window.addEventListener('load', cp_init);

function cp_init() {
  cpParties = cp_listeParties();
  $("#cpChoix").empty();
  cpParties.forEach(function (_ref, i) {
      var nom = _ref.nom;
      var choix = document.getElementById("cpChoix");
      if (choix !== null) {
          choix.add(new Option(nom));
      }
  });
  $("#cpChoix").prop("selectedIndex", 0);
  cpVisibles = cp_litCookie(cpParties);
  cp_MajAffichage();
  cp_MajVisibles();
}

function cp_listeParties() {
  var titresForums = $(".forumheading a").get();
  var titresParties = titresForums.filter(function (titre) {
    return titre.innerHTML.match(/^Partie [A-Z]*-.*$/i);
  });
  return titresParties.map(function (titrePartie) {
    return {
      nom: titrePartie.innerHTML.match(/^Partie (.*)$/i)[1],
      noeud: $(titrePartie).parent().parent().parent() };
  });
}

function cp_litCookie(parties) {
  var infos = document.cookie.split(";");
  var i = 0;
  var trouvé = false;
  var valeur = "";
  while (i < infos.length && !trouvé) {
    var info = infos[i];
    while (info.charAt(0) == ' ') {
      info = info.substring(1, info.length);
    }
    if (info.indexOf("parties-visibles=") == 0) {
      trouvé = true;
      valeur = info.substring("parties-visibles=".length, info.length);
    }
    i++;
  }
  var visibles = [];
  if (valeur == "") return visibles;
  valeur.split("|").forEach(function (nom) {
    var pos = 0;
    while (pos < cpParties.length && nom != cpParties[pos].nom) {
      pos++;
    }
    if (pos == cpParties.length) {
      console.log("Erreur : partie visible pas trouvée " + nom);
    } else {
      visibles.push(pos);
    }
  });
  return visibles.sort(function (a, b) {
    return a - b;
  });
}

// Màj forums visibles, liste vignettes et cookie

function cp_maj() {
  cp_MajVisibles();
  cp_MajAffichage();
  cp_écritCookie();
}

function cp_MajAffichage() {
  cpParties.forEach(function (partie, pos) {
    if (cpVisibles.indexOf(pos) == -1) {
      partie.noeud.hide();
    } else {
      partie.noeud.show();
    }
  });
}

function cp_MajVisibles() {
  $("#cpVisibles").empty();
  if (cpVisibles.length == 0) {
    $("#cpVisibles").html("Aucune !");
    return;
  }
  cpParties.forEach(function (partie, pos) {
    if (cpVisibles.indexOf(pos) != -1) {
      $("#cpVisibles").append($("<div></div>").html(partie.nom).addClass("partie-visible").click(function () {
        cp_enleveVis(pos);
      }));
    }
  });
}

function cp_écritCookie() {
  var valeur = cpVisibles.map(function (pos) {
    return cpParties[pos].nom;
  }).join("|");
  document.cookie = "parties-visibles=" + valeur;
}

// Gestion liste des parties visibles

function cp_ajouteVis(pos) {
  if (cpVisibles.indexOf(pos) == -1) {
    cpVisibles.push(pos);
    cpVisibles.sort(function (a, b) {
      return a - b;
    });
    cp_maj();
  }
}

function cp_enleveVis(pos) {
  var posDansVisibles = cpVisibles.indexOf(pos);
  if (posDansVisibles != -1) {
    cpVisibles.splice(posDansVisibles, 1);
    cp_maj();
  }
}

function cp_toutvoir() {
  cpVisibles = [];
  var nbParties = cpParties.length;
  for (var pos = 0; pos < nbParties; pos++) {
    cpVisibles.push(pos);
  }cp_maj();
}

function cp_toutcacher() {
  cpVisibles = [];
  cp_maj();
}

function cp_ajouter() {
  cp_ajouteVis($("#cpChoix").prop("selectedIndex"));
}