# 🐸 FrogCreator

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![C#](https://img.shields.io/badge/C%23-.NET%208-purple.svg)](https://dotnet.microsoft.com/)
[![Godot](https://img.shields.io/badge/Godot-4.2-blue.svg)](https://godotengine.org/)

**FrogCreator** est un moteur de création de MMORPG 2D open source, réécriture moderne en C# avec le moteur Godot du célèbre projet [FRoG Creator](https://fr.wikipedia.org/wiki/FRoG_Creator) (French Online Game Creator), originellement développé en Visual Basic 6.

Ce dépôt représente la **version 2 (v2)** du projet, migrée de Java/LibGDX vers **C#/Godot 4**, avec pour objectif de proposer une base solide, multiplateforme et extensible pour la création de jeux de rôle en ligne massivement multijoueurs en 2D.

---

## 📖 Sommaire

- [Historique](#-historique)
- [Fonctionnalités](#-fonctionnalités)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Prérequis](#-prérequis)
- [Installation et compilation](#-installation-et-compilation)
- [Modules](#-modules)
- [Roadmap](#-roadmap)
- [Contribuer](#-contribuer)
- [Licence](#-licence)

---

## 📜 Historique

FRoG Creator est un projet communautaire français né au début des années 2000, permettant à quiconque de créer son propre MMORPG 2D sans connaissances avancées en programmation. Développé initialement en VB6 par la FRoG Team, il s'est inspiré de moteurs comme Konfuze et Mirage Source.

Le projet original a fédéré une large communauté francophone de créateurs de jeux, avant que le développement officiel ne s'arrête vers 2015.

**FrogCreator v2** est une migration complète vers C# et le moteur Godot 4, visant à :
- Profiter de l'écosystème Godot (éditeur intégré, rendu, physique, audio, export multiplateforme)
- Utiliser C# et .NET 8 pour un code moderne, performant et typé
- Offrir une compatibilité multiplateforme (Windows, macOS, Linux, et potentiellement mobile)
- Proposer une architecture modulaire et extensible via un système de plugins
- Conserver l'esprit d'accessibilité et de simplicité de l'original

---

## ✨ Fonctionnalités

- **Architecture client-serveur** pour le jeu multijoueur en ligne
- **Système d'entités à composants (ECS)** pour une gestion flexible des objets du jeu
- **Système de cartes** par tuiles avec support de couches (layers) et de chunks
- **Pathfinding A\*** pour l'intelligence artificielle des PNJ
- **Communication réseau** asynchrone par paquets avec chiffrement RSA
- **Système d'événements** découplé pour la communication entre modules
- **Système de plugins** pour étendre les fonctionnalités du moteur
- **Internationalisation (i18n)** avec support du français et de l'anglais
- **Client de jeu** intégré au moteur Godot 4 (effets, transitions d'écrans, caméra 2D)
- **Éditeur de jeu** tirant parti de l'éditeur intégré Godot

---

## 🏗 Architecture

Le projet est structuré dans un **projet Godot C#** avec les modules suivants :

```
godot/
├── scripts/
│   ├── api/        # Bibliothèque partagée (entités, réseau, systèmes, utilitaires)
│   ├── client/     # Client de jeu (Godot, rendu graphique, écrans)
│   ├── server/     # Serveur de jeu (gestion des connexions, plugins, concurrence)
│   └── editor/     # Éditeur de contenu
├── assets/         # Ressources graphiques
├── localization/   # Fichiers de localisation
├── tests/          # Tests unitaires (NUnit)
├── FrogCreator.csproj   # Projet C# principal (Godot SDK)
├── FrogCreator.sln      # Solution .NET
└── project.godot        # Configuration du projet Godot
```

### Diagramme simplifié

```
┌──────────────┐       ┌──────────────┐
│    Client    │◄─────►│    Server    │
│  (Godot C#)  │  TCP  │    (C#)     │
└──────┬───────┘       └──────┬───────┘
       │                       │
       └─────────┬─────────────┘
                 │
            ┌────▼────┐
            │   API   │
            │ (Core)  │
            └─────────┘

┌──────────────┐
│    Editor    │
│  (Godot C#)  │
└──────────────┘
```

---

## 🔧 Technologies

| Technologie | Utilisation |
|---|---|
| **C# / .NET 8** | Langage principal |
| **Godot 4.2** | Moteur de jeu (rendu, entrées, audio, éditeur) |
| **Godot.NET.Sdk** | SDK pour l'intégration C# avec Godot |
| **System.Text.Json** | Sérialisation des données |
| **System.Net.Sockets** | Communication réseau TCP |
| **NUnit 3** | Tests unitaires |
| **RSA** | Chiffrement des communications réseau |

---

## 📋 Prérequis

- **Godot 4.2** (version avec support C# / .NET)
- **.NET 8 SDK**
- **Git**

---

## 🚀 Installation et compilation

```bash
# Cloner le dépôt
git clone https://github.com/ClementDidier/FrogCreator.git
cd FrogCreator/godot

# Restaurer les dépendances .NET
dotnet restore

# Compiler le projet
dotnet build

# Lancer via Godot
# Ouvrir project.godot dans l'éditeur Godot 4.2 (C#)
```

### Lancer les tests

```bash
cd godot
dotnet test tests/FrogCreator.Tests.csproj
```

---

## 📦 Modules

### API (`scripts/api/`)
Bibliothèque centrale partagée entre le client et le serveur. Contient :
- **Entités** : `Entity`, `Character`, `Player`, `NPC`, `Item`
- **Carte** : `GameMap`, `GameMapChunk`, `GameMapLayer`
- **Réseau** : `Packet`, `FrogClientSocket`, `FrogServerSocket`, gestion asynchrone des paquets
- **Systèmes** : architecture ECS avec `GameSystem`, `HealthSystem`, composants et événements
- **IA** : pathfinding A* (`AStarPathfinder`)
- **Plugins** : interfaces `IPlugin` et attribut `FrogPlugin`
- **Sécurité** : chiffrement RSA
- **i18n** : support multilingue (FR, EN)

### Client (`scripts/client/`)
Client de jeu utilisant Godot 4 :
- Écrans : splash, menu principal, sélection de serveur, sélection de personnage, jeu principal
- Effets visuels et transitions (fondu entrant/sortant, tremblement de caméra)
- Gestion de caméra 2D et batch de rendu

### Server (`scripts/server/`)
Serveur multijoueur gérant :
- Connexions simultanées via `ClientWorker` et `RequestManager`
- Exécution concurrente des tâches (`RequestExecutor`, `FrogTask`)
- Chargement dynamique de plugins (`PluginLoader`)

### Editor (`scripts/editor/`)
Éditeur de contenu tirant parti des outils intégrés de Godot pour la création et la modification des ressources du jeu (cartes, PNJ, objets, etc.).

### Tests (`tests/`)
Tests unitaires NUnit couvrant :
- Système d'entités et composants
- Gestion de carte et chunks
- Sérialisation et routage de paquets réseau

---

## 🗺 Roadmap

### ✅ v1.0 — Fondations (Java/LibGDX — version précédente)

- [x] Architecture Maven multi-modules (API, Client, Server, Editor)
- [x] Système d'entités à composants (ECS)
- [x] Système de cartes par tuiles avec couches et chunks
- [x] Communication réseau client-serveur par paquets TCP
- [x] Chiffrement RSA des communications
- [x] Pathfinding A*
- [x] Système d'événements de jeu
- [x] Architecture de plugins
- [x] Client LibGDX avec gestion des écrans et transitions

### ✅ v2.0 — Migration C# / Godot 4 (version actuelle)

- [x] Migration du langage Java vers C# (.NET 8)
- [x] Migration du moteur LibGDX vers Godot 4 (C#)
- [x] Migration de JavaFX vers l'éditeur intégré Godot
- [x] Migration de Maven vers .NET SDK / Godot.NET.Sdk
- [x] Migration de JUnit vers NUnit 3
- [x] Migration de org.json vers System.Text.Json
- [x] Migration des sockets Java vers System.Net.Sockets
- [x] Conversion complète de l'API, du client, du serveur et de l'éditeur

### 🔜 v2.1 — Consolidation

- [ ] Éditeur de cartes complet via Godot TileMap
- [ ] Éditeur de PNJ et de dialogues
- [ ] Système d'inventaire et gestion des objets
- [ ] Système de combat de base (tour par tour ou temps réel)
- [ ] Système de chat en jeu
- [ ] Authentification et gestion des comptes joueurs
- [ ] Persistance des données (base de données ou fichiers)

### 🔮 v2.2 — Enrichissement

- [ ] Système de quêtes complet
- [ ] Système de scripting pour les événements de jeu
- [ ] Éditeur d'animations et de sprites via Godot
- [ ] Système de guildes / groupes
- [ ] Commerce entre joueurs
- [ ] Système de classes et d'arbres de compétences

### 🚀 v3.0 — Vision à long terme

- [ ] Support mobile (Android/iOS via export Godot)
- [ ] Interface web d'administration du serveur
- [ ] Système de marketplace pour partager les ressources communautaires
- [ ] Documentation complète et tutoriels pour les créateurs

---

## 🤝 Contribuer

Les contributions sont les bienvenues ! Pour contribuer :

1. Forkez le dépôt
2. Créez votre branche de fonctionnalité (`git checkout -b feature/ma-fonctionnalite`)
3. Commitez vos changements (`git commit -m 'Ajout de ma fonctionnalité'`)
4. Poussez la branche (`git push origin feature/ma-fonctionnalite`)
5. Ouvrez une Pull Request

Merci de respecter le style de code existant et d'ajouter des tests unitaires pour toute nouvelle fonctionnalité.

---

## 📄 Licence

Ce projet est distribué sous licence **GNU General Public License v3.0**. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

> *FrogCreator — Une seule limite... votre imagination !*
