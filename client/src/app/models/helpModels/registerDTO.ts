import { PredmetOboru } from "./predmetOboru";

export interface RegisterDTO {
    name: string;
    password: string;
    upolNumber: string;
    predmety: PredmetOboru[];
    oborIdno: number;
    rocnikRegistrace: number;
}