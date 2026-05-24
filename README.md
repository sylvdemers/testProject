# Hockey Pool API

API .NET 8 pour gérer un pool de hockey avec MySQL et Docker.

## Fonctionnalités

- Gestion des équipes de hockey
- Gestion des joueurs
- Gestion des participants au pool
- Gestion des choix et du score des entrées
- Conteneurisé avec Docker
- Configuration prête pour MySQL via `docker-compose`

## Exécution locale

1. Construire l’image Docker :
   ```powershell
   docker compose build
   ```
2. Démarrer les services :
   ```powershell
   docker compose up
   ```
3. Ouvrir Swagger :
   - http://localhost:8080/swagger

## Configuration MySQL

Le projet utilise la chaîne de connexion suivante dans `appsettings.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=HockeyPoolDb;user=root;password=MySqlPassword!"
}
```

Quand Docker tourne, l’application se connecte au service MySQL sur `mysql:3306` via `docker-compose`.

## Entités principales

- `Team` : équipe de hockey
- `Player` : joueur associé à une équipe
- `PoolUser` : participant du pool
- `PoolEntry` : sélection d’un joueur pour une semaine

## Notes

- Le projet initialise automatiquement la base de données et ajoute des exemples.
- Pour modifier le mot de passe MySQL, met à jour `docker-compose.yml` et la chaîne de connexion dans `appsettings.json`.
