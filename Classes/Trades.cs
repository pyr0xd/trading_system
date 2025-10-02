using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App;

// En trade-förfrågan mellan två användare.
// - FromUser: den som skickar förfrågan (erbjuder något)
// - ToUser:   den som tar emot förfrågan (ska acceptera/denya)
// - Status: "pending" (väntar), "denied" (nekad), "completed" (genomförd)
public class TradeRequest
{
    public int Id;
    public string FromUser = "";       // den som skickar förfrågan
    public string ToUser = "";         // den som får förfrågan
    public string OfferedItem = "";    // vad FromUser erbjuder
    public int OfferedAmount;
    public string RequestedItem = "";  // vad FromUser vill ha tillbaka
    public int RequestedAmount;
    public string Status = "pending";  // "pending", "denied", "completed"

    public override string ToString()
    {
        return $"[{Id}] {FromUser} → {ToUser} | " +
               $"Offers: {OfferedItem} x{OfferedAmount} | " +
               $"Wants: {RequestedItem} x{RequestedAmount} | " +
               $"Status: {Status}";
    }
}

// Sköter laddning/sparning + enkel hantering av trades
public class TradeManager
{
    // Vi återanvänder UserManager.memoryDir så att alla filer ligger i ./memory/
    public static readonly string memoryDir = UserManager.memoryDir;
    public static readonly string tradeFile = "trades.txt";
    public readonly string tradeMemory = Path.Combine(memoryDir, tradeFile);

    public List<TradeRequest> Trades = new();

    public TradeManager()
    {
        // Se till att katalogen och filen finns
        Directory.CreateDirectory(memoryDir);
        if (!File.Exists(tradeMemory)) File.WriteAllText(tradeMemory, "");
        Load();
    }

    // Skriv alla trades till fil.
    // Varje rad:
    // TRADE;Id;FromUser;ToUser;OfferedItem;OfferedAmount;RequestedItem;RequestedAmount;Status
    public void Save()
    {
        var lines = new List<string>();
        foreach (var t in Trades)
        {
            lines.Add($"TRADE;{t.Id};{t.FromUser};{t.ToUser};{t.OfferedItem};{t.OfferedAmount};{t.RequestedItem};{t.RequestedAmount};{t.Status}");
        }
        File.WriteAllLines(tradeMemory, lines);
    }
    // Läs in alla trades från fil (robust mot tomma/röra rader).
    public void Load()
    {
        Trades.Clear();
        if (!File.Exists(tradeMemory)) return;

        foreach (var raw in File.ReadAllLines(tradeMemory))
        {
            var line = raw.Trim();
            if (line.Length == 0) continue;
            if (!line.StartsWith("TRADE;")) continue;

            var p = line.Split(';');
            if (p.Length < 9) continue;

            // p[0]=TRADE, p[1]=Id, p[2]=From, p[3]=To, p[4]=OfferedItem,
            // p[5]=OfferedAmount, p[6]=RequestedItem, p[7]=RequestedAmount, p[8]=Status
            if (!int.TryParse(p[1], out int id)) continue;
            if (!int.TryParse(p[5], out int offeredAmount)) continue;
            if (!int.TryParse(p[7], out int requestedAmount)) continue;

            Trades.Add(new TradeRequest
            {
                Id = id,
                FromUser = p[2],
                ToUser = p[3],
                OfferedItem = p[4],
                OfferedAmount = offeredAmount,
                RequestedItem = p[6],
                RequestedAmount = requestedAmount,
                Status = p[8]
            });
        }
    }

    private int NextId() => Trades.Count == 0 ? 1 : Trades.Max(t => t.Id) + 1;

    // Skapa en ny "pending" trade-förfrågan.
    // Vi flyttar INTE items här – flytten sker först vid Accept.
    public TradeRequest Create(Users from, Users to, string offeredItem, int offeredAmount, string requestedItem, int requestedAmount)
    {
        var t = new TradeRequest
        {
            Id = NextId(),
            FromUser = from.Name,
            ToUser = to.Name,
            OfferedItem = offeredItem,
            OfferedAmount = offeredAmount,
            RequestedItem = requestedItem,
            RequestedAmount = requestedAmount,
            Status = "pending"
        };
        Trades.Add(t);
        Save();
        return t;
    }

