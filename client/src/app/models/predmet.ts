import { Student } from "./student";

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
    studentName: string;
    accountName: string;
    studentsLikedBy: SouborLike[];
  }

  export interface Hodnoceni {
    id: number;
    text: string;
    rating: number;
    accountName: string;
    created: string;
    edited: string;
    predmet: Predmet;
    studentsLikedBy: HodnoceniLike[];
  }

  export interface SouborLike {
    souborId: number;
    studentId: number;
  }

  export interface HodnoceniLike {
    hodnoceniId: number;
    studentId: number;
  }