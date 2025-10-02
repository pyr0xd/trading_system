# Trading System

## Hur man använder
1. Kör programmet:
   ```bash
   dotnet build
   dotnet run
   ```
2. I huvudmenyn:
   - **Login** (1) eller **Add user** (2).
3. När du är inloggad:
   - **Add item** (1) – lägg till saker du äger.
   - **Trade** (2) – skapa en förfrågan: erbjud *A* mot *B* från en annan användare.
   - **remove** (3) – ta bort ett item från dig själv.
   - **Show all** (4) – se andras saker.
   - **Show mine** (5) – se dina saker.
   - **Browse trade requests** (8) – pending *incoming* och *outgoing*.
   - **Accept/Deny** (9) – hantera en pending trade (endast mottagaren kan acceptera).
   - **Browse completed** (10) – se completed/denied.

> All data sparas som textfiler i `./memory/` (`user.txt`, `trades.txt`).

---

## Våra implementationsval (kort resonemang)
- **Simpelt > avancerat**  
  Vi använder **textfiler** i `./memory/` i stället för databas. Lätt att läsa, testa och återställa.
- **Komposition framför arv**  
  En `Users` **har** en lista `List<Item>` (komposition). Vi skapade inte en stor arvshierarki eftersom vi bara behöver enkla behållare (namn + antal). Komposition håller klasserna små och tydliga.
- **Separera ansvar**  
  - `UserManager` läser/sparar användare + listning/lookup.  
  - `TradeManager` skapar/läser/uppdaterar trades och byter items vid accept.  
  - `Users` hanterar sitt **eget inventory** (Add/Has/Remove).  
  Denna uppdelning gör koden lättare att förstå och ändra.
- **Prestanda och förutsägbarhet i jämförelser**  
  Vi jämför strängar (användarnamn, item-namn) med **OrdinalIgnoreCase** för att undvika problem med versaler/gemener och kulturregler (t.ex. “Kalle” == “kalle”). Det gör programmet stabilt och användarvänligt.
- **Atomisk trade (enkel rollback)**  
  Vid *accept*: ta bort A:s erbjudna item; om B:s borttagning misslyckas → återställ A. Först när båda lyckas, lägger vi till mottagna items och markerar `completed`. Vid *deny* ändras bara status.
- **Enkel felhantering**  
  Vi returnerar `true/false` och korta feltexter där det behövs (t.ex. vid accept). Tillräckligt för ett litet konsolprogram.

---

## Varför dessa val?
- **Enkelhet**: lätt att rätta och visa upp i kursen.
- **Läsbarhet**: var klass gör en sak; lätt att hitta logiken.
- **Trygg trade**: rollback så inget försvinner vid halv-fel.

