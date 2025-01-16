using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PrintFarm
{
    class Program
    {
        static void Main(string[] args)
        {
            var (imprimante, comenzi, stocFilament) = ServiciuPersistenta.IncarcaDate();
            bool ruleaza = true;

            while (ruleaza)
            {
                Console.Clear();
                Console.WriteLine("=== Meniu Principal ===");
                Console.WriteLine("1. Cont Utilizator");
                Console.WriteLine("2. Cont Administrator");
                Console.WriteLine("3. Salveaza si iesi");

                Console.Write("Alegerea ta: ");
                string alegere = Console.ReadLine();

                try
                {
                    switch (alegere)
                    {
                        case "1":
                            MeniuUtilizator(imprimante, comenzi);
                            break;
                        case "2":
                            MeniuAdministrator(imprimante, comenzi, stocFilament);
                            break;
                        case "3":
                            ServiciuPersistenta.SalveazaDate(imprimante, comenzi, stocFilament);
                            ruleaza = false;
                            break;
                        default:
                            Console.WriteLine("Alegere invalida. Incearca din nou.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Eroare: {ex.Message}");
                }

                Console.WriteLine("Apasa Enter pentru a continua...");
                Console.ReadLine();
            }
        }

        static void MeniuUtilizator(List<Imprimanta> imprimante, List<Comanda> comenzi)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Meniu Utilizator ===");
                Console.WriteLine("1. Vizualizează costurile pe gram");
                Console.WriteLine("2. Calculează costul unui obiect");
                Console.WriteLine("3. Comandă un obiect");
                Console.WriteLine("4. Inapoi");

                Console.Write("Alegerea ta: ");
                string alegere = Console.ReadLine();

                if (alegere == "1")
                {
                    Console.WriteLine("Costuri pe gram:");
                    Console.WriteLine("- Răsină: 0.5 RON/g");
                    Console.WriteLine("- Filament: 0.2 RON/g");
                }
                else if (alegere == "2")
                {
                    Console.Write("Greutatea obiectului (g): ");
                    double greutate = double.Parse(Console.ReadLine());
                    Console.Write("Tipul printării (rasina/filament): ");
                    string tip = Console.ReadLine().ToLower();

                    double cost = tip == "rasina" ? 0.5 * greutate : 0.2 * greutate;
                    Console.WriteLine($"Costul obiectului: {cost} RON");
                }
                else if (alegere == "3")
                {
                    Console.Write("Nume obiect: ");
                    string nume = Console.ReadLine();
                    Console.Write("Greutate (g): ");
                    double greutate = double.Parse(Console.ReadLine());
                    Console.Write("Culoare: ");
                    string culoare = Console.ReadLine();
                    Console.Write("Adresa livrare: ");
                    string adresa = Console.ReadLine();

                    var comanda = new Comanda
                    {
                        NumeObiect = nume,
                        Greutate = greutate,
                        Culoare = culoare,
                        AdresaLivrare = adresa
                    };
                    comenzi.Add(comanda);

                    Console.WriteLine("Comanda a fost adăugată!");
                }
                else if (alegere == "4")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Alegere invalidă.");
                }

                Console.WriteLine("Apasă Enter pentru a continua...");
                Console.ReadLine();
            }
        }

        static void MeniuAdministrator(List<Imprimanta> imprimante, List<Comanda> comenzi, StocFilament stocFilament)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Meniu Administrator ===");
                Console.WriteLine("1. Vizualizează imprimante");
                Console.WriteLine("2. Vizualizează detalii imprimantă");
                Console.WriteLine("3. Adaugă răsină");
                Console.WriteLine("4. Schimbă filament");
                Console.WriteLine("5. Vizualizează stoc filament");
                Console.WriteLine("6. Adaugă filament în stoc");
                Console.WriteLine("7. Aruncă role cu filament putin");
                Console.WriteLine("8. Vizualizează comenzi");
                Console.WriteLine("9. Procesează comandă");
                Console.WriteLine("10. Înapoi");

                Console.Write("Alegerea ta: ");
                string alegere = Console.ReadLine();

                if (alegere == "1")
                {
                    Console.WriteLine("Lista imprimantelor:");
                    foreach (var imprimanta in imprimante)
                        Console.WriteLine(imprimanta.Descriere());
                }
                else if (alegere == "2")
                {
                    Console.Write("Introdu numele imprimantei: ");
                    string nume = Console.ReadLine();
                    var imprimanta = imprimante.Find(i => i.Nume == nume);
                    Console.WriteLine(imprimanta != null ? imprimanta.Descriere() : "Imprimanta nu a fost găsită.");
                }
                else if (alegere == "3")
                {
                    Console.Write("Nume imprimantă cu răsină: ");
                    string nume = Console.ReadLine();
                    var imprimanta = imprimante.Find(i => i is ImprimantaRasina && i.Nume == nume) as ImprimantaRasina;

                    if (imprimanta != null)
                    {
                        imprimanta.AdaugaRasina(50);
                        Console.WriteLine("Răsina a fost adăugată.");
                    }
                    else
                    {
                        Console.WriteLine("Imprimanta nu a fost găsită.");
                    }
                }
                else if (alegere == "4")
                {
                    Console.Write("Nume imprimantă cu filament: ");
                    string nume = Console.ReadLine();
                    var imprimanta = imprimante.Find(i => i is ImprimantaFilament && i.Nume == nume) as ImprimantaFilament;

                    if (imprimanta != null)
                    {
                        Console.Write("Culoare nouă: ");
                        string culoare = Console.ReadLine();
                        Console.Write("Cantitate nouă (g): ");
                        double cantitate = double.Parse(Console.ReadLine());

                        imprimanta.SchimbaFilament(culoare, cantitate);
                        Console.WriteLine("Filamentul a fost schimbat.");
                    }
                    else
                    {
                        Console.WriteLine("Imprimanta nu a fost găsită.");
                    }
                }
                else if (alegere == "5")
                {
                    Console.WriteLine("Stoc filament:");
                    foreach (var item in stocFilament.VizualizeazaStoc())
                    {
                        Console.WriteLine($"- Culoare: {item.Key}, Cantitate: {item.Value} g");
                    }
                }
                else if (alegere == "6")
                {
                    Console.Write("Culoare filament: ");
                    string culoare = Console.ReadLine();
                    Console.Write("Cantitate (g): ");
                    double cantitate = double.Parse(Console.ReadLine());

                    stocFilament.AdaugaFilament(culoare, cantitate);
                    Console.WriteLine("Filamentul a fost adăugat în stoc.");
                }
                else if (alegere == "7")
                {
                    Console.WriteLine("Se vor arunca rolele cu filament sub 10 g...");
                }
                else if (alegere == "8")
                {
                    Console.WriteLine("Lista comenzilor:");
                    foreach (var comanda in comenzi)
                        Console.WriteLine(comanda);
                }
                else if (alegere == "9")
                {
                    Console.Write("ID comandă: ");
                    string idComanda = Console.ReadLine();
                    var comanda = comenzi.Find(c => c.IdComanda.ToString() == idComanda);

                    if (comanda != null && !comanda.EsteProcesata)
                    {
                        comanda.EsteProcesata = true;
                        Console.WriteLine("Comanda a fost procesată.");
                    }
                    else
                    {
                        Console.WriteLine("Comanda nu a fost găsită sau este deja procesată.");
                    }
                }
                else if (alegere == "10")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Alegere invalidă.");
                }

                Console.WriteLine("Apasă Enter pentru a continua...");
                Console.ReadLine();
            }
        }
    }
}

