Das Projekt ist für .NET 8 aufgesetzt.
Die Methoden mit den angefragten features liegen im Controller:
Controllers/Users/UsersAdminController.cs

appsettings.json:
- ConnectionString für die Datenbank
- Ein paar Optionen bzgl. Invitation Code Generation
- Min & Max für Pagination der Users

Notizen zu allgemeinen Entscheidungen:
- Benutzung der Controller anstatt Minimal-Web-API, weil Sebastian Controller im Gespräch erwähnt hatte.
- Nutzung von manchen Global Usings in GlobalUsings.cs für Reduzierung von vertikaler Höhe der einzelnen Dateien.
- Für den Zweck keiner zusätzlichen Abhänigkeiten beim Starten, werde ich für den DB-Teil eine SQLite Datenbank nutzen. (Im realen wäre das natürlich eine andere)
- Wegen der limitierten Zeit und da das initiale Setup auch ein wenig Zeit braucht, verwende ich das Entity Framework für die Leichtigkeit.
- Ich nehme einfach für das Beispiel an, dass man auch das erste Passwort des Mitarbeiters setzt. (Wahrscheinlich anders)
- Für das Backend für das Anzeigen von Nutzern nutze ich aus Zeitgründen einfache Pagination, welche bei sehr großer Anzahl an Seiten pro Seite immer langsamer wird (besser wäre keyset pagination)
- Benutzung der default roles, da es ja nur ein Probe Projekt ist
- Beim Löschen von Nutzern werden diese nicht tatsächlich gelöscht, allerdings nur auf "IsDeleted" true gesetzt (Kein Datenverlust, Kann rückgängig gemacht werden)
- Zur Vollständigkeit gibt es aber eine zweite "Hard Delete" option wenn man ihn wirklich komplett löschen will
- Nutzung von keyword sealed für Klassen die keine Unterklassen haben werden, da es dem Compiler diverse (sehr) kleine Speed Möglichkeiten gibt. (Inlining, Devirtualization, etc.)
- Das war mein erstes mal, dass ich das Entity Framework benutzt habe, brauchte nur etwas mit dem ich schnell und easy die Datenbank aufbauen konnte ohne viel Zeit zu investieren



