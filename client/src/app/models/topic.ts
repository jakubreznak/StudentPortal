import { Reply } from "./reply";

export interface Topic {
    id: number;
    name: string;
    predmetID: string;
    studentName: string;
    accountName: string;
    created: string;
    createdDateTime: string;
    comments: Comment[];
}

export interface Comment {
    id: number;
    text: string;
    created: string;
    edited: string;
    studentName: string;
    accountName: string;
    topicID: number;
    topic: Topic;
    studentsLikedBy: CommentLike[];
    replies: Reply[];
}

export interface CommentLike {
    commentId: number;
    studentId: number;
  }