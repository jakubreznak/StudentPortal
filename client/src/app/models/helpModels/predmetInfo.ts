export interface PredmetKatedry {
    katedra: string;
    zkratka: string;
    rok: string;
    nazev: string;
    semestr: string;
    vyukaZS: string;
    vyukaLS: string;
}

export interface PredmetyKatedry {
    predmetKatedry: PredmetKatedry[];
}