namespace PrintFarm
{
    public class Comanda
    {
        public Guid IdComanda { get; private set; }
        public string NumeObiect { get; set; }
        public double Greutate { get; set; }
        public string Culoare { get; set; }
        public string AdresaLivrare { get; set; }
        public bool EsteProcesata { get; set; }

        public Comanda()
        {
            IdComanda = Guid.NewGuid();
            EsteProcesata = false;
        }

        public override string ToString()
        {
            return $"Comanda ID: {IdComanda}, Obiect: {NumeObiect}, Greutate: {Greutate} g, Culoare: {Culoare}, Procesata: {EsteProcesata}";
        }
    }
}

namespace PrintFarm
{
    public abstract class Imprimanta
    {
        public string Nume { get; set; }
        public bool EsteInUz { get; set; }

        public Imprimanta(string nume)
        {
            Nume = nume;
            EsteInUz = false;
        }

        public abstract string Descriere();
    }
}

namespace PrintFarm
{
    public class ImprimantaRasina : Imprimanta
    {
        public double CapacitateRasina { get; set; }
        public double RasinaCurenta { get; set; }

        public ImprimantaRasina(string nume, double capacitate) : base(nume)
        {
            CapacitateRasina = capacitate;
            RasinaCurenta = capacitate;
        }

        public void AdaugaRasina(double cantitate)
        {
            RasinaCurenta = Math.Min(RasinaCurenta + cantitate, CapacitateRasina);
        }

        public override string Descriere()
        {
            return $"Imprimanta cu rasina '{Nume}' - Rasina curenta: {RasinaCurenta}/{CapacitateRasina} ml";
        }
    }

