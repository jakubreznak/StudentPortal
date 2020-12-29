using System.Collections.Generic;
using API.Entities;

namespace API.HelpClass
{
        public class PlanInfo    {
        public int stplIdno { get; set; } 
        public int oborIdno { get; set; } 
        public string nazev { get; set; } 
        public string kreditne { get; set; } 
        public int limitKreditu { get; set; } 
        public int etapa { get; set; } 
        public int pocetSemestru { get; set; } 
        public string rokPlatnosti { get; set; } 
        public string verze { get; set; } 
        public object poznamka { get; set; } 
        public string specializace { get; set; } 
        public string vyucJazyk { get; set; } 
        public object poradi { get; set; } 
        public string ectsZobrazit { get; set; } 
    }

    public class Root    {
        public List<PlanInfo> planInfo { get; set; } 
    }

        public class RootPredmet    {
        public List<Predmet> predmetOboru { get; set; } 
    }
}