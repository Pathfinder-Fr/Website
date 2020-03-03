Projet Pathfinder-Fr
====================

Se projet se base sur le défunt projet Sueetie.

# Publication

Exemple de lien symbolique permettant de se brancher sur les transforms hébergés sur OneDrive :

    cd Publish
    mklink /d Transforms C:\Users\Thomas\OneDrive\Dev\Pathfinder-Fr\ConfigTransforms

ou en PowerShell

    cd Publish
    New-Item -ItemType SymbolicLink -Path . -Name Transforms -Value C:\Users\Tom\OneDrive\Dev\Pathfinder-Fr\ConfigTransforms

# Développement

- Recopier la librairie `lib\ScrewTurn304\SqlServerProviders.dll` dans le dossier `src\WebApplication\Wiki\public\Plugins`
- Modifier la table `PluginStatus` pour remplacer la chaîne de connexion SQL par `data source=.\sqlexpress;initial catalog=Pathfinder-Fr;integrated security=true`