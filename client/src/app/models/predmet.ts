export interface Predmet {
    id: number;
    zkratka: string;
    katedra: string;
    nazev: string;
    kreditu?: number;
    rok: string;
    statut: string;
    doporucenyRocnik?: number;
    doporucenySemestr: string;
    vyznamPredmetu: string;
    vyukaLS: string;
    vyukaZS: string;
    rozsah: string;
    typZk: string;
    oborIdNum?: number;
    files: Soubor[];
  }

  export interface Soubor {
    id: number;
    url: string;
    fileName: string;
    extension: string;
    dateAdded: string;
  }

  export interface Hodnoceni {
    id: number;
    text: string;
    rating: number;
    created: string;
    predmet: Predmet;
  }