    public class ImprimantaFilament : Imprimanta
    {
        public string CuloareFilament { get; set; }
        public double CantitateFilament { get; set; }

        public ImprimantaFilament(string nume, string culoare, double cantitate) : base(nume)
        {
            CuloareFilament = culoare;
            CantitateFilament = cantitate;
        }

        public void SchimbaFilament(string culoare, double cantitate)
        {
            CuloareFilament = culoare;
            CantitateFilament = cantitate;
        }

        public override string Descriere()
        {
            return $"Imprimanta cu filament '{Nume}' - Filament: {CuloareFilament}, Cantitate: {CantitateFilament} g";
        }
    }

    public class StocFilament
    {
        private Dictionary<string, double> _stoc;

        public StocFilament()
        {
            _stoc = new Dictionary<string, double>();
        }

        public void AdaugaFilament(string culoare, double cantitate)
        {
            if (_stoc.ContainsKey(culoare))
            {
                _stoc[culoare] += cantitate;
            }
            else
            {
                _stoc[culoare] = cantitate;
            }
        }

        public Dictionary<string, double> VizualizeazaStoc()
        {
            return _stoc;
        }
    }

public class ServiciuPersistenta
    {
        private const string FisierImprimante = "imprimante.json";
        private const string FisierComenzi = "comenzi.json";
        private const string FisierStocFilament = "stoc_filament.json";

        public static (List<Imprimanta>, List<Comanda>, StocFilament) IncarcaDate()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            settings.Converters.Add(new ImprimantaConverter());

            var imprimante = File.Exists(FisierImprimante)
                ? JsonConvert.DeserializeObject<List<Imprimanta>>(File.ReadAllText(FisierImprimante), settings)
                : new List<Imprimanta>();

            var comenzi = File.Exists(FisierComenzi)
                ? JsonConvert.DeserializeObject<List<Comanda>>(File.ReadAllText(FisierComenzi))
                : new List<Comanda>();

            var stocFilament = File.Exists(FisierStocFilament)
                ? JsonConvert.DeserializeObject<StocFilament>(File.ReadAllText(FisierStocFilament))
                : new StocFilament();

            return (imprimante, comenzi, stocFilament);
        }

        public static void SalveazaDate(List<Imprimanta> imprimante, List<Comanda> comenzi, StocFilament stocFilament)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            settings.Converters.Add(new ImprimantaConverter());

            File.WriteAllText(FisierImprimante, JsonConvert.SerializeObject(imprimante, settings));
            File.WriteAllText(FisierComenzi, JsonConvert.SerializeObject(comenzi));
            File.WriteAllText(FisierStocFilament, JsonConvert.SerializeObject(stocFilament));
        }
    }

    public class ImprimantaConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Imprimanta).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
            var tipImprimanta = jsonObject["TipImprimanta"]?.ToString();

            Imprimanta imprimanta = tipImprimanta switch
            {
                "ImprimantaRasina" => new ImprimantaRasina(
                    jsonObject["Nume"]?.ToString(),
                    jsonObject["CapacitateRasina"]?.ToObject<double>() ?? 0
                )
                {
                    RasinaCurenta = jsonObject["RasinaCurenta"]?.ToObject<double>() ?? 0
                },
                "ImprimantaFilament" => new ImprimantaFilament(
                    jsonObject["Nume"]?.ToString(),
                    jsonObject["CuloareFilament"]?.ToString(),
                    jsonObject["CantitateFilament"]?.ToObject<double>() ?? 0
                ),
                _ => throw new Exception("Tip imprimanta necunoscut!")
            };

            imprimanta.EsteInUz = jsonObject["EsteInUz"]?.ToObject<bool>() ?? false;
            return imprimanta;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var imprimanta = (Imprimanta)value;
            var jsonObject = new Newtonsoft.Json.Linq.JObject
            {
                ["TipImprimanta"] = imprimanta is ImprimantaRasina ? "ImprimantaRasina" : "ImprimantaFilament",
                ["Nume"] = imprimanta.Nume,
                ["EsteInUz"] = imprimanta.EsteInUz
            };

            if (imprimanta is ImprimantaRasina imprimantaRasina)
            {
                jsonObject["CapacitateRasina"] = imprimantaRasina.CapacitateRasina;
                jsonObject["RasinaCurenta"] = imprimantaRasina.RasinaCurenta;
            }
            else if (imprimanta is ImprimantaFilament imprimantaFilament)
            {
                jsonObject["CuloareFilament"] = imprimantaFilament.CuloareFilament;
                jsonObject["CantitateFilament"] = imprimantaFilament.CantitateFilament;
            }

            jsonObject.WriteTo(writer);
        }
    }
}

