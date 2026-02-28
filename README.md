# 🐸 FrogCreator

[![Build Status](https://travis-ci.org/ClementDidier/FrogCreator.svg?branch=master)](https://travis-ci.org/ClementDidier/FrogCreator)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Java](https://img.shields.io/badge/Java-8-orange.svg)](https://www.oracle.com/java/technologies/javase/javase8-archive-downloads.html)

**FrogCreator** est un moteur de création de MMORPG 2D open source, réécriture moderne en Java du célèbre projet [FRoG Creator](https://fr.wikipedia.org/wiki/FRoG_Creator) (French Online Game Creator), originellement développé en Visual Basic 6.

Ce dépôt représente la **version 1 (v1)** du projet, avec pour objectif de proposer une base solide, multiplateforme et extensible pour la création de jeux de rôle en ligne massivement multijoueurs en 2D.

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

**FrogCreator v1** est une réécriture complète en Java, visant à :
- Moderniser la base de code avec des technologies actuelles (Java 8, LibGDX, Maven)
- Offrir une compatibilité multiplateforme (Windows, macOS, Linux)
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
- **Éditeur de jeu** basé sur JavaFX pour la création de contenu
- **Client de jeu** avec rendu graphique LibGDX (effets, transitions d'écrans, etc.)

---

## 🏗 Architecture

Le projet est structuré en **4 modules Maven** :

```
FrogCreator/
├── api/        # Bibliothèque partagée (entités, réseau, systèmes, utilitaires)
├── client/     # Client de jeu (LibGDX, rendu graphique, écrans)
├── editor/     # Éditeur de contenu (JavaFX)
├── server/     # Serveur de jeu (gestion des connexions, plugins, concurrence)
└── pom.xml     # POM parent Maven
```

### Diagramme simplifié

```
┌──────────┐       ┌──────────┐
│  Client  │◄─────►│  Server  │
│ (LibGDX) │  TCP  │  (Java)  │
└────┬─────┘       └────┬─────┘
     │                   │
     └───────┬───────────┘
             │
        ┌────▼────┐
        │   API   │
        │ (Core)  │
        └─────────┘

┌──────────┐
│  Editor  │
│ (JavaFX) │
└──────────┘
```

---

## 🔧 Technologies

| Technologie | Utilisation |
|---|---|
| **Java 8** | Langage principal |
| **Maven** | Gestion de build et dépendances |
| **LibGDX 1.9.6** | Framework de jeu (rendu, entrées, audio) |
| **LWJGL** | Backend OpenGL pour le client desktop |
| **JavaFX** | Interface de l'éditeur |
| **JSON (org.json)** | Sérialisation des données |
| **JUnit 4** | Tests unitaires |
| **RSA** | Chiffrement des communications réseau |

---

## 📋 Prérequis

- **Java JDK 8** ou supérieur
- **Apache Maven 3.x**
- **Git**

---

## 🚀 Installation et compilation

```bash
# Cloner le dépôt
git clone https://github.com/ClementDidier/FrogCreator.git
cd FrogCreator

# Compiler l'ensemble du projet
mvn clean install

# Compiler un module spécifique
mvn clean install -pl api
mvn clean install -pl client
mvn clean install -pl server
mvn clean install -pl editor
```

### Lancer les tests

```bash
mvn test
```

---

## 📦 Modules

### API (`api/`)
Bibliothèque centrale partagée entre le client et le serveur. Contient :
- **Entités** : `Entity`, `Character`, `Player`, `NPC`, `Item`
- **Carte** : `GameMap`, `GameMapChunk`, `GameMapLayer`
- **Réseau** : `Packet`, `FrogClientSocket`, `FrogServerSocket`, gestion asynchrone des paquets
- **Systèmes** : architecture ECS avec `GameSystem`, `HealthSystem`, composants et événements
- **IA** : pathfinding A* (`AStarPathfinder`)
- **Plugins** : interfaces `Plugin` et `FrogPlugin`
- **Sécurité** : chiffrement RSA
- **i18n** : support multilingue (FR, EN)

### Client (`client/`)
Client de jeu desktop utilisant LibGDX :
- Écrans : menu principal, sélection de serveur, sélection de personnage, jeu principal
- Effets visuels et transitions (fondu, tremblement)
- Rendu graphique par batch

### Server (`server/`)
Serveur multijoueur gérant :
- Connexions simultanées via `ClientWorker` et `RequestManager`
- Exécution concurrente des tâches (`RequestExecutor`, `FrogTask`)
- Chargement dynamique de plugins (`PluginLoader`)

### Editor (`editor/`)
Éditeur de contenu basé sur JavaFX pour la création et la modification des ressources du jeu (cartes, PNJ, objets, etc.).

---

## 🗺 Roadmap

### ✅ v1.0 — Fondations (version actuelle)

- [x] Architecture Maven multi-modules (API, Client, Server, Editor)
- [x] Système d'entités à composants (ECS)
- [x] Système de cartes par tuiles avec couches et chunks
- [x] Communication réseau client-serveur par paquets TCP
- [x] Chiffrement RSA des communications
- [x] Pathfinding A*
- [x] Système d'événements de jeu
- [x] Architecture de plugins
- [x] Client LibGDX avec gestion des écrans et transitions
- [x] Internationalisation (FR/EN)
- [x] Serveur concurrent multi-clients
- [x] Structure de base de l'éditeur JavaFX
- [x] Intégration continue (Travis CI)

### 🔜 v1.1 — Consolidation

- [ ] Éditeur de cartes complet dans l'éditeur JavaFX
- [ ] Éditeur de PNJ et de dialogues
- [ ] Système d'inventaire et gestion des objets
- [ ] Système de combat de base (tour par tour ou temps réel)
- [ ] Système de chat en jeu
- [ ] Authentification et gestion des comptes joueurs
- [ ] Persistance des données (base de données ou fichiers)

### 🔮 v1.2 — Enrichissement

- [ ] Système de quêtes complet
- [ ] Système de scripting pour les événements de jeu
- [ ] Éditeur d'animations et de sprites
- [ ] Système de guildes / groupes
- [ ] Commerce entre joueurs
- [ ] Système de classes et d'arbres de compétences

### 🚀 v2.0 — Vision à long terme

- [ ] Migration vers une version Java plus récente (11+)
- [ ] Support du scripting Lua ou JavaScript pour les créateurs de jeux
- [ ] Interface web d'administration du serveur
- [ ] Support mobile (Android/iOS via LibGDX)
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