    public TradeRequest? GetById(int id) => Trades.FirstOrDefault(t => t.Id == id);
    // Listar inkommande "pending" förfrågningar för en användare.
    public IEnumerable<TradeRequest> Incoming(string username) =>
        Trades.Where(t => t.ToUser.Equals(username) && t.Status == "pending");
    // Listar utgående "pending" förfrågningar som användaren skickat.
    public IEnumerable<TradeRequest> Outgoing(string username) =>
        Trades.Where(t => t.FromUser.Equals(username) && t.Status == "pending");
    // Listar färdiga/nekade trades som involverar användaren.
    public IEnumerable<TradeRequest> CompletedFor(string username) =>
        Trades.Where(t =>
            (t.FromUser.Equals(username) || t.ToUser.Equals(username)) &&
            (t.Status == "denied" || t.Status == "completed"));

    // Försök acceptera en trade.
    // - Endast mottagaren (ToUser) får acceptera.
    // - Vi kontrollerar att båda verkligen har tillräckligt många av sina items.
    // - Vid minsta problem: avbryt + bra feltext i 'error'.
    public bool TryAccept(int id, UserManager userManager, string actingUser, out string error)
    {
        error = "";
        TradeRequest t = GetById(id);
        if (t == null) { error = "Trade not found."; return false; }
        if (!string.Equals(t.Status, "pending"))
        {
            error = $"Trade is '{t.Status}'.";
            return false;
        }

        // Bara mottagaren får acceptera
        if (!string.Equals(actingUser, t.ToUser))
        {
            error = "Only the recipient of the request can accept it.";
            return false;
        }
        // Hitta båda användarna 
        Users from = userManager.UserList.FirstOrDefault(u =>
            string.Equals(u.Name, t.FromUser));
        Users to = userManager.UserList.FirstOrDefault(u =>
            string.Equals(u.Name, t.ToUser));

        if (from == null || to == null) { error = "Users not found."; return false; }
        // Sanity checks för items/antal
        if (string.IsNullOrWhiteSpace(t.OfferedItem) || t.OfferedAmount <= 0)
        {
            error = "Invalid offered item/amount.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(t.RequestedItem) || t.RequestedAmount <= 0)
        {
            error = "Invalid requested item/amount.";
            return false;
        }

        // Förkolla lager – bättre felmeddelande
        if (!from.HasItems(t.OfferedItem, t.OfferedAmount))
        {
            error = $"{from.Name} lacks {t.OfferedItem} x{t.OfferedAmount}.";
            return false;
        }
        if (!to.HasItems(t.RequestedItem, t.RequestedAmount))
        {
            error = $"{to.Name} lacks {t.RequestedItem} x{t.RequestedAmount}.";
            return false;
        }

        // Utför bytet med rollback
        if (!from.RemoveItems(t.OfferedItem, t.OfferedAmount))
        { error = "Failed to remove from sender."; return false; }

        if (!to.RemoveItems(t.RequestedItem, t.RequestedAmount))
        {
            from.AddItems(t.OfferedItem, t.OfferedAmount); // rollback
            error = "Failed to remove from recipient.";
            return false;
        }

        from.AddItems(t.RequestedItem, t.RequestedAmount);
        to.AddItems(t.OfferedItem, t.OfferedAmount);
        // Markera som klar och spara
        t.Status = "completed";
        Save();
        return true;
    }

    // Behåll gamla Accept-signaturen för bakåtkomp., men använd robust logik
    public bool Accept(int id, UserManager userManager)
    {
        // Om ingen acting user skickas in här, hoppa kontrollen på mottagare
        // (använd Program.cs-patchen nedan som kallar TryAccept med aktiv användare!)
        TradeRequest t = GetById(id);
        if (t == null || t.Status != "pending") return false;

        Users from = userManager.UserList.FirstOrDefault(u =>
            string.Equals(u.Name, t.FromUser));
        Users to = userManager.UserList.FirstOrDefault(u =>
            string.Equals(u.Name, t.ToUser));
        if (from == null || to == null) return false;
        if (!from.HasItems(t.OfferedItem, t.OfferedAmount)) return false;
        if (!to.HasItems(t.RequestedItem, t.RequestedAmount)) return false;

        if (!from.RemoveItems(t.OfferedItem, t.OfferedAmount)) return false;
        if (!to.RemoveItems(t.RequestedItem, t.RequestedAmount))
        {

            from.AddItems(t.OfferedItem, t.OfferedAmount);
            return false;
        }

        from.AddItems(t.RequestedItem, t.RequestedAmount);
        to.AddItems(t.OfferedItem, t.OfferedAmount);
        t.Status = "completed";
        Save();
        return true;
    }
    // Nekar en "pending" förfrågan (ingen item-flytt sker)
    public bool Deny(int id)
    {
        TradeRequest t = GetById(id);
        if (t == null || !string.Equals(t.Status, "pending"))
            return false;
        t.Status = "denied";
        Save();
        return true;
    }
}

