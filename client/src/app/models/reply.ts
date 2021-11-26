import { Comment } from "./topic";

export interface Reply{
    id: number;
    text: string;
    created: string;
    studentName: string;
    accountName: string;
    edited: string;
    comment: Comment;
}