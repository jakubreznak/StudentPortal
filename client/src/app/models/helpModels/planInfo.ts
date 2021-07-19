export interface PlanInfo {
    stplIdno: number;
    oborIdno: number;
    nazev: string;
    kreditne: string;
    limitKreditu: number;
    etapa: number;
    pocetSemestru: number;
    rokPlatnosti: string;
    verze: string;
    poznamka?: any;
    specializace: string;
    vyucJazyk: string;
    poradi?: any;
    ectsZobrazit: string;
}

export interface RootPlanInfo {
    planInfo: PlanInfo;
}