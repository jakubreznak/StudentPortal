using System.Collections.Generic;

namespace API.Entities
{
    public class Predmet    {
        public Predmet() => this.Files = new List<Soubor>();
        public int ID { get; set; }
        public string zkratka { get; set; } 
        public string katedra { get; set; } 
        public string nazev { get; set; } 
        public int? kreditu { get; set; } 
        public string rok { get; set; } 
        public string statut { get; set; } 
        public int? doporucenyRocnik { get; set; } 
        public string doporucenySemestr { get; set; } 
        public string vyznamPredmetu { get; set; } 
        public string vyukaLS { get; set; } 
        public string vyukaZS { get; set; } 
        public string rozsah { get; set; } 
        public string typZk { get; set; } 
        public int? oborIdNum { get; set; }
        public List<Soubor> Files { get; set; } 
    }
